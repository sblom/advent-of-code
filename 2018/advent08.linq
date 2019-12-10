<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

public struct Node {
	public List<Node> Children;
	public List<int> Metadata;
}

(Node, int, int) ParseNode(int[] nums, int i)
{
	var node = new Node();
	node.Children = new List<Node>();
	node.Metadata = new List<int>();
	
	var vals = new List<int>();
	
	var childrencount = nums[i++];
	var metadatacount = nums[i++];
	
	var metadatasum = 0;
	
	for (int children = 0; children < childrencount; children++)
	{
		var (child, newi, sum) = ParseNode(nums, i);
		node.Children.Add(child);
		vals.Add(sum);
		i = newi;
	}

	node.Metadata = nums.Skip(i).Take(metadatacount).ToList();
	
	if (node.Children.Count == 0)
	{
		metadatasum = node.Metadata.Sum();
	}
	else
	{
		for (int x = 0; x < metadatacount; x++)
		{
			if (nums[x + i] > node.Children.Count) continue;
			metadatasum += vals[nums[x + i] - 1];
		}
	}
	
	return (node, i + metadatacount, metadatasum);
}

async Task Main()
{
	var lines = await AoC.GetLinesWeb();
	
	var nums = lines.First().Split(' ').Select(x=>int.Parse(x)).ToArray();

	var (top, _, sum) = ParseNode(nums, 0);
	
	sum.Dump();
}

// Define other methods and classes here
