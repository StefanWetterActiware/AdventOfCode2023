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

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
                                           
        input = """
.F----7F7F7F7F-7....
.|F--7||||||||FJ....
.||.FJ||||||||L7....
FJL7L7LJLJ||LJ.L-7..
L--J.L7...LJS7F-7L7.
....F-J..F7FJ|L7L7L7
....L7.F7||L7|.L7L7|
.....|FJLJ|FJ|F7|.LJ
....FJL-7.||.||||...
....L---J.LJ.LJLJ...
""";

        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(a=>a.ToCharArray()).ToArray();

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

        //alles voll I
        for (int i = 0; i < lines.Count(); i++)
        {
            for (int j = 0; j < lines[i].Count(); j++)
            {
                lines[i][j] = 'I';
            }
            
        }
        //pipe rein
        foreach (var item in list)
        {
            lines[item.point.Y][item.point.X] = item.type;
        }

        //jetzt 'O's füllen vom Rand
        //TODO

        foreach (var item in list)
        {
            var nachbarnLinks = new List<Point>();
            if (item.type  == '|' ) {
                if (item.isStart)
                    nachbarnLinks.Add(new Point(item.point.X + 1 ,item.point.Y));
                else
                    nachbarnLinks.Add(new Point(item.point.X + (item.point.Y - item.previous.Y) ,item.point.Y));
            }
            else if (item.type  == '-' )
                nachbarnLinks.Add(new Point(item.point.X ,item.point.Y - (item.point.X-item.previous.X)));
            else if (item.type  == '7' ){
                if ((item.point.X-item.previous.X) > 0){//kommt von links ->Außenecke
                    nachbarnLinks.Add(new Point(item.point.X ,item.point.Y-1));
                    nachbarnLinks.Add(new Point(item.point.X+1 ,item.point.Y-1));
                    nachbarnLinks.Add(new Point(item.point.X+1 ,item.point.Y));
                }
            } else if (item.type  == 'F' ){
                if ((item.point.Y-item.previous.Y) < 0){//kommt von unten ->Außenecke
                    nachbarnLinks.Add(new Point(item.point.X-1 ,item.point.Y));
                    nachbarnLinks.Add(new Point(item.point.X-1 ,item.point.Y-1));
                    nachbarnLinks.Add(new Point(item.point.X ,item.point.Y-1));
                }
            } else if (item.type  == 'L' ){
                if ((item.point.X-item.previous.X) < 0){//kommt von rechts ->Außenecke
                    nachbarnLinks.Add(new Point(item.point.X ,item.point.Y+1));
                    nachbarnLinks.Add(new Point(item.point.X-1 ,item.point.Y+1));
                    nachbarnLinks.Add(new Point(item.point.X-1 ,item.point.Y));
                }
            } else if (item.type  == 'J' ){
                if ((item.point.Y-item.previous.Y) > 0){//kommt von oben ->Außenecke
                    nachbarnLinks.Add(new Point(item.point.X+1 ,item.point.Y));
                    nachbarnLinks.Add(new Point(item.point.X+1 ,item.point.Y+1));
                    nachbarnLinks.Add(new Point(item.point.X ,item.point.Y+1));
                }
            }

            foreach (var nachbar in nachbarnLinks)
            {
                if (nachbar.X >= 0 && nachbar.X < lines.First().Count() && nachbar.Y >= 0 && nachbar.Y < lines.Count())
                    if (lines[nachbar.Y][nachbar.X] == ' ')
                        lines[nachbar.Y][nachbar.X] = 'I';
            }
        }

        Console.WriteLine("");
        foreach (var item in lines)
        {
            Console.WriteLine(new string(item));
            sumB += System.Text.RegularExpressions.Regex.Matches(new string(item), "I").Count;
        }

        Console.WriteLine($"Sum: {list.Count()/2}");
        Console.WriteLine($"SumB: {sumB}");
    }
}