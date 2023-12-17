using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

/* WORST SOLUTION EVER. DON'T READ...
   Something went wrong, columns & rows are switched, i don't know how that was. But finally it worked...
*/


public class Pipepoint {
    public Point point {get; set;}
    public long stepcount {get;set;}
    public char type {get;set;}
    public Point previous {get;set;}
    public bool isStart {get; set;} = false;
}

static class Day10 {
    internal static void floodFill(int x, int y, ref List<List<char>> lines){
        if (x < 0 || y < 0) return;
        if (x>= lines.Count()) return;
        if (y>= lines[0].Count()) return;
        if (lines[x][y] == 'x') return;
        if (lines[x][y] == 'o') return;
        lines[x][y] = 'o';

        floodFill(x, y+1, ref lines);
        floodFill(x, y-1, ref lines);
        floodFill(x+1, y, ref lines);
        floodFill(x-1, y, ref lines);
    }

    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
                                           
//         input = """
// .F----7F7F7F7F-7....
// .|F--7||||||||FJ....
// .||.FJ||||||||L7....
// FJL7L7LJLJ||LJ.L-7..
// L--J.L7...LJS7F-7L7.
// ....F-J..F7FJ|L7L7L7
// ....L7.F7||L7|.L7L7|
// .....|FJLJ|FJ|F7|.LJ
// ....FJL-7.||.||||...
// ....L---J.LJ.LJLJ...
// """;

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
        Console.WriteLine($"Sum: {list.Count()/2}");

        //Part2
        List<List<char>> dottetInput = new();
        List<char> muster = new();
        for (int i = 0; i < lines[0].Length; i++)
        {
            muster.Add('.');
            muster.Add('.');
            muster.Add('.');
        }

        //alles voll .
        Console.Clear();
        for (int i = 0; i < lines.Count()*3; i++)
        {
            dottetInput.Add(muster.ToArray().ToList());
        }

        //pipe rein
        foreach (var item in list)
        {
            switch (item.type)
            {
                case '|':
                    dottetInput[item.point.Y*3][item.point.X * 3+1] = 'x';
                    dottetInput[item.point.Y*3+1][item.point.X * 3+1] = 'x';
                    dottetInput[item.point.Y*3+2][item.point.X * 3+1] = 'x';
                    break;
                case 'F':
                    dottetInput[item.point.Y*3+1][item.point.X * 3+1] = 'x';
                    dottetInput[item.point.Y*3+2][item.point.X * 3+1] = 'x';
                    dottetInput[item.point.Y*3+1] [item.point.X * 3+2]= 'x';
                    break;
                case '7':
                    dottetInput[item.point.Y*3+1][item.point.X * 3+1] = 'x';
                    dottetInput[item.point.Y*3+1][item.point.X * 3] = 'x';
                    dottetInput[item.point.Y*3+2][item.point.X * 3+1] = 'x';
                    break;
                case '-':
                    dottetInput[item.point.Y*3+1][item.point.X * 3] = 'x';
                    dottetInput[item.point.Y*3+1][item.point.X * 3+1] = 'x';
                    dottetInput[item.point.Y*3+1][item.point.X * 3+2] = 'x';
                    break;
                case 'J':
                    dottetInput[item.point.Y*3] [item.point.X * 3+1]= 'x';
                    dottetInput[item.point.Y*3+1][item.point.X * 3+1] = 'x';
                    dottetInput[item.point.Y*3+1][item.point.X * 3] = 'x';
                    break;
                case 'L':
                    dottetInput[item.point.Y*3][item.point.X * 3+1] = 'x';
                    dottetInput[item.point.Y*3+1][item.point.X * 3+1] = 'x';
                    dottetInput[item.point.Y*3+1][item.point.X * 3+2] = 'x';
                    break;
            }
        }

        foreach (var item in dottetInput)
        {
            Console.WriteLine(item.ToArray());
        }

        floodFill(0,0,ref dottetInput);
        foreach (var item in dottetInput)
        {
            Console.WriteLine(item.ToArray());
        }

        for (int i = 0; i < dottetInput.Count; i+=3)
        {
            for (int j = 0; j < dottetInput[0].Count; j+=3)
            {
                if (dottetInput[i][j] == '.' && dottetInput[i][j+1] == '.' && dottetInput[i][j+2] == '.' &&
                    dottetInput[i+1][j] == '.' && dottetInput[i+1][j+1] == '.' && dottetInput[i+1][j+2] == '.' &&
                    dottetInput[i+2][j] == '.' && dottetInput[i+2][j+1] == '.' && dottetInput[i+2][j+2] == '.')
                    sumB++;
            }
        }
        Console.WriteLine("B: " + sumB);
    }
}