using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using static Neofilia.Domain.Table;

namespace Neofilia.Domain;

//Current idea, partecipant has a token linked to a given table, it will elapse in 1h,
//the token can only be refreshed by scanning the table qr
//If a different table is scanned while token is active, partecipant will switch table.
//Implementation detalis missing
[NotMapped]
public class Partecipant
{
    private Partecipant() { }
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

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Partecipant)
            return false;
        if (Object.ReferenceEquals(this, obj))
            return true;
        if (this.GetType() != obj.GetType())
            return false;
        Partecipant item = (Partecipant)obj;
        return item.ConnectionId == this.ConnectionId;
    }
    public override int GetHashCode() => ConnectionId.GetHashCode();
    public static bool operator ==(Partecipant left, Partecipant right)
    {
        if (Object.Equals(left, null))
            return (Object.Equals(right, null));
        else
            return left.Equals(right);
    }
    public static bool operator !=(Partecipant left, Partecipant right)
    {
        return !(left == right);
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