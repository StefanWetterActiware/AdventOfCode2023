using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

static class Day12 {
    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
                                           
//         input = """

// """;

        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(a=>a.ToCharArray()).ToArray();

        var sumA = 0;
        var sumB = 0;




        Console.WriteLine($"Sum: {sumA}");
        Console.WriteLine($"SumB: {sumB}");
    }
}