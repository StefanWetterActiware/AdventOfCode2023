using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

static class Day14 {
    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
                                           
//         input = """
// OOOO.#.O..
// OO..#....#
// OO..O##..O
// O..#.OO...
// ........#.
// ..#....#.#
// ..O..#.O.O
// ..O.......
// #....###..
// #....#....
// """;

        IEnumerable<string> lines = Helper.getLines(input);
        lines=Helper.traverse(lines);

        var sumA = 0;
        var sumB = 0;

        Regex findOs = new("O", RegexOptions.Compiled);

        foreach (var line in lines)
        {
            var parts=line.Split('#');
            var newLine = string.Join('#',parts.Select(a=>a.Replace(".","").PadRight(a.Length,'.')));
            sumA += findOs.Matches(newLine).Select(a=>line.Length-a.Index).Sum();
        }
        Console.WriteLine($"Sum: {sumA}");

        lines = Helper.getLines(input);
        lines=Helper.turnLeft(lines);
        var ergList = new List<int>();
        ergList.Add(0); //dummy weil isso
        for (int i = 0; i < 1000/*00000*/; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                //roll
                lines = lines.Select(a=> string.Join('#',a.Split('#').Select(a=>a.Replace(".","").PadRight(a.Length,'.'))));
                //turn
                lines=Helper.turnRight(lines);
            }
            Console.Write(i + ": ");
            sumB=0;
            foreach (var line in lines) {
                sumB += findOs.Matches(line).Select(a=>line.Length-a.Index).Sum();
            }
            ergList.Add(sumB);
            Console.WriteLine($"SumB: {sumB}");
        }

        var mustertreffer = new List<int>();
        var muster = ergList.TakeLast(100).ToArray();
        for (int c = ergList.Count()-200; c >0; c--)
        {
            if (Enumerable.SequenceEqual(ergList.Skip(c).Take(100), muster)) {
                Console.WriteLine("Muster found: " + c);
                mustertreffer.Add(c);
                if (mustertreffer.Count() == 5)
                    break;
            }
        }

        var diff=mustertreffer[0]-mustertreffer[1];
        for (int i = 1; i < mustertreffer.Count()-1; i++)
        {
            if (diff != mustertreffer[i]-mustertreffer[i+1]) throw new Exception("kein muster");
        }

        var rest = 1000000000%diff;

        Console.WriteLine("Ergebnis Teil2: " + ergList[rest + 5*diff]);
    }
}