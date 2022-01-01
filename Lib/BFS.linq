<Query Kind="Program">
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <Namespace>Wintellect.PowerCollections</Namespace>
</Query>

void Main()
{
	(int dy, int dx)[] dirs = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

	var bfs = new BFS<((int x, int y) loc, int d)>(
		init: ((0, 0), 0),
		next: (p) =>
		{
			return from dir in dirs select ((p.loc.x + dir.dx, p.loc.y + dir.dy), p.d + 1);
		},
		isTerminal: (p) => Math.Abs(p.loc.x) + Math.Abs(p.loc.y) == 6,
		isInteresting: (p) => Math.Abs(p.loc.x) + Math.Abs(p.loc.y) >= 5,
		loc: L => L.loc
	);
	
	bfs.Search().Dump();
}

// Define other methods, classes and namespaces here
public class BFS<TState>
{
	Func<TState,IEnumerable<TState>> _next;
	Func<TState,bool> _isTerminal;
	Func<TState,bool> _isInteresting;
	Func<TState,int> _weight;
	Func<TState,object> _loc;
	OrderedBag<TState> _frontier;
	HashSet<object> _visited;
	
	public BFS(TState init, Func<TState,IEnumerable<TState>> next, Func<TState, bool> isTerminal, Func<TState, bool> isInteresting = null, Func<TState, object> loc = null, Func<TState, int> weight = null)
	{
		if (weight == null)
		{
			weight = (_) => 1;
		}
		
		if (loc == null)
		{
			loc = x => x;
		}
		
		if (isInteresting == null)
		{
			isInteresting = isTerminal;
		}
		
		_loc = loc;
		_weight = weight;
		_next = next;
		_isTerminal = isTerminal;
		_isInteresting = isInteresting;
		_frontier = new OrderedBag<TState>(Comparer<TState>.Create((a,b) => _weight(a) - _weight(b)));
		_frontier.Add(init);
		_visited = new HashSet<object>();
	}
	
	public BFS(TState init, Func<TState,TState> next, Func<TState, bool> isTerminal, Func<TState, bool> isInteresting, Func<TState, object> loc, Func<TState, int> weight = null)
	{
		if (weight == null)
		{
			weight = (_) => 1;
		}

		if (loc == null)
		{
			loc = x => x;
		}

		if (isInteresting == null)
		{
			isInteresting = isTerminal;
		}

		_loc = loc;
		_weight = weight;
		_next = x => Enumerable.Repeat(next(x),1);
		_isTerminal = isTerminal;
		_isInteresting = isInteresting;
		_frontier = new OrderedBag<TState>(Comparer<TState>.Create((a,b) => _weight(a) - _weight(b)));
		_frontier.Add(init);		
		_visited = new HashSet<object>();
	}

	public IEnumerable<TState> Search()
	{
		while (_frontier.Count > 0)
		{
			var current = _frontier.RemoveFirst();
			
			if (_isInteresting(current))
			{
				yield return current;
			}
			
			if (!_isTerminal(current))
			{
				var next = _next(current).Where(x => !_visited.Contains(_loc(x)));

				foreach (var nx in next)
				{
					_frontier.Add(nx);
					_visited.Add(_loc(nx));
				}
			}
		}
	}
}