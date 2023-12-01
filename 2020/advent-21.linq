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
var lines = @"".GetLines();
#endif

var items = lines.Extract<(List<string> ingredients,List<string> allergens)>(@"(?:(\w+) )+\(contains (?:(\w+)(?:, |\)))+").Select(item => (ingredients: item.ingredients.ToHashSet(), allergens: item.allergens.ToHashSet())).ToArray();
var allergens = items.SelectMany(x => x.allergens).Distinct();
var ingredients = items.SelectMany(x => x.ingredients).Distinct();

var mapping = allergens.Select(allergen => (allergen, possibleIngredients: items.Where(item => item.allergens.Contains(allergen)).Aggregate(default(HashSet<string>),(x,y) => (x?.Intersect(y.ingredients).ToHashSet() ?? y.ingredients)))).ToDictionary(x => x.allergen, x => x.possibleIngredients).Dump();

var maybeAllergens = mapping.SelectMany(m => m.Value).Distinct();

while (mapping.Any(map => map.Value.Count > 1))
{
	var lockedIngredients = mapping.Where(map => map.Value.Count == 1).Select(x => x.Value.First()).ToHashSet();
	foreach (var kv in mapping)
	{
		if (kv.Value.Count > 1)
			kv.Value.ExceptWith(lockedIngredients);
	}	
}

items.Select(item => item.ingredients.Where(ingredient => !maybeAllergens.Contains(ingredient)).Count()).Sum().Dump("Part 1");

string.Join(",", mapping.OrderBy(kv => kv.Key).Select(kv => kv.Value.Single())).Dump("Part 2");