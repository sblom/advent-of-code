<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();

var hash = new Dictionary<string,int[]>();
string curguard = "";
DateTime curstart = DateTime.Now;

var events = from line in lines 
			let timeevent = line.Split(']') 
			select new {Time = DateTime.Parse(timeevent[0].Substring(1)), Event = timeevent[1].Substring(1)};

events = events.OrderBy(x=>x.Time);

foreach (var @event in events)
{
	var eventname = @event.Event;
	var time = @event.Time;

	if (@eventname.StartsWith("Guard #"))
	{
		var num = @eventname.Substring(7).Split(' ')[0];
		curguard = num;
	}
	if (@eventname == "falls asleep")
	{
		curstart = time;
	}
	if (@eventname == "wakes up")
	{
		if (hash.ContainsKey(curguard))
		{
			for (int i = curstart.Minute; i < time.Minute; i++)
			{
				hash[curguard][i]++;
			}
		}
		else
		{
			hash[curguard] = new int[60];
			for (int i = curstart.Minute; i < time.Minute; i++)
			{
				hash[curguard][i]++;
			}
		}
	}
}

//hash.Select(k => new { k.Key, Sum = k.Value.Sum()}).Dump();

//hash["2351"].Select((x, y) => new { x, y}).Dump();
hash["1871"].Select((x, y) => new { x, y}).Dump();

hash.Select(k => new {k.Key, Max = k.Value.Max()}).Dump();

