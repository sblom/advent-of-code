<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
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
var lines = @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]".GetLines();
#endif

var score = 0;

var scores = new List<long>();

foreach (var line in lines)
{
	var stack = ImmutableStack<char>.Empty;
	foreach (var ch in line)
	{
		switch (ch)
		{
			case '(':
			case '{':
			case '[':
			case '<':
				stack = stack.Push(ch);
				break;
			case ')':
				if (stack.Peek() != '(')
				{
					score += 3;
					goto nextline;
				}
				stack = stack.Pop();
				break;
			case '}':
				if (stack.Peek() != '{')
				{
					score += 1197;
					goto nextline;
				}
				stack = stack.Pop();
				break;

			case ']':
				if (stack.Peek() != '[')
				{
					score += 57;
					goto nextline;
				}
				stack = stack.Pop();
				break;

			case '>':
				if (stack.Peek() != '<')
				{
					score += 25137;
					goto nextline;
				}
				stack = stack.Pop();
				break;

		}
	}
	
	long linescore = 0;
	while (stack.Any())
	{
		linescore *= 5;
		linescore += stack.Peek() switch {
			'(' => 1,
			'[' => 2,
			'{' => 3,
			'<' => 4
		};
		stack = stack.Pop();
	}
	scores.Add(linescore);
nextline:;
}

score.Dump();

scores.OrderBy(s => s).ToList()[scores.Count / 2].Dump();