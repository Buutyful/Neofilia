using System.Security.Cryptography;
using System.Text;
using static Neofilia.Domain.Table;

namespace Neofilia.Domain;

//Current idea, partecipant has a token linked to a given table, it will elapse in 1h,
//the token can only be refreshed by scanning the table qr
//If a different table is scanned while token is active, partecipant will switch table.
//Implementation detalis missing
public class Partecipant
{    
    public string ConnectionId { get; }
    public TableId TableId { get; private set; }
    public NotEmptyString UserName { get; private set; }
    public NotEmptyString Token { get; private set; }

    public Partecipant(
        TableId tableId,
        string connectionId,
        NotEmptyString userName)
    {
        TableId = tableId;
        ConnectionId = connectionId;
        UserName = userName;
        Token = GenerateToken(tableId.Value, connectionId);
    }
    private NotEmptyString GenerateToken(int id, string connectionId)
    {
        var now = DateTime.UtcNow;
        //combine id, connectionId and now and generate a token. (temp implementation)
        string token = $"{id}+{connectionId}+{now}";
        return new NotEmptyString(token);
    }

}

public static class TokenGenerator
{
    private const string SecretKey = "key";

    public static NotEmptyString GenerateToken(int id, string connectionId)
    {
        var now = DateTime.UtcNow;
        var data = $"{id}-{connectionId}-{now.Ticks}";

        string token;

        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey)))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            token = Convert.ToBase64String(hash);
        }

        return new NotEmptyString(token);
    }
}