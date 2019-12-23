<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "Lib\IntCodeVM.linq"

async Task Main()
{
	var lines = await AoC.GetLinesWeb();
	// lines.Dump();
	
	var mem = lines.First().Split(',').Select(long.Parse).Concat(Enumerable.Repeat(0L,1000000)).ToArray();
	
	var nics = new List<NIC>();
	var idleflags = new bool[50];
	var vms = new List<IntCodeVM>();
	var nicmem = new long[2];
	
	for (int i = 0; i < 50; i++)
	{
		var nic = new NIC(nics,idleflags,i,nicmem);
		nics.Add(nic);
		nic.Inputs.Add(i);
		var vm = new IntCodeVM(mem, nic);
		vms.Add(vm);
	}

	while (true)
	{
		for (int i = 0; i < 50; i++)
		{
			if (!vms[i].ExecuteNext())
			{
				$"VM #{i} quit".Dump();
			}
		}
	}
}

// Define other methods, classes and namespaces here
public class NIC: IntCodeVM.IInOut {

	public static (long,long) nicvals = (-1, -1);
	public static bool firstnic = true;
	List<NIC> nics;
	bool[] idleflags;
	int addr;
	long[] nicmem;
	public NIC(List<NIC> nics, bool[] idleflags, int addr, long[] nicmem)
	{
		this.nics = nics;
		this.idleflags = idleflags;
		this.addr = addr;
		this.nicmem = nicmem;
	}
	
	public List<long> Inputs = new List<long>();
	public List<long> Outputs = new List<long>();
	
	public long Read()
	{
		if (Inputs.Count == 0)
		{
			idleflags[addr] = true;
			if (idleflags.All(x => x))
			{
				nics[0].Inputs.Add(nicmem[0]);
				nics[0].Inputs.Add(nicmem[1]);
				
				if (nicvals == (nicmem[0], nicmem[1]))
				{
					(nicmem[0], nicmem[1]).ToString().Dump("Part 2");
					Environment.Exit(0);
				}
				else{
					nicvals = (nicmem[0], nicmem[1]);
				}
				
				for (int i = 0; i < 50; i++) idleflags[i] = false;
			}
			return -1;
		}
		else
		{
			idleflags[addr] = false;
			var result = Inputs[0];
			Inputs.RemoveAt(0);
			return result;
		}
	}
	
	public void Write(long val)
	{
		idleflags[addr] = false;
		Outputs.Add(val);
		if (Outputs.Count >= 3)
		{
			//$"Sending ({Outputs[1]},{Outputs[2]}) to ::{Outputs[0]}".Dump();
			if (Outputs[0] == 255)
			{
				nicmem[0] = Outputs[1];
				nicmem[1] = Outputs[2];
				if (firstnic)
				{
					Outputs[2].Dump("Part 1");
					firstnic = false;
				}
				// Environment.Exit(0);
			}
			else{
				nics[(int)Outputs[0]].Inputs.Add(Outputs[1]);
				nics[(int)Outputs[0]].Inputs.Add(Outputs[2]);
			}
			Outputs.RemoveAt(0);
			Outputs.RemoveAt(0);
			Outputs.RemoveAt(0);
		}
	}
}