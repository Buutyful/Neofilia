using ErrorOr;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Neofilia.Server.Services.Quiz;

//using https://opentdb.com/api_config.php
//TODO: replace console logs with an actuall logger
public static class QuizService
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly List<Question> _questions = new List<Question>();
    private static string? _sessionToken;
    private static DateTime _tokenExpiration;

    public static async Task<ErrorOr<QuestionDto>> GetQuestionDtoAsync()
    {
        try
        {
            var question = await GetQuestionAsync();

            return question is not null ? 
                Question.ConvertToDto(question) : 
                Error.NotFound();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            return Error.Unexpected(description: ex.Message);
        }
    }
    private static async Task<string?> GetJsonFromApiAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(_sessionToken) || DateTime.Now > _tokenExpiration)
            {
                // If the session token has expired or is not available, retrieve a new one
                if (!await RetrieveSessionTokenAsync())
                {
                    Console.WriteLine("Failed to retrieve a new session token.");
                    return null;
                }
            }

            //DefaultEncoding
            string apiUrl = $"https://opentdb.com/api.php?amount=1&type=boolean&token={_sessionToken}";

            // Configure timeout and retry policy before sending the request
            //_httpClient.Timeout = TimeSpan.FromSeconds(30);

            // Simple retry policy
            int maxRetries = 3;
            int currentRetry = 0;

            while (currentRetry < maxRetries)
            {
                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Non-successful response: {response.StatusCode}");
                        return null;
                    }
                }
                catch (HttpRequestException ex)
                {
                    currentRetry++;
                    Console.WriteLine($"HTTP Request Error (Retry {currentRetry}/{maxRetries}): {ex.Message}");

                    // Backoff strategy or sleep if needed before retrying
                    await Task.Delay(1000 * currentRetry);
                }
            }

            Console.WriteLine($"Failed to get response after {maxRetries} retries.");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    private static async Task<Question?> GetQuestionAsync()
    {
        try
        {
            string? jsonData = await GetJsonFromApiAsync();

            if (string.IsNullOrEmpty(jsonData))
            {
                Console.WriteLine("Failed to retrieve JSON data from the API.");
                return null;
            }

            var rootData = JsonSerializer.Deserialize<Root>(jsonData);

            if (rootData?.results != null && rootData.results.Count != 0)
            {
                var question = rootData.results.First();
                question.QuestionText = System.Net.WebUtility.HtmlDecode(question.QuestionText);
                _questions.Add(question);
                return question;
            }
            else
            {
                Console.WriteLine("No questions received from the API.");
                return _questions.Count != 0 ?
                    _questions[new Random().Next(0, _questions.Count)] : null;
            }
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            return null;
        }
    }
    private static async Task<bool> RetrieveSessionTokenAsync()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://opentdb.com/api_token.php?command=request");

            if (response.IsSuccessStatusCode)
            {
                string tokenJson = await response.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<TokenResponse>(tokenJson);

                if (tokenData != null && tokenData.ResponseCode == (int)ApiResponseCode.Success)
                {
                    _sessionToken = tokenData.Token;
                    _tokenExpiration = DateTime.Now.AddHours(6);
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve a new session token. " +
                        $"Response Code: {tokenData?.ResponseCode}, Message: {tokenData?.ResponseMessage}");
                }
            }
            else
            {
                Console.WriteLine($"Non-successful response: {response.StatusCode}");
            }

            return false;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP Request Error: {ex.Message}");
            return false;
        }
    }

    private class Question
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("difficulty")]
        public string Difficulty { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("question")]
        public string QuestionText { get; set; }

        [JsonPropertyName("correct_answer")]
        public string CorrectAnswer { get; set; }

        [JsonPropertyName("incorrect_answers")]
        public List<string> IncorrectAnswers { get; set; }

        public static QuestionDto ConvertToDto(Question quest) =>
            new QuestionDto(quest.QuestionText, ConvertToBool(quest.CorrectAnswer));
        private static bool ConvertToBool(string s) => s.ToLower().Equals("true");
    }
    private class Root
    {
        public int response_code { get; set; }
        public List<Question> results { get; set; }
    }
    private class TokenResponse
    {
        [JsonPropertyName("response_code")]
        public int ResponseCode { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("response_message")]
        public string ResponseMessage { get; set; }
    }
    private enum ApiResponseCode
    {
        Success = 0,
        NoResults = 1,
        InvalidParameter = 2,
        TokenNotFound = 3,
        TokenEmpty = 4,
        RateLimit = 5
    }
}
public record QuestionDto(string Text, bool CorrectAnswer);