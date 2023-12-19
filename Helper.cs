using System.Diagnostics;
using System.Reflection.Metadata;

static class Helper
{
    private static string cachefileTempl = "input/day{0}";

    [DebuggerHidden]
    private static string getCacheFileName(int tag, bool test = false) {
        return string.Format(cachefileTempl, tag) + (test ? "test":"");
    }
    
    [DebuggerHidden]
    public static void loadInput(int tag)
    {
        string urltempl = "https://adventofcode.com/2023/day/{0}/input";

        System.IO.Directory.CreateDirectory("input");

        if (System.IO.File.Exists(getCacheFileName(tag))) return;

        string url = string.Format(urltempl, tag);

        HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Cookie", "session=53616c7465645f5f3adbd65bfe3e53fc8f064a5ba4be40d5a029aee26d2012bda4f38d5aec5b9a9ec60f902755e34fc4a1012b6aec9c4db5d9d85c13299a3cfc");

        System.IO.File.WriteAllText(getCacheFileName(tag), httpClient.GetStringAsync(url).GetAwaiter().GetResult());
    }

    [DebuggerHidden]
    public static IEnumerable<string> getInputAsLines(int tag, bool test = false){
        loadInput(tag);
        return System.IO.File.ReadAllLines(getCacheFileName(tag,test)).Where(a=> !String.IsNullOrWhiteSpace(a));
    }

    [DebuggerHidden]
    public static string getInput(int tag, bool test = false){
        loadInput(tag);
        return System.IO.File.ReadAllText(getCacheFileName(tag, test)).TrimEnd("@\r\n".ToCharArray());
    }

    [DebuggerHidden]
    public static IEnumerable<IEnumerable<string>> getBlocks(IEnumerable<string> lines) {
        return getBlocks(String.Join("\n", lines));
    }
    [DebuggerHidden]
    public static IEnumerable<List<string>> getBlocks(string input) {
        return input.Split("\n\n").Select(a => a.Split("\n").ToList());
    }
    [DebuggerHidden]
    public static List<string> getLines(string input) {
        return input.Trim('\n').Split("\n").Select(a=>a.TrimEnd('\r')).ToList();
    }

    public static IEnumerable<string> traverse(IEnumerable<string> lines) {
        var colStrings = new List<string>();
        for (int colNo = 0; colNo < lines.First().Length; colNo++)
        {
            colStrings.Add(String.Join("",lines.Select(a=>a[colNo])));
        }
        return colStrings;
    }

    public static IEnumerable<string> turnRight(IEnumerable<string> lines) {
        var colStrings = new List<string>();
        for (int colNo = 0; colNo < lines.First().Count(); colNo++)
        {
            colStrings.Add(String.Join("",lines.Reverse().Select(a=>a[colNo])));
        }
        return colStrings;
    }

    public static IEnumerable<string> turnLeft(IEnumerable<string> lines) {
        var colStrings = new List<string>();
        for (int colNo = lines.First().Count()-1; colNo >= 0; colNo--)
        {
            colStrings.Add(String.Join("",lines.Select(a=>a[colNo])));
        }
        return colStrings;
    }

}