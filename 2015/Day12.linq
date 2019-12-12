<Query Kind="Statements">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>System.Collections.Immutable</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

var lines = await AoC.GetLinesWeb();

var doc = JArray.Parse(string.Join("\n",lines));

int total(JToken fragment)
{
	switch (fragment)
	{
		case JObject o:
			if (o.Values().Any(v => v.Type == JTokenType.String && v.Value<string>() == "red"))
			{
				return 0;
			}
			return o.Children().Select(v => total(v)).Sum();
		case JArray a:
			return a.Children().Select(v => total(v)).Sum();
		case JProperty p:
			return total(p.Value);
		case JValue v:
			if (v.Type == JTokenType.Integer)
				return v.Value<int>();
			else
				return 0;
		default:
			return 0;
	}
}

//109528 is too high for Part 2
//42855 is too low

total(doc).Dump();