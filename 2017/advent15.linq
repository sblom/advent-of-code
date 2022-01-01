<Query Kind="Statements" />

var lines = AoC.GetLines();

var A0 = 289;
var B0 = 629;

var Ag = 16807;
var Bg = 48271;

var R = 2147483647;

long A = A0;
long B = B0;

var c = 0;

IEnumerable<int> Gen(int a0, int p, int f)
{
	long a = a0;
	
	while (true)
	{
		a = (a * p) % R;
		if (a % f == 0)
		{
			yield return (int)a;
		}
	}
}

var AA = Gen(A0, Ag, 4);
var BB = Gen(B0, Bg, 8);

AA.Take(5_000_000).Zip(BB, (x, y) => ((x & 0xffff) == (y & 0xffff))).Where(x => x).Count().Dump();


//for (int i = 0; i < 5_000_000; i++)
//{
//	A = (A * Ag) % R;
//	B = (B * Bg) % R;
//	
//	
//	if ((A & 0xffff) == (B & 0xffff))
//		c++;
//}

//c.Dump();