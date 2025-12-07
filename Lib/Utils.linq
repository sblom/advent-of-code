<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

public static partial class AoC
{
    public static DumpContainer[] _outputs = new DumpContainer[2];
    
    public static IEnumerable<string> GetLines(this string lines)
    {
        return lines.Trim().Split("\n").Select(line => line.Trim());
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
            contents.DumpFixed("Downloaded");
        }
        return File.ReadLines(inputName);
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
    
    public static T Dump1<T>(this T output)
    {        
        if (_outputs[0] == null)
        {
            _outputs[0] = new DumpContainer(output).Dump();
        }
        else
        {
            _outputs[0].Content = output;
        }
        
        return output;
    }
    public static T Dump2<T>(this T output)
    {
        if (_outputs[1] == null)
        {
            _outputs[1] = new DumpContainer(output).Dump();
        }
        else
        {
            _outputs[1].Content = output;
        }
        
        return output;
    }

}