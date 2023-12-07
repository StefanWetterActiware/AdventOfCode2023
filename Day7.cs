using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

        class SetStrength{
            public static int get(string a){
                var b = a.ToCharArray();
                var anzDiffC = b.Distinct().Count();
                if (anzDiffC == 1)
                    return 15;
                if (anzDiffC == 2) {
                    var c = a.Replace(b.First().ToString(), "");
                    if (c.Length == 1 || c.Length == 4) return 14; //4ofakind
                    else return 13; //fullhouse
                }
                if (anzDiffC == 3) {
                    if (b.GroupBy(d => d).Any(x => x.Count() == 3)) return 12; //3ofak
                    if (b.GroupBy(d=>d).Where(x=>x.Count() == 2).Count() == 2) return 11; //2pair
                }
                if (anzDiffC == 4) return 10; //1pair
                return 1;
            }
        }

        class SetStrengthB{
            public static int get(string a){
                var b = a.ToCharArray();

                //Joker berechnen
                if (a.Contains("J") && a != "JJJJJ") {
            var gruppen = b.GroupBy(d => d).OrderByDescending(a=> a.Count());
            //Wenn Joker die GRÖSSTE Gruppe sind, dann der 2. Gruppe hinzufügen, sonst der grössten Gruppe
            if (gruppen.First().Key == 'J') {
                a = a.Replace('J', gruppen.Skip(1).First().Key);
            } else {
                a = a.Replace('J', gruppen.First().Key);
            }
            b = a.ToCharArray();
        }

                var anzDiffC = b.Distinct().Count();
                if (anzDiffC == 1)
                    return 15;
                if (anzDiffC == 2) {
                    var c = a.Replace(b.First().ToString(), "");
                    if (c.Length == 1 || c.Length == 4) return 14; //4ofakind
                    else return 13; //fullhouse
                }
                if (anzDiffC == 3) {
                    if (b.GroupBy(d => d).Any(x => x.Count() == 3)) return 12; //3ofak
                    if (b.GroupBy(d=>d).Where(x=>x.Count() == 2).Count() == 2) return 11; //2pair
                }
                if (anzDiffC == 4) return 10; //1pair
                return 1;
            }
        }

        class cardStrenA{
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
class cardStrenB {
    public static int cardVal(char a) {
        switch (a) {
            case 'A':
                return 14;
            case 'K':
                return 13;
            case 'Q':
                return 12;
            case 'J':
                return 1;
            case 'T':
                return 10;
            default:
                return int.Parse(a.ToString());
        }
    }
}

class Comp :IComparer<string>{
            public int Compare(string? x, string? y) {
                if (SetStrength.get(x) > SetStrength.get(y)) return 1;
                if (SetStrength.get(x) < SetStrength.get(y)) return -1;

                for (int i = 0; i < x.Length; i++)
                {
                    var c = cardStrenA.cardVal(x[i]) - cardStrenA.cardVal(y[i]);
                    if (c!=0) return c;
                }

                return 0;
            }
        }

class CompB :IComparer<string>{
            public int Compare(string? x, string? y) {
                if (SetStrengthB.get(x) > SetStrengthB.get(y)) return 1;
                if (SetStrengthB.get(x) < SetStrengthB.get(y)) return -1;

                for (int i = 0; i < x.Length; i++)
                {
                    var c = cardStrenB.cardVal(x[i]) - cardStrenB.cardVal(y[i]);
                    if (c!=0) return c;
                }

                return 0;
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

        var anzDiffSets = sets.Keys.Distinct().Count();

        var c = new Comp();
        var orderedsets = sets.OrderBy(a => a.Key, c);

        for (int i = 0; i < orderedsets.Count(); i++)
        {
            sumA += (i+1)*orderedsets.Skip(i).Take(1).Single().Value;
        }

        Console.WriteLine($"Sum: {sumA}");


        var cB = new CompB();
        var orderedsetsB = sets.OrderBy(a => a.Key, cB);

        for (int i = 0; i < orderedsetsB.Count(); i++) {
            sumB += (i + 1) * orderedsetsB.Skip(i).Take(1).Single().Value;
        }
        Console.WriteLine($"SumB: {sumB}");
    }
}