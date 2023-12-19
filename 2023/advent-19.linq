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

var workflows = lines.GroupLines().First().Extract<(string,List<Step>)>(@"(\w+){((.)([<>])(\d+):(\w+),|(\w+))+").ToDictionary(x=> x.Item1, x=>x.Item2);
var parts = lines.GroupLines().Skip(1).First().Extract<Dictionary<char,int>>(@"\{((.)=(\d+),?)+\}");

List<Dictionary<char,int>> accepted = new();

foreach (var part in parts)
{
    string workflow = "in";
    while (true)
    {
        foreach (var rule in workflows[workflow])
        {
            if (rule.abs == "A")
            {
                accepted.Add(part);
                goto next_part;
            }
            else if (rule.abs == "R")
            {
                goto next_part;
            }
            else if (rule.abs != null)
            {
                workflow = rule.abs;
                goto next_workflow;
            }

            if (rule.test switch
            {
                '<' => part[rule.prop ?? ' '] < rule.val,
                '>' => part[rule.prop ?? ' '] > rule.val,
            })
            {
                if (rule.step.Length > 1)
                {
                    workflow = rule.step;
                    goto next_workflow;
                }
                else if (rule.step == "A")
                {
                    accepted.Add(part);
                    goto next_part;
                }
                else if (rule.step == "R")
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
        if (step.abs != null)
        {
            WalkTree(step.abs, cond + (cond != "" ? "," : "") + negcond);
        }
        else
        {
            WalkTree(step.step, cond + (cond != "" ? ",": "") + negcond + (cond != "" || negcond != "" ? "," : cond) + step.prop + step.test + step.val);
            negcond += (negcond != "" ? ",": "") + step.prop + (step.test == '>' ? "<=" : ">=") + step.val;
        }
    }
}

WalkTree("in","");

var rules = acc.Select(a => a.Extract<List<(char,string,int)>>(@"(([xmas])([><]=?)(\d+),*)+").OrderBy(x => "xmas".IndexOf(x.Item1)));

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

record Workflow(string name, List<Step> contitions);
record Step(char? prop, char? test, int? val, string step, string abs);
record Part {
    public int x {get;set;}
    public int m {get;set;}
    public int a {get;set;}
    public int s {get;set;}
}

#if CHECKED
}
#endif
