<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

var seats = AoC.GetLines().Select(x => x.ToArray()).ToArray();

var newseats = seats.Select(row => row.ToArray()).ToArray();

var dirs = (from dx in new[]{-1,0,1} from dy in new[]{-1,0,1} where dx != 0 || dy != 0 select (dx,dy)).ToArray();

do
{
	seats = newseats;
	newseats = seats.Select(row => row.ToArray()).ToArray();
	
	for (int y = 0; y < seats.Length; y++)
	{
		for (int x = 0; x < seats[0].Length; x++)
		{
			int neighbors = 0;
			foreach (var (dx, dy) in dirs)
			{
				neighbors += (y + dy, x + dx) switch
				{
					var (row, col) when row >= 0 && row < seats.Length && col >= 0 && col < seats[0].Length => seats[row][col] == '#' ? 1 : 0,
					_ => 0
				};
			}
			
			if (seats[y][x] == 'L' && neighbors == 0) newseats[y][x] = '#';
			else if (seats[y][x] == '#' && neighbors >= 4) newseats[y][x] = 'L';
			else newseats[y][x] = seats[y][x];
		}
	}
} while (!seats.Zip(newseats).All(z => z.Item1.Zip(z.Item2).All(q => q.Item1 == q.Item2)));

newseats.Sum(row => row.Count(ch => ch == '#')).Dump("Part 1");



seats = AoC.GetLines().Select(x => x.ToArray()).ToArray();
newseats = seats.Select(row => row.ToArray()).ToArray();

do
{
	seats = newseats;
	newseats = seats.Select(row => row.ToArray()).ToArray();

	for (int y = 0; y < seats.Length; y++)
	{
		for (int x = 0; x < seats[0].Length; x++)
		{
			int neighbors = 0;
			foreach (var (dx, dy) in dirs)
			{
				var (row, col) = (y + dy, x + dx);
				
				while (row >= 0 && row < seats.Length && col >= 0 && col < seats[0].Length)
				{					
					if (seats[row][col] == '#')
					{
						neighbors++;
						break;
					}
					if (seats[row][col] == 'L')
					{
						break;
					}
					
					(row, col) = (row + dy, col + dx);
				}
			}

			if (seats[y][x] == 'L' && neighbors == 0) newseats[y][x] = '#';
			else if (seats[y][x] == '#' && neighbors >= 5) newseats[y][x] = 'L';
			else newseats[y][x] = seats[y][x];
		}
	}
} while (!seats.Zip(newseats).All(z => z.Item1.Zip(z.Item2).All(q => q.Item1 == q.Item2)));

newseats.Sum(row => row.Count(ch => ch == '#')).Dump("Part 2");