namespace ParkingSlot.Entities;

public record Vehicles
{
    public string PoliceNumber { get; set; } = null!;
    public VehiclesTypeEnum Type { get; set; }
    public string Color { get; set; } = null!;
}
