using System.Data;

namespace BusinessReportingAI.Models;

public class ReportResultViewModel
{
    /// <summary>
    /// Pitanje koje je korisnik postavio na prirodnom jeziku.
    /// </summary>
    public string UserPrompt { get; set; } = string.Empty;

    /// <summary>
    /// Validiran i izvršen SQL upit koji je generisao LLM.
    /// </summary>
    public string? GeneratedSql { get; set; }

    /// <summary>
    /// Tabelarni podaci dobijeni iz SQL Server baze.
    /// </summary>
    public DataTable? ReportData { get; set; }

    /// <summary>
    /// Poruka o grešci (ako validacija ili upit padne).
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Vrijeme izvršavanja cjelokupnog procesa u milisekundama.
    /// </summary>
    public long ExecutionTimeMs { get; set; }

    /// <summary>
    /// Oznaka da li je izvještaj uspješno generisan.
    /// </summary>
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage) && ReportData != null;

    /// <summary>
    /// Broj vraćenih redova iz baze podataka.
    /// </summary>
    public int RowCount => ReportData?.Rows.Count ?? 0;
}