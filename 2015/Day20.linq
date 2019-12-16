<Query Kind="Statements" />

var input = int.Parse((await AoC.GetLinesWeb()).First());

for (int i = 1; ; i++)
{
	long total = 0;
	for (int f = 1; f < Math.Sqrt(i); f++)
	{
		if (i % f == 0)
		{
			total += f * 10;
			total += (i / f) * 10;
		}
	}
	if (i == (int)Math.Sqrt(i) * (int)Math.Sqrt(i))
	{
		total += (int)Math.Sqrt(i) * 10;
	}
	
	if (total >= input)
	{
		i.Dump("Part 1");
		break;
	}
}

for (int i = 1; ; i++)
{
	long total = 0;
	for (int f = 1; f < Math.Sqrt(i); f++)
	{
		if (i % f == 0)
		{
			if (i / f <= 50)
			{
				total += f * 11;
			}
			if (f <= 50)
			{
				total += (i / f) * 11;
			}
		}
	}
	if (i == (int)Math.Sqrt(i) * (int)Math.Sqrt(i))
	{
		if ((int)Math.Sqrt(i) <= 50)
		{
			total += (int)Math.Sqrt(i) * 11;
		}
	}

	if (total >= input)
	{
		i.Dump("Part 2");
		break;
	}
}