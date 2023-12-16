using System.Drawing;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public class pInfo{
    public long cnt {get;set;}=0;
    public bool vLinks {get;set;}=false;
    public bool vonRechts {get;set;}=false;
    public bool vonUnten {get;set;}=false;
    public bool vonOben {get;set;}=false;
}

static class Day16 {

    internal static Dictionary<Point, pInfo> cache = new();
    static List<string> lines;

    internal static void goOn(int x, int y, int prevX, int prevY){
        bool vonLinks = prevX<x;
        bool vonRechts = prevX>x;
        bool vonUnten = prevY>y;
        bool vonOben = prevY<y;

        if (y >= lines.Count) return;
        if (x >= lines.First().Length) return;
        if (x < 0 || y < 0) return;

        var cur = new Point(x,y);
        if (!cache.ContainsKey(cur)) cache.Add(cur, new());

        //Ausstieg wenn wir hier schon waren...?

        if (vonOben && cache[cur].vonOben) return;
        if (vonLinks && cache[cur].vLinks) return;
        if (vonRechts && cache[cur].vonRechts) return;
        if (vonUnten && cache[cur].vonUnten) return;


        cache[cur].cnt++;
        if (vonLinks) cache[cur].vLinks=true;
        if (vonOben) cache[cur].vonOben=true;
        if (vonRechts) cache[cur].vonRechts=true;
        if (vonUnten) cache[cur].vonUnten=true;

        var goWeiter = () => {
            goOn(x+(x-prevX), y+y-prevY, x, y);
        };
        var goRechts = () => {
            goOn(x+1, y, x, y);
        };
        var goLinks = () => {
            goOn(x-1, y, x, y);
        };
        var goHoch = () => {
            goOn(x, y-1, x, y);
        };
        var goRunter = () => {
            goOn(x, y+1, x, y);
        };

        switch (lines[y][x])
        {
            case '.':
                goWeiter();
                break;
            case '-':
                if (vonLinks || vonRechts) {
                    goWeiter();
                } else {
                    goLinks();
                    goRechts();
                }
                break;
            case '|':
                if (vonOben || vonUnten) {
                    goWeiter();
                } else {
                    goHoch();
                    goRunter();
                }
                break;

            case '/':
                if (vonOben)
                    goLinks();
                if (vonLinks)
                    goHoch();
                if (vonUnten)
                    goRechts();
                if (vonRechts)
                    goRunter();
                break;
            case '\\':
                if (vonOben)
                    goRechts();
                if (vonLinks)
                    goRunter();
                if (vonUnten)
                    goLinks();
                if (vonRechts)
                    goHoch();
                break;
        }
    }

    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
                                           
//         input = """
// .|...\....
// |.-.\.....
// .....|-...
// ........|.
// ..........
// .........\
// ..../.\\..
// .-.-/..|..
// .|....-|.\
// ..//.|....
// """;

        lines = Helper.getLines(input);

        long sumA = 0;
        long sumB=0;

        goOn(0,0, -1, 0);

        for (int i = 0; i < lines.Count; i++)
        {
            cache.Clear();
            goOn(0, i, -1, i);
            sumB=Math.Max(sumB, cache.Count());
            cache.Clear();
            goOn(lines[i].Length-1, i, lines[i].Length, i);
            sumB=Math.Max(sumB, cache.Count());
        }
        for (int i = 0; i < lines[0].Length; i++)
        {
            cache.Clear();
            goOn(i, 0, i, -1);
            sumB=Math.Max(sumB, cache.Count());
            cache.Clear();
            goOn(i, lines.Count()-1, i, lines.Count());
            sumB=Math.Max(sumB, cache.Count());
        }

        Console.WriteLine("a: " + cache.Count);
        Console.WriteLine("b: " + sumB);
    }
}