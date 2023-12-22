using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

static class Day21 {
    static List<Point> destList = new();
    static List<string> lines;
    static Point start = new();

    static SortedDictionary<string, bool> cache = new();
    static bool canReachMitte(int x, int y, int stepsLeft) {
        var key = $"{x}:{y}:{stepsLeft}";
        if (cache.ContainsKey(key))
            return cache[key];
        
        var val = canReachMitteCached(x,y,stepsLeft);
        cache.Add(key,val);
        return val;
    }
    static bool canReachMitteCached(int x, int y, int stepsLeft) {        
        if (x==start.X && y==start.Y)
            return stepsLeft%2==0;
        
        var stepsLeftMinusDist = stepsLeft-(Math.Abs(start.X-x) + Math.Abs(start.Y-y));
        if (stepsLeftMinusDist<0)
            return false;

        if (y != start.Y && lines[x][Math.Sign(start.Y-y)+y] != '#')
            if (canReachMitte(x,Math.Sign(start.Y-y)+y,stepsLeft-1))
                return true;

        if (x != start.X && lines[Math.Sign(start.X-x)+x][y] != '#')
            if (canReachMitte(Math.Sign(start.X-x)+x,y,stepsLeft-1))
                return true;
        
        if (stepsLeftMinusDist<2)
            return false;

        if (y != start.Y && lines[x][Math.Sign(start.Y-y)*-1+y] != '#')
            if (canReachMitte(x,Math.Sign(start.Y-y)*-1+y,stepsLeft-1))
                return true;

        if (x != start.X && lines[Math.Sign(start.X-x)*-1+x][y] != '#')
            if (canReachMitte(Math.Sign(start.X-x)*-1+x,y,stepsLeft-1))
                return true;

        if (stepsLeftMinusDist>0) {
            if (y==start.Y) {
                if (lines[x][y+1] != '#')
                    if (canReachMitte(x,y+1,stepsLeft-1))
                        return true;
                if (lines[x][y-1] != '#')
                    if (canReachMitte(x,y-1,stepsLeft-1))
                        return true;
            }
            if (x==start.X) {
                if (lines[x+1][y] != '#')
                    if (canReachMitte(x+1,y,stepsLeft-1))
                        return true;
                if (lines[x-1][y] != '#')
                    if (canReachMitte(x-1,y,stepsLeft-1))
                        return true;
            }
        }
        
        return false;
    }

    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
        var anzSteps=64;

//        input = """
// ...........
// .....###.#.
// .###.##..#.
// ..#.#...#..
// ....#.#....
// .##..S####.
// .##..#...#.
// .......##..
// .##.#.####.
// .##..##.##.
// ...........
// """;
//         anzSteps=6;

        lines = Helper.getLines(input);

        start.X = lines.IndexOf(lines.Single(a=>a.Contains("S")));
        start.Y = lines[start.X].IndexOf("S");

        long sumA=0;

        for (int i = Math.Max(start.X-anzSteps,0); i < Math.Min(start.X+anzSteps, lines.Count); i++)
        {
            Console.Write(i + ":");
            var diff = Math.Abs(start.X-i);
            for (int j = Math.Max(0,start.Y-(anzSteps-diff)); j < Math.Min(anzSteps-diff+start.Y+1, lines[i].Length); j++)
            {
                Console.Write(j + ",");
                if (lines[i][j] != '#') {
                    if (canReachMitte(i,j,anzSteps)) {
                        Console.Write("!");
                        sumA++;
                    }
                }
            }
            Console.WriteLine();
        }

        long sumB=0;

        Console.WriteLine("a: " + sumA);
        Console.WriteLine("b: " + sumB);
    }
}