using ParkingSlot.Entities;
using ParkingSlot.Services;

namespace ParkingSlot;

class Program
{
    public static readonly ParkingSlotService _service = new();

    static void Main(string[] ss)
    {
        Console.WriteLine("#==============================#");
        Console.WriteLine("# Welcome to My Parking System #");
        Console.WriteLine("#==============================#\n");

        Console.WriteLine("You can use by this following commands:\n");
        Console.WriteLine("0. Create parking space:\n#create_parking_lot [parking space]\n");
        Console.WriteLine(
            "1. Park your vehicle:\n#park [police number] [ vehicle color] [Mobil/Motor]\n"
        );
        Console.WriteLine("2. Leave your vehicle:\n#leave [parking number]\n");
        Console.WriteLine("3. See parking:\n#status\n");
        Console.WriteLine("4. Count type behicle:\n#type_of_vehicles [Mobil/Motor]\n");
        Console.WriteLine("5. Find vehicle with odd plate:\n#registration_numbers_for_vehicles_with_odd_plate\n");
        Console.WriteLine("6. Find vehicle with even plate:\n#registration_numbers_for_vehicles_with_even_plate\n");
        Console.WriteLine("7. Find vehicle by color:\n#registration_numbers_for_vehicles_with_color\n");
        Console.WriteLine("8. Find slot parking by color:\n#slot_numbers_for_vehicles_with_color\n");
        Console.WriteLine("9. Find slot parking by plate:\n#slot_number_for_registration_number\n");

        bool start = true;
        do
        {
            Console.WriteLine("\nEnter your command:");
            string? input = Console.ReadLine();
            if (input != null)
            {
                string[] args = input.Split(' ');
                string command = args[0];
                switch (command)
                {
                    case "create_parking_lot":
                        if (!ValidArgs(args, 2))
                            break;
                        HandleCreateParkSpace(args[1]);

                        break;

                    case "park":
                        if (!ValidArgs(args, 4))
                            break;
                        HandleParkVehicle(args);

                        break;

                    case "status":
                        if (!ValidArgs(args, 1))
                            break;
                        HandleViewStatus();

                        break;

                    case "leave":
                        if (!ValidArgs(args, 2))
                            break;
                        HandleLeave(args[1]);

                        break;

                    case "type_of_vehicles":
                        if (!ValidArgs(args, 2))
                            break;
                        HandleCountTypeVehicle(args[1]);
                        break;
                    case "registration_numbers_for_vehicles_with_odd_plate":
                        HandleFindGroupModuloPlate("odd");
                        break;
                    case "registration_numbers_for_vehicles_with_even_plate":
                        HandleFindGroupModuloPlate("even");
                        break;
                    case "registration_numbers_for_vehicles_with_color":
                        if (!ValidArgs(args, 2))
                            break;
                        HandleFindByColor(args[1]);
                        break;

                    case "slot_numbers_for_vehicles_with_color":
                        if (!ValidArgs(args, 2))
                            break;
                        HandleSlotNumberFindByColor(args[1]);
                        break;
                    case "slot_number_for_registration_number":
                        if (!ValidArgs(args, 2))
                            break;
                        HandleSlotNumberFindByPoliceNumber(args[1]);
                        break;
                    case "quit":
                    case "q":
                    case "exit":
                        start = false;
                        break;
                    default:
                        Console.WriteLine("Command not supported");
                        break;
                }
            }
        } while (start);
    }

    public static bool ValidArgs(string[] args, int nArgs)
    {
        var result = args.Length == nArgs;
        if (!result)
        {
            Console.WriteLine("Please fullfill the parameters");
        }
        return result;
    }

    public static void HandleParkVehicle(string[] args)
    {
        string policeNumber = args[1];
        string color = args[2];

        // is type valid
        VehiclesTypeEnum? type = _service.TranslateVehicleToEnum(args[3]);
        if (type == null)
        {
            Console.WriteLine("Invalid vehicle type!");
            return;
        }

        if (!_service.IsParkingSlotDefined())
        {
            Console.WriteLine("What Parking ?! Define it first!");
            return;
        }

        int index = _service.Park(type ?? new(), color, policeNumber);
        if (index == -1)
        {
            Console.WriteLine("Sorry, parking lot is full");
            return;
        }

        Console.WriteLine($"Allocated slot number: {index}");
    }

    public static void HandleCreateParkSpace(string input)
    {
        int parkSpace = int.Parse(input);
        _service.CreateParkingLot(parkSpace);
    }

    public static void HandleViewStatus()
    {
        _service.Status();
    }

    public static void HandleLeave(string input)
    {
        int parkNumber = int.Parse(input);
        if (!_service.Leave(parkNumber))
        {
            Console.WriteLine($"Failed to leave slot: {parkNumber}");
            return;
        }
        Console.WriteLine($"Slot number {parkNumber} is free");
    }

    public static void HandleCountTypeVehicle(string input)
    {
        VehiclesTypeEnum? type = _service.TranslateVehicleToEnum(input);
        if (type == null)
        {
            Console.WriteLine("Invalid vehicle type!");
            return;
        }
        Console.WriteLine(_service.CountTypeVehicle(type ?? new()));
    }

    public static void HandleFindGroupModuloPlate(string input = "odd")
    {
        var data = _service.FindGroupByModulo(input);
        _service.PrintPoliceNumber(data);
    }

    public static void HandleFindByColor(string color)
    {
        var data = _service.FindByColor(color);
        _service.PrintPoliceNumber(data);
    }

    public static void HandleSlotNumberFindByColor(string color)
    {
        var data = _service.FindByColor(color);
        _service.PrintSlotNumber(data);
    }

    public static void HandleSlotNumberFindByPoliceNumber(string policeNumber)
    {
        var data = _service.FindSlotNumberByPoliceNumber(policeNumber);
        if (data == null)
        {
            Console.WriteLine("Not found");
        }
        else
        {
            Console.WriteLine(data);
        }
    }
}
