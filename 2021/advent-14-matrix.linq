<Query Kind="Statements" />

var rules = @"NS->H
FS->O
PO->C
NV->N
CK->B
FK->N
PS->C
OF->F
KK->F
PP->S
VS->K
VB->V
BP->P
BB->K
BF->C
NN->V
NO->F
SV->C
OK->N
PH->P
KV->B
PN->O
FN->V
SK->V
VC->K
BH->P
BO->S
HS->H
HK->S
HC->S
HF->B
PC->C
CF->B
KN->H
CS->N
SP->O
VH->N
CC->K
KP->N
NP->C
FO->H
FV->N
NC->F
KB->N
VP->O
KO->F
CP->F
OH->F
KC->H
NB->F
HO->P
SC->N
FF->B
PB->H
FB->K
SN->B
VO->K
OO->N
NF->B
ON->P
SF->H
FP->H
HV->B
NH->B
CO->C
PV->P
VV->K
KS->P
OS->S
SB->P
OC->N
SO->K
BS->B
CH->V
PK->F
OB->P
CN->N
CB->N
VF->O
VN->K
PF->P
SH->H
FH->N
HP->P
KF->V
BK->H
OP->C
HH->F
SS->V
BN->C
OV->F
HB->P
FC->C
BV->H
VK->S
NK->K
CV->K
HN->K
BC->K
KH->P".Split("\n\r".ToCharArray(),StringSplitOptions.RemoveEmptyEntries).OrderBy(x => x);

var nums = "BCFHKNOPSV".Select((c,i) => (c,i)).ToDictionary(x => x.c, x => x.i);

int PairToNum(string pair) => nums[pair[0]] * 10 + nums[pair[1]];

foreach (var rule in rules)
{
	var row = new long[100];

	row[PairToNum($"{rule[0]}{rule[4]}")] += 1;
	row[PairToNum($"{rule[4]}{rule[1]}")] += 1;
	
	Console.WriteLine("{" + string.Join(",", row) + "},");
}