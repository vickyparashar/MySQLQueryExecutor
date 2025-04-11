using System;
using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace MySqlMcpServer;

[McpServerToolType]
public static class MySqlTools
{
    [McpServerTool, Description("Execute any MySQL query and return result or error.")]
    public static async Task<string> ExecuteMySqlQuery(
        MySqlService mySqlService,
        [Description("The SQL query to execute")] string query)
    {
        try
        {
            var result = await mySqlService.ExecuteQueryAsync(query);
            return JsonSerializer.Serialize(result);
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
