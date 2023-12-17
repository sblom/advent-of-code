<Query Kind="Statements">
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>RegExtract</Namespace>
</Query>

#load "..\Lib\Utils"

var lines = AoC.GetLines().Extract<(int lo,int hi,char ch,string s)>(@"(\d+)-(\d+) (.): (.*)");

(from line in lines
 let occurrences = line.s.Count(ch => ch == line.ch)
 where line.lo <= occurrences && occurrences <= line.hi
 select line).Count().Dump("Part 1");

int count = 0;

foreach (var line in lines)
{
	int checks = 0;
	
	if (line.s[line.lo - 1] == line.ch) checks++;
	if (line.s[line.hi - 1] == line.ch) checks++;
	
	if (checks == 1) count++;
}

count.Dump("Part 2");