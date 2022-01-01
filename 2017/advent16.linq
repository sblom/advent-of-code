<Query Kind="Statements" />

var lines = AoC.GetLines();

var things = "abcdefghijklmnop".ToCharArray();

//var instrs = lines.First().Split(',').Dump();
//
//foreach (var instr in instrs)
//{
//	if (instr[0] == 's')
//	{
//		var num = int.Parse(string.Join("", instr.Skip(1)));
//		things = things.Skip(16 - num).Concat(things.Take(16 - num)).ToArray();
//	}
//	else if (instr[0] == 'x')
//	{
//		var inst = string.Join("", instr.Skip(1));
//		var sides = inst.Split('/').Select(x => int.Parse(x)).ToArray();
//
//		(things[sides[0]], things[sides[1]]) = (things[sides[1]], things[sides[0]]);
//	}
////	else if (instr[0] == 'p')
////	{
////		var a = Array.FindIndex(things, x => x == instr[1]);
////		var b = Array.FindIndex(things, x => x == instr[3]);
////
////		(things[a], things[b]) = (things[b], things[a]);
////	}
//}
//
//string.Join("",things).Dump();

for (int i = 0; i < 1_000_000_000; ++i)
{
	(things[0],
	things[1],
	things[2], 
	things[3], 
	things[4], 
	things[5], 
	things[6], 
	things[7], 
	things[8], 
	things[9], 
	things[10], 
	things[11], 
	things[12], 
	things[13], 
	things[14],
	things[15]) =
	
//benlihakpcmgfdjo

	(things[1],
	things[4],
	things[13],
	things[11],
	things[8],
	things[7],
	things[0],
	things[10],
	things[15],
	things[2],
	things[12],
	things[6],
	things[5],
	things[3],
	things[9],
	things[14]);
}

//pkgnhomelfdibjac

string.Join("",things).Dump();