using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class MySqlService
{
    private readonly string _connectionString;

    public MySqlService()
    {
        // You can later move this to appsettings.json or environment variables
        _connectionString = "Server=172.20.146.242;Database=demo;User ID=test;Password=Test@1234#Secure!987;";
    }

    public async Task<List<Dictionary<string, object>>> ExecuteQueryAsync(string query)
    {
        var results = new List<Dictionary<string, object>>();

        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                row[reader.GetName(i)] = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
            }
            results.Add(row);
        }

        return results;
    }
}
