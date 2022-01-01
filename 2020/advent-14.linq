<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

var lines = AoC.GetLines().ToArray();

var instrs = lines.Select(instr.Parse).ToArray();

Dictionary<long,long> mem = new();
string mask = "";

for (int i = 0; i < instrs.Count(); i++)
{
	switch (instrs[i])
	{
		case mask(string val): 
			mask = val;
			break;
		case mem(long loc, long val):
			long bit = 1L;
			foreach (var ch in mask.Reverse())
			{
				if (ch == '0') val &= ~bit;
				if (ch == '1') val |= bit;
				
				bit <<= 1;
			}
			mem[loc] = val;
			break;
	}
}

mem.Values.Sum().Dump("Part 1");

mem = new();
mask = "";

for (int i = 0; i < instrs.Count(); i++)
{
	switch (instrs[i])
	{
		case mask(string val): 
			mask = val;
			break;
		case mem(long loc, long val):
			long bit = 1L;
			long flotsize = 1 << mask.Count(x => x == 'X');
			foreach (var ch in mask.Reverse())
			{
				if (ch == '1') loc |= bit;
				bit <<= 1;
			}

			for (long f = 0; f < flotsize; f++)
			{
				bit = 1L;
				long floc = f;
				foreach (var ch in mask.Reverse())
				{
					if (ch == 'X')
					{
						if (floc % 2 == 1)
						{
							loc |= bit;
						}
						else
						{
							loc &= ~bit;
						}
						floc /= 2;
					}
					bit <<= 1;
				}
				mem[loc] = val;
			}
			break;
	}
}

mem.Values.Sum().Dump("Part 2");


record instr
{
	public static instr Parse(string str)
	{
		if (str.StartsWith("mask"))
		{
			return new mask(str.Split(" = ")[1]);
		}
		else
		{
			return str.Extract<mem>();
		}
	}
}
record mask(string val) : instr;
record mem(long loc, long val) : instr
{
	public const string REGEXTRACT_REGEX_PATTERN = @"mem\[(\d+)\] = (\d+)";
};