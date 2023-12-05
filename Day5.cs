using System.Diagnostics.Tracing;
using System.Text.RegularExpressions;


static class Day5{
    internal static void doit(){
        Regex dayNoR = new(@"\d*$");
        var lines = Helper.getInputAsLines(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value),
                                           test:false);
        lines.Append("");

        var sumA = 0;
        var sumB = 0;

        var startseeds = lines.First().Split(':')[1].Trim().Split(' ',StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(a => long.Parse(a)).ToList();
        var levelvals = new List<List<long>>();
        levelvals.Add(startseeds);

        var step = (List<List<long>> tab) => {
            if (tab.Any()) {
                var newLine = new List<long>();
                foreach (var source in levelvals.Last())
                {
                    var f = tab.Where(a=> source >= a[1] && source < a[1]+a[2]);
                    if (f.Any()) {
                        var a = f.First();
                        newLine.Add(source - (a[1]-a[0]));
                    } else {
                        newLine.Add(source);
                    }
                }
                levelvals.Add(newLine);
            }
        };

        var tab = new List<List<long>>();
        foreach (var line in lines.Skip(1))
        {
            if (!String.IsNullOrWhiteSpace(line)){
                if (line.EndsWith("ap:")) {
                    step(tab);
                    tab.Clear();
                } else {
                    var l = line.Split(' ', StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries);
                    var d = long.Parse(l[0]);
                    var s = long.Parse(l[1]);
                    var c = long.Parse(l[2]);
                    tab.Add(new List<long>(){d, s, c});
                }
            }
        }
        step(tab);


        Console.WriteLine($"Lowest: {levelvals.Last().Min()}");
        Console.WriteLine($"SumB: {sumB}");
    }
}