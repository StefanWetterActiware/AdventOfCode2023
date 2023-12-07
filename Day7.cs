using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

        class SetStrength{
            public static int get(string a){
                var b = a.ToCharArray();
                if (b.Distinct().Count() == 1) return 15;
                if (b.Distinct().Count() == 2) {
                    var c = a.Replace(b.First().ToString(), "");
                    if (c.Length == 1) return 14; //4ofakind
                    if (c.Length == 2) return 13; //fullhouse
                }
                if (b.Distinct().Count() == 3) {
                    if (b.GroupBy(d => d).Any(x => x.Count() == 3)) return 12; //3ofak
                    if (b.GroupBy(d=>d).Where(x=>x.Count() == 2).Count() == 2) return 11; //2pair
                    if (b.GroupBy(d=>d).Where(x=>x.Count() == 2).Count() == 1) return 10; //1pair
                }
                return 1;
            }
        }

        class Xn{
                    public static int cardVal  (char a)  {
            switch (a)
            {
                case 'A':return 14;
                case 'K':return 13;
                case 'Q':return 12;
                case 'J':return 11;
                case 'T': return 10;
                default:
                return int.Parse(a.ToString());
            }
        }
        }

        class Comp:IComparer<string>{
            public int Compare(string? x, string? y) {
                if (SetStrength.get(x) > SetStrength.get(y)) return 1;
                if (SetStrength.get(x) < SetStrength.get(y)) return -1;

                for (int i = 0; i < x.Length; i++)
                {
                    var c = Xn.cardVal(x[i]) - Xn.cardVal(y[i]);
                    if (c!=0) return c;
                }

                return 0;

                if (Xn.cardVal(x.ToCharArray().GroupBy(a=>a).OrderBy(a=>a.Count()).First().Key) > 
                Xn.cardVal(y.ToCharArray().GroupBy(a=>a).OrderBy(a=>a.Count()).First().Key)) return 1;
                if (Xn.cardVal(x.ToCharArray().GroupBy(a=>a).OrderBy(a=>a.Count()).First().Key) <
                Xn.cardVal(y.ToCharArray().GroupBy(a=>a).OrderBy(a=>a.Count()).First().Key)) return -1;

                if (Xn.cardVal(x.ToCharArray().GroupBy(a=>a).OrderBy(a=>a.Count()).Skip(1).First().Key) > 
                Xn.cardVal(y.ToCharArray().GroupBy(a=>a).OrderBy(a=>a.Count()).Skip(1).First().Key)) return 1;
                if (Xn.cardVal(x.ToCharArray().GroupBy(a=>a).OrderBy(a=>a.Count()).Skip(1).First().Key) <
                Xn.cardVal(y.ToCharArray().GroupBy(a=>a).OrderBy(a=>a.Count()).Skip(1).First().Key)) return -1;

                return x.ToCharArray().Select(a => Xn.cardVal(a)).Sum() -y.ToCharArray().Select(a => Xn.cardVal(a)).Sum();
            }
        }

static class Day7{
    internal static void doit(){
        Regex dayNoR = new(@"\d*$");

        var lines = Helper.getInputAsLines(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value),
                                           test:false);
        
        
        var sumA = 0;
        var sumB = 0;

        var sets = lines.Select(a => a.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)).ToDictionary(a => a[0], a => int.Parse(a[1]));



        var c = new Comp();
        var orderedsets = sets.OrderBy(a => a.Key, c);

        for (int i = 0; i < orderedsets.Count(); i++)
        {
            sumA += (i+1)*orderedsets.Skip(i).Take(1).Single().Value;
        }

        Console.WriteLine($"Sum: {sumA}");
        Console.WriteLine($"SumB: {sumB}");
    }
}