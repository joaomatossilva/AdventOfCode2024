// See https://aka.ms/new-console-template for more information

using System.Runtime.InteropServices;

var input = File.ReadAllText("input.txt").Split(Environment.NewLine)
    .Select(long.Parse)
    .ToArray();

// var total = input.Select(x => Next(x, 2000))
//     .Aggregate(0L, (a, b) => a + b);
//
// Console.WriteLine(total);
Dictionary<Sequence, long> sequences = new Dictionary<Sequence, long>();

foreach (var seller in input)
{
    Next(seller, 2000, sequences);
}

var topSequence = sequences.OrderByDescending(x => x.Value);
Console.WriteLine(topSequence.First().Value);

public record Sequence(int A, int B, int C, int D);

public partial class Program
{
    public static long Next(long number, int count, Dictionary<Sequence, long> sequences)
    {
        var singleSequences = new Dictionary<Sequence, long>();
        
        number = Next(number);
        var a = (int)number % 10;
        count--;
        number = Next(number);
        var b = (int)number % 10;
        count--;
        number = Next(number);
        var c = (int)number % 10;
        count--;
        number = Next(number);
        var d = (int)number % 10;
        count--;
        while (count > 0)
        {
            number = Next(number);
            var e = (int)number % 10;
            var sequence = new Sequence(b - a, c - b, d - c, e - d);

            //just save first
            if (!singleSequences.TryGetValue(sequence, out var single))
            {
                singleSequences.Add(sequence, e);
            }
            
            a = b;
            b = c;
            c = d;
            d = e;
            count--;
        };

        foreach (var sequence in singleSequences)
        {
            if (sequence.Key == new Sequence(-2, 1, -1, 3))
            {
                int afds = 0;
            }
            
            CollectionsMarshal.GetValueRefOrAddDefault(sequences, sequence.Key, out _)+=sequence.Value;
        }
        
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