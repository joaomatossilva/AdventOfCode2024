// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

// string input = @"
// 3   4
// 4   3
// 2   5
// 1   3
// 3   9
// 3   3";

string input = @"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20";

string[] lines = input.Split(Environment.NewLine);

long accumulator = 0;

foreach (string line in lines)
{
    var numbers = GetNumbers().Matches(line);
    if (numbers.Count == 0) // not enough operators
    {
        continue;
    }
    
    var operators = new int[numbers.Count - 1];
    for (int i = 0; i < operators.Length; i++)
    {
        operators[i] = int.Parse(numbers[i + 1].Value);
    }
    
    var operation = new Operation(long.Parse(numbers[0].Value), operators);
    if (CheckOperation(operation))
    {
        accumulator += operation.result;
    }
}

Console.WriteLine($"Answer: {accumulator}");

bool CheckOperation(Operation operation1)
{
    long result = operation1.operands[0];
    return CheckOperationDeep(operation1, 1, '+', result) || CheckOperationDeep(operation1, 1, '*', result);
}

bool CheckOperationDeep(Operation operation1, int index1, char op, long acc)
{
    switch (op)
    {
        case '+':
            acc += operation1.operands[index1];
            break;
        case '*':
            acc *= operation1.operands[index1];
            break;
        default:
            throw new Exception($"Invalid operation: {op}");
    }
    
    // no more operands
    if (index1 >= operation1.operands.Length - 1)
    {
        return acc == operation1.result;
    }
    
    return CheckOperationDeep(operation1, index1 + 1, '+', acc) || CheckOperationDeep(operation1, index1 + 1, '*', acc);
}

record Operation(long result, int[] operands);

partial class Program
{
    [GeneratedRegex("(\\d+)+")]
    private static partial Regex GetNumbers();
}