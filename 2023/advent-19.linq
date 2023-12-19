<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>Wintellect.PowerCollections</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
//#load "..\Lib\BFS"

//#define TEST
//#define CHECKED

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"px{a<2006:qkq,m>2090:A,rfg}
pv{a>1716:R,A}
lnx{m>1548:A,A}
rfg{s<537:gd,x>2440:R,A}
qs{s>3448:A,lnx}
qkq{x<1416:A,crn}
crn{x>2662:A,R}
in{s<1351:px,qqz}
qqz{s>2770:qs,m<1801:hdj,R}
gd{a>3333:R,R}
hdj{m>838:A,pv}

{x=787,m=2655,a=1222,s=2876}
{x=1679,m=44,a=2067,s=496}
{x=2036,m=264,a=79,s=2244}
{x=2461,m=1339,a=466,s=291}
{x=2127,m=1623,a=2188,s=1013}".GetLines();
#endif

#if CHECKED
checked{
#endif

AoC._outputs = new DumpContainer[] {new(), new()};
Util.HorizontalRun("Part 1,Part 2",AoC._outputs).Dump();

#endregion

Dictionary<string,List<Rule>> workflows = lines.GroupLines().First().Extract<(string workflow,List<Rule> rules)>(@"(\w+){(([^,]+),?)+}").ToDictionary(x=> x.workflow, x=>x.rules);

var parts = lines.GroupLines().Skip(1).First().Extract<Dictionary<char,int>>(@"\{((.)=(\d+),?)+\}");

List<Dictionary<char,int>> accepted = new();

foreach (var part in parts)
{
    string workflow = "in";
    while (true)
    {
        foreach (var rule in workflows[workflow])
        {
            if (rule is Absolute(Accept))
            {
                accepted.Add(part);
                goto next_part;
            }
            else if (rule is Absolute(Reject))
            {
                goto next_part;
            }
            else if (rule is Absolute(Workflow(var wf)))
            {
                workflow = wf;
                goto next_workflow;
            }

            if (rule switch
            {
                Conditional(Condition(var prop, '<', var val), var wf) => part[prop] < val,
                Conditional(Condition(var prop, '>', var val), var wf) => part[prop] > val,
                _ => false
            })
            {
                if (rule is Conditional(_,Workflow(var next)))
                {
                    workflow = next;
                    goto next_workflow;
                }
                else if (rule is Conditional(_,Accept))
                {
                    accepted.Add(part);
                    goto next_part;
                }
                else if (rule is Conditional(_,Reject))
                {
                    goto next_part;
                }
            }
        }
        next_workflow:;        
    }
    next_part:;
}

accepted.Sum(x => x.Values.Sum()).Dump1();

var acc = new List<string>();
var rej = new List<string>();

void WalkTree(string workflow, string cond)
{
    if (workflow == "A")
    {
        acc.Add(cond);
        return;
    }
    else if (workflow == "R")
    {
        rej.Add(cond);
        return;
    }
    
    var negcond = "";
    
    foreach (var step in workflows[workflow])
    {
        if (step is Absolute(var wf))
        {
            WalkTree(wf switch { Accept => "A", Reject => "R", Workflow(var next) => next, _ => throw new Exception() }, cond + (cond != "" ? "," : "") + negcond);
        }
        else if (step is Conditional(Condition(var prop, var test, var val), var nwf))
        {
            WalkTree(nwf switch { Accept => "A", Reject => "R", Workflow(var next) => next, _ => throw new Exception() }, cond + (cond != "" ? ",": "") + negcond + (cond != "" || negcond != "" ? "," : cond) + prop + test + val);
            negcond += (negcond != "" ? ",": "") + prop + (test == '>' ? "<=" : ">=") + val;
        }
    }
}

WalkTree("in","");

var rules = acc.Select(a => (a.Extract<List<(char,string,int)>>(@"(([xmas])([><]=?)(\d+),*)+") ?? new()).OrderBy(x => "xmas".IndexOf(x.Item1)));

long tot = 0;

foreach (var rule in rules)
{
    var groups = rule.GroupBy(x => x.Item1);
    
    long ruletot = 1;
    ruletot *= (long)Math.Pow(4000,(4 - groups.Count()));
    
    foreach (var group in groups)
    {
        long min = 1;
        long max = 4000;        
        foreach (var item in group)
        {
            var val = item.Item3;
            if (item.Item2[0] == '>')
            {
                if (item.Item2.Length == 1)
                {
                    val++;
                }
                if (val > min) min = val;
            }
            else
            {
                if (item.Item2.Length == 1)
                {
                    val--;
                }
                if (val < max) max = val;
            }
        }
        //(group.Key, min, max).Dump();
        if (max - min + 1 < 0) max = min;
        ruletot *= max - min + 1;
    }
    
    tot += ruletot;
}

tot.Dump2();

record Rule()
{
    public static Rule? Parse(string str)
    {
        if (str.Contains(":"))
            return str.Extract<Conditional>();
        else
            return str.Extract<Absolute>();
    }
}

record Absolute(Action step) : Rule
{
    public const string REGEXTRACT_REGEX_PATTERN = @"(.*)";
}
record Conditional(Condition cond, Action step) : Rule
{
    public const string REGEXTRACT_REGEX_PATTERN = @"((.)([<>])(\d+)):(\w+)";
}
record Condition(char prop, char test, int val);

record Action
{
    public static Action Parse(string str)
    {
        return str switch
        {
            "A" => new Accept(),
            "R" => new Reject(),
            _ => new Workflow(str)
        };
    }
}
record Accept : Action;
record Reject : Action;
record Workflow(string workflow) : Action;

#if CHECKED
}
#endif
