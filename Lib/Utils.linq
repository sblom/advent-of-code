<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Windows</Namespace>
</Query>

public static partial class AoC
{
	public static Stopwatch timer = new Stopwatch();
	
    public static IEnumerable<string> GetLines(this string str)
    {
		var lines = str.Trim().Split("\n").Select(line => line.Trim());

		Init();

        return lines;
    }

    public static IEnumerable<string> GetLines()
    {
        var curqp = Util.CurrentQueryPath;
        var dirname = Path.GetDirectoryName(curqp);
        Directory.SetCurrentDirectory(dirname);
        var inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp).Replace("-cs", "") + ".txt");
        return File.ReadLines(inputName);
    }
	
    public static async Task<IEnumerable<string>> GetLinesWeb()
    {
	
        Util.CreateSynchronizationContext();
        var curqp = Util.CurrentQueryPath;
        var dirname = Path.GetDirectoryName(curqp);
        Directory.SetCurrentDirectory(dirname);
        var inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp).Replace("-cs", "").Replace("-2.", ".") + ".txt");
        var rx = new Regex(@"(?<year>\d+)\\\D+(?<day>\d+)(-[^.]+)?.txt$");
        var match = rx.Match(inputName);
        var url = $"http://adventofcode.com/{match.Groups["year"]}/day/{match.Groups["day"]}/input";
        if (!File.Exists(inputName))
        {
            Console.WriteLine($"Downloading input: {url}");
            var contents = await DownloadInput(int.Parse(match.Groups["year"].Value), int.Parse(match.Groups["day"].Value));
            File.WriteAllText(inputName, contents);
            contents.Dump("Downloaded");
        }
		
		var lines = File.ReadLines(inputName);

		Init();

        return lines;
    }

    public static async Task<string> DownloadInput(int year, int day)
    {
        using (var handler = new HttpClientHandler { UseCookies = false })
        using (var hc = new HttpClient(handler))
        {
            var hr = new HttpRequestMessage(HttpMethod.Get, $"http://adventofcode.com/{year}/day/{day}/input");
            hr.Headers.Add("Cookie", $"session={Util.LoadString("adventofcode: session")}");
            var resp = await hc.SendAsync(hr);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }
    }
	
	public static bool hasDumpedOne = false;
	
	public static DumpContainer dc1 = new DumpContainer();
	public static DumpContainer dc2 = new DumpContainer();

	public static object Dump1(this object d, bool keepLatest = false)
	{
		if (keepLatest || dc1.Content.GetType().FullName.Dump() != "LINQPad.ObjectGraph.RawHtml")
		{
			
			dc1.UpdateContent(d);
			dc1.AppendContent(timer.Elapsed);
			
			// Clipboard.SetText(d.ToString());
		}

		return d;
	}

	public static object Dump2(this object d, bool keepLatest = false)
	{
		if (keepLatest || dc2.Content.GetType().FullName != "LINQPad.ObjectGraph.RawHtml")
		{

			dc2.UpdateContent(d);
			dc2.AppendContent(timer.Elapsed);

			// Clipboard.SetText(d.ToString());
		}

		return d;
	}
	
	public static void Init()
	{
		dc1.ClearContent();
		dc2.ClearContent();
		
		Util.HorizontalRun("Part 1,Part 2",dc1,dc2).Dump();
		
		hasDumpedOne = false;
		timer.Reset();
		timer.Start();
	}
}