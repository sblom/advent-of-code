<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();

var pipes = lines.Select(line => new[] { line.Split('/') }.Select(x => (int.Parse(x[0]), int.Parse(x[1]))).First());

var sortedPipes = pipes.Select(x => x.Item1 > x.Item2 ? (x.Item2, x.Item1) : (x.Item1, x.Item2)).OrderBy(x => x).GroupBy(x => x);