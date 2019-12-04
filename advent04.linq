<Query Kind="Statements">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

int yes = 0;

for (int i = 146810; i <= 612564; i++)
{
	bool eq = false, incr = true;
	var str = i.ToString();
	int run = 0;
	for (int ii = 0; ii < str.Length - 1; ii++)
	{
		if (str[ii] == str[ii + 1])
		{
			run++;
		}
		else
		{
			if (run == 1)
			{
				run = 0;
				eq = true;
			}
			if (run > 1)
			{
				run = 0;
			}
		}
		if (str[ii] > str[ii + 1])
		{
			incr = false;
		}
	}
	if (run == 1)
	{
		eq = true;
	}
	if (eq && incr) 
		++yes;
next_pass:;
}

yes.Dump("Part 2");