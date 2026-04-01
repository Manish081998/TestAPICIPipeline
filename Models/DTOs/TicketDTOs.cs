public class DeleteTicketRequest
{
    public string TicketNumber { get; set; }
}

public class DeleteTicketResponse
{
    public string TicketNumber { get; set; }
}

public class InsertTicketDetialsRequest
{
    public string CaseNumber                 { get; set; }
    public string SoNumber                   { get; set; }
    public string SoldToName                 { get; set; }
    public string ControlNumber              { get; set; }
    public string FonNumber                  { get; set; }
    public string PlantCode                  { get; set; }
    public Guid   JobOrLineItemOpenModelType { get; set; }
    public string GlobalID                   { get; set; }
}

public class InsertTicketDetialsResponse
{
    public string CaseNumber { get; set; }
    public string SoNumber   { get; set; }
    public bool   Success    { get; set; }
}
