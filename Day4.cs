using System.Text.RegularExpressions;


static class Day4{
    internal static void doit(){
        Regex dayNoR = new(@"\d*$");
        var lines = Helper.getInputAsLines(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value),
                                           test:false);
        
        double sumA = 0;
        var sumB = 0;

        foreach (var line in lines)
        {
            var parts = line.Split(":");
            var nos = parts[1].Split("|");
            var winners = nos[0].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var mine = nos[1].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var anz = mine.Where(a=>winners.Contains(a)).Count();

            if (anz>0){
            var val = Math.Pow(2,anz-1);
            sumA+=val;
            }
        }


        Console.WriteLine($"Sum: {sumA}");
        Console.WriteLine($"SumB: {sumB}");
    }
}