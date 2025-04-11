using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

public class MySqlService
{
    private readonly string _connectionString;

    public MySqlService()
    {
        // Move this to config/env later for security
        _connectionString = "Server=172.20.146.242;Database=demo;User ID=test;Password=Test@1234#Secure!987;";
    }

    public async Task<List<Dictionary<string, object?>>> ExecuteQueryAsync(string query)
    {
        var results = new List<Dictionary<string, object?>>();

        try
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object?>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
                }
                results.Add(row);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(BuildFullExceptionMessage(ex));
        }

        return results;
    }

    private string BuildFullExceptionMessage(Exception ex)
    {
        var sb = new StringBuilder();
        sb.AppendLine("An error occurred while executing the MySQL query:");
        sb.AppendLine($"Message: {ex.Message}");
        sb.AppendLine($"Type: {ex.GetType().FullName}");
        sb.AppendLine($"Stack Trace: {ex.StackTrace}");

        Exception? inner = ex.InnerException;
        while (inner != null)
        {
            sb.AppendLine("\n--- Inner Exception ---");
            sb.AppendLine($"Message: {inner.Message}");
            sb.AppendLine($"Type: {inner.GetType().FullName}");
            sb.AppendLine($"Stack Trace: {inner.StackTrace}");
            inner = inner.InnerException;
        }

        return sb.ToString();
    }
}
