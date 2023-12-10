using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public class Pipepoint {
    public Point point {get; set;}
    public long stepcount {get;set;}
    public char type {get;set;}
    public Point previous {get;set;}
    public bool isStart {get; set;} = false;
}

static class Day10 {
    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var lines = Helper.getInputAsLines(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value),
                                           test:false).Select(a=>a.ToCharArray()).ToArray();


        var sumB = 0;

        var pipe = new List<Point>(){new Point(0,-1), new Point(0,1)};
        var minus = new List<Point>(){new Point(1,0), new Point(-1,0)};
        var ell = new List<Point>(){new Point(1,0), new Point(0,-1)};
        var jot = new List<Point>(){new Point(-1,0), new Point(0,-1)};
        var sieben = new List<Point>(){new Point(-1,0), new Point(0,1)};
        var ef = new List<Point>(){new Point(1,0), new Point(0,1)};

        Point sp = new(-1,-1);
        for (int i = 0; i < lines.Count(); i++)
        {
            for (int j = 0; j < lines[i].Count(); j++)
            {
                if (lines[i][j] == 'S'){
                    sp = new Point(j,i);
                    break;
                }
            }
            
        }


        var start = new Pipepoint{point = sp,type='|',stepcount=0,isStart=true};
        var list = new List<Pipepoint>(){start};
        var second = new Pipepoint{point = new Point(start.point.X, start.point.Y+1),type=lines[start.point.Y+1][start.point.X],stepcount=1,previous=start.point};
        list.Add(second);
        while (list.Last().point != start.point && list.Count>1){
            var prev = list.Last().previous;
            var cur = list.Last().point;
            var next = new Pipepoint{stepcount = list.Last().stepcount+1, previous=list.Last().point};
            var nextCnds = new List<Point>();
            switch (list.Last().type)
            {
                case '|':
                    nextCnds.Add(new Point(cur.X+pipe.First().X, cur.Y + pipe.First().Y));
                    nextCnds.Add(new Point(cur.X+pipe.Last().X, cur.Y + pipe.Last().Y));
                    break;
                case '-':
                    nextCnds.Add(new Point(cur.X+minus.First().X, cur.Y + minus.First().Y));
                    nextCnds.Add(new Point(cur.X+minus.Last().X, cur.Y + minus.Last().Y));
                    break;
                case 'F':
                    nextCnds.Add(new Point(cur.X+ef.First().X, cur.Y + ef.First().Y));
                    nextCnds.Add(new Point(cur.X+ef.Last().X, cur.Y + ef.Last().Y));
                    break;
                case 'J':
                    nextCnds.Add(new Point(cur.X+jot.First().X, cur.Y + jot.First().Y));
                    nextCnds.Add(new Point(cur.X+jot.Last().X, cur.Y + jot.Last().Y));
                    break;
                case 'L':
                    nextCnds.Add(new Point(cur.X+ell.First().X, cur.Y + ell.First().Y));
                    nextCnds.Add(new Point(cur.X+ell.Last().X, cur.Y + ell.Last().Y));
                    break;
                case '7':
                    nextCnds.Add(new Point(cur.X+sieben.First().X, cur.Y + sieben.First().Y));
                    nextCnds.Add(new Point(cur.X+sieben.Last().X, cur.Y + sieben.Last().Y));
                    break;
                default:
                    throw new ArgumentException();
            }
            
            next.point=nextCnds.Where(a => !a.Equals(prev)).Single();
            next.type=lines[next.point.Y][next.point.X];
            if (next.type=='S') break;
            list.Add(next);
        }

        Console.WriteLine($"Sum: {list.Count()/2}");
        Console.WriteLine($"SumB: {sumB}");
    }
}