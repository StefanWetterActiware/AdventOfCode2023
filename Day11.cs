using System.Drawing;
using System.Text.RegularExpressions;


static class Day11 {
    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var lines = Helper.getInputAsLines(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value),
                                           test:false).Select(a=>a.ToCharArray().ToList()).ToList();

        var sumA = 0;
        var sumB = 0L;

var noGalR = new List<int>();
        for (int i = 0; i < lines.Count; i++)
        {
            if (!(new string(lines[i].ToArray()).Contains('#')))
            noGalR.Add(i);
        }

var noGalC=new List<int>();
        for (int i = 0; i < lines.First().Count; i++)
        {
            if (!(lines.Any(a => a[i] == '#')))
                noGalC.Add(i);
        }

        var gals = new List<Point>();
        for (int i = 0; i < lines.Count(); i++)
        {
            for (int j = 0; j < lines[i].Count; j++)
            {
                if (lines[i][j] == '#')
                    gals.Add(new Point(j,i));
            }
        }

        foreach (var item in lines)
        {
            Console.WriteLine(new string(item.ToArray()));
        }

        var factor=1_000_000L;
        for (int i = 0; i < gals.Count-1; i++)
        {
            for (int j = i+1; j < gals.Count; j++)
            {
                sumA += Math.Abs(gals[i].X-gals[j].X) + Math.Abs(gals[i].Y-gals[j].Y)
                        + noGalR.Where(a=> a>Math.Min(gals[i].Y,gals[j].Y) && a<Math.Max(gals[i].Y,gals[j].Y)).Count()
                        + noGalC.Where(a=> a>Math.Min(gals[i].X,gals[j].X) && a<Math.Max(gals[i].X,gals[j].X)).Count();
                sumB += Math.Abs(gals[i].X-gals[j].X) + Math.Abs(gals[i].Y-gals[j].Y)
                        + noGalR.Where(a=> a>Math.Min(gals[i].Y,gals[j].Y) && a<Math.Max(gals[i].Y,gals[j].Y)).Count()*(factor-1)
                        + noGalC.Where(a=> a>Math.Min(gals[i].X,gals[j].X) && a<Math.Max(gals[i].X,gals[j].X)).Count()*(factor-1);
            }
        }

        Console.WriteLine($"Sum: {sumA}");
        Console.WriteLine($"SumB: {sumB}");
    }
}