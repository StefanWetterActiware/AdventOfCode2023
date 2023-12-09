using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


static class Day9 {
    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var lines = Helper.getInputAsLines(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value),
                                           test:false);


        var sumA = 0L;
        var sumB = 0L;

        foreach (var line in lines)
        {
            List<long[]> all=new();
            all.Add(line.Split(" ",StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.RemoveEmptyEntries).Select(a=>long.Parse(a)).ToArray());
            var offset = all.First().First();

            while(!all.Last().All(a=>a==0)){
                var nextL = new List<long>();
                for (int i = 0; i < all.Last().Count()-1; i++)
                {
                    nextL.Add(all.Last()[i+1] - all.Last()[i]);
                }
                all.Add(nextL.ToArray());
            }
            
            all.Reverse();

            long last=all.Skip(1).First().First();
            long prev=last;
            for (int i = 2; i < all.Count; i++)
            {
                last = all[i].Last() + last;
                prev = all[i].First() - prev;
            }
            sumA+=last;
            sumB+=prev;
        }

        Console.WriteLine($"Sum: {sumA}");
        Console.WriteLine($"SumB: {sumB}");
    }
}