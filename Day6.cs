using System.Text.RegularExpressions;


static class Day6{
    internal static void doit(){
        Regex dayNoR = new(@"\d*$");

        var lines = Helper.getInputAsLines(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value),
                                           test:false);
        
        
        long sumA = 1;
        long sumB = 0;

        var times = lines.First().Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(a=>long.Parse(a));
        var records = lines.Last().Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(a=>long.Parse(a));

        for (int i = 0; i < times.Count(); i++)
        {
            var time=times.Skip(i).First();
            var rec=records.Skip(i).First();

            long count=0;
            for (long j = 0; j <= time; j++)
            {
                var meters = (time-j)*j;
                if (meters>rec) count++;
            }
            sumA*=count;
        }
        
        var time2 = long.Parse(lines.First().Split(":")[1].Replace(" ",""));
        var rec2 = long.Parse(lines.Last().Split(":")[1].Replace(" ",""));
        
        for (long j = 0; j <= time2; j++)
        {
            var meters = (time2-j)*j;
            if (meters>rec2) sumB++;
        }

        Console.WriteLine($"Sum: {sumA}");
        Console.WriteLine($"SumB: {sumB}");
    }
}