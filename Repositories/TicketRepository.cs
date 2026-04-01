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

    public async Task<int> InsertTicketDetialsAsync(InsertTicketDetialsRequest request)
    {
        try
        {
            var parameters = new[]
            {
                SqlParamHelper.Param("@CaseNumber",                 request.CaseNumber,                 SqlDbType.NVarChar),
                SqlParamHelper.Param("@SoNumber",                   request.SoNumber,                   SqlDbType.NVarChar),
                SqlParamHelper.Param("@SoldToName",                 request.SoldToName,                 SqlDbType.NVarChar),
                SqlParamHelper.Param("@ControlNumber",              request.ControlNumber,              SqlDbType.NVarChar),
                SqlParamHelper.Param("@FonNumber",                  request.FonNumber,                  SqlDbType.NVarChar),
                SqlParamHelper.Param("@PlantCode",                  request.PlantCode,                  SqlDbType.NVarChar),
                SqlParamHelper.Param("@JobOrLineItemOpenModelType", request.JobOrLineItemOpenModelType, SqlDbType.UniqueIdentifier),
                SqlParamHelper.Param("@GlobalID",                   request.GlobalID,                   SqlDbType.NVarChar)
            };
            return await _dbHelper.ExecuteNonQueryAsync(
                "sp_InsertTicketDetials", parameters, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository Error in InsertTicketDetialsAsync");
            throw;
        }
    }
}
