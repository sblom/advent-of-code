<Query Kind="Statements">
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>RegExtract</Namespace>
</Query>

#load "..\Lib\Utils"

var lines = AoC.GetLines().ToArray();

int linenum = 0;

int cpassports = 0;
int vpassports = 0;

var fields = new Dictionary<string,string>();
var validations = new Dictionary<string,string>
{
	["byr"] = "19[2-9][0-9]|200[0-2]",
	["iyr"] = "201[0-9]|2020",
	["eyr"] = "202[0-9]|2030",
	["hgt"] = "1[5-8][0-9]cm|19[0-3]cm|59in|6[0-9]in|7[0-6]in",
	["hcl"] = "#[0-9a-f]{6}",
	["ecl"] = "amb|blu|brn|gry|grn|hzl|oth",
	["pid"] = "^\\d{9}$",
};

while (linenum < lines.Length)
{
	if (string.IsNullOrWhiteSpace(lines[linenum]))
	{
		if (validations.Keys.All(f => fields.ContainsKey(f)))
		{
			cpassports++;
			if (validations.All(kvp => Regex.IsMatch(fields[kvp.Key],kvp.Value))) vpassports++;
		}
				
		fields.Clear();
	}
	else
	{
		var newfields = lines[linenum].Split(" ");
		foreach (var newfield in newfields)
		{
			var kv = newfield.Split(":");
			fields[kv[0]] = kv[1];
		}
	}
	
	linenum++;
}

if (fields.Any())
{
	if (validations.Keys.All(f => fields.ContainsKey(f)))
	{
		cpassports++;
		if (validations.All(kvp => Regex.IsMatch(fields[kvp.Key], kvp.Value))) vpassports++;
	}

	fields.Clear();
}

cpassports.Dump("Part 1");
vpassports.Dump("Part 2");