// Services/IOllamaService.cs
namespace BusinessReportingAI.Services;

public interface IOllamaService
{
    Task<string> GenerateSqlQueryAsync(string userPrompt);
}