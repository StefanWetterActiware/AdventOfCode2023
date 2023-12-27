using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

static class Day23 {
    class CalcPoint {
        public List<CalcPoint> vorgaenger {get;set;}
        public Point p {get;set;}
        public string pStr{get{return $"{p.X},{p.Y}";}}
        public long cost {get;set;}=0;

        public IEnumerable<Point> getFollowers (IEnumerable<string> input, bool partB = false, bool noFilterVorg = false) {
            var res = new List<Point>();
            var cur = partB ? '.' : input.Skip(p.X).Take(1).Single()[p.Y];
            if (this.p.X > 0 && (cur == '.' || cur == '^')) res.Add(new Point(p.X-1,p.Y));
            if (this.p.Y > 0&& (cur == '.' || cur == '<')) res.Add(new Point(p.X,p.Y-1));
            if (this.p.X < input.Count()-1&& (cur == '.' || cur == 'v')) res.Add(new Point(p.X+1,p.Y));
            if (this.p.Y < input.First().Count()-1&& (cur == '.' || cur == '>')) res.Add(new Point(p.X,p.Y+1));
            var alleFollower = res.Where(a=>input.Skip(a.X).Take(1).Single()[a.Y] != '#');
            var followerOhneVorg = alleFollower.Except(vorgaenger.Select(b => b.p));
            return noFilterVorg ? alleFollower : followerOhneVorg;
        }

        public int GetHashCode([DisallowNull] CalcPoint obj)
        {
            throw new NotImplementedException();
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

        var partBPfadCache = new SortedDictionary<string, CalcPoint>();
        var doCalc = (bool partB)=>{
            while (calcQ.TryDequeue(out CalcPoint c)){
                if (c.vorgaenger.Select(a=>a.p).Contains(c.p)) continue;

                if (!closed.TryAdd(c.pStr, c.cost)) {
                    if (closed[c.pStr] > c.cost)
                        continue;
                    closed[c.pStr]=c.cost;
                }

                if (c.p.Equals(ende)) continue;

                var unfilteredFollowers = c.getFollowers(lines, partB, true);

                if (partB && unfilteredFollowers.Count() == 2) {
                    var next=unfilteredFollowers.Where(a=> !c.vorgaenger.Select(b=>b.p).Contains(a)).Single();
                    if (!partBPfadCache.ContainsKey(c.pStr)) {
                        //Pfadanfang gefunden!
                        var pathVG = new List<CalcPoint>{c};
                        var nextF=(new CalcPoint(){p=next, cost = 0, vorgaenger=pathVG}).getFollowers(lines, partB, false);
                        while (nextF.Count() == 1){
                            pathVG.Add(new CalcPoint(){p=next});
                            next = nextF.Single();
                            nextF=(new CalcPoint(){p=next, cost = 0, vorgaenger=pathVG}).getFollowers(lines, partB, false);
                        }
                        partBPfadCache.Add(c.pStr, new CalcPoint(){p=next,cost=pathVG.Count, vorgaenger=pathVG});
                    }
                    var cacheP = partBPfadCache[c.pStr];
                    if (cacheP.vorgaenger.Any(a=> c.vorgaenger.Select(b=>b.p).Contains(a.p)))
                        continue;
                    CalcPoint newP = new CalcPoint{vorgaenger=cacheP.vorgaenger.ToList(), cost=cacheP.cost, p=cacheP.p};
                    newP.vorgaenger.AddRange(c.vorgaenger.ToList());
                    newP.vorgaenger.Add(c);
                    newP.cost+=c.cost;
                    // calcQ.Enqueue(c);
                    // continue;
                    unfilteredFollowers = newP.getFollowers(lines, partB, true);
                    c=newP;
                    
                    if (!closed.TryAdd(c.pStr, c.cost)) closed[c.pStr]=c.cost;
                    if (c.p.Equals(ende)) continue;
                }

                var follower = unfilteredFollowers.Except(c.vorgaenger.Select(b=>b.p));
                foreach (var item in follower)
                {
                    var nextQ = new CalcPoint(){p=item, cost=c.cost+1,vorgaenger=c.vorgaenger.ToList()};
                    nextQ.vorgaenger.Add(c);
                    if (closed.ContainsKey(nextQ.pStr) && closed[nextQ.pStr] > nextQ.cost)
                        continue;
                    calcQ.Enqueue(nextQ);
                }
            }
        };

        doCalc(false);
        Console.WriteLine("a: " + closed[$"{ende.X},{ende.Y}"]);

        closed = new SortedDictionary<string, long>();
        calcQ.Enqueue(new CalcPoint(){p = start, vorgaenger=new()});

        // Ergebnis passt leider nicht. Ich gebe auf.
        doCalc(true);
        Console.WriteLine("b: " + closed[$"{ende.X},{ende.Y}"]);
    }
}