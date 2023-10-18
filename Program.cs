using System;
using System.Collections.Generic;

public interface IConverter<in T, out U>
{
    U Convert(T value);
}

public class StringToIntConverter : IConverter<string, int>
{
    public int Convert(string value)
    {
        return int.Parse(value);
    }
}

public class ObjectToStringConverter : IConverter<object, string>
{
    public string Convert(object value)
    {
        return value.ToString();
    }
}

public abstract class Animal
{
    public string Name { get; set; }

    public abstract void SayHello();
}

public class Dog : Animal
{
    public override void SayHello()
    {
        Console.WriteLine($"Woof! I'm {Name}.");
    }
}

public class Cat : Animal
{
    public override void SayHello()
    {
        Console.WriteLine($"Meow! I'm {Name}.");
    }
}

public class Calculator
{
    public static double Add(double x, double y)
    {
        return x + y;
    }

    public static double Subtract(double x, double y)
    {
        return x - y;
    }

    public static double Multiply(double x, double y)
    {
        return x * y;
    }

    public static double Divide(double x, double y)
    {
        if (y == 0)
        {
            throw new ArgumentException("Division by zero is not allowed.");
        }
        return x / y;
    }
}

public class Program
{
    public static U[] ConvertArray<T, U>(T[] inputArray, IConverter<T, U> converter)
    {
        U[] resultArray = new U[inputArray.Length];
        for (int i = 0; i < inputArray.Length; i++)
        {
            resultArray[i] = converter.Convert(inputArray[i]);
        }
        return resultArray;
    }

    public static void PerformActionsOnAnimals(List<Animal> animals, Func<Animal, object> action)
    {
        foreach (Animal animal in animals)
        {
            action(animal);
        }
    }

    public static double PerformOperation(double x, double y, Func<double, double, double> operation)
    {
        return operation(x, y);
    }

    public static void Main()
    {
        // Пример использования ConvertArray
        string[] stringArray = { "1", "2", "3" };
        object[] objectArray = { 4, 5, 6 };

        IConverter<string, int> stringToIntConverter = new StringToIntConverter();
        IConverter<object, string> objectToStringConverter = new ObjectToStringConverter();

        int[] intArray = ConvertArray(stringArray, stringToIntConverter);
        string[] stringResultArray = ConvertArray(objectArray, objectToStringConverter);

        Console.WriteLine("Converted int array:");
        foreach (int number in intArray)
        {
            Console.WriteLine(number);
        }

        Console.WriteLine("Converted string array:");
        foreach (string str in stringResultArray)
        {
            Console.WriteLine(str);
        }

        // Пример использования PerformActionsOnAnimals
        List<Animal> animals = new List<Animal>
        {
            new Dog { Name = "Buddy" },
            new Cat { Name = "Whiskers" }
        };

        Func<Animal, object> animalAction = animal =>
        {
            animal.SayHello();
            return null;
        };

        Console.WriteLine("Using Animal Action:");
        PerformActionsOnAnimals(animals, animalAction);

        // Пример использования PerformOperation
        double num1 = 10;
        double num2 = 5;

        Func<double, double, double> addFunc = Calculator.Add;
        Func<double, double, double> subtractFunc = Calculator.Subtract;
        Func<double, double, double> multiplyFunc = Calculator.Multiply;
        Func<double, double, double> divideFunc = Calculator.Divide;

        double result1 = PerformOperation(num1, num2, addFunc);
        double result2 = PerformOperation(num1, num2, subtractFunc);
        double result3 = PerformOperation(num1, num2, multiplyFunc);
        double result4 = PerformOperation(num1, num2, divideFunc);

        Console.WriteLine($"Addition Result: {result1}");
        Console.WriteLine($"Subtraction Result: {result2}");
        Console.WriteLine($"Multiplication Result: {result3}");
        Console.WriteLine($"Division Result: {result4}");
    }
}
