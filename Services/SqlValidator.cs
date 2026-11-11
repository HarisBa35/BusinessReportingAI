// Services/SqlValidator.cs
using System.Text.RegularExpressions;

namespace BusinessReportingAI.Services;

public class SqlValidator : ISqlValidator
{
    // Lista nedozvoljenih DDL/DML operacija
    private static readonly string[] ForbiddenKeywords =
    {
        "INSERT", "UPDATE", "DELETE", "DROP", "ALTER", "TRUNCATE", 
        "EXEC", "EXECUTE", "MERGE", "GRANT", "REVOKE", "CREATE", 
        "SHUTDOWN", "XP_", "SP_"
    };

    public (bool IsValid, string ErrorMessage) Validate(string sql)
    {
        if (string.IsNullOrWhiteSpace(sql))
            return (false, "Generisani SQL upit je prazan.");

        string trimmedSql = sql.Trim().ToUpperInvariant();

        // 1. Mora počinjati sa SELECT ili WITH
        if (!trimmedSql.StartsWith("SELECT") && !trimmedSql.StartsWith("WITH"))
        {
            return (false, "Dozvoljeni su samo SELECT (read-only) upiti.");
        }

        // 2. Provjera da li ima višestrukih iskaza odvojenih sa tačka-zarezom (spriječiti batch chaining)
        var statements = sql.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (statements.Length > 1)
        {
            return (false, "Višestruki SQL upiti u jednom pozivu nisu dozvoljeni.");
        }

        // 3. Provjera postojanja destruktivnih ključnih riječi pomoću Regex granica riječi (\b)
        foreach (var keyword in ForbiddenKeywords)
        {
            var pattern = $@"\b{keyword}\b";
            if (Regex.IsMatch(trimmedSql, pattern, RegexOptions.IgnoreCase))
            {
                return (false, $"Detektovana zabranjena ključna riječ: {keyword}. Upit je odbijen iz sigurnosnih razloga.");
            }
        }

        return (true, string.Empty);
    }
}