<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
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

Dictionary<string, Rule> rules = new();

var groups = lines.GroupLines();
rules["|"] = new Rule("\"|\"",rules);
foreach (var rule in groups.First())
{
	var parts = rule.Split(": ");
	rules[parts[0]] = new Rule(parts[1],rules);
}

var regex = new Regex("^" + rules["0"].ToString() + "$");

groups.Skip(1).First().Where(line => regex.IsMatch(line)).Count().Dump("Part 1");

foreach (var rule in groups.First())
{
	var parts = rule.Split(": ");
	rules[parts[0]] = new Rule(parts[1], rules);
}

rules["8"] = new Plus("",rules);
rules["11"] = new Nesting("",rules);

regex = new Regex("^" + rules["0"].ToString() + "$");

groups.Skip(1).First().Where(line => regex.IsMatch(line)).Count().Dump("Part 2");

public class Rule
{
	Dictionary<string, Rule> _rules;
	string _rule;
	string _toString;
	
	public Rule(string rule, Dictionary<string,Rule> rules)
	{
		_rule = rule;
		_rules = rules;
	}

	public override string ToString()
	{
		 if (_toString is null)
		{
			if (_rule.StartsWith('"'))
			{
				_toString = _rule.Substring(1, 1);
				return _toString;
			}
			
		 	var splits = _rule.Split(" ");
			
			_toString = "(" + string.Join("",splits.Select(sp => _rules[sp].ToString())) + ")";
		 }
		 
		 return _toString;
	}
}

public class Plus: Rule
{
	Dictionary<string, Rule> _rules;
	string _toString;

	public Plus(string rule, Dictionary<string, Rule> rules): base(rule, rules)
	{
		_rules = rules;		
	}

	public override string ToString()
	{
		if (_toString is null)
		{
			_toString = "(" + _rules["42"] + ")+";
		}

		return _toString;
	}
}

public class Nesting : Rule
{
	Dictionary<string, Rule> _rules;
	string _toString;

	public Nesting(string rule, Dictionary<string, Rule> rules): base(rule, rules)
	{
		_rules = rules;
	}

	public override string ToString()
	{
		if (_toString is null)
		{
			_toString = "(?'open'" + _rules["42"] + ")+(?'-open'" + _rules["31"] + ")+(?(open)(?!))";
		}

		return _toString;
	}
}