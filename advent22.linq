<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();
//lines.Dump();

//lines = @"cut 5".Split('\n');

long decksize = 10007;
long pos = 2019;

foreach (var line in lines)
{
	var words = line.Split(' ');
	switch (words[0])
	{
		case "cut":
			pos = (pos - long.Parse(words.Last()) + decksize) % decksize;
			//pos.Dump();
			break;
		case "deal" when words[1] == "into":
			pos = decksize - pos - 1;
			//pos.Dump();
			break;
		case "deal" when words[1] == "with":
			pos = (pos * long.Parse(words.Last())) % decksize;
			//pos.Dump();
			break;
	}
}

// 15299 is too high.
// 52089 is too high.
// 2014 is too high.

pos.Dump("Part 1");

long inverse(long a, long n)
{
	long t = 0, newt = 1;
	long r = n, newr = a;
	while (newr != 0)
	{
		long quotient = r / newr;
		(t, newt) = (newt, t - quotient * newt);
		(r, newr) = (newr, r - quotient * newr);
	}

	if (r > 1) throw new Exception("a is not invertible");

	if (t < 0) t = t + n;
	return t;
}

//decksize = 119315717514047L;
pos = 1538;

foreach (var line in lines.Reverse())
{
	var words = line.Split(' ');
	switch (words[0])
	{
		case "cut":
			pos = (pos + long.Parse(words.Last()) + decksize) % decksize;
			//pos.Dump();
			break;
		case "deal" when words[1] == "into":
			pos = decksize - pos - 1;
			//pos.Dump();
			break;
		case "deal" when words[1] == "with":
			var inv = inverse(long.Parse(words.Last()), decksize);
			pos = (pos * inv) % decksize;
			//pos.Dump();
			break;
	}
}

pos.Dump("Part 2");