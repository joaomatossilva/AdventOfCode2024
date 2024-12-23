// See https://aka.ms/new-console-template for more information

var input = File.ReadAllText("input.txt").Split(Environment.NewLine)
    .Select(long.Parse)
    .ToArray();

var total = input.Select(x => Next(x, 2000))
    .Aggregate(0L, (a, b) => a + b);

Console.WriteLine(total);

public partial class Program
{
    public static long Next(long number, int count)
    {
        while (count > 0)
        {
            number = Next(number);
            count--;
        };
        
        return number;
    }
    
    public static long Next(long number)
    {
        number = Prune(Mix(number * 64));
        number = Prune(Mix(number / 32));
        return Prune(Mix(number * 2048));
        
        long Mix(long x)
        {
            return number ^ x;
        }

        long Prune(long x)
        {
            return x % 16777216;
        }
    }
}