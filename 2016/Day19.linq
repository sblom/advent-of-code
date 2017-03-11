<Query Kind="Statements" />

int hob(int num)
{
	if (num == 0)
		return 0;

	int ret = 1;

	while ((num >>= 1) > 0)
		ret <<= 1;

	return ret;
}

int getSafePosition(int n)
{
	// find value of L for the equation
	int valueOfL = n - hob(n);
	int safePosition = 2 * valueOfL + 1;

	return safePosition;
}

getSafePosition(3004953).Dump("Part 1");