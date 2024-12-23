using ParkingSlot.Entities;

namespace ParkingSlot.Services;

public class ParkingSlotService
{
    private ParkingSlots?[] ParkingSlots;

    public ParkingSlotService()
    {
        ParkingSlots = [];
    }

    public bool IsParkingSlotDefined()
    {
        return ParkingSlots.Length != 0;
    }

    // create parking size
    public void CreateParkingLot(int sizeParkLot)
    {
        ParkingSlots = new ParkingSlots[sizeParkLot];
        Console.WriteLine($"Created a parking lot with {sizeParkLot} slots");
    }

    //Park a vehicle
    public int Park(VehiclesTypeEnum type, string color, string policeNumber)
    {
        ParkingSlots vehicle = new()
        {
            Type = type,
            Color = color,
            PoliceNumber = policeNumber,
        };
        for (int i = 0; i < ParkingSlots.Length; i++)
        {
            if (ParkingSlots[i] != null)
                continue;

            vehicle.SlotNumber = i + 1;
            ParkingSlots[i] = vehicle;
            return vehicle.SlotNumber;
        }

        return -1;
    }

    // Leave Parking
    public bool Leave(int slotNumber)
    {
        slotNumber -= 1;
        if (slotNumber == -1)
            return false;
        if (slotNumber > ParkingSlots.Length - 1)
            return false;

        ParkingSlots[slotNumber] = null;
        return true;
    }

    public void Status()
    {
        Console.WriteLine("Slot\tNo.\tType\tRegistration No Color");
        foreach (var record in ParkingSlots)
        {
            if (record != null)
            {
                string vehicleType = record.Type == VehiclesTypeEnum.Motor ? "Motor" : "Mobil";
                Console.WriteLine(
                    $"{record.SlotNumber}\t{record.PoliceNumber}\t{vehicleType}\t{record.Color}"
                );
            }
        }
    }

    // valid Type
    public VehiclesTypeEnum? TranslateVehicleToEnum(string input)
    {
        if (
            Enum.TryParse(typeof(VehiclesTypeEnum), input, true, out var result)
            && Enum.IsDefined(typeof(VehiclesTypeEnum), result)
        )
        {
            return (VehiclesTypeEnum)result; // Explicit cast to the enum type
        }
        return null;
    }

    // count type Vehicle
    public int CountTypeVehicle(VehiclesTypeEnum type)
    {
        return ParkingSlots.Count(record => record?.Type == type);
    }

    public List<ParkingSlots?> FindGroupByModulo(string input = "odd")
    {
        List<ParkingSlots?> data = ParkingSlots
            .Where(
                (record) =>
                {
                    var policeNumber = record?.PoliceNumber.Split('-');
                    if (policeNumber?.Length != 3)
                        return false;

                    var number = int.Parse(policeNumber![1]);
                    if (input == "odd")
                    {
                        return number % 2 == 1;
                    }
                    return number % 2 == 0;
                }
            )
            .ToList();
        return data;
    }

    public List<ParkingSlots?> FindByColor(string color)
    {
        var data = ParkingSlots
            .Where(record =>
                string.Equals(record?.Color, color, StringComparison.OrdinalIgnoreCase)
            )
            .ToList();
        return data;
    }

    public void PrintPoliceNumber(List<ParkingSlots?> data)
    {
        if (data.Count == 0)
        {
            Console.WriteLine("Not found");
            return;
        }
        Console.Write($"{data[0]?.PoliceNumber!}");
        for (int i = 1; i < data.Count; i++)
        {
            var vehicle = data[i];
            Console.Write($", {vehicle?.PoliceNumber!}");
        }
        Console.WriteLine();
    }

    public void PrintSlotNumber(List<ParkingSlots?> data)
    {
        if (data.Count == 0)
        {
            Console.WriteLine("Not found");
            return;
        }
        Console.Write($"{data[0]?.SlotNumber!}");
        for (int i = 1; i < data.Count; i++)
        {
            var vehicle = data[i];
            Console.Write($", {vehicle?.SlotNumber!}");
        }
        Console.WriteLine();
    }

    public int? FindSlotNumberByPoliceNumber(string policeNumber){
        return ParkingSlots.Where(record => record?.PoliceNumber == policeNumber).FirstOrDefault()?.SlotNumber;
    }
}
