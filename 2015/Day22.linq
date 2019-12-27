<Query Kind="Statements">
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>Wintellect.PowerCollections</Namespace>
</Query>

var frontier = new OrderedBag<
(int hp, int mana, int shieldturns, int poisonturns, int rechargeturns, int bosshp, int bossdam, int manaspent)
>(Comparer<(int hp, int mana, int shieldturns, int poisonturns, int rechargeturns, int bosshp, int bossdam, int manaspent)>.Create((a,b) => a.manaspent - b.manaspent));

var startingstate = (hp: 50, mana: 500, shieldturns: 0, poisonturns: 0, rechargeturns: 0, bosshp: 58, bossdam: 9, manaspent: 0);

frontier.Add(startingstate);

int max = 0;

while (frontier.Count > 0)
{
	var cur = frontier.RemoveFirst();
	//cur.ToString().Dump();

	// Illegal turn.
	//if (cur.nextspell == 5)
	//{
	//	goto illegal_turn;
	//}
	
	var (hp, mana, shieldturns, poisonturns, rechargeturns, bosshp, bossdam, manaspent) = cur.state;
	
	if (manaspent > max)
	{
		manaspent.Dump();
		max = manaspent;
	}
	
	int armor = 0;
	
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

	switch (cur.nextspell)
	{
		case 0: // Magic missile.
			if (mana < 53) goto illegal_turn;
			mana -= 53; bosshp -= 4; manaspent += 53;
			break;
		case 1:
			if (mana < 73) goto illegal_turn;
			hp += 2; mana -= 73; bosshp -= 2; manaspent += 73;
			break;
		case 2:
			if (mana < 113 || shieldturns > 0) goto illegal_turn;
			mana -= 113; shieldturns += 6; manaspent += 113;
			break;
		case 3:
			if (mana < 173 || poisonturns > 0) goto illegal_turn;
			mana -= 173; poisonturns += 6; manaspent += 173;
			break;
		case 4:
			if (mana < 229 || rechargeturns > 0) goto illegal_turn;		
			mana -= 229; rechargeturns += 5; manaspent += 229;
			break;
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
	
	if (shieldturns > 0)
	{
		shieldturns--;
		armor = 7;
	}

	if (poisonturns > 0)
	{
		poisonturns--;
		bosshp -= 3;
	}

	if (rechargeturns > 0)
	{
		rechargeturns--;
		mana += 101;
	}

	var realbossdam = bossdam - armor;
	
	if (hp - realbossdam <= 0) goto illegal_turn;

	for (int spell = 0; spell < 5; spell++)
	{
		frontier.Add(((hp - realbossdam, mana, shieldturns, poisonturns, rechargeturns, bosshp, bossdam, manaspent), spell));
	}

illegal_turn:;
	//turns.Dump();
}