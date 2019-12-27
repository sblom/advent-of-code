<Query Kind="Statements">
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>Wintellect.PowerCollections</Namespace>
</Query>

var frontier = new OrderedBag<
(int hp, int mana, int shieldturns, int poisonturns, int rechargeturns, int bosshp, int bossdam, int manaspent, bool bossturn)
>(Comparer<(int hp, int mana, int shieldturns, int poisonturns, int rechargeturns, int bosshp, int bossdam, int manaspent, bool bossturn)>.Create((a,b) => a.manaspent - b.manaspent));

var startingstate = (hp: 50, mana: 500, shieldturns: 0, poisonturns: 0, rechargeturns: 0, bosshp: 58, bossdam: 9, manaspent: 0, bossturn: false);

frontier.Add(startingstate);

int max = 0;

while (frontier.Count > 0)
{
	var cur = frontier.RemoveFirst();
	
	var (hp, mana, shieldturns, poisonturns, rechargeturns, bosshp, bossdam, manaspent, bossturn) = cur;
	
	hp -= 1;

	if (manaspent > max)
	{
		manaspent.Dump();
		max = manaspent;
	}
	
	int armor = 0;

	if (bosshp < 0)
	{
		// 1915 is too high.
		// 1289 is still too high.
		// 1196 is too low.
		// 1249 is wrong.
		manaspent.Dump("Part 1");
		break;
	}

	if (shieldturns > 0)
	{
		shieldturns--;
		armor = 7;
	}
	
	if (poisonturns > 0)
	{
		poisonturns--;
		bosshp -= 3;
		if (bosshp <= 0) { "Died of poison".Dump(); cur.ToString().Dump(); }
	}
	
	if (rechargeturns > 0)
	{
		rechargeturns--;
		mana += 101;
	}

	if (bosshp < 0)
	{
		// 1915 is too high.
		// 1289 is still too high.
		// 1196 is too low.
		// 1249 is wrong.
		manaspent.Dump("Part 1");
		break;
	}

	if (bossturn)
	{
		var realbossdam = bossdam - armor;

		if (hp - realbossdam <= 0) continue;
		else
			frontier.Add((hp - realbossdam, mana, shieldturns, poisonturns, rechargeturns, bosshp, bossdam, manaspent, false));
	}
	else{
		// Magic missile.
		if (mana >= 53)
			frontier.Add((hp, mana - 53, shieldturns, poisonturns, rechargeturns, bosshp - 4, bossdam, manaspent + 53, true));
	
		if (mana >= 73)
			frontier.Add((hp + 2, mana - 73, shieldturns, poisonturns, rechargeturns, bosshp - 2, bossdam, manaspent + 73, true));
	
		if (mana >= 113 && shieldturns == 0)
			frontier.Add((hp, mana - 113, shieldturns + 6, poisonturns, rechargeturns, bosshp, bossdam, manaspent + 113, true));
	
		if (mana >= 173 && poisonturns == 0)
			frontier.Add((hp, mana - 173, shieldturns, poisonturns + 6, rechargeturns, bosshp, bossdam, manaspent + 173, true));
			
		if (mana >= 229 && rechargeturns == 0)
			frontier.Add((hp, mana - 229, shieldturns, poisonturns, rechargeturns + 5, bosshp, bossdam, manaspent + 229, true));
	}
}