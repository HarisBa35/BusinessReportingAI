// Services/DbReportService.cs
using System.Data;
using Microsoft.Data.SqlClient;

namespace BusinessReportingAI.Services;

public class DbReportService : IDbReportService
{
    private readonly string _connectionString;

    public DbReportService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Konekcijski string nije pronađen.");
    }

    public async Task<DataTable> ExecuteQueryAsync(string sql)
    {
        var dataTable = new DataTable();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand(sql, connection))
            {
                // Timeout u sekundama radi sigurnosti
                command.CommandTimeout = 30;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }
        }

        return dataTable;
    }
}