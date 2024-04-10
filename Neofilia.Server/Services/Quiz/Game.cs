using System.Timers;
using Timer = System.Timers.Timer;

namespace Neofilia.Server.Services.Quiz;
//quiz loop manager class
//TODO: Link this with signalR hub
public class Game
{
    private static readonly int _roundTimer = 60000;

    private IState _currentState;

    public bool IsRoundActive { get; private set; }
    public Timer Timer { get; private set; }
    public QuestionDto Question { get; private set; }

    public void SwitchQuestion(QuestionDto question) => Question = question;
    public void SwitchState(IState newState) => _currentState = newState;
    public void StartRound()
    {
        IsRoundActive = true;
        Timer.Start();
    }
    public void StopRound()
    {
        IsRoundActive = false;
        Timer.Stop();
    }
    public void ResetTimer()
    {
        if (Timer is not null)
        {
            Timer.Elapsed -= OnTimerElapsed;
            Timer.Stop();
            Timer.Dispose();
        }
        Timer = new Timer(_roundTimer);
        Timer.Elapsed += OnTimerElapsed;
    }

    private void OnTimerElapsed(object? state, ElapsedEventArgs e)
    {
        if (_currentState is Execution executionState)
        {
            executionState.OnTimerElapsed();
        }
        else throw new InvalidOperationException(
            "game is not in the correct state," +
            "timer elapsed while the game was not in execution");
    }

    public async Task Run(IState initialState)
    {
        _currentState = initialState;
        //TODO: change this to match the event duration
        while (true)
        {
            await _currentState.ExecuteAsync(this);
        }
    }
}

public interface IState
{
    public Task ExecuteAsync(Game game);
}

public class SetUp : IState
{   
    //fetch question
    private async Task<QuestionDto> GetQuestionAsync()
    {
        var question = await QuizService.GetQuestionDtoAsync();
        //TODO: change this to use a result pattern
        return question ?? new QuestionDto("blabla", true);
    }    

    //TODO: implement a new round starting notification system
    public async Task ExecuteAsync(Game game)
    {
        var quest = await GetQuestionAsync();
        game.SwitchQuestion(quest);
        game.ResetTimer();
        game.SwitchState(new Execution());
    }
}
public class Execution : IState
{
    private readonly TaskCompletionSource<bool> _timerElapsed = new();    
    private void StartRound(Game game) => game.StartRound();   
    private void StopRound(Game game) => game.StopRound();    
    public async Task ExecuteAsync(Game game)
    {
        StartRound(game);

        await _timerElapsed.Task;

        StopRound(game);

        game.SwitchState(new End());
    }
    public void OnTimerElapsed()
    {        
        _timerElapsed.TrySetResult(true);
    }
}

public class End : IState
{
    //Fetch Answers
    private async Task GetAnswersAsync()
    {        
        throw new NotImplementedException();
    }
    //Call SetUp
    public async Task ExecuteAsync(Game game)
    {
        //evaluete answers
        game.SwitchState(new SetUp());
    }
}