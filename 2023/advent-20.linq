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
  <Namespace>System.Collections.ObjectModel</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
//#load "..\Lib\BFS"

//#define TEST
//#define CHECKED
//#define TRACE

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"broadcaster -> a
%a -> inv, con
&inv -> b
%b -> con
&con -> output".GetLines();
#endif

#if CHECKED
checked{
#endif

AoC._outputs = new DumpContainer[] {new(), new()};
Util.HorizontalRun("Part 1,Part 2",AoC._outputs).Dump();

#endregion

var modulerules = lines.Extract<((char? type, string name), List<string> output)>(@"^(([%&])?([a-z]+)) -> (([a-z]+),? ?)+$");

var moduleslist = modulerules.Select(x => (module: x.Item1, outputs: x.Item2, inputs: modulerules.Where(y => y.Item2.Contains(x.Item1.name)).Select(x => x.Item1.Item2)));

var modules = moduleslist.ToDictionary(x => x.module.name, x => x);
var messages = new Queue<(string from, string to, bool pulse)>();

var inputs = new Dictionary<string, Dictionary<string,bool>>();
foreach (var module in moduleslist)
{
    if (module.module.type == '&') inputs[module.module.name] = module.inputs.ToDictionary(x => x, x => false);
    if (module.module.type == '%') inputs[module.module.name] = new Dictionary<string, bool> { { "state", false } };
}

long low = 0, high = 0;

for (int i = 0; i < 1000; i++)
{
    messages.Enqueue(("", "broadcaster", false));
    
    while (messages.Any())
    {
        var message = messages.Dequeue();
        message.DumpTrace();

        if (message.pulse)
        {
            high++;
        }
        else
        {
            low++;
        }

        if (!modules.ContainsKey(message.to))
        {
            continue;
        }
        
        var module = modules[message.to];
        
        switch (module.module.type)
        {
            case '%': 
                if (message.pulse == false) {
                    inputs[message.to]["state"] = !inputs[message.to]["state"];
                    foreach (var output in module.outputs)
                    {
                        messages.Enqueue((module.module.name, output, inputs[message.to]["state"]));
                    }
                }
                break;
            case '&':
                inputs[message.to][message.from] = message.pulse;
                foreach (var output in module.outputs)
                {
                    messages.Enqueue((module.module.name, output, !inputs[message.to].Values.All(x => x)));
                }            
                break;
            default:
                foreach (var output in module.outputs)
                {
                    messages.Enqueue((module.module.name, output, message.pulse));
                }
                break;
        }
    }
}

low.Dump();
high.Dump();

(low * high).Dump1();

#if CHECKED
}
#endif
