<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();

var nums = new Regex(@"-?\d+");

var ingredients = lines.Select(x => x.Split(':')).Select(x => (x[0], nums.Matches(x[1]).Select(x => int.Parse(x.Captures[0].Value)).ToList())).ToList();

int maxscore = 0, maxscore2 = 0;
int I = 0, J = 0, K = 0, L = 0;

for (int i = 1; i <= 100; i++)
{
	for (int j = 1; j <= 100 - i; j++)
	{
		for (int k = 1; k <= 100 - i - j; k++)
		{
			var l = 100 - i - j - k;
			
			var capacity = i * ingredients[0].Item2[0] + j * ingredients[1].Item2[0] + k * ingredients[2].Item2[0] + l * ingredients[3].Item2[0];				
			if (capacity <= 0) continue;

			var durability = i * ingredients[0].Item2[1] + j * ingredients[1].Item2[1] + k * ingredients[2].Item2[1] + l * ingredients[3].Item2[1];
			if (durability <= 0) continue;
			
			var flavor = i * ingredients[0].Item2[2] + j * ingredients[1].Item2[2] + k * ingredients[2].Item2[2] + l * ingredients[3].Item2[2];
			if (flavor <= 0) continue;
			
			var texture = i * ingredients[0].Item2[3] + j * ingredients[1].Item2[3] + k * ingredients[2].Item2[3] + l * ingredients[3].Item2[3];
			if (texture <= 0) continue;

			var score = capacity * durability * flavor * texture;
			if (score > maxscore) 
			{
				maxscore = score;
				(I,J,K,L) = (i,j,k,l);
			}
			if (i * ingredients[0].Item2[4] + j * ingredients[1].Item2[4] + k * ingredients[2].Item2[4] + l * ingredients[3].Item2[4] == 500)
			{
				if (score > maxscore2)
				{
					maxscore2 = score;
				}
			}
		}
	}
}

maxscore.Dump("Part 1");
maxscore2.Dump("Part 2");