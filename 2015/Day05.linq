<Query Kind="Statements" />

// In[1]:= vowel = "a" | "e" | "i" | "o" | "u"

// In[46]:= triplevowel = ___ ~~vowel ~~___ ~~vowel ~~___ ~~ vowel ~~___

// In[18]:= doubleletter = ___ ~~x_ ~~x_ ~~___

// In[35]:= banned = ___ ~~("ab" | "cd" | "pq" | "xy") ~~___

// In[61]:= NiceQ = StringMatchQ[#, triplevowel] && StringMatchQ[#, doubleletter] && ! StringMatchQ[#, banned] &

// In[60]:= strings = ReadList["C:/Users/sblom/Documents/LINQPad Queries/Advent/2015/Day05.txt", String]

// In[64]:= Length[Select[strings, NiceQ]]

// Out[64] = 25

// In[56]:= Length[strings]

// Out[56] = 1000

// In[69]:= Nice2Q = StringMatchQ[#, ___ ~~ x_ ~~ y_ ~~ ___ ~~ x_ ~~ y_ ~~ ___] && StringMatchQ[#, ___ ~~ x_ ~~ _ ~~ x_ ~~ ___] &

// In[70]:= Length[Select[strings, Nice2Q]]

// Out[70] = 55