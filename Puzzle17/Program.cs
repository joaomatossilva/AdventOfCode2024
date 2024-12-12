// See https://aka.ms/new-console-template for more information

string input = "233313312141413140211";

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

int lastBlock = blocks.Count - 1;
for (var i = 0; i < blocks.Count; i++)
{
    if (blocks[i] == -1)
    {
        var lastBlockId = GetFurthestBlock(ref lastBlock);
        if (lastBlockId == -1)
        {
            break; // finitto
        }

        if (i > lastBlock)
        {
            break;
        }
        
        blocks[i] = lastBlockId;
        blocks[lastBlock] = -1;
    }
}




for (int i = 0; i < blocks.Count; i++)
{
    Console.Write(blocks[i] == -1 ? "." : $"{i} * {blocks[i]},");
}
Console.WriteLine();

long acc = 0;
for (int i = 0; i < blocks.Count; i++)
{
    if(blocks[i] == -1)
        continue;
    
    acc += i * blocks[i];
}

Console.WriteLine(acc);


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