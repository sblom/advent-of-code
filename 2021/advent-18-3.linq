<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>static UserQuery</Namespace>
</Query>

//#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = File.ReadLines(@"C:\Users\sblom\Downloads\2021_18.txt");
#else
var lines = @"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]
[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]
[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]
[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]
[7,[5,[[3,8],[1,4]]]]
[[2,[2,2]],[8,[8,1]]]
[2,9]
[1,[[[9,3],9],[[9,0],[0,7]]]]
[[[5,[7,4]],7],1]
[[[[4,2],2],6],[8,7]]".GetLines();
#endif

snailfish acc = null;

foreach (var line in lines)
{
	var (snailfish, _) = ParseSnailfish(line.AsSpan());
	
	if (acc != null)
	{
		acc = new pair { left = acc, right = snailfish };
		
		bool reduced = false;

		do
		{
			reduced = false;
			while (ExplodeSnailfish(acc)) reduced = true;
			if (SplitSnailfish(acc)) reduced = true;
		} while (reduced);
	}
	else 
	{
		acc = snailfish;
	}	
}

Checksum(acc).Dump("Part 1");

var pairsij = from i in Enumerable.Range(0,lines.Count()) from j in Enumerable.Range(0,lines.Count()) where i != j select (i,j);

foreach (var pair in pairsij)
{
	var (s1, _) = ParseSnailfish(lines.ToArray()[pair.i]);
	var (s2, _) = ParseSnailfish(lines.ToArray()[pair.j]);

	acc = new pair { left = s1, right = s2 };

	bool reduced = false;

	do
	{
		reduced = false;
		while (ExplodeSnailfish(acc)) reduced = true;
		if (SplitSnailfish(acc)) reduced = true;
	} while (reduced);

	$"{pair.i} + {pair.j} == {Checksum(acc)}".Dump();
}



var pairs = from x1 in lines from x2 in lines where x1 != x2 select (x1, x2);

int max = 0;

foreach (var pair in pairs)
{
	var (s1, _) = ParseSnailfish(pair.x1);
	var (s2, _) = ParseSnailfish(pair.x2);

	acc = new pair { left = s1, right = s2 };

	bool reduced = false;

	do
	{
		reduced = false;
		while (ExplodeSnailfish(acc)) reduced = true;
		if (SplitSnailfish(acc)) reduced = true;
	} while (reduced);
	
	max = Math.Max(max, Checksum(acc));
}

max.Dump();

int Checksum(snailfish fish)
{
	return fish switch {
		pair(var left, var right) => 3 * Checksum(left) + 2 * Checksum(right),
		number(var val) => val
	};
}

(snailfish, int size) ParseSnailfish(ReadOnlySpan<char> span)
{
	if (span[0] is >= '0' and <= '9') return (new number { val = span[0] - '0' }, 1);
	else // if (span[0] is '[')
	{
		var (left,lsize) = ParseSnailfish(span[1..]);
		var (right, rsize) = ParseSnailfish(span[(1 + lsize + 1)..]);
		return (new pair { left = left, right = right}, 1 + lsize + 1 + rsize + 1);
	}
}

//snailfish ReduceSnailfish(snailfish fish)
//{
//	bool exploded = false;
//
//	do
//	{
//		(fish, exploded) = ExplodeSnailfish(fish);
//	} while (exploded);
//}

bool SplitSnailfish(snailfish fish)
{
	int depth = 0;
	Stack<(pair, snailfish, int)> stack = new Stack<(pair, snailfish, int)>();

	stack.Push((null, fish, 0));
	snailfish cur = null;
	pair par = null;
	
	while (stack.Any())
	{
		if (cur is null)
		{
			(par, cur, depth) = stack.Pop();
		}

		switch (cur)
		{
			case pair(var left, var right):
				depth++;
				par = (pair)cur;
				cur = left;
				stack.Push((par, right, depth));
				break;
			case number(var val):
				if (val >= 10)
				{
					if (par.left == cur)
					{
						par.left = new pair { left = new number { val = val / 2 }, right = new number { val = (val + 1) / 2 } };
					}
					else if (par.right == cur)
					{
						par.right = new pair { left = new number { val = val / 2 }, right = new number { val = (val + 1) / 2 } };
				}
					return true;
				}
				cur = null;
				break;
		}
	}

	return false;
}

bool ExplodeSnailfish(snailfish fish)
{
	int depth = 0;
	Stack<(pair,snailfish,int)> stack = new Stack<(pair,snailfish,int)>();

	stack.Push((null,fish,0));

	snailfish cur = null;
	pair par = null;
	number prev = null;
	int? next = null;
	
	bool exploded = false;
	
	while(stack.Any())
	{
		if (cur is null)
		{
			(par,cur,depth) = stack.Pop();
		}

		switch (cur)
		{
			case pair(var left, var right):
				depth++;
				if (!next.HasValue && depth == 5)
				{
					if (prev != null)
					{
						prev.val += (left as number).val;
					}
					next = (right as number).val;
					if (par.left == cur) par.left = new number { val = 0 };
					if (par.right == cur) par.right = new number { val = 0 };
					cur = null;
					exploded = true;
					continue;
				}
				par = (pair)cur;
				cur = left;
				stack.Push((par,right,depth));
				break;
			case number(var val):
				if (next != null)
				{
					(cur as number).val += next.Value;
					return true;
				}
//				(depth,val,prev).ToString().Dump();
				prev = (number)cur;				
				cur = null;
				break;
		}
	}
	
	return exploded;
}

//(snailfish fish, bool exploded) ExplodeSnailfish(snailfish fish)
//{
//	throw new NotImplementedException();
//}

class snailfish { }
class pair: snailfish
{
	public snailfish left;
	public snailfish right;
	public void Deconstruct(out snailfish left, out snailfish right) {
		left = this.left;
		right = this.right;
	}
	public override string ToString() => $"[{left.ToString()},{right.ToString()}]";
}
class number: snailfish
{
	public int val;
	public void Deconstruct(out int val)
	{
		val = this.val;
	}
	public override string ToString() => $"{val}";
}


