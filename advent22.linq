<Query Kind="Statements">
  <Namespace>System.Numerics</Namespace>
</Query>

var lines = await AoC.GetLinesWeb();
//lines.Dump();

long decksize = 10007;
long pos = 2019;

//lines = @"deal with increment 7
//deal into new stack
//deal into new stack".Split('\n');
//decksize = 10;

var (a, b) = (1L, 0L);

foreach (var line in lines)
{
	var words = line.Split(' ');
	switch (words[0])
	{
		case "cut":
			b = (b - long.Parse(words.Last()) + decksize) % decksize;
			//pos.Dump();
			break;
		case "deal" when words[1] == "into":
			a = -a + decksize;
			b = decksize - b - 1;
			//pos.Dump();
			break;
		case "deal" when words[1] == "with":
			a = (a * long.Parse(words.Last())) % decksize;		
			b = (b * long.Parse(words.Last())) % decksize;
			//pos.Dump();
			break;
	}
	if (a < 0 || b < 0)
	{
		throw new Exception("Aaaaaaaaa");
	}
}

(a,b).ToString().Dump();

// 15299 is too high.
// 52089 is too high.
// 2014 is too high.

((a * pos + b) % decksize).Dump("Part 1");

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

decksize = 119315717514047L;
//pos = 1538;

pos = 2020;

(a, b) = (1L, 0L);

foreach (var line in lines.Reverse())
{
	var words = line.Split(' ');
	switch (words[0])
	{
		case "cut":
			b = (b + long.Parse(words.Last()) + decksize) % decksize;
			//pos.Dump();
			break;
		case "deal" when words[1] == "into":
			a = -a + decksize;
			b = decksize - b - 1;
			//pos.Dump();
			break;
		case "deal" when words[1] == "with":
			var inv = inverse(long.Parse(words.Last()), decksize);
			a = (long)((new BigInteger(a) * inv) % decksize);
			b = (long)((new BigInteger(b) * inv) % decksize);
			//pos.Dump();
			break;
	}
	if (a < 0 || b < 0)
	{
		words.Dump();
		throw new Exception("Aaaaaaaaa");
	}
}

var times = 101741582076661;

var p2 = (BigInteger.ModPow(a, times, decksize) * pos +
	 b * (BigInteger.ModPow(a, times, decksize) + decksize - 1)
	   * (BigInteger.ModPow(a - 1, decksize - 2, decksize))
	 + decksize) % decksize;

// 95064543565353 is too low.
p2.Dump("Part 2");