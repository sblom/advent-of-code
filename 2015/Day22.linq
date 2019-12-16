<Query Kind="Statements">
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

IImmutableStack<((int hp, int mana, int shieldturns, int poisonturns, int rechargeturns, int bosshp, int bossdam) state, int nextspell)> turns = ImmutableStack<((int, int, int, int, int, int, int), int)>.Empty;

var startingstate = (hp: 50, mana: 500, shieldturns: 0, poisonturns: 0, rechargeturns: 0, bosshp: 58, bossdam: 9);

turns = turns.Push((startingstate, 0));

while (true)
{
	var cur = turns.Peek();

	// Illegal turn.
	if (cur.nextspell == 5)
	{
		goto illegal_turn;
	}
	
	var (hp, mana, shieldturns, poisonturns, rechargeturns, bosshp, bossdam) = cur.state;
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
			turns = turns.Push(((hp, mana - 53, shieldturns, poisonturns, rechargeturns, bosshp - 4, bossdam), 0));
			break;
		case 1:
			if (mana < 73) goto illegal_turn;
			turns = turns.Push(((hp + 2, mana - 73, shieldturns, poisonturns, rechargeturns, bosshp - 2, bossdam), 0));
			break;
		case 2:
			if (mana < 113 || shieldturns > 0) goto illegal_turn;
			turns = turns.Push(((hp, mana - 113, shieldturns + 6, poisonturns, rechargeturns, bosshp, bossdam), 0));
			break;
		case 3:
			if (mana < 173 || poisonturns > 0) goto illegal_turn;
			turns = turns.Push(((hp, mana - 173, shieldturns, poisonturns + 6, rechargeturns, bosshp, bossdam), 0));
			break;
		case 4:
			if (mana < 229 || rechargeturns > 0) goto illegal_turn;		
			turns = turns.Push(((hp, mana - 229, shieldturns, poisonturns, rechargeturns + 5, bosshp, bossdam), 0));
			break;
	}
	
	cur = turns.Peek(); turns = turns.Pop();
	(hp, mana, shieldturns, poisonturns, rechargeturns, bosshp, bossdam) = cur.state;

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

	turns = turns.Push(((hp - realbossdam, mana, shieldturns, poisonturns, rechargeturns, bosshp, bossdam),cur.nextspell));
	
	if (hp - realbossdam <= 0) goto illegal_turn;
	
	continue;

illegal_turn:
	turns = turns.Pop();
	cur = turns.Peek();
	turns = turns.Pop().Push((cur.state, cur.nextspell + 1));
}