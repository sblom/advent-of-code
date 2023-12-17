<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
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

var decks = lines.GroupLines().Select(group => ImmutableQueue.CreateRange<int>(group.Skip(1).Select(int.Parse))).ToArray();
var originalDecks = decks.ToArray();

while (decks.All(deck => deck.Any()))
{
	decks[0] = decks[0].Dequeue(out var c1);
	decks[1] = decks[1].Dequeue(out var c2);
	
	if (c1 > c2)
	{
		decks[0] = decks[0].Enqueue(c1);
		decks[0] = decks[0].Enqueue(c2);
	}
	else
	{
		decks[1] = decks[1].Enqueue(c2);
		decks[1] = decks[1].Enqueue(c1);
	}
}

decks.Select(deck => deck.Select((c,i) => (deck.Count() - i) * c).Sum()).Sum().Dump("Part 1");

PlayGame(originalDecks);

int PlayGame(IImmutableQueue<int>[] decks, int depth = 0)
{
	HashSet<string> states = new();

	while (decks.All(deck => deck.Any()))
	{
		var state = string.Join("|", decks.Select(deck => string.Join(",", deck.Select(card => card.ToString()))));

		if (states.Contains(state)) return 0;

		states.Add(state);

		var cards = new[] { decks[0].Peek(), decks[1].Peek() };
		decks[0] = decks[0].Dequeue();
		decks[1] = decks[1].Dequeue();

		int winner;

		if (decks[0].Count() >= cards[0] && decks[1].Count() >= cards[1])
		{
			winner = PlayGame(new[] { ImmutableQueue.CreateRange(decks[0].Take(cards[0])), ImmutableQueue.CreateRange(decks[1].Take(cards[1])) }, depth + 1);
		}
		else
		{
			winner = cards[0] > cards[1] ? 0 : 1;
		}

		decks[winner] = decks[winner].Enqueue(cards[winner]);
		decks[winner] = decks[winner].Enqueue(cards[1 - winner]);
	}
	
	if (depth == 0)
	{
		decks.Select(deck => deck.Select((c,i) => (deck.Count() - i) * c).Sum()).Sum().Dump("Part 2");
	}
	
	return decks[0].Any() ? 0 : 1;
}