<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>static UserQuery</Namespace>
</Query>

#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
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

var nums = lines.Select(Parse).ToList();

Snailfish acc = null;
	
foreach (var num in nums)
{	
	if (acc != null)
	{
		Add(acc,num);
		Check(acc);

		bool reduced = false;

		acc.head.ToString().Dump();

		do
		{
			reduced = false;
			while (Explode(acc)) reduced = true;
			Check(acc);
			if (Split(acc)) reduced = true;
			Check(acc);
			acc.head.ToString().Dump();
		} while (reduced);
	}
	else
	{
		acc = num;
	}
}

acc.ToString().Dump();

bool Check(Snailfish fish)
{
	Debug.Assert(fish.head.prev is null);
	LLC ch = null;
	for (ch = fish.head; ch != fish.tail; ch = ch.next)
	{
		Debug.Assert(ch.next.prev == ch);
	}
	Debug.Assert(ch.next == null);
	
	return true;
}

Snailfish Parse(string fish)
{
	return fish.Aggregate(new Snailfish(), (fish, ch) =>
		 {
			 if (fish.head == null)
			 {
				 fish.head = new LLC { ch = ch };
				 fish.tail = fish.head;
			 }
			 else
			 {
				 fish.tail.next = new LLC { prev = fish.tail, ch = ch };
				 fish.tail = fish.tail.next;
			 }

			 return fish;
		 });
}

bool Explode(Snailfish fish)
{
	fish.head.ToString().Dump();
	int depth = 0;
	
	for (LLC ch = fish.head; ch != null; ch = ch.next)
	{
		depth += ch.ch switch { '[' => 1, ']' => -1, _ => 0 };
		if (depth == 5)
		{
			var start = ch;
			var L = start.next;
			var Lnum = L.ch - '0';
			var R = L.next.next;
			var Rnum = R.ch - '0';
			var end = R.next;
			
			for (LLC nextleft = start; nextleft != null; nextleft = nextleft.prev)
			{
				if (nextleft.ch is >= '0' and <= '9')
				{
					nextleft.ch = (char)(nextleft.ch + Lnum);
					break;
				}
			}

			for (LLC nextright = end; nextright != null; nextright = nextright.next)
			{
				if (nextright.ch is >= '0' and <= '9')
				{
					nextright.ch = (char)(nextright.ch + Rnum);
					break;
				}
			}

			var zero = new LLC {ch = '0', prev = start.prev, next = end.next };
			start.prev.next = zero;
			end.next.prev = zero;
	
			fish.head.ToString().Dump();
			
			return true;
		}
	}
	
	fish.head.ToString().Dump();
	
	return false;
}

bool Split(Snailfish fish)
{
	for (LLC ch = fish.head; ch != null; ch = ch.next)
	{
		if (ch.ch is >= (char)('9' + 1) and < '[' || (ch.ch >= '[' && (ch.prev?.ch ?? ' ') is '[' or ','))
		{
			var val = ch.ch - '0';
			val = Math.DivRem(val, 2, out int rem);
			var splitFish = Parse($"[{val},{val + rem}]");

			ch.prev.next = splitFish.head;
			splitFish.head.prev = ch.prev;
			splitFish.tail.next = ch.next;
			ch.next.prev = splitFish.tail;
			
			return true;
		}
	}

	return false;
}

void Add(Snailfish left, Snailfish right)
{
	left.head = new LLC { ch = '[', next = left.head };
	left.head.next.prev = left.head;
	
	left.tail.next = new LLC { ch = ',', prev = left.tail };
	left.tail = left.tail.next;
	
	left.tail.next = right.head;
	right.head.prev = left.tail;
	
	right.tail.next = new LLC { ch = ']', prev = right.tail};
	right.tail = right.tail.next;
	
	left.tail = right.tail;
}

class Snailfish {
	public LLC head;
	public LLC tail;
}

class LLC {
	public LLC prev;
	public LLC next;
	public char ch;

	public override string ToString()
	{
		return ch.ToString() + next?.ToString() ?? ""; 
	}
}