using Microsoft.AspNetCore.SignalR;
using Neofilia.Domain;
using static Neofilia.Domain.Local;
using static Neofilia.Domain.Table;

namespace Neofilia.Server.Services.Quiz;

public class QuizHub : Hub
{
    //lista di locali, accedere ai tavoli dal locale
    //per avere il contesto in cui il tavolo esiste:
    //ex: che tipo di rewards offre il locale?
    private readonly List<Local> Locals = [];

    public override Task OnConnectedAsync()
    {        
        return base.OnConnectedAsync();
    }

    public async Task<TableDto> JoinTable(
        LocalId localId, 
        TableId tableId, 
        NotEmptyString userName)
    {
        var partecipant = new Partecipant(
            tableId, 
            Context.ConnectionId, 
            userName);

        var local = Locals.FirstOrDefault(l => l.Id.Equals(localId));

        var table = local?.Tables.FirstOrDefault(t => t.Id.Equals(tableId));

        if (local is null || table is null) throw new ArgumentException("local or table not found");

        local.AddPartecipantToTable(partecipant, table);

        var test = new TableDto("test");

        await Clients.Caller.SendAsync("Joined", test);

        return test;        
    }
}

//empty dto for design purpose
public record TableDto(string Test);