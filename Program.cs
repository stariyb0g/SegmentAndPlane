// Клас для роботи з відрізками на площині
using System;
using System.IO;

public class Segment
{
    // Властивості
    public (double X, double Y) StartPoint { get; private set; }
    public (double X, double Y) EndPoint { get; private set; }

    // Конструктор
    public Segment((double X, double Y) startPoint, (double X, double Y) endPoint)
    {
        if (startPoint == endPoint)
            throw new ArgumentException("Start and end points cannot be the same.");
        StartPoint = startPoint;
        EndPoint = endPoint;
    }

    // Введення координат
    public void InputFromConsole()
    {
        Console.WriteLine("Enter the start point (X Y):");
        var startInput = Console.ReadLine()?.Split();
        Console.WriteLine("Enter the end point (X Y):");
        var endInput = Console.ReadLine()?.Split();

        if (startInput == null || endInput == null || startInput.Length != 2 || endInput.Length != 2)
            throw new FormatException("Invalid input format.");

        StartPoint = (double.Parse(startInput[0]), double.Parse(startInput[1]));
        EndPoint = (double.Parse(endInput[0]), double.Parse(endInput[1]));
    }

    // Виведення координат
    public void OutputToConsole()
    {
        Console.WriteLine($"Start Point: ({StartPoint.X}, {StartPoint.Y})");
        Console.WriteLine($"End Point: ({EndPoint.X}, {EndPoint.Y})");
    }

    // Довжина відрізка
    public double Length()
    {
        return Math.Sqrt(Math.Pow(EndPoint.X - StartPoint.X, 2) + Math.Pow(EndPoint.Y - StartPoint.Y, 2));
    }

    // Середина відрізка
    public (double X, double Y) MidPoint()
    {
        return ((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2);
    }

    // Масштабування
    public Segment Scale(double factor)
    {
        var newEndX = StartPoint.X + (EndPoint.X - StartPoint.X) * factor;
        var newEndY = StartPoint.Y + (EndPoint.Y - StartPoint.Y) * factor;
        return new Segment(StartPoint, (newEndX, newEndY));
    }

    // Запис у файл
    public void WriteToFile(string filePath)
    {
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"{StartPoint.X} {StartPoint.Y}");
            writer.WriteLine($"{EndPoint.X} {EndPoint.Y}");
        }
    }

    // Зчитування з файлу
    public void ReadFromFile(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        {
            var startCoords = reader.ReadLine()?.Split();
            var endCoords = reader.ReadLine()?.Split();

            if (startCoords == null || endCoords == null || startCoords.Length != 2 || endCoords.Length != 2)
                throw new FormatException("Invalid file format.");

            StartPoint = (double.Parse(startCoords[0]), double.Parse(startCoords[1]));
            EndPoint = (double.Parse(endCoords[0]), double.Parse(endCoords[1]));
        }
    }

    // Перевірка на паралельність осі X
    public bool IsParallelToXAxis()
    {
        return StartPoint.Y == EndPoint.Y;
    }

    // Кут нахилу до осі X
    public double AngleToXAxis()
    {
        return Math.Atan2(EndPoint.Y - StartPoint.Y, EndPoint.X - StartPoint.X) * (180 / Math.PI);
    }
}

// Головна програма
class Program
{
    static void Main()
    {
        var segment = new Segment((0, 0), (1, 1));
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Input coordinates");
            Console.WriteLine("2. Output coordinates");
            Console.WriteLine("3. Calculate length");
            Console.WriteLine("4. Find midpoint");
            Console.WriteLine("5. Scale segment");
            Console.WriteLine("6. Check if parallel to X-axis");
            Console.WriteLine("7. Calculate angle to X-axis");
            Console.WriteLine("8. Write to file");
            Console.WriteLine("9. Read from file");
            Console.WriteLine("10. Exit");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    segment.InputFromConsole();
                    break;
                case "2":
                    segment.OutputToConsole();
                    break;
                case "3":
                    Console.WriteLine($"Length: {segment.Length()}");
                    break;
                case "4":
                    var mid = segment.MidPoint();
                    Console.WriteLine($"Midpoint: ({mid.X}, {mid.Y})");
                    break;
                case "5":
                    Console.WriteLine("Enter scale factor:");
                    double factor = double.Parse(Console.ReadLine() ?? "1");
                    var scaledSegment = segment.Scale(factor);
                    Console.WriteLine("Scaled segment:");
                    scaledSegment.OutputToConsole();
                    break;
                case "6":
                    Console.WriteLine(segment.IsParallelToXAxis() ? "Segment is parallel to X-axis" : "Segment is not parallel to X-axis");
                    break;
                case "7":
                    Console.WriteLine($"Angle to X-axis: {segment.AngleToXAxis()} degrees");
                    break;
                case "8":
                    Console.WriteLine("Enter file path:");
                    var writePath = Console.ReadLine();
                    segment.WriteToFile(writePath);
                    Console.WriteLine("Segment written to file.");
                    break;
                case "9":
                    Console.WriteLine("Enter file path:");
                    var readPath = Console.ReadLine();
                    segment.ReadFromFile(readPath);
                    Console.WriteLine("Segment read from file.");
                    break;
                case "10":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}
