using System;
using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace MySqlMcpServer;

[McpServerToolType]
public static class MySqlTools
{
    [McpServerTool, Description(
        "Execute any MySQL query using the method. Supports all SQL operation types: " +
        "DML (INSERT, UPDATE, DELETE), DDL (CREATE, DROP, ALTER), DCL (GRANT, REVOKE), and DQL (SELECT). " +
        "Always runs with a test environment setup. Returns either query result or error.")]
    public static async Task<string> ExecuteMySqlQuery(
        MySqlService mySqlService,
        [Description(
            "Provide any valid SQL query to execute (DML, DDL, DCL, or DQL). " +
            "This will be run using the test authentication method in a safe, test-scoped environment.")]
        string query)
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
