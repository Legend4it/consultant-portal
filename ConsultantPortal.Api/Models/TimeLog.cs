namespace ConsultantPortal.Api.Models;

public class TimeLog
{
    public string? id { get; set; }
    public string? UserId { get; set; }
    public string? Client { get; set; }
    public string? Date { get; set; }
    public double Hours { get; set; }
    public string? Description { get; set; }
}
