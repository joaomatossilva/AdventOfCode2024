// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var input = @"Register A: 729
Register B: 0
Register C: 0

Program: 0,1,5,4,3,0";

var registerMatches = Registers().Matches(input);
var instructionMatches = Instrucions().Matches(input.Split(Environment.NewLine + Environment.NewLine)[1]);

Dictionary<char, long> registers = new Dictionary<char, long>();
foreach (Match registerMatch in registerMatches)
{
    registers.Add(registerMatch.Groups[1].Value[0], long.Parse(registerMatch.Groups[2].Value));
}
var instructions = instructionMatches.Select(instruction => int.Parse(instruction.Groups[1].Value)).ToArray();

var output = new List<long>();
int instructionPointer = 0;
do
{
    Execute(instructions[instructionPointer], instructions[instructionPointer +1], output, ref instructionPointer);
} while (instructionPointer < instructions.Length);
Console.WriteLine();
Console.WriteLine(string.Join(",", output));

void Execute(long instruction, long operand, List<long> output, ref int instructionPointer)
{
    bool incrementPointer = true;
    switch (instruction)
    {
        case 0:
            adv();
            break;
        case 1:
            bxl();
            break;
        case 2:
            bst();
            break;
        case 3:
            incrementPointer = jnz(ref instructionPointer);
            break;
        case 4:
            bxc();
            break;
        case 5:
            @out();
            break;
        case 6:
            bdv();
            break;
        case 7:
            cdv();
            break;
    }
    
    if(incrementPointer)
        instructionPointer += 2;

    long combo()
    {
        switch (operand)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                return operand;
            case 4:
                return registers['A'];
            case 5:
                return registers['B'];
            case 6:
                return registers['C'];
        }
        throw new Exception();
    }
    
    void adv()
    {
        var a = registers['A'];
        var b = (long) Math.Pow(2, combo());
        registers['A'] = a / b;
    }
    void bxl()
    {
        var a = registers['B'];
        registers['B'] = a ^ operand;
    }
    void bst()
    {
        registers['B'] = combo() % 8;
    }
    bool jnz(ref int instructionPointer)
    {
        var a = registers['A'];
        if (a != 0)
        {
            instructionPointer = (int)operand;
            return false;
        }
        return true;
    }
    void bxc()
    {
        registers['B'] ^= registers['C'];
    }
    void @out()
    {
        var outValue = combo() % 8;
        output.Add(outValue);
    }
    void bdv()
    {
        var a = registers['A'];
        var b = (long) Math.Pow(2, combo());
        registers['B'] = a / b;
    }
    void cdv()
    {
        var a = registers['A'];
        var b = (long) Math.Pow(2, combo());
        registers['C'] = a / b;
    }
}

public partial class Program
{
    [GeneratedRegex("Register (A|B|C): (\\d+)", RegexOptions.Multiline)]
    public static partial Regex Registers();
    
    [GeneratedRegex(@",?(\d)", RegexOptions.Multiline)]
    public static partial Regex Instrucions();
}