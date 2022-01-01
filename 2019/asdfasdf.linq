<Query Kind="Statements">
  <Reference>&lt;ProgramFilesX64&gt;\Wolfram Research\Mathematica\12.1\SystemFiles\Links\NETLink\Wolfram.NETLink.dll</Reference>
  <Namespace>Wolfram.NETLink</Namespace>
  <Namespace>Wolfram.NETLink.Internal</Namespace>
  <Namespace>Wolfram.NETLink.UI</Namespace>
</Query>

#define USE_TEST_DATA


#region Preamble
#load "..\Lib\Utils"

int c1 = 0; int c2 = 0;

var part1 = new DumpContainer().Dump("Part 1");
var part2 = new DumpContainer().Dump("Part 2");

bool dump2 = false;

void Dump1(object o)
{
    part1.AppendContent(o);
    if (!dump2) System.Windows.Forms.Clipboard.SetText(o.ToString());
}

void Dump2(object o)
{
    part2.AppendContent(o);
    System.Windows.Forms.Clipboard.SetText(o.ToString());
    dump2 = true;
}

Util.CreateSynchronizationContext();
#endregion

#region Data
var lines =
#if !USE_TEST_DATA
await AoC.GetLinesWeb();
#else
AoC.GetLines(@"
1008169
29,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,41,x,x,x,37,x,x,x,x,x,653,x,x,x,x,x,x,x,x,x,x,x,x,13,x,x,x,17,x,x,x,x,x,23,x,x,x,x,x,x,x,823,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,19
");
#endif
#endregion

var time = int.Parse(lines.First());
var buses = lines.Skip(1).First().Split(",").Select(x => int.TryParse(x, out var bus) ? bus : 0).Where(x => x != 0).ToArray();

int minwait = int.MaxValue;
int busid = 0;

foreach (var bus in buses)
{
    int wait = (time / bus + 1) * bus - time;
    if (wait < minwait)
    {
        minwait = wait;
        busid = bus;
    }
}

Dump1(minwait * busid);

var buses2 = lines.Skip(1).First().Split(",").Select((x, i) => int.TryParse(x, out var bus) ? (bus, i) : (0, 0)).Where(x => x.Item1 != 0).ToArray().Dump();

var wolflang = $"ChineseRemainder[{{{string.Join(",", buses2.Select(x => -x.Item2))}}},{{{string.Join(",", buses2.Select(x => x.Item1))}}}]".Dump();



IKernelLink ml = MathLinkFactory.CreateKernelLink();

ml.WaitAndDiscardAnswer();

ml.EvaluateToInputForm(wolflang,100);
ml.WaitForAnswer();
Dump2(ml.GetString());
ml.Close();