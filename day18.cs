using System.Diagnostics.Contracts;
using System.Drawing;
using System.Text.RegularExpressions;


static class Day18 {
    static List<string> lines;

    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
                                           
//         input = """
// R 6 (#70c710)
// D 5 (#0dc571)
// L 2 (#5713f0)
// D 2 (#d2c081)
// R 2 (#59c680)
// D 2 (#411b91)
// L 5 (#8ceee2)
// U 2 (#caa173)
// L 1 (#1b58a2)
// U 2 (#caa171)
// R 2 (#7807d2)
// U 3 (#a77fa3)
// L 2 (#015232)
// U 2 (#7a21e3)
// """;

        lines = Helper.getLines(input);

        long sumA = 0;
        long sumB=0;

        var allPoints = new List<Point>();

        var curPos=new Point(0,0);
        allPoints.Add(curPos);
        foreach (var line in lines)
        {
            var parts = line.Split(' ');
            var met = int.Parse(parts[1]);
            var dir = parts[0][0];
            for (int i = 0; i < met; i++)
            {
                curPos = new Point(curPos.X + (dir=='U'?-1:dir=='D'?1:0), curPos.Y + (dir=='R'? 1: dir == 'L'? -1:0));
                allPoints.Add(curPos);
            }
        }
        var minX = allPoints.Select(a=>a.X).Min();
        var maxX = allPoints.Select(a=>a.X).Max();
        var minY = allPoints.Select(a=>a.Y).Min();
        var maxY = allPoints.Select(a=>a.Y).Max();
        var diffX=-1*minX;maxX-=minX;minX=0;
        var diffY=-1*minY;maxY-=minY;minY=0;

        var field = new List<List<char>>();
        for (int i = 0; i <= maxX; i++)
        {
            var fLine = new List<char>();
            for (int j = 0; j <= maxY; j++)
            {
                fLine.Add('.');
            }
            field.Add(fLine);
        }
        foreach (var item in allPoints)
        {
            field[item.X+diffX][item.Y+diffY] = 'x';
        }

        for (int i = 0; i <= maxX; i++)
        {
            Day10.floodFill(i,0,ref field);
            Day10.floodFill(i,maxY,ref field);
        }
        for (int i = 0; i <= maxY; i++)
        {
            Day10.floodFill(0,i,ref field);
            Day10.floodFill(maxX,i,ref field);
        }

        //Count
        for (int i = 0; i <= maxX; i++)
        {
            var printLine=new string(field[i].ToArray()).Replace(".","#").Replace('x', '#').Replace('o','.');
            // Console.WriteLine(field[i].ToArray());
            Console.WriteLine(printLine);
            sumA+=new string(field[i].ToArray()).Replace("o","").Length;
        }

        Console.WriteLine("a: " + sumA);
        Console.WriteLine("b: " + sumB);
    }
}