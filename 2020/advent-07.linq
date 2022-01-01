<Query Kind="Statements">
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>RegExtract</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

var bagSpecs = AoC.GetLines().Extract<(string,List<(int,string)>?)>(@"([\w\s]+) bags contain(\s(\d+) ([\w\s]+) bags?[,.])*").ToDictionary(x => x.Item1, x => x.Item2);

var containsShinyGold = new BFS<string>("shiny gold", from => bagSpecs.Where(spec => spec.Value.Any(sp => sp.Item2 == from)).Select(x => x.Key), _ => false, _ => true);

(containsShinyGold.Search().Count() - 1).Dump("Part 1");

int NumBagsInside(string bagColor)
{
	return 1 + bagSpecs[bagColor].Select(x => x.Item1 * NumBagsInside(x.Item2)).Sum();
}

(NumBagsInside("shiny gold") - 1).Dump("Part 2");