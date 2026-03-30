using System.Data;

public class TicketRepository : ITicketRepository
{
    private readonly IDbHelper                  _dbHelper;
    private readonly ILogger<TicketRepository>  _logger;

    public TicketRepository(IDbHelper dbHelper, ILogger<TicketRepository> logger)
    {
        _dbHelper = dbHelper;
        _logger   = logger;
    }

    public async Task<int> DeleteTicketAsync(string ticketNumber)
    {
        try
        {
            var parameters = new[]
            {
                SqlParamHelper.Param("@TicketNumber", ticketNumber, SqlDbType.NVarChar)
            };
            return await _dbHelper.ExecuteNonQueryAsync(
                "sp_deleteTicketAsPerTicketNumber", parameters, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository Error in DeleteTicketAsync");
            throw;
        }
    }
}
