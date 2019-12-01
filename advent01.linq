<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();
//var lines = @"";

var rx = new Regex(@"");

var norms = from L in lines
            let rxr = rx.Match(L)
			select (rxr.Groups[""], rxr.Groups[""]);
			//select int.Parse(L);