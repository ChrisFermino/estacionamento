using Infrastructure.Models.DTO;

namespace Infrastructure.Models;

public class Parking : Base
{
    public string Plate { get; set; }

    public string Name { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime? CheckOutDate { get; set; }

    public int? HoursDuration { get; set; }

    public int? MinutesDuration { get; set; }

    public int? ChargedTime { get; set; }

    public decimal AmountCharged { get; set; }

    public bool IsInsideParking { get; set; }


    public Parking() { }
    public Parking(CheckInDTO checkInDTO)
    {
        Plate = checkInDTO.Plate;
        Name = checkInDTO.Name;
        CheckInDate = checkInDTO.CheckInDate;
        IsInsideParking = true;
    }
}
