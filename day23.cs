using System.Drawing;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

static class Day23 {
    class CalcPoint{
        public List<CalcPoint> vorgaenger {get;set;}
        public Point p {get;set;}
        public string pStr{get{return $"{p.X},{p.Y}";}}
        public long cost {get;set;}=0;
        public IEnumerable<Point> getFollowers (IEnumerable<string> input) {
            var res = new List<Point>();
            var cur = input.Skip(p.X).Take(1).Single()[p.Y];
            if (this.p.X > 0 && (cur == '.' || cur == '^')) res.Add(new Point(p.X-1,p.Y));
            if (this.p.Y > 0&& (cur == '.' || cur == '<')) res.Add(new Point(p.X,p.Y-1));
            if (this.p.X < input.Count()-1&& (cur == '.' || cur == 'v')) res.Add(new Point(p.X+1,p.Y));
            if (this.p.Y < input.First().Count()-1&& (cur == '.' || cur == '>')) res.Add(new Point(p.X,p.Y+1));
            return res.Where(a=>input.Skip(a.X).Take(1).Single()[a.Y] != '#');
        }
    }
    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));

//        input = """
// #.#####################
// #.......#########...###
// #######.#########.#.###
// ###.....#.>.>.###.#.###
// ###v#####.#v#.###.#.###
// ###.>...#.#.#.....#...#
// ###v###.#.#.#########.#
// ###...#.#.#.......#...#
// #####.#.#.#######.#.###
// #.....#.#.#.......#...#
// #.#####.#.#.#########v#
// #.#...#...#...###...>.#
// #.#.#v#######v###.###v#
// #...#.>.#...>.>.#.###.#
// #####v#.#.###v#.#.###.#
// #.....#...#...#.#.#...#
// #.#########.###.#.#.###
// #...###...#...#...#.###
// ###.###.#.###v#####v###
// #...#...#.#.>.>.#.>.###
// #.###.###.#.###.#.#v###
// #.....###...###...#...#
// #####################.#
// """;

        var lines = Helper.getLines(input);
        var startY = lines[0].IndexOf('.');
        var start = new Point(0,startY);
        var endeY = lines.Last().IndexOf('.');
        var ende = new Point(lines.Count-1,endeY);

        var calcQ = new Queue<CalcPoint>();
        var closed = new SortedDictionary<string, long>();
        calcQ.Enqueue(new CalcPoint(){p = start, vorgaenger=new()});

        var doCalc = ()=>{
            while (calcQ.TryDequeue(out CalcPoint c)){
                // if (closed.ContainsKey(c.p) && closed[c.p] > c.cost)
                //     continue;
                
                if (!closed.TryAdd(c.pStr, c.cost)) closed[c.pStr]=c.cost;

                if (c.p.Equals(ende)) continue;

                var nextVorg = c.vorgaenger.ToList();
                nextVorg.Add(c);

                foreach (var item in c.getFollowers(lines).Except(nextVorg.Select(b=>b.p)))
                {
                    var nextQ = new CalcPoint(){p=item, cost=c.cost+1,vorgaenger=nextVorg.ToList()};
                    if (closed.ContainsKey(nextQ.pStr) && closed[nextQ.pStr] > nextQ.cost)
                        continue;
                    calcQ.Enqueue(nextQ);
                }
            }
        };

        doCalc();

        Console.WriteLine("a: " + closed[$"{ende.X},{ende.Y}"]);

        // Console.WriteLine("b: " + sumB);
    }
}