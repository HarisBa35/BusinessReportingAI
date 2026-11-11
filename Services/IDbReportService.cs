// Services/IDbReportService.cs
using System.Data;

namespace BusinessReportingAI.Services;

public interface IDbReportService
{
    Task<DataTable> ExecuteQueryAsync(string sql);
}