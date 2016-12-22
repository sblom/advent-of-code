<Query Kind="Statements" />

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));

	var seed = "00101000101111010";
	int length = 35651584;
	
	//seed = "10000";
	//length = 20;
	
	string notback(string input)
	{
		StringBuilder result = new StringBuilder(input.Length);
		for (int i = input.Length - 1; i >= 0; i--)
		{
			result = result.Append(input[i] == '0' ? "1" : "0");
		}
		
		return result.ToString();
	}
	
	while (seed.Length < length)
	{
		seed = seed + "0" + notback(seed);
	}
	
	string data = seed.Substring(0, length);
	
	string checksum = data;
	
	while (checksum.Length % 2 == 0)
	{
		StringBuilder newchecksum = new StringBuilder(checksum.Length / 2 + 1);
		for (int ii = 0; ii < checksum.Length; ii += 2)
		{
			if (checksum[ii] == checksum[ii + 1]) newchecksum.Append("1");
			else newchecksum.Append("0");
		}
		checksum = newchecksum.ToString();
	}
	
	checksum.Dump();