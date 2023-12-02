using System.Diagnostics;
using System.Reflection.Metadata;

static class Helper
{
    private static string cachefileTempl = "input/day{0}";

    private static string getCacheFileName(int tag) {
        return string.Format(cachefileTempl, tag);
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
    public static IEnumerable<string> getInputAsLines(int tag){
        loadInput(tag);
        return System.IO.File.ReadAllLines(getCacheFileName(tag)).Where(a=> !String.IsNullOrWhiteSpace(a));
    }

    [DebuggerHidden]
    public static string getInput(int tag){
        loadInput(tag);
        return System.IO.File.ReadAllText(getCacheFileName(tag)).TrimEnd("@\r\n".ToCharArray());
    }

}