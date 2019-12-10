<Query Kind="Statements" />

var curqp = Util.CurrentQueryPath;
var dirname = Path.GetDirectoryName(curqp);
Directory.SetCurrentDirectory(dirname);
var inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt");
var input = File.ReadLines(inputName);

var nums = "97,167,54,178,2,11,209,174,119,248,254,0,255,1,64,190".Split(',').Select(x => int.Parse(x));

var list = Enumerable.Range(0,256).ToArray();

int loc = 0, skip = 0;

foreach (var len in lens)
{
	int x0 = loc, y0 = (loc + len - 1) % 256;
	
	for (int i = 0; i < 256; i++)
	{
		string str = list[i].ToString();
		if (i == loc)
			str = "[" + str + "]";
		if (i == x0)
			str = "(" + str;
		if (i == y0)
			str = str + ")";
			
		Console.Write(str + " ");
	}
	Console.WriteLine();
	
	for (int x = x0, y = y0; x != y && x != (y + 1) % 256; x = (x + 1) % 256, y = (y + 255) % 256)
	{
		(list[x], list[y]) = (list[y], list[x]);
	}

	for (int i = 0; i < 256; i++)
	{
		string str = list[i].ToString();
		if (i == loc)
			str = "[" + str + "]";
		if (i == x0)
			str = "(" + str;
		if (i == y0)
			str = str + ")";

		Console.Write(str + " ");
	}
	Console.WriteLine();


	loc = (loc + len + (skip++)) % 256;
}

(list[0] * list[1]).Dump();