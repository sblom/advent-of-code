<Query Kind="Program" />

async void Main()
{
	var lines = await AoC.GetLinesWeb();
	lines.First().Dump();

	var playercount = 426;
	var maxmarb = 72058;

	var players = new long[playercount];
	var circle = new List<int>(maxmarb + 1);

	circle.Add(0);
	var curmarb = 0;
	var curplayer = 0;
	var val = 1;

	void PrintCircle()
	{
		Util.RawHtml($"<pre>[{curplayer}]  {string.Join(" ", circle.Take(curmarb))} ({circle[curmarb]}) {string.Join(" ", circle.Skip(curmarb + 1))}</pre>").Dump();
	}

	while (val <= maxmarb)
	{
		if (val % 10000 == 0) (val, Util.ElapsedTime).ToString().Dump();
		if (val % 23 == 0)
		{
			players[curplayer] += val;
			players[curplayer] += circle[(curmarb + circle.Count - 7) % circle.Count];
			curmarb = (curmarb + circle.Count - 7) % circle.Count;
			circle.RemoveAt(curmarb);
		}
		else
		{
			curmarb = (curmarb + 2) % circle.Count;
			circle.Insert(curmarb, val);
		}

		curplayer = (curplayer + 1) % playercount;
		val++;
		//PrintCircle();
	}

	players.Max().Dump("Part 1");
	
	Node cur = new Node();
	cur.prev = cur; cur.next = cur;
	
	players = new long[playercount];
	curplayer = 0;
	val = 1;

	void PrintLinked()
	{
		var curval = cur;

		Console.Write($"({curval.val}) ");
		
		cur = cur.next;
				
		while (cur != curval)
		{
			Console.Write($"{cur.val} ");
			cur = cur.next;
		}
		
		Console.WriteLine();
	}

	while (val <= maxmarb * 100)
	{
		if (val % 10000 == 0) (val, Util.ElapsedTime).ToString().Dump();
		if (val % 23 == 0)
		{
			players[curplayer] += val;
			for (int i = 0; i < 7; i++)
			{
				cur = cur.prev;
			}
			players[curplayer] += cur.val;
			cur.next.prev = cur.prev;
			cur.prev.next = cur.next;
			cur = cur.next;
		}
		else
		{
			cur = cur.next;
			var @new = new Node {val = val, prev = cur, next = cur.next };
			cur.next.prev = @new;
			cur.next = @new;
			cur = @new;
		}

		curplayer = (curplayer + 1) % playercount;
		val++;
		//PrintLinked();
	}
	
	players.Max().Dump("Part 2");
}

class Node
{
	public Node next;
	public Node prev;
	public int val;
}