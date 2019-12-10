<Query Kind="Statements">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

var key = "iwrupvqb";
var keylen = key.Length;
var bytes = new byte[100];
Encoding.UTF8.GetBytes(key).CopyTo(bytes,0);

var md5 = new MD5CryptoServiceProvider();

byte[] bi = Enumerable.Repeat((byte)'0',100).ToArray();
int i = 0;
byte[] hash;
int maxpos = 0;

// This is the insane version with no string allocation or conversion.
// In fact, I'm not even converting an int to a char(8) array every time.
// It's still way slower than I suspect this would be in C or Rust. :(

foreach (byte mask in new[] { 0xf0, 0xff })
{
	do
	{
		i++;
		int pos = 0;
		bool carried = false;
		do
		{
			bi[pos]++;
			if (bi[pos] > (byte)'9')
			{
				bi[pos] = (byte)'0';
				carried = true;
				pos++;
			}
			else
			{
				carried = false;
			}
			if (pos > maxpos) maxpos = pos;
		} while (carried);
		for (int ii = maxpos; ii >= 0; --ii)
		{
			bytes[keylen + (maxpos - ii)] = bi[ii];
		}
		hash = md5.ComputeHash(bytes, 0, keylen + maxpos + 1);
	} while (hash[0] != 0 || hash[1] != 0 || (hash[2] & mask) != 0);

	i.Dump();
}