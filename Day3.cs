using System.Drawing;
using System.Text.RegularExpressions;


static class Day3{
    internal static void doit(){
        Regex dayNoR = new(@"\d*$");
        var lines = Helper.getInputAsLines(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value),
                                           test:false);
        
        var sumA = 0;
        var sumB = 0;

        Regex num = new(@"\d+");
        Regex sign = new(@"[^\d\.]");
        Regex gearR = new(@"\*");

        List<Point> pl = new();
        Dictionary<Point,List<int>> gearCand = new();
        var lNo = 0;
        foreach (string line in lines)
        {
            foreach (Match sMat in sign.Matches(line))
            {
                pl.Add(new(lNo,sMat.Index));
            }
            foreach (Match sMat in gearR.Matches(line))
            {
                gearCand.Add(new(lNo,sMat.Index), new());
            }
            lNo++;
        }

        lNo = 0;
        foreach (string line in lines)
        {
            foreach (Match zM in num.Matches(line))
            {
                var p = pl.Where(a => a.X >= lNo -1 && a.X <= lNo+1 && a.Y >= zM.Index - 1 && a.Y <= zM.Index + zM.Length);
                if (p.Any()) {
                    sumA+=int.Parse(zM.Value);
                    Console.WriteLine($"Drin: {lNo+1},{zM.Index}: {zM.Value}");
                    if (p.Count()>1){
                        Console.WriteLine("achtung");
                    }
                    if (gearCand.ContainsKey(p.First())){
                        gearCand[p.First()].Add(int.Parse(zM.Value));
                    }
                }
                else{
                    Console.WriteLine($"Aussortiert: {lNo+1},{zM.Index}: {zM.Value}");
                }
            }
            lNo++;
            Console.WriteLine();
        }

        sumB=gearCand.Where(a => a.Value.Count() == 2).Select(a=> a.Value.First() * a.Value.Last()).Sum();

        Console.WriteLine($"Sum: {sumA}");
        Console.WriteLine($"SumB: {sumB}");
    }
}
