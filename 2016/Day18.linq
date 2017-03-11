<Query Kind="Statements">
  <NuGetReference>System.Interactive</NuGetReference>
  <Namespace>System.Linq</Namespace>
</Query>

var traprow = "......^.^^.....^^^^^^^^^...^.^..^^.^^^..^.^..^.^^^.^^^^..^^.^.^.....^^^^^..^..^^^..^^.^.^..^^..^^^..";

//traprow = ".^^.^.^^^^";
int rowcount = 400000;

char[] previous = new char[2 + traprow.Length];
previous[0] = '.';
previous[previous.Length - 1] = '.';

char[] current = (char[])previous.Clone();

int Count(char[] row)
{
	int count = 0;
	for (int i = 1; i < row.Length - 1; i++)
	{
		if (row[i] == '.') count++;
	}

	return count;
}

traprow.CopyTo(0, previous, 1, traprow.Length);
int c = Count(previous);

for (int i = 1; i < rowcount; i++)
{
	for (int n = 1; n < previous.Length - 1; n++)
	{
		if ((previous[n - 1] == '.' && previous[n + 1] == '^') ||
			(previous[n - 1] == '^' && previous[n + 1] == '.'))
		{
			current[n] = '^';
		}
		else
		{
			current[n] = '.';
		}
	}
	
	string.Join("",current.Skip(1).Take(traprow.Length));
	c += Count(current);
	
	if (i == 39) c.Dump("Part 1");
	
	(previous, current) = (current, previous);
}

c.Dump("Part 2");