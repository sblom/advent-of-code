<Query Kind="Statements" />

var input = 366;

var list = new List<int>(2018){0};

var loc = 0;

for (int i = 1; i <= 2017; i++)
{
	loc = (loc + input) % list.Count + 1;
	list.Insert(loc, i);
}

list[list.IndexOf(2017)+1].Dump();

loc = 0;
var size = 1;
var val = 0;

for (int i = 1; i <= 50_000_000; i++)
{
	loc = (loc + input) % size + 1;
	size++;
	if (loc == 1) val = i;
}

val.Dump();