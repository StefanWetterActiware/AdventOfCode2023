using System.Drawing;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

class Point3D{
    public int x{get;set;}
    public int y{get;set;}
    public int z{get;set;}
    static public Point3D fromCoord(string str){
        Point3D r = new();
        var p = str.Split(",").Select(a=>int.Parse(a)).ToList();
        r.x=p[0];
        r.y=p[1];
        r.z=p[2];
        return r;
    }
}
class Block{
    public IEnumerable<Block> stuetzt {get;set;}
    public IEnumerable<Block> gestuetztVon {get;set;}
    private bool _isSettled = false;
    public bool isSettled{get{return _isSettled;}}
    public void moveDown(ref SortedDictionary<string, int> highestPoints){
        IEnumerable<Point3D> tmp = allPoints.ToList();
        _allP=null;

        var howManyDown=0;
        do
        {
            tmp=tmp.Select(a=>new Point3D{x=a.x, y=a.y, z=a.z-1 });
            var allDownOk=true;
            foreach (var item in tmp)
            {
                if (highestPoints[$"{item.x}:{item.y}"]>=item.z) {
                    allDownOk=false;
                    break;
                }
            }
            if (!allDownOk) break;
            howManyDown++;
        } while (true);

        if (howManyDown>0){
            start.z-=howManyDown;
            end.z-=howManyDown;
        }

        foreach (var item in allPoints)
        {
            if (highestPoints[$"{item.x}:{item.y}"]<item.z) {
                highestPoints[$"{item.x}:{item.y}"]=item.z;
            }
        }

        _isSettled=true;
    }
    public int minZ {get{
        return Math.Min(start.z, end.z);
    }}
    public Point3D start{get;set;}
    public Point3D end{get;set;}
    public IEnumerable<Point3D> highestPoints {get{
        if (start.z == end.z)
            return allPoints;
        if (start.z>end.z)
            return new List<Point3D>{start};
        return new List<Point3D>{end};
    }}
    public IEnumerable<Point3D> lowestPoints {get{
        if (start.z == end.z)
            return allPoints;
        if (start.z<end.z)
            return new List<Point3D>{start};
        return new List<Point3D>{end};
    }}
    private IEnumerable<Point3D> _allP;
    public IEnumerable<Point3D> allPoints{get{
        if (_allP == null) {
            _allP=allPointsDirect();
        }
        return _allP;
    }}
    private IEnumerable<Point3D> allPointsDirect(){
        var res = new List<Point3D>();
        for (int x = start.x; x <= end.x; x++)
        {
            for (int y = start.y; y <= end.y; y++)
            {
                for (int z = start.z; z <= end.z; z++)
                {
                    res.Add(new(){x=x, y=y, z=z});
                }
            }
        }
        return res;
    }

    static public Block fromLine(string line){
        var parts=line.Split("~");
        var res = new Block();
        res.start=Point3D.fromCoord(parts[0]);
        res.end=Point3D.fromCoord(parts[1]);
        return res;
    }
}

static class Day22 {
    static List<Point> destList = new();
    //static List<List<List<char>>> grid;
    static Point start = new();

    static SortedDictionary<string, bool> cache = new();
    
    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));

//        input = """
// 1,0,1~1,2,1
// 0,0,2~2,0,2
// 0,2,3~2,2,3
// 0,0,4~0,2,4
// 2,0,5~2,2,5
// 0,1,6~2,1,6
// 1,1,8~1,1,9
// """;

        var lines = Helper.getLines(input);

        var blocks = new List<Block>();

        foreach (var aLine in lines)
        {
            blocks.Add(Block.fromLine(aLine));
        }

        var orderedBlocks=blocks.OrderBy(a=> a.minZ);
        SortedDictionary<string,int> highestPoints = new();
        for (int i = blocks.Select(a=>Math.Min(a.start.x,a.end.x)).Min(); i <= blocks.Select(a=>Math.Max(a.start.x,a.end.x)).Max(); i++)
        {
            for (int j = blocks.Select(a=>Math.Min(a.start.y,a.end.y)).Min(); j <= blocks.Select(a=>Math.Max(a.start.y,a.end.y)).Max(); j++)
            {
                highestPoints.Add($"{i}:{j}", 0);
            }
        }

        foreach (var item in orderedBlocks)
        {
            item.moveDown(ref highestPoints);
        }

        foreach (var aBlock in blocks)
        {
            var punkteUeberDiesem = aBlock.highestPoints.Select(a=>new Point3D{x=a.x, y=a.y, z=a.z+1});
            var obendrauf = blocks.Where(a=>a.lowestPoints.Any(a=> punkteUeberDiesem.Any(b=> a.x==b.x && a.y==b.y && a.z==b.z)));
            aBlock.stuetzt = obendrauf.ToList();
        }
        foreach (var aBlock in blocks)
        {
            var gestuetztVon = blocks.Where(a=>a.stuetzt.Contains(aBlock));
            aBlock.gestuetztVon = gestuetztVon.ToList();
        }

        var anzKannNICHTentferntwerden = blocks.Where(a=> a.gestuetztVon.Count() == 1).Select(a=>a.gestuetztVon.Single()).Distinct().Count();

        Console.WriteLine($"a: {blocks.Count - anzKannNICHTentferntwerden}");

        // Console.WriteLine("a: " + sumA);
        // Console.WriteLine("b: " + sumB);
    }
}