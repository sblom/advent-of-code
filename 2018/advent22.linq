<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var lines = await AoC.GetLinesWeb();
	
	var X = 1000;
	var Y = 10000;
	
	int[,] cave = new int[Y+1,X+1];

	int[,,] times = new int[3, Y+1, X+1];
	for (int i = 0; i <= Y; i++)
	{
		for (int j = 0; j <= X; j++)
		{
			for (int k = 0; k < 3; k++)
			{
				times[k,i , j] = int.MaxValue;
			}
		}
	}

	for (int i = 0; i <= Y; i++)
	{
		for (int j = 0; j <= X; j++)
		{
			if (i == 0 && j == 0)
			{
				cave[i,j] = 0;
			}
			if (i == 0)
			{
				cave[i,j] = j * 16807;
			}
			else if (j == 0)
			{
				cave[i,j] = i * 48271;
			}
			else
			{
				cave[i,j] = ((cave[i-1,j] + 8103) % 20183) * ((cave[i,j-1] + 8103) % 20183);
			}
		}
	}
	
	
	var total = 0;
	
	for (int i = 0; i <= 758; i++)
	{
		for (int j = 0; j <= 9; j++)
		{
			total += ((cave[i,j] + 8103) % 20183) % 3;
		}
	}
	
	total.Dump("Part 1");
	
	for (int i = 0; i <= Y; i++)
	{
		for (int j = 0; j <= X; j++)
		{
			cave[i,j] = ((cave[i, j] + 8103) % 20183) % 3;
		}
	}
	
	
	const int NEITHER = 0;
	const int TORCH = 1;
	const int GEAR = 2;
	
	const int ROCKY = 0;  // gear or torch
	const int WET = 1;    // gear or none
	const int NARROW = 2; // torch or none
	
	var dirs = new[]{(0,-1),(-1,0),(1,0),(0,1)};
	
	var frontier = new List<((int x, int y) loc, int equip, int time)> { ((0, 0), TORCH, 0), ((0,0), GEAR, 7) };
	
	while (frontier.Count > 0 && !(frontier.First().loc == (9,758) && frontier.First().equip == TORCH))
	{
		var cur = frontier.First();
		frontier.RemoveAt(0);
		
		foreach (var (dx,dy) in dirs)
		{
			var nextloc = (x: cur.loc.x + dx, y: cur.loc.y + dy);
			
			if (nextloc.x < 0 || nextloc.y < 0) continue;
			
			if (nextloc.x > X || nextloc.y > Y) continue;
			
			switch (cave[nextloc.y, nextloc.x])
			{
				case ROCKY:
					if (cur.equip != GEAR && cur.equip != TORCH) continue;
					else
					{					
						if (times[cur.equip,nextloc.x,nextloc.y] > cur.time + 1)
						{
							times[cur.equip,nextloc.x,nextloc.y] = cur.time + 1;
							frontier.PriorityInsert(((nextloc.x, nextloc.y), cur.equip, cur.time + 1));
						}
						if (times[(GEAR | TORCH) ^ cur.equip, nextloc.x, nextloc.y] > cur.time + 8)
						{
							times[(GEAR | TORCH) ^ cur.equip, nextloc.x, nextloc.y] = cur.time + 8;
							frontier.PriorityInsert(((nextloc.x, nextloc.y), (GEAR | TORCH) ^ cur.equip, cur.time + 8));
						}
					}
					break;
				case WET:
					if (cur.equip != GEAR && cur.equip != NEITHER) continue;
					else
					{
						if (times[cur.equip,nextloc.x,nextloc.y] > cur.time + 1)
						{
							times[cur.equip, nextloc.x, nextloc.y] = cur.time + 1;
							frontier.PriorityInsert(((nextloc.x, nextloc.y), cur.equip, cur.time + 1));
						}
						if (times[GEAR ^ cur.equip, nextloc.x, nextloc.y] > cur.time + 8)
						{
							times[GEAR ^ cur.equip, nextloc.x, nextloc.y] = cur.time + 8;
							frontier.PriorityInsert(((nextloc.x, nextloc.y), GEAR ^ cur.equip, cur.time + 8));
						}
					}
					break;
				case NARROW:
					if (cur.equip != TORCH && cur.equip != NEITHER) continue;
					else
					{
						if (times[cur.equip,nextloc.x,nextloc.y] > cur.time + 1)
						{
							times[cur.equip, nextloc.x, nextloc.y] = cur.time + 1;
							frontier.PriorityInsert(((nextloc.x, nextloc.y), cur.equip, cur.time + 1));
						}
						if (times[TORCH ^ cur.equip, nextloc.x, nextloc.y] > cur.time + 8)
						{
							times[TORCH ^ cur.equip, nextloc.x, nextloc.y] = cur.time + 8;
							frontier.PriorityInsert(((nextloc.x, nextloc.y), TORCH ^ cur.equip, cur.time + 8));
						}
					}
					break;
			}
		}
	}

	frontier.First().time.Dump("Part 2");

	//depth: 8103
	//target: 9,758
}

// Define other methods and classes here
public static class Extensions
{
	public static int Dist((int x,int y) loc)
	{
		return Math.Abs(9 - loc.x) + Math.Abs(758 - loc.x);
	}
	
	public static void PriorityInsert(this List<((int x, int y) loc, int equip, int time)> frontier, ((int x, int y) loc, int equip, int time) val)
	{
		int i;
		for (i = 0; i < frontier.Count; i++)
		{
			if (val.time > frontier[i].time)
			{
				continue;
			}
			else if (val.time == frontier[i].time && Dist(val.loc) > Dist(frontier[i].loc))
			{
				continue;
			}
			else
			{
				break;
			}
		}
		frontier.Insert(i, val);
	}
}