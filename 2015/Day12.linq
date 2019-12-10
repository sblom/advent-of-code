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
			return o.Values().Select(v => total(v)).Sum();
		case JArray a:
			return a.Values().Select(v => total(v)).Sum();
		case JProperty p:
			if (p.Type == JTokenType.Integer)
			{
				return p.Value<int>();
			}
			else if (p.Type == JTokenType.Property)
			{
				return total(p.Value);
			}
			else
			{
				return 0;
			}
		default:
			return 0;
	}
}

total(doc).Dump();