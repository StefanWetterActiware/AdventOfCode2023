using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

static class Day12 {
    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
                                           
        input = """
???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1
""";

        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToDictionary(a=> a.Split(' ')[0], a=> a.Split(' ')[1].Split(',').Select(a=>int.Parse(a)).ToList());

        var sumA = 0;
        var sumB = 0;

        foreach (var kvp in lines)
        {
            var groups=kvp.Value;
            var line=kvp.Key.Trim('.');
            var l = line.Length;
            var minL = groups.Sum()+groups.Count()-1;
            if (l==minL){
                sumA++;
                continue;
            }

            var somethingfound=true;
            while (somethingfound && groups.Count()>0){
                somethingfound=false;

                if (groups.Count() == 1 && line.Length == groups.Single()) {
                    line="";
                    groups.Clear();
                    continue;
                }

                //Gruppen am Ende finden
                if (line.EndsWith('#')){
                    line=line.Substring(0,line.Length-groups.Last());
                    groups=groups.Take(groups.Count()-1).ToList();
                    if (line.EndsWith('?')){
                        line=line.Substring(0,line.Length-1);
                    }
                    line=line.Trim('.');
                    somethingfound=true;
                }
                //Gruppen am Anfang finden
                else if (line.StartsWith('#')){
                    line=line.Substring(groups.First());
                    groups=groups.Skip(1).ToList();
                    if (line.StartsWith('?')){
                        line=line.Substring(1);
                    }
                    line=line.Trim('.');
                    somethingfound=true;
                }

                else if (line.Substring(groups.First(),1) == "#"){
                    // Wenn EINS weiter hinen als die Länge der ersten Gruppe ein # ist, MUSS das erste ein Leerding sein, damit wie oben
                    line=line.Substring(groups.First()+1);
                    groups=groups.Skip(1).ToList();
                    if (line.StartsWith('?')){
                        line=line.Substring(1);
                    }
                    line=line.Trim('.');
                    somethingfound=true;
                }
                else if (line.Substring(line.Length-1-groups.Last(),1) == "#"){
                    line=line.Substring(0,line.Length-groups.Last()-1);
                    groups=groups.Take(groups.Count()-1).ToList();
                    if (line.EndsWith('?')){
                        line=line.Substring(0,line.Length-1);
                    }
                    line=line.Trim('.');
                    somethingfound=true;
                }
            }

            if (groups.Count() == 0){
                //ok, alle unter. Keine Varianzen
                sumA++;
                continue;
            }

            line=line.Trim('.');


            System.Text.RegularExpressions.Regex fr = new("\\?");
            var frM = fr.Matches(line);
            var anzFragez=frM.Count;

            // Wenn die Anzahl der GRUPPEN von Fragezeichen jetzt gleich der Anzahl in den "groups" ist, dann...
            System.Text.RegularExpressions.Regex frGrp = new("\\?+");
            var frGrpM = frGrp.Matches(line);
            if (frGrpM.Count == groups.Count()){
                var zwischenres=1;
                for (int i = 0; i < groups.Count(); i++)
                {
                    Match match = frGrpM[i];
                    zwischenres*=(match.Length-groups[i]+1);
                }
                sumA+=zwischenres;
                continue;
            }
            

            // TODO: 6 Fragezeichen und 2,1 zum Beispiel. Oder meherere Gruppen von fragezeichen mit mehreren Gruppen von Zahlen
            line=line.Replace("?",".");
            //Tolle Schleife über ALLLLLE Möglichkeiten?

            var countGrups = line.Split('.',StringSplitOptions.RemoveEmptyEntries);
            if (countGrups.Length == groups.Count() && String.Join(",",countGrups.Select(a=>a.ToString())).Equals(String.Join(",",countGrups.Select(a=>a.ToString())))){
                sumA++;
            }
        }


        Console.WriteLine($"Sum: {sumA}");
        Console.WriteLine($"SumB: {sumB}");
    }
}