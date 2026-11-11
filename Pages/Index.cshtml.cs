using System.Data;
using System.Diagnostics;
using BusinessReportingAI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessReportingAI.Pages;

public class IndexModel : PageModel
{
    private readonly IOllamaService _ollamaService;
    private readonly ISqlValidator _sqlValidator;
    private readonly IDbReportService _dbReportService;

    public IndexModel(IOllamaService ollamaService, ISqlValidator sqlValidator, IDbReportService dbReportService)
    {
        _ollamaService = ollamaService;
        _sqlValidator = sqlValidator;
        _dbReportService = dbReportService;
    }

    [BindProperty]
    public string UserPrompt { get; set; } = string.Empty;

    public string? GeneratedSql { get; set; }
    public DataTable? ReportData { get; set; }
    public string? ErrorMessage { get; set; }
    public long ExecutionTimeMs { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(UserPrompt))
        {
            ErrorMessage = "Unesite pitanje prije pokretanja izvještaja.";
            return Page();
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            // 1. LLM korak: Generisanje SQL-a putem Ollama modela
            GeneratedSql = await _ollamaService.GenerateSqlQueryAsync(UserPrompt);

            // 2. Sigurnosni korak: Validacija SQL upita
            var (isValid, validationError) = _sqlValidator.Validate(GeneratedSql);
            if (!isValid)
            {
                ErrorMessage = $"Validacija nije prošla: {validationError}";
                return Page();
            }

            // 3. Izvršavanje validiranog upita koji je vratio Ollama
            ReportData = await _dbReportService.ExecuteQueryAsync(GeneratedSql);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Greška prilikom obrade: {ex.Message}";
        }
        finally
        {
            stopwatch.Stop();
            ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
        }

        return Page();
    }
}