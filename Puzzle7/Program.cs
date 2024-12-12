// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

int lineLenght = 140;
string input = @"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX";

var lines = input.ReplaceLineEndings("");
var occurrences = 0;

for (int pos = 0; pos < lines.Length; pos++)
{
    var x = pos % lineLenght;
    var y = pos / lineLenght;
    
    if (lines[pos] == 'X' && HasOccurrence(x, y, out var count))
    {
        Console.WriteLine($"Found {count} occurences at {x},{y}");
        occurrences += count;
    }
}

bool HasOccurrence(int x, int y, out int count)
{
    var startChar = 'X';
    count =
        HasOccurrenceRecursive(startChar, x, y, 1, 0).ToInt() + //forward;
        HasOccurrenceRecursive(startChar, x, y, -1, 0).ToInt() + //backwards
        HasOccurrenceRecursive(startChar, x, y, 0, 1).ToInt() + //downwards
        HasOccurrenceRecursive(startChar, x, y, 0, -1).ToInt() + //upwards
        HasOccurrenceRecursive(startChar, x, y, 1, 1).ToInt() + // diagonal fw downwards
        HasOccurrenceRecursive(startChar, x, y, -1, 1).ToInt() + // diagonal bw downwards
        HasOccurrenceRecursive(startChar, x, y, 1, -1).ToInt() + // diagonal fw upward
        HasOccurrenceRecursive(startChar, x, y, -1, -1).ToInt(); // diagonal fw upward
    return count > 0;
}

bool HasOccurrenceRecursive(char charAt, int x, int y, int deltaX, int deltaY)
{
    if (charAt == '.')
    {
        //out of bounds;
        return false;
    }
    
    var nextChar = GetNextChar(charAt);
    if (nextChar == '!')
    {
        // finished the sequence
        return true;
    }

    x += deltaX;
    y += deltaY;

    var thisChar = GetCharAt(x, y);
    if (thisChar == nextChar)
    {
        return HasOccurrenceRecursive(thisChar, x, y, deltaX, deltaY);
    }

    return false;
}

char GetNextChar(char c)
{
    switch (c)
    {
        case 'X':
            return 'M';
        case 'M':
            return 'A';
        case 'A':
            return 'S';
        case 'S':
            return '!';
        default:
            throw new Exception("Invalid character");
    }
}

char GetCharAt(int x, int y)
{
    if (x < 0 || y < 0 || x >= lineLenght)
    {
        return '.';
    }
    
    int pos = x + y * lineLenght;
    if (pos >= lines.Length)
    {
        return '.';
    }
    
    return lines[pos];
}

Console.WriteLine(occurrences);

public static class ToIntExt
{
    public static int ToInt(this bool value)
    {
        return value ? 1 : 0;
    }
}