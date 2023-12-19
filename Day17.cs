using System.Diagnostics.Contracts;
using System.Drawing;
using System.Text.RegularExpressions;

class PointWithDirectionCounter {
    public int X {get; set;}
    public int Y {get; set;}
    public char lastDirection {get; set;}
    public int lastDirCounter {get; set;}
    public Point previous { get; set;}
    public long currentCost { get; set;}
}

static class Day17 {
    static long calcCount=0;
    static Dictionary<Point, PointWithDirectionCounter> calculatedPoints = new();
    static PriorityQueue<PointWithDirectionCounter, long> calcQueue = new();

    static void calculate() {
        while (calcQueue.Count > 0) {
            var p = calcQueue.Dequeue();
            var x = p.X;
            var y = p.Y;
            var currentCost = p.currentCost;
            var previosPoint = p.previous;
            var currentForwardSteps = p.lastDirCounter;


            if (x < 0 || y < 0)
                continue;
            if (x >= lines.Count || y >= lines[0].Length)
                continue;

            calcCount++;
            if (calcCount%1000==0) Console.WriteLine($"Calculated: {calcCount}, QueueLength: {calcQueue.Count}");

            int costOfCurrentStep = int.Parse(lines[x][y].ToString());
            if (x == 0 && y == 0) costOfCurrentStep = 0;
            var costInclMe = costOfCurrentStep + currentCost;

            if (calculatedPoints.ContainsKey(lastP) && calculatedPoints[lastP].currentCost < (costInclMe + lastStepCost - 1 + lines.Count - x - 1 + lines[0].Length - y - 1))
                continue;

            var curPointPoint = new Point{X=x, Y=y};
            if (calculatedPoints.ContainsKey(curPointPoint) && calculatedPoints[curPointPoint].currentCost <= costInclMe && !(x + y == 0)) {
                continue;
            }

            bool vonLinks = previosPoint.Y < y;
            bool vonRechts = previosPoint.Y > y;
            bool vonOben = previosPoint.X < x;
            bool vonUnten = previosPoint.X > x;

            var curPoint = new PointWithDirectionCounter{X=x, Y=y, lastDirection=vonLinks?'L':vonRechts?'R':vonOben?'O':'U', lastDirCounter=currentForwardSteps, previous=previosPoint};

            curPoint.currentCost = costInclMe;

            if (!calculatedPoints.ContainsKey(curPointPoint)) {
                calculatedPoints.Add(curPointPoint, curPoint);
            } else {
                // Gut, dann haben wir n neues Optimum.
                calculatedPoints.Remove(curPointPoint);
                calculatedPoints.Add(curPointPoint, curPoint);
            }

            if (x == lines.Count -1 && y == lines.Last().Length -1) {
                Console.WriteLine($"Reached End. PointsInCache:{calculatedPoints.Count} CalcCount: {calcCount}, Cost: {costInclMe}");
                //if (!shortestway.Any() || (costInclMe < shortestway.Single().Key)) {
                //    shortestway.Clear();
                //    shortestway.Add(costInclMe, previosSteps);
                //}
                //return -9;
            }


            if (!vonRechts && (!vonLinks || currentForwardSteps < 3)) calcQueue.Enqueue(new() { currentCost=costInclMe, X=x, Y=y+1, lastDirCounter= vonLinks ? currentForwardSteps + 1 : 1, previous=curPointPoint}, lines.Count - x - 1 + lines[0].Length - y - 1 + costInclMe);
            if (!vonUnten && (!vonOben || currentForwardSteps < 3)) calcQueue.Enqueue(new() { currentCost = costInclMe, X = x+1, Y = y, lastDirCounter = vonOben ? currentForwardSteps + 1 : 1, previous = curPointPoint }, lines.Count - x - 1 + lines[0].Length - y - 1 + costInclMe);
            if (!vonLinks && (!vonRechts || currentForwardSteps < 3)) calcQueue.Enqueue(new() { currentCost = costInclMe, X = x, Y = y - 1, lastDirCounter = vonRechts ? currentForwardSteps + 1 : 1, previous = curPointPoint }, lines.Count - x - 1 + lines[0].Length - y - 1 + costInclMe);
            if (!vonOben && (!vonUnten || currentForwardSteps < 3)) calcQueue.Enqueue(new() { currentCost = costInclMe, X = x-1, Y = y, lastDirCounter = vonUnten ? currentForwardSteps + 1 : 1, previous = curPointPoint }, lines.Count - x - 1 + lines[0].Length - y - 1 + costInclMe);
        }
    }

    static List<string> lines;
    private static Dictionary<long, List<PointWithDirectionCounter>> shortestway = new();
    static int lastStepCost;
    static Point lastP;


    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));

 //       input = """
 //2413432311323
 //3215453535623
 //3255245654254
 //3446585845452
 //4546657867536
 //1438598798454
 //4457876987766
 //3637877979653
 //4654967986887
 //4564679986453
 //1224686865563
 //2546548887735
 //4322674655533
 //""";

        lines = Helper.getLines(input);

        long sumA = 0;
        long sumB=0;

        lastP = new Point(lines.Count - 1, lines.Last().Count() - 1);
        lastStepCost = int.Parse(lines.Last().Last().ToString());

        PointWithDirectionCounter startP = new PointWithDirectionCounter();
        startP.X = 0;
        startP.Y = 0;
        startP.currentCost = 0;

        calculatedPoints.Add(new Point(0, 0), startP);
        //calculateWay1(0, 0, 0, 0, new Point(0,0));
        calcQueue.Enqueue(new() { currentCost = 0, lastDirCounter = 0, previous=new Point(0,0), X=0, Y=0 }, 1);
        calculate();

        if (!calculatedPoints.ContainsKey(lastP)) { throw new ApplicationException(); }

        //var shortest = new List<Point>();
        //shortest.Add(lastP);
        //var nextPoint = calculatedPoints[lastP].previous;
        //while (!(nextPoint.X == 0 && nextPoint.Y == 0)) {
        //    shortest.Add(nextPoint);
        //    nextPoint = calculatedPoints[nextPoint].previous;
        //}
        //shortest.Add(new Point(0, 0));


        Console.WriteLine(DateTime.Now.ToString());
        Console.WriteLine("a: " + calculatedPoints[lastP].currentCost);
        Console.WriteLine("b: " + sumB);
    }
}