<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

//#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"".GetLines();
#endif

long sum1 = 0;
long sum2 = 0;

foreach (var line in lines)
{
	var n = Parse(line);
	var m = Parse(line, withPrecedence: true);
	sum1 += n;
	sum2 += m;
}

sum1.Dump("Part 1");
sum2.Dump("Part 2");

void DoVal(long val, Stack<char> ops, Stack<long> vals, bool withPrecedence)
{
	var top = ops.Peek();
	if (top != '+' && top != '*')
		vals.Push(val);
	else
	{
		if (!withPrecedence)
			vals.Push(ops.Pop() switch { '+' => vals.Pop() + val, '*' => vals.Pop() * val });
		else
			if (ops.Peek() == '+')
		{
			ops.Pop();
			vals.Push(vals.Pop() + val);
		}
		else
		{
			vals.Push(val);
		}
	}
}

long Parse(ReadOnlySpan<char> line, bool withPrecedence = false){
	Stack<char> ops = new();
	Stack<long> vals = new();
	
	int loc = 0;
	
	ops.Push('(');
	
	while (loc < line.Length)
	{
		switch (line[loc])
		{
			case ' ': break;
			case '(':
				ops.Push('(');
				break;
			case ')':
				while(ops.Peek() != '(')
				{
					long val = vals.Pop();
					var top = ops.Peek();
					if (top != '+' && top != '*')
						vals.Push(val);
					else
						DoVal(val, ops, vals, false);
				}
				ops.Pop();
				DoVal(vals.Pop(), ops, vals, withPrecedence);
				break;
			case '+':
				ops.Push('+');
				break;
			case '*':
				ops.Push('*');
				break;
			default:
				{
					long val = line[loc] - '0';
					DoVal(val, ops, vals, withPrecedence);
				}
				break;
		}
		loc++;
	}
	
	while (vals.Count > 1)
	{
		long val = vals.Pop();
		vals.Push(ops.Pop() switch { '+' => vals.Pop() + val, '*' => vals.Pop() * val });
	}
	
	return vals.Pop();
}