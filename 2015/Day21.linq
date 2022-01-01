<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();

var boss = (hp: 103, damage: 9, armor: 2);

var weapons = new (string name, int cost, int damage, int armor)[] {
	("Dagger", 8, 4, 0),
	("Shortsword", 10, 5, 0),
	("Warhammer", 25, 6, 0),
	("Longsword", 40, 7, 0),
	("Greataxe", 74, 8, 0),
};

var armors = new (string name, int cost, int damage, int armor)[] {
	("No armor", 0, 0, 0),
	("Leather", 13, 0, 1),
	("Chainmail", 31, 0, 2),
	("Splintmail", 53, 0, 3),
	("Bandedmail", 75, 0, 4),
	("Platemail", 102, 0, 5),
};

var rings = new (string name, int cost, int damage, int armor)[] {
	("No left ring", 0, 0, 0),
	("No right ring", 0, 0, 0),
	("Damage + 1", 25, 1, 0),
	("Damage + 2", 50, 2, 0),
	("Damage + 3", 100, 3, 0),
	("Defense + 1", 20, 0, 1),
	("Defense + 2", 40, 0, 2),
	("Defense + 3", 80, 0, 3)
};

var loadouts = 	from weapon in weapons
				from armor in armors
				from lring in rings
				from rring in rings
				where lring != rring
				select new (string name, int cost, int damage, int armor)[]{weapon, armor, lring, rring};
				
bool IsWinner(int damage, int armor)
{
	var actualMyDamage = (damage - boss.armor <= 0) ? 1 : damage - boss.armor;
	var actualBossDamage = (boss.damage - armor <= 0) ? 1 : boss.damage - armor;
	var survivableRounds = (int)Math.Ceiling(100.0 / actualBossDamage);
	return survivableRounds * actualMyDamage >= boss.hp;
}

loadouts.Where(L => IsWinner(L.Sum(D => D.damage), L.Sum(A => A.armor))).OrderBy(L => L.Sum(C => C.cost)).First().Sum(C => C.cost).Dump("Part 1");

loadouts.Where(L => !IsWinner(L.Sum(D => D.damage), L.Sum(A => A.armor))).OrderByDescending(L => L.Sum(C => C.cost)).First().Sum(C => C.cost).Dump("Part 2");