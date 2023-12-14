using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

static class Day15 {
    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
                                           
//         input = """
// OOOO.#.O..
// OO..#....#
// """;

        IEnumerable<string> lines = Helper.getLines(input);

        long sumA = 0;
        long sumB = 0;

        //Go


        Console.WriteLine("a: " + sumA);
        Console.WriteLine("b: " + sumB);
    }
}