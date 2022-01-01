<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>static UserQuery</Namespace>
</Query>

//#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"--- scanner 0 ---
404,-588,-901
528,-643,409
-838,591,734
390,-675,-793
-537,-823,-458
-485,-357,347
-345,-311,381
-661,-816,-575
-876,649,763
-618,-824,-621
553,345,-567
474,580,667
-447,-329,318
-584,868,-557
544,-627,-890
564,392,-477
455,729,728
-892,524,684
-689,845,-530
423,-701,434
7,-33,-71
630,319,-379
443,580,662
-789,900,-551
459,-707,401

--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
95,138,22
-476,619,847
-340,-569,-846
567,-361,727
-460,603,-452
669,-402,600
729,430,532
-500,-761,534
-322,571,750
-466,-666,-811
-429,-592,574
-355,545,-477
703,-491,-529
-328,-685,520
413,935,-424
-391,539,-444
586,-435,557
-364,-763,-893
807,-499,-711
755,-354,-619
553,889,-390

--- scanner 2 ---
649,640,665
682,-795,504
-784,533,-524
-644,584,-595
-588,-843,648
-30,6,44
-674,560,763
500,723,-460
609,671,-379
-555,-800,653
-675,-892,-343
697,-426,-610
578,704,681
493,664,-388
-671,-858,530
-667,343,800
571,-461,-707
-138,-166,112
-889,563,-600
646,-828,498
640,759,510
-630,509,768
-681,-892,-333
673,-379,-804
-742,-814,-386
577,-820,562

--- scanner 3 ---
-589,542,597
605,-692,669
-500,565,-823
-660,373,557
-458,-679,-417
-488,449,543
-626,468,-788
338,-750,-386
528,-832,-391
562,-778,733
-938,-730,414
543,643,-506
-524,371,-870
407,773,750
-104,29,83
378,-903,-323
-778,-728,485
426,699,580
-438,-605,-362
-469,-447,-387
509,732,623
647,635,-688
-868,-804,481
614,-800,639
595,780,-596

--- scanner 4 ---
727,592,562
-293,-554,779
441,611,-461
-714,465,-776
-743,427,-804
-660,-479,-426
832,-632,460
927,-485,-438
408,393,-506
466,436,-512
110,16,151
-258,-428,682
-393,719,612
-211,-452,876
808,-476,-593
-575,615,604
-485,667,467
-680,325,-822
-627,-443,-432
872,-547,-609
833,512,582
807,604,487
839,-516,451
891,-625,532
-652,-548,-490
30,-46,-14".GetLines();
#endif

var scanners = lines.GroupLines().ToArray();

var beaconsets = scanners.Select(scanner => scanner.Skip(1).Extract<(int x, int y, int z)>(@"(-?\d+),(-?\d+),(-?\d+)").ToArray()).ToArray();

var alignmentvectors = beaconsets.Select(beaconset =>
{
	return (from b1 in beaconset from b2 in beaconset select new[] { Math.Abs(b1.x - b2.x), Math.Abs(b1.y - b2.y), Math.Abs(b1.z - b2.z) }.OrderBy(x => x).ToArray()).Select(x => (x[0],x[1],x[2])).ToArray();
}).ToArray();

var matches = (from b1 in alignmentvectors.Select((x,i) => (x,i)) from b2 in alignmentvectors.Select((x,i) => (x,i)) where (b1.x.Intersect(b2.x).Count() >= 12) where (b1.i != b2.i) select (b1.i, b2.i));

