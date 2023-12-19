using System.Diagnostics.Contracts;
using System.Drawing;
using System.Text.RegularExpressions;

class PointWithDirectionCounter {
    public int X {get; set;}
    public int Y {get; set;}
    public char lastDirection {get; set;}
    public int lastDirCounter {get; set;}
}

static class Day17 {
    static long calcCount=0;
    static /*Priority*/Queue<Point/*, int*/> openList = new();

    static long calculateWay1(long currentCost, int currentForwardSteps, int x, int y, List<PointWithDirectionCounter> previosSteps) {
        if (x < 0 || y < 0) return -1;
        if (x >= lines.Count || y >= lines[0].Length) return -1;

        bool vonLinks = previosSteps.Any() && previosSteps.Last().Y < y;
        bool vonRechts = previosSteps.Any() && previosSteps.Last().Y > y;
        bool vonOben = previosSteps.Any() && previosSteps.Last().X < x;
        bool vonUnten = previosSteps.Any() && previosSteps.Last().X > x;

        var curPoint = new PointWithDirectionCounter{X=x, Y=y, lastDirection=vonLinks?'L':vonRechts?'R':vonOben?'O':'U', lastDirCounter=currentForwardSteps};

        if (previosSteps.Contains(curPoint)) return -2;

        calcCount++;

        //Klon anlegen
        previosSteps = previosSteps.ToList();
        previosSteps.Add(curPoint);

        int costOfCurrentStep = int.Parse(lines[x][y].ToString());
        if (x==0&&y==0) costOfCurrentStep=0;
        var costInclMe = costOfCurrentStep + currentCost;

        if (x == lines.Count -1 && y == lines.Last().Length -1) {
            Console.WriteLine($"Reached End. CalcCount: {calcCount}, Steps: {previosSteps.Count()}, Cost: {costInclMe}");
            if (!shortestway.Any() || (costInclMe < shortestway.Single().Key)) {
                shortestway.Clear();
                shortestway.Add(costInclMe, previosSteps);
            }
            return -9;
        }

        if (shortestway.Any() && shortestway.Single().Key < (costInclMe + lastStepCost-1 + lines.Count - x-1 + lines[0].Length-y-1)) return -5;


        if (!vonRechts && (!vonLinks || currentForwardSteps < 3)) calculateWay1(costInclMe, vonLinks ? currentForwardSteps + 1 : 1, x, y+1, previosSteps);
        if (!vonUnten && (!vonOben || currentForwardSteps < 3)) calculateWay1(costInclMe, vonOben ? currentForwardSteps + 1 : 1, x+1, y, previosSteps);
        if (!vonLinks && (!vonRechts || currentForwardSteps < 3)) calculateWay1(costInclMe, vonRechts ? currentForwardSteps + 1 : 1, x, y-1, previosSteps);
        if (!vonOben && (!vonUnten || currentForwardSteps < 3)) calculateWay1(costInclMe, vonUnten ? currentForwardSteps + 1 : 1, x-1, y, previosSteps);

        return 1;
    }

    static List<string> lines;
    private static Dictionary<long, List<PointWithDirectionCounter>> shortestway = new();
    static int lastStepCost;

    static void goCalc() {
        while(openList.Any()){

        }
    }

    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
                                           
//         input = """
// 2413432311323
// 3215453535623
// 3255245654254
// 3446585845452
// 4546657867536
// 1438598798454
// 4457876987766
// 3637877979653
// 4654967986887
// 4564679986453
// 1224686865563
// 2546548887735
// 4322674655533
// """;

        lines = Helper.getLines(input);

        long sumA = 0;
        long sumB=0;

        // lastStepCost = int.Parse(lines.Last().Last().ToString());
        // calculateWay1(0, 0, 0, 0, new());

        openList.Enqueue(new Point(0,1));
        openList.Enqueue(new Point(1,0));
        goCalc();

        Console.WriteLine("a: " + shortestway.Single().Key);
        Console.WriteLine("b: " + sumB);
    }
}