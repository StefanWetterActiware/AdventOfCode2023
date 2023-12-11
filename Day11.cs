using System.Drawing;
using System.Text.RegularExpressions;


static class Day11 {
    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var lines = Helper.getInputAsLines(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value),
                                           test:false).Select(a=>a.ToCharArray().ToList()).ToList();

        var sumA = 0;
        var sumB = 0;

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

        var diff=0;
        var newL=lines[noGalR.First()].ToArray();
        foreach (var item in noGalR)
        {
            lines.Insert(item+diff++, newL.ToList());
        }

var diffC=0;
        foreach (var item in noGalC)
        {
            foreach (var row in lines)
            {
                row.Insert(item+diffC, '.');
            }
            diffC++;
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
var calcs=0;
        for (int i = 0; i < gals.Count-1; i++)
        {
            for (int j = i+1; j < gals.Count; j++)
            {
                sumA += Math.Abs(gals[i].X-gals[j].X) + Math.Abs(gals[i].Y-gals[j].Y);
                calcs++;
            }
        }

        Console.WriteLine($"Sum: {sumA}");
        Console.WriteLine($"SumB: {sumB}");
    }
}