<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Frozen</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Collections.ObjectModel</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Wintellect.PowerCollections</Namespace>
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

var modules = lines.Extract<((char? type, string name) module, List<string> outputs)>(@"^(([%&])?([a-z]+)) -> (([a-z]+),? ?)+$").ToDictionary(x => x.module.name, x => (x.module.name, x.module.type, x.outputs));

long ans = 1;

// The overall structure of the network is 4 branches, each with a chain
// of flip-flops that count to a prime in the neighborhood of 4000.

// For each of those 4 branches:
foreach (var output in modules["broadcaster"].outputs)
{
    // Find the conjunction in this branch that looks for the right values
    // from each flip-flop in the branch.
    var conj = modules[output].outputs.Single(x => modules[x].type == '&');
    
    var cur = output;
    var val = 1;
    var acc = 0;

    // Walk to the end of the chain of flip-flops. They represent the lowest order bit to the highest.
    while (cur != null)
    {
        // If the flip-flop sends an output _to_ the conjuction, the target flip-flop state is a 1 bit.
        // (If the flip-flop gets an input _from_ the conjunction, the target flip-flop state is a 0 bit.)
        if (modules[cur].outputs.Contains(conj))
        {
            acc += val;
        }        
        val <<= 1;
        
        // Move to the next flip-flop in this branch's chain.
        cur = modules[cur].outputs.SingleOrDefault(x => modules[x].type == '%');
    }
    
    // Multiply the answer by our current accumulator, and move on to the next branch.
    ans *= acc;
}

ans.Dump2();

#if CHECKED
}
#endif
