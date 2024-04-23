using ErrorOr;
using Microsoft.AspNetCore.SignalR;
using Neofilia.Domain;
using System.Collections.Concurrent;
using static Neofilia.Domain.Local;
using static Neofilia.Domain.Table;

namespace Neofilia.Server.Services.Quiz;


public class QuizHub : Hub
{
    public const string HubUrl = "quiz/quizhub";
    //lista di locali, accedere ai tavoli dal locale
    //per avere il contesto in cui il tavolo esiste:
    //ex: che tipo di rewards offre il locale?   
    private readonly static List<Local> Locals = [];
    private static readonly ConcurrentDictionary<string, TableId> Players = [];
    

    public override Task OnConnectedAsync()
    {        
        return base.OnConnectedAsync();
    }

    public async Task JoinTable(
        LocalId localId, 
        TableId tableId, 
        NotEmptyString userName)
    {
        var id = Context.ConnectionId;
        var partecipant = new Partecipant(
            tableId, 
            id, 
            userName);

        var local = Locals.FirstOrDefault(l => l.Id.Equals(localId));

        var table = local?.Tables.FirstOrDefault(t => t.Id.Equals(tableId));

        if (local is null || table is null) throw new ArgumentException("local or table not found");

        local.AddPartecipantToTable(partecipant, table);

        if (Players.TryGetValue(id, out TableId value))
        {
            await Groups.RemoveFromGroupAsync(id, value.Value.ToString());
        }

        Players[id] = tableId;
        
        var groupKey = tableId.Value.ToString();

        await Groups.AddToGroupAsync(Context.ConnectionId, groupKey);        

        await Clients.Groups(groupKey).SendAsync(
            "UserJoined",
            new PartecipantDto(userName.Value));       
    }
    
    public async Task SendAnswer(bool answer)
    {        
        var id = Context.ConnectionId;        
        if (!Players.TryGetValue(id, out TableId value))
        {
            throw new InvalidOperationException("player not registered correctly to tables");
        }
        var tableId = value;
        var groupKey = tableId.Value.ToString();
        var table = Locals.SelectMany(l => l.Tables).FirstOrDefault(t => t.Id.Equals(tableId));
        //TODO: implement a functional score system
        if (answer) table?.AddScore();

        await Clients.Groups(groupKey).SendAsync("ScoreUpdated", table?.TableScore);
    }

    public ErrorOr<Table> GetTable(LocalId localId, TableId tableId)
    {
        var id = Context.ConnectionId;
        if (!Players.TryGetValue(id, out TableId value))
        {
            return Error.Unauthorized(description: "partecipant has not joined a table");
        }
        if(tableId != value)
        {
            return Error.Unexpected(
                description: "partecipant has joined a different table than the one requested");
        }
        var table = Locals.FirstOrDefault(l => l.Id.Equals(localId))!
                    .Tables.First(t => t.Id.Equals(tableId));
        return table;
    }

    public static async Task CreateSignalRGroups(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<QuizHub>>();            

            var locals = await LocalHelpers.GetLocalsAsync(serviceProvider);
            //TODO: this should not be created once per application lifetime            
            Locals.AddRange(locals);
            // Create a SignalR group for each table
            foreach (var table in locals.SelectMany(l => l.Tables))
            {
                await hubContext.Groups.AddToGroupAsync("", table.Id.Value.ToString());
            }
        }
    }
}

//empty dto for demo purpose
public record TableDto(string Test);
public record PartecipantDto(string UserName);