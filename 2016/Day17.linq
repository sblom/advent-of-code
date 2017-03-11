<Query Kind="Statements">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

var passcode = "veumntbg";

var frontier = new List<(string, (int, int))> { ("", (0,0)) };
var dirs = new List<(int dy, int dx, char c, int shift)>
{
	(-1,  0, 'U', 12),
	( 1,  0, 'D', 8),
	( 0, -1, 'L', 4),
	( 0,  1, 'R', 0),
};

var hasher = new MD5CryptoServiceProvider();

int max = 0;

(int, int) Position (string path)
{
	(int x, int y) loc = (0, 0);
	
	foreach (char p in path)
	{
		var dir = dirs.Single(x => x.c == p);
		loc = (loc.x + dir.dx, loc.y + dir.dy);
	}
	return loc;
}

while (frontier.Count > 0)
{
	var (p, (x, y)) = frontier[0]; frontier.RemoveAt(0);

	//var loc = Position(p);
	if (x < 0 || x > 3 || y < 0 || y > 3) continue;

	if (x == 3 && y == 3)
	{
		if (max == 0) p.Dump("Part 1");
		if (p.Length > max)
		{
			max = p.Length;
		}
		continue;	
	}

	var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(passcode + p));

	int code = hash[0] * 0x100 + hash[1];

	foreach (var (dy, dx, c, sh) in dirs)
	{
		if (((code >> sh) & 0xf) >= 0xb)
		{
			frontier.Add((p + c, (x + dx, y+dy)));
		}
	}
}

max.Dump("Part 2");