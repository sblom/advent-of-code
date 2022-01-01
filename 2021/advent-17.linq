<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>static UserQuery</Namespace>
</Query>

//#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
var targetMin = (60, -171);
var targetMax = (94, -136);

#else
var lines = @"9C0141080250320F1802104A08".GetLines();
var targetMin = (20, -10);
var targetMax = (30, -5);


#endif



var ys = new List<int>();
var xs = new List<int>();

int y = 0;

bool doesY(int vy)
{
	for (int n = 0; (y = vy * n + n * (n + 1) / 2) != 0 || true; n++)
	{
		if (y > -targetMin.Item2)
		{
			return false;
			
		}
		if (y >= -targetMin.Item2)
		{
			ys.Add(vy);
			return true;
		}
	}
	return false;
}

bool doesX(int vx)
{
	int lvx = vx;
	for (int x = 0; x <= targetMax.Item1 && lvx > 0; x += lvx, lvx = lvx > 0 ? lvx - 1 : lvx)
	{
		if (x >= targetMin.Item1)
		{
			xs.Add(vx);
			return true;
		}
	}
	return false;
}

for (int vy = 1; vy <= 171; vy++)
{	
	doesY(vy);
}

for (int vx = 1; vx < 95; vx++)
{
	doesX(vx);
}

for (int vy = 0; vy > -171; vy--)
{
	int lvy = vy;
	for (y = 0; y >= targetMin.Item2; y -= lvy, lvy--)
	{
		if (y <= targetMax.Item2)
		{
			ys.Add(vy);
			break;
		}
	}
}

(ys.Count() * xs.Count()).Dump();

bool doesXY(int vx, int vy)
{
	for (int x = 0, y = 0; x <= targetMax.Item1 && y >= targetMin.Item2; x += vx, y += vy, vy--, vx = vx > 0 ? vx - 1 : vx)
	{
		if (x >= targetMin.Item1 && y <= targetMax.Item2) return true;
	}
	
	return false;
}

(from vx in Enumerable.Range(-200,400) from vy in Enumerable.Range(-200,400) where doesXY(vx, vy) select (vx,vy)).Count().Dump();