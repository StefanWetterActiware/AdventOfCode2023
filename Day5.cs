using System.Diagnostics.Tracing;
using System.Text.RegularExpressions;


static class Day5{
    internal static void doit(){
        Regex dayNoR = new(@"\d*$");
        var lines = Helper.getInputAsLines(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value),
                                           test:false);
        lines.Append("");

        var valB = long.MaxValue;

        var startseeds = lines.First().Split(':')[1].Trim().Split(' ',StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(a => long.Parse(a)).ToList();
        var levelvals = new List<List<long>>();
        levelvals.Add(startseeds);

        var allTabs = new List<List<List<long>>>();

        var oneStep = (List<List<long>> tab, long no) => {
            var f = tab.Where(a=> no >= a[1] && no < a[1]+a[2]);
            if (f.Any()) {
                var a = f.First();
                return no - (a[1]-a[0]);
            } else {
                return no;
            }
        };

        var step = (List<List<long>> tab) => {
            if (tab.Any()) {
                allTabs.Add(tab);

                var newLine = new List<long>();
                foreach (var source in levelvals.Last())
                {
                    newLine.Add(oneStep(tab, source));
                }
                levelvals.Add(newLine);
            }
        };

        var allSteps = (long no) => {
            foreach (var lev in allTabs)
            {
                no=oneStep(lev, no);
            }
            return no;
        };

        var tab = new List<List<long>>();
        foreach (var line in lines.Skip(1))
        {
            if (!String.IsNullOrWhiteSpace(line)){
                if (line.EndsWith("ap:")) {
                    step(tab);
                    tab=new();
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

        //Völlige Gurkenlösung. Brute Force. Nach 2/3 auf gut Glück den aktuellen Min-Wert eingegeben und passt. Glück gehabt
        // Besser wäre vermutlich einfach, bei jeder Berechnung zurückzubekommen,
        // wie viele Zahlen ab der zuletzt berechneten in allen Stufen noch ins gleiche Raster fallen, und die dann einfach zu überspringen
        for (int i = 0; i < startseeds.Count; i+=2)
        {
            for (long j = startseeds[i]; j < startseeds[i] + startseeds[i+1]; j++)
            {
                valB=Math.Min(valB, allSteps(j));
            }
        }

        Console.WriteLine($"LowestA: {levelvals.Last().Min()}");
        Console.WriteLine($"LowestB: {valB}");
    }
}