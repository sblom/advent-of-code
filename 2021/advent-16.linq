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
var lines = @"9C0141080250320F1802104A08".GetLines();
#endif

var input = lines.First();
int numbits = 0;
int buffer = 0;
int next = 0;

int GetBits(int n)
{
	while (numbits < n)
	{
		buffer <<= 4;
		buffer |= input[next] switch {
			>= '0' and <= '9' => input[next] - '0',
			>= 'A' and <= 'F' => input[next] -'A' + 10
		};
		++next;
		numbits += 4;
	}
	
	var mask = ((1 << n) - 1) << (numbits - n);
	
	var result = (buffer & mask) >> (numbits - n);
	buffer &= ~mask;
	
	numbits -= n;
	
	return result;
}

long verTotal = 0;

(long val,int length) ParsePacket()
{
	"BEGIN PACKET".DumpTrace();
	int ver = GetBits(3);
	int type = GetBits(3);

	verTotal += ver;

	int length = 6;

	switch (type)
	{
		case 4:
			long n = 0;
			long numpart = 0;
			do
			{
				n <<= 4;
				numpart = GetBits(5);
				n |= (numpart & 0xf);
				length += 5;
			} while ((numpart & (1 << 4)) > 0);
			$"END PACKET ({length})".DumpTrace();
			return (n,length);
			break;
		case 0:
			var (result, len) = AggregateSubpackets((a,b) => a + b);
			length += len;
			$"END PACKET ({length})".DumpTrace();
			return (result,length);
			break;
		case 1:
			(result, len) = AggregateSubpackets((a, b) => a * b);
			length += len;
			$"END PACKET ({length})".DumpTrace();
			return (result, length);
			break;
		case 2:
			(result, len) = AggregateSubpackets((a, b) => Math.Min(a, b));
			length += len;
			$"END PACKET ({length})".DumpTrace();
			return (result, length);
			break;
		case 3:
			(result, len) = AggregateSubpackets((a, b) => Math.Max(a, b));
			length += len;
			$"END PACKET ({length})".DumpTrace();
			return (result, length);
			break;
		case 5:
			(result, len) = AggregateSubpackets((a, b) => a > b ? 1 : 0);
			length += len;
			$"END PACKET ({length})".DumpTrace();
			return (result, length);
			break;
		case 6:
			(result, len) = AggregateSubpackets((a, b) => a < b ? 1 : 0);
			length += len;
			$"END PACKET ({length})".DumpTrace();
			return (result, length);
			break;
		case 7:
			(result, len) = AggregateSubpackets((a, b) => a == b ? 1 : 0);
			length += len;
			$"END PACKET ({length})".DumpTrace();
			return (result, length);
			break;
		default:
			throw new Exception();
			break;
	}
}

(long val,int length) AggregateSubpackets(Func<long,long,long> op)
{
	int length = 0;
	
	int lengthType = GetBits(1);
	length++;
	if (lengthType == 0)
	{
		int packetsLength = GetBits(15);
		length += 15;
		length += packetsLength;
		var bits = 0;
		long result = 0;

		var (n, len) = ParsePacket();
		result = n;
		packetsLength -= len;

		while (packetsLength > 0)
		{
			(n, len) = ParsePacket();			
			result = op(result, n);
			packetsLength -= len;
		} 
		return (result, length);
	}
	else
	{
		int packets = GetBits(11);
		length += 11;
		int packetsLength = 0;
		
		long result = 0;
		
		var (n, len) = ParsePacket();
		packetsLength += len;
		result = n;
		
		for (int i = 1; i < packets; i++)
		{
			(n, len) = ParsePacket();
			packetsLength += len;
			result = op(result,n);
		}

		length += packetsLength;
		return (result, length);
	}
	
}


while (next < input.Length - 1)
{
	var (n,len) = ParsePacket();
	n.Dump("Part 2");
}

verTotal.Dump();

record packet();
record literal(int lit): packet;
record oper(): packet;
