using Microsoft.AspNetCore.SignalR;
using Neofilia.Domain;

namespace Neofilia.Server.Services.Quiz
{
    public class ChallengeManager : BackgroundService
    {
        private readonly IHubContext<QuizHub> _hubContext;
        private readonly PeriodicTimer _timer = new(TimeSpan.FromMinutes(15));
        private readonly IServiceProvider _serviceProvider;
        private readonly Game _game;
        private static List<Local> _locals = [];
        public ChallengeManager(            
            IHubContext<QuizHub> hubContext,
            IServiceProvider serviceProvider)
        {
            _hubContext = hubContext;           
            _serviceProvider = serviceProvider;
            _game = new(hubContext);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (await _timer.WaitForNextTickAsync(stoppingToken) &&
            //       !stoppingToken.IsCancellationRequested)
            while(true)
            {
                _locals = await LocalHelpers.GetLocalsAsync(_serviceProvider);
                //TODO: create a challenge class
                //TODO: create a new tokensource that get's canceld when the challenger is over
                if (IsChallengeActive() || 
                    true /*testing always active*/)
                {
                    await _game.Run(stoppingToken);
                }
            }
        }
        private bool IsChallengeActive() =>
            _locals.Any(l => l.EventStartsAt <= DateTimeOffset.Now &&
                             l.EventEndsAt >= DateTimeOffset.Now);
        
      
    }
}
