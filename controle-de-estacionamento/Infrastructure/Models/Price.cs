namespace Infrastructure.Models;

public class Price : Base
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal StartTimeValue { get; set; }

    public decimal AdditionalFeePerHour { get; set; }

    public bool IsActive { get; set; }
}
