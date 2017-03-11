<Query Kind="Statements">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

var dict = new Dictionary<int,string>();

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));

var input = "ahsbgdzn";
//var input = "abc";

int i = 0;
int c = 0;
int n = 0;

var hasher = new MD5CryptoServiceProvider();

var rx = new Regex(@"(.)\1\1");

while (true)
{
	var str = input + $"{i}";
	var hashbyes = hasher.ComputeHash(Encoding.UTF8.GetBytes(str));
	var hash = Hash(i);
	var match = rx.Match(hash);
	i++;
	if (!match.Success) continue;
	var findstr = string.Join("", Enumerable.Repeat(match.Captures[0].Value[0], 5));	
	for (n = 0; n < 1000; n++)
	{
		var hash2 = Hash(i+n);
		
		if (hash2.Contains(findstr))
		{
			c++;
			c.Dump();
			(i - 1).Dump();
		}
	}
}

string Hash(int x)
{
	string hash = "";
	
	if (dict.ContainsKey(x))
		return dict[x];
	else
	{
		var str = input + $"{x}";
		var hashbyes = hasher.ComputeHash(Encoding.UTF8.GetBytes(str));
		for (int ii = 0; ii < 2016; ii++)
		{
			hash = BitConverter.ToString(hashbyes).Replace("-", "").ToLower();
			hashbyes = hasher.ComputeHash(Encoding.UTF8.GetBytes(hash));
		}
		hash = BitConverter.ToString(hashbyes).Replace("-", "").ToLower();		
		dict[x] = hash;
		return hash;
	}
}