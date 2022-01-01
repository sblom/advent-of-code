<Query Kind="Statements" />

var state = "A";
var loc = 0;

List<int> positive = new List<int>{0,0,0,0,0};
List<int> negative = new List<int>{0,0,0,0,0};

int getval()
{
	if (loc >= 0)
	{
		if (loc >= positive.Count) positive.Add(0);
		return positive[loc];
	}
	else{
		if (-loc >= negative.Count) negative.Add(0);
		return negative[-loc];
	}
}

void setval(int n)
{
	if (loc >= 0)
		positive[loc] = n;
	else
		negative[-loc] = n;
}

var rules = new Dictionary<string, Action> {
	{"A", () =>{
		if (getval() == 0)
		{
			setval(1);
			loc++;
			state = "B";
		}
		else
		{
			setval(0);
			loc--;
			state = "C";
		}
	}},

	{"B", () =>{
		if (getval() == 0)
		{
			setval(1);
			loc--;
			state = "A";
		}
		else
		{
			setval(1);
			loc++;
			state = "D";
		}
	}},
	{"C", () =>{
		if (getval() == 0)
		{
			setval(1);
			loc++;
			state = "A";
		}
		else
		{
			setval(0);
			loc--;
			state = "E";
		}
	}},
	{"D", () =>{
		if (getval() == 0)
		{
			setval(1);
			loc++;
			state = "A";
		}
		else
		{
			setval(0);
			loc++;
			state = "B";
		}
	}},
	{"E", () =>{
		if (getval() == 0)
		{
			setval(1);
			loc--;
			state = "F";
		}
		else
		{
			setval(1);
			loc--;
			state = "C";
		}
	}},
	{"F", () =>{
		if (getval() == 0)
		{
			setval(1);
			loc++;
			state = "D";
		}
		else
		{
			setval(1);
			loc++;
			state = "A";
		}
	}},
};

for (int i = 0; i < 12919244; i++)
{
	rules[state]();	
}

(positive.Where(x => x==1).Count() + negative.Where(x => x==1).Count()).Dump("part 1");