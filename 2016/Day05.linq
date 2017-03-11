<Query Kind="Statements">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

string pwd = "wtnhxymk";
int pwdLen = pwd.Length;
byte[] tmpSource = new byte[pwdLen + 12];
byte[] numStr = new byte[12];
byte[] tmpHash;

int num = 0;
string password = "________";

var hasher = new MD5CryptoServiceProvider();

for (int i = 0; i < pwdLen; ++i)
{
	tmpSource[i] = (byte)pwd[i];
}

for (int i = 0; i < int.MaxValue; ++i)
{
	int pos = numStr.Length - 1;
	int loopnum = i;
	if (loopnum == 0)
	{
		pos--;
		tmpSource[pwdLen] = (byte)'0';
	}
	else
	{
		while (loopnum > 0)
		{
			numStr[pos--] = (byte)('0' + (loopnum % 10));
			loopnum = loopnum / 10;
		}

		for (int j = 0; j + pos + 1 < numStr.Length; ++j)
		{
			tmpSource[pwdLen + j] = numStr[pos + j + 1];
		}
	}
	
	if (i % 500000 == 0) $"{i / 1000}k".Dump();
	tmpHash = hasher.ComputeHash(tmpSource, 0, pwdLen + numStr.Length - pos - 1);

	if (tmpHash[0] == 0 && tmpHash[1] == 0 && (tmpHash[2] & 0xf0) == 0)
	{
		string hex = BitConverter.ToString(tmpHash).Replace("-", "");

		if (hex[5] < '0' || hex[5] > '7') continue;
		var loc = hex[5] - '0';
		if (password[loc] == '_')
		{
			password = password.Substring(0,loc) + hex[6].ToString() + password.Substring(loc+1);
			password.Dump();
			num++;
			if (num == 8) break;
		}
	}
}

password.Dump();