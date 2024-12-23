namespace ParkingSlot.Entities;

public record ParkingSlots: Vehicles
{
    public int SlotNumber { get; set; }
}
