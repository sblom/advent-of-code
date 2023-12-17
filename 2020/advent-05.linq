<Query Kind="Statements">
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>RegExtract</Namespace>
</Query>

#load "..\Lib\Utils"

var lines = AoC.GetLines().ToArray();

var seats = lines.Select(x => x.Select(c => c switch {'F' => 0, 'B' => 1, 'L' => 0, 'R' => 1}).Aggregate(0, (a,n) => a * 2 + n)).OrderByDescending(n => n);

seats.First().Dump("Part 1");

seats.Skip(1).Zip(seats).Where(z => z.First != z.Second - 1).Select(z => (z.First + z.Second) / 2).Single().Dump("Part 2");