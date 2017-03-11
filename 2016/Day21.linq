<Query Kind="Statements" />

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var lines = File.ReadLines("advent21.txt");

var swappos = new Regex(@"swap position (?<X>\d+) with position (?<Y>\d+)");
var swapltr = new Regex(@"swap letter (?<X>\w) with letter (?<Y>\w)");
var reverse = new Regex(@"reverse positions (?<X>\d+) through (?<Y>\d+)");
var rotate = new Regex(@"rotate (?<dir>left|right) (?<X>\d+) step");
var move = new Regex(@"move position (?<X>\d+) to position (?<Y>\d+)");
var rotatebased = new Regex(@"rotate based on position of letter (?<X>\w)");

var input = "abcdefgh".ToCharArray();

foreach (var line in lines)
{
	switch (line)
	{
		case string L when swappos.IsMatch(L):
			var match = swappos.Match(L);
			var (x, y) = (input[int.Parse(match.Groups["X"].Value)], input[int.Parse(match.Groups["Y"].Value)]);
			input[int.Parse(match.Groups["X"].Value)] = y; input[int.Parse(match.Groups["Y"].Value)] = x;
			break;
		case string L when swapltr.IsMatch(L):
			var match2 = swapltr.Match(L);		
			input = input.Select(ch => ch == match2.Groups["X"].Value[0] ? match2.Groups["Y"].Value[0] : (ch == match2.Groups["Y"].Value[0] ? match2.Groups["X"].Value[0] : ch)).ToArray();
			break;
		case string L when reverse.IsMatch(L):
			var match3 = reverse.Match(L);
			int x2 = int.Parse(match3.Groups["X"].Value);
			int y2 = int.Parse(match3.Groups["Y"].Value);
			input = input.Take(x2).Concat(input.Skip(x2).Take(y2-x2+1).Reverse()).Concat(input.Skip(y2+1)).ToArray();
			break;
		case string L when rotate.IsMatch(L):
			var match4 = rotate.Match(L);
			var dir = match4.Groups["dir"].Value;
			var x3 = int.Parse(match4.Groups["X"].Value);
			if (dir == "right") input = input.Skip(input.Count() - x3).Take(x3).Concat(input.Take(input.Count() - x3)).ToArray();
			else input = input.Skip(x3).Take(input.Count() - x3).Concat(input.Take(x3)).ToArray();
			break;
		case string L when move.IsMatch(L):
			var match5 = move.Match(L);
			var list = input.ToList();
			char ch1 = list[int.Parse(match5.Groups["X"].Value)];
			list.RemoveAt(int.Parse(match5.Groups["X"].Value));
			list.Insert(int.Parse(match5.Groups["Y"].Value),ch1);
			input = list.ToArray();
			break;
		case string L when rotatebased.IsMatch(L):
			var match6 = rotatebased.Match(L);
			var ch2 = match6.Groups["X"].Value[0];
			var list2 = input.ToList();
			var loc2 = list2.IndexOf(ch2);
			input = input.Skip(input.Count() - loc2).Concat(input.Take(input.Count() - loc2)).ToArray();
			input = input.Skip(input.Count() - 1).Concat(input.Take(input.Count() - 1)).ToArray();
			if (loc2 >= 4) input = input.Skip(input.Count() - 1).Concat(input.Take(input.Count() - 1)).ToArray();
			break;
		default:
			break;
	}
}

string.Join("",input).Dump();