// See https://aka.ms/new-console-template for more information

string input = "23331331214141314021";

List<int> blocks = new List<int>();

bool isSpace = false;
int id = 0;
foreach(char letter in input)
{
    var length = letter - '0';

    for (var i = 0; i < length; i++)
    {
        if(!isSpace)
            blocks.Add(id);
        else
            blocks.Add(-1);
    }

    if (!isSpace)
    {
        id++;
    }

    isSpace ^= true;
}

foreach (var block in blocks)
{
    Console.Write(block == -1 ? "." : block.ToString());
}
Console.WriteLine();
//
// int lastBlock = blocks.Count - 1;
// for (var i = 0; i < blocks.Count; i++)
// {
//     if (blocks[i] == -1)
//     {
//         var lastBlockId = GetFurthestBlock(ref lastBlock);
//         if (lastBlockId == -1)
//         {
//             break; // finitto
//         }
//
//         if (i > lastBlock)
//         {
//             break;
//         }
//         
//         blocks[i] = lastBlockId;
//         blocks[lastBlock] = -1;
//     }
// }

int firstFreeSpace = 0;

for (int i = blocks.Count - 1; i > 0; i--)
{
    var blockId = blocks[i];
    if (blockId == -1)
    {
        continue;
    }
    
    var start = GetStart(i);
    var lenght = i - start + 1;

    var freePos = GetFirstSpaceBiggerThan(lenght);
    if (freePos >= 0 && freePos < start)
    {
        for (int j = 0; j < lenght; j++)
        {
            blocks[j + freePos] = blocks[i];
            blocks[j + start] = -1;
        }
    }
    
    i = start;
    
    // for (int a = 0; a < blocks.Count; a++)
    // {
    //     Console.Write(blocks[a] == -1 ? "." : $"({blocks[a]})");
    // }
    // Console.WriteLine();
}




// for (int i = 0; i < blocks.Count; i++)
// {
//     Console.Write(blocks[i] == -1 ? "." : $"({blocks[i]})");
// }
// Console.WriteLine();

long acc = 0;
for (int i = 0; i < blocks.Count; i++)
{
    if(blocks[i] == -1)
        continue;
    
    acc += i * blocks[i];
}

Console.WriteLine(acc);


int GetStart(int index)
{
    var blockId = blocks[index];
    while (index > 0 && blockId == blocks[index -1])
    {
        index--;
    }

    return index;
}

int GetFirstSpaceBiggerThan(int lenght)
{
    int foundLenght = 0;
    int foundStart = -1;
    for (int i = 0; i < blocks.Count; i++)
    {
        if (blocks[i] == -1)
        {
            foundStart = foundStart < 0 ? i : foundStart;
            foundLenght++;

            if (foundLenght >= lenght)
            {
                return foundStart;
            }
        }
        else
        {
            foundLenght = 0;
            foundStart = -1;
        }
    }
    
    return foundStart;
}

int GetFurthestBlock(ref int pos)
{
    do
    {
        if (blocks[pos] != -1)
        {
            return blocks[pos];
        }

        pos--;
    } while (pos > 0);

    return -1;
}