var rotations = new[]{
	//right
	((int x, int y, int z) b) => (b.y,b.z,b.x),
	((int x, int y, int z) b) => (b.y,b.x,-b.z),
	((int x, int y, int z) b) => (b.y,-b.z,-b.x),
	((int x, int y, int z) b) => (b.y,-b.x,b.z),
	
	//left
	((int x, int y, int z) b) => (-b.y,b.x,b.z),
	((int x, int y, int z) b) => (-b.y,b.z,-b.x),
	((int x, int y, int z) b) => (-b.y,-b.x,-b.z),
	((int x, int y, int z) b) => (-b.y,-b.z,b.x),
	
	//up
	((int x, int y, int z) b) => (b.x,b.y,b.z),
	((int x, int y, int z) b) => (b.z,b.y,-b.x),
	((int x, int y, int z) b) => (-b.x,b.y,-b.z),
	((int x, int y, int z) b) => (-b.z,b.y,b.x),

	//down
	((int x, int y, int z) b) => (b.z,-b.y,b.x),
	((int x, int y, int z) b) => (b.x,-b.y,-b.z),
	((int x, int y, int z) b) => (-b.z,-b.y,-b.x),
	((int x, int y, int z) b) => (-b.x,-b.y,b.z),

	//toward
	((int x, int y, int z) b) => (b.z,b.x,b.y),
	((int x, int y, int z) b) => (b.x,-b.z,b.y),
	((int x, int y, int z) b) => (-b.z,-b.x,b.y),
	((int x, int y, int z) b) => (-b.x,b.z,b.y),

	//away
	((int x, int y, int z) b) => (b.x,b.z,-b.y),
	((int x, int y, int z) b) => (b.z,-b.x,-b.y),
	((int x, int y, int z) b) => (-b.x,-b.z,-b.y),
	((int x, int y, int z) b) => (-b.z,b.x,-b.y),
};

((int,int),Func<(int,int,int),(int,int,int)>,(int,int,int)) Align(int s1, int s2)
{
	var bs1 = beaconsets[s1];
	var bs2 = beaconsets[s2];

	foreach (var rot in rotations)
	{
		var alignments = bs1.SelectMany(b1 => bs2.Select(b2 => (First: b1, Second: rot(b2)))).Select(x => (x.First.x - x.Second.Item1, x.First.y - x.Second.Item2, x.First.z - x.Second.Item3));
		var offsets = alignments.GroupBy(x => x).OrderByDescending(g => g.Count()).First();
		var overlap = offsets.Count();
		var offset = offsets.First();
		if (overlap >= 12)
		{
			return ((s1,s2),rot, offset);
		}
	}	
	
	return (default,default,default);
}

var truematches = matches.Select(match => Align(match.Item1,match.Item2)).Where(x => x.Item2 != null).ToArray();

var merged = new Dictionary<int,(Func<(int,int,int),(int,int,int)>,(int,int,int))>();

merged[0] = (rotations[0], (0, 0, 0));

var centers = new List<(int, int, int)> { (0, 0, 0) };

while (merged.Count() < beaconsets.Length)
{
	var neighbors = merged.Keys.SelectMany(k => truematches.Where(tm => tm.Item1.Item1 == k && !merged.ContainsKey(tm.Item1.Item2)).Select(tm => (k, tm.Item1.Item2)));
	var toMerge = neighbors.First();

	var align = Align(toMerge.k, toMerge.Item2);

	centers.Add(align.Item3);
	
	beaconsets[toMerge.Item2] = beaconsets[toMerge.Item2].Select(b => align.Item2(b)).Select(b => (b.Item1 + align.Item3.Item1, b.Item2 + align.Item3.Item2, b.Item3 + align.Item3.Item3)).ToArray();

	merged[toMerge.Item2] = default;
}

var normalizedBeacons = new HashSet<(int,int,int)>();

for (int i = 0; i < beaconsets.Length; i++)
{
	foreach (var beacon in beaconsets[i])
	{
		normalizedBeacons.Add(beacon);
	}
}

normalizedBeacons.Count().Dump("Part 1");

(from c1 in centers from c2 in centers select (Math.Abs(c1.Item1 - c2.Item1) + Math.Abs(c1.Item2 - c2.Item2) + Math.Abs(c1.Item3 - c2.Item3))).Max().Dump("Part 2");