// Services/OllamaService.cs
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BusinessReportingAI.Services;

public class OllamaService : IOllamaService
{
    private readonly HttpClient _httpClient;
    private readonly string _model;

    // Dajemo LLM-u šemu baze (Context/Grounding)
    private const string DatabaseSchemaContext = @"
Tabela: Artikli(ArtikalID INT, Naziv NVARCHAR, Kategorija NVARCHAR, Cijena DECIMAL)
Tabela: Kupci(KupacID INT, NazivFirme NVARCHAR, Grad NVARCHAR)
Tabela: Prodaja(ProdajaID INT, ArtikalID INT, KupacID INT, DatumProdaje DATE, Kolicina INT, UkupanIznos DECIMAL)
Relacije: 
- Prodaja.ArtikalID se spaja sa Artikli.ArtikalID
- Prodaja.KupacID se spaja sa Kupci.KupacID
";

    public OllamaService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["Ollama:BaseUrl"] ?? "http://localhost:11434");
        _model = configuration["Ollama:Model"] ?? "llama3";
    }

    public async Task<string> GenerateSqlQueryAsync(string userPrompt)
    {
        var systemInstruction = $@"Ti si Microsoft SQL Server (T-SQL) ekspert.
Tvoj jedini zadatak je pretvoriti korisnički upit u validan T-SQL upit.
Pravila:
1. Koristi samo ove tabele i kolone:
{DatabaseSchemaContext}
2. Vrati SAMO čist SQL upit, bez ikakvih objašnjenja, uvodnih ili završnih rečenica.
3. Upit mora biti isključivo SELECT ili WITH (Common Table Expression). Nikada nemoj generisati INSERT, UPDATE, DELETE, DROP ili ALTER.
4. Nemoj formatirati tekst sa markdown blokovima (bez ```sql).";

        var requestBody = new
        {
            model = _model,
            system = systemInstruction,
            prompt = userPrompt,
            stream = false
        };

        var response = await _httpClient.PostAsJsonAsync("/api/generate", requestBody);
        response.EnsureSuccessStatusCode();

        var jsonDoc = await response.Content.ReadFromJsonAsync<JsonElement>();
        var generatedSql = jsonDoc.GetProperty("response").GetString() ?? string.Empty;

        return CleanSqlOutput(generatedSql);
    }

    private static string CleanSqlOutput(string rawSql)
    {
        if (string.IsNullOrWhiteSpace(rawSql))
        {
            return string.Empty;
        }

        // Ako model vrati Markdown kodni blok, koristi samo njegov sadržaj.
        var codeBlock = Regex.Match(
            rawSql,
            @"```(?:sql|tsql)?\s*(?<sql>.*?)```",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        var cleaned = codeBlock.Success
            ? codeBlock.Groups["sql"].Value
            : rawSql;

        // Uklanja eventualni uvod koji model doda prije samog upita.
        var sqlStart = Regex.Match(cleaned, @"\b(?:SELECT|WITH)\b", RegexOptions.IgnoreCase);
        if (sqlStart.Success)
        {
            cleaned = cleaned[sqlStart.Index..];
        }

        return Regex.Replace(cleaned, @"```", string.Empty).Trim();
    }
}