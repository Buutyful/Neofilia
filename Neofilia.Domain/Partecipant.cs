using static Neofilia.Domain.Table;

namespace Neofilia.Domain;

//Current idea, partecipant has a token that will elapse in 1h,
//the token can only be refreshed by scanning the table qr
//If a different table is scanned while token is active, partecipant will switch table.
//Implementation detalis missing
public class Partecipant
{
    public Guid Id { get; private set; }
    public TableId TableId { get; private set; }
    public NotEmptyString UserName { get; private set; }
    public NotEmptyString Token { get; private set; }

    public Partecipant(TableId tableId, NotEmptyString userName, NotEmptyString token)
    {
       
        TableId = tableId;
        UserName = userName;
        Token = token;
    }

   
}
