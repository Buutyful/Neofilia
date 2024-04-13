﻿using Microsoft.AspNetCore.SignalR;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Neofilia.Server.Services.Quiz;
//quiz loop manager class
//TODO: Link this with signalR hub
public class Game(IHubContext<QuizHub> hubContext)
{
    private static readonly int _roundTimer = 60000;
    private readonly IHubContext<QuizHub> _hubContext = hubContext;

    private IState _currentState = new SetUp();

    public bool IsRoundActive { get; private set; }
    public Timer? Timer { get; private set; }
    public QuestionDto? Question { get; private set; }

    public async Task SwitchQuestion(QuestionDto question)
    {
        Question = question;
        await _hubContext.Clients.All.SendAsync("QuestionChanged", Question.Text);
    }
    public void SwitchState(IState newState) => _currentState = newState;
    public async Task StartRound()
    {
        IsRoundActive = true;
        if (Timer is null)
            throw new InvalidOperationException("timer should not be null when round active");

        await _hubContext.Clients.All.SendAsync("RoundStarted", _roundTimer);
        Timer.Start();
    }
    public async Task StopRound()
    {
        IsRoundActive = false;
        if (Timer is null)
            throw new InvalidOperationException("timer should not be null when round active");

        await _hubContext.Clients.All.SendAsync("RoundEnded");
        Timer.Stop();
    }
    public void ResetTimer()
    {
        if (Timer is not null)
        {
            Timer.Elapsed -= OnTimerElapsed;            
            Timer.Dispose();
        }
        Timer = new Timer(_roundTimer);
        Timer.Elapsed += OnTimerElapsed;
    }

    public async Task FetchAnswersAsync()
    {
        await _hubContext.Clients.All.SendAsync("GetAnswers");
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

    public async Task Run()
    {        
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
    public async Task ExecuteAsync(Game game)
    {
        var quest = await GetQuestionAsync();
        await game.SwitchQuestion(quest);
        game.ResetTimer();
        game.SwitchState(new Execution());
    }
}
public class Execution : IState
{
    private readonly TaskCompletionSource<bool> _timerElapsed = new();
   
    public async Task ExecuteAsync(Game game)
    {
        await game.StartRound();

        await _timerElapsed.Task;

        await game.StopRound();

        game.SwitchState(new End());
    }
    public void OnTimerElapsed()
    {
        _timerElapsed.TrySetResult(true);
    }
}

public class End : IState
{
    //Call SetUp
    public async Task ExecuteAsync(Game game)
    {
        //fetch answers from clients
        await game.FetchAnswersAsync();
        game.SwitchState(new SetUp());
    }
}