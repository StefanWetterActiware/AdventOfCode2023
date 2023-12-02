using System.Text.RegularExpressions;


static class Day3{
    internal static void doit(){
        Regex dayNoR = new(@"\d*$");
        var lines = Helper.getInputAsLines(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
        
        var sumA = 0;
        var sumB = 0;


        Console.WriteLine($"Sum: {sumA}");
        Console.WriteLine($"SumB: {sumB}");
    }
}