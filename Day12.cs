using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

static class Day12 {
    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
                                           
//         input = """
// ???.### 1,1,3
// .??..??...?##. 1,1,3
// ?#?#?#?#?#?#?#? 1,3,1,6
// ????.#...#... 4,1,1
// ????.######..#####. 1,6,5
// ?###???????? 3,2,1
// """;

        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        var sumA = 0;
        var sumB = 0;

        Regex amAnfangPassend = new("^[\\?#]*$");

        var before=0;
        System.IO.File.Delete("counterNoBF.txt");
        foreach (var lineRaw in lines)
        {
            System.IO.File.AppendAllText("counterNoBF.txt", $"{sumA-before}{Environment.NewLine}");
            before=sumA;

            var rawParts = lineRaw.Split(' ');
            //Teil2
            rawParts[0] = rawParts[0] + '?' + rawParts[0] + '?' + rawParts[0] + '?' + rawParts[0] + '?' + rawParts[0];
            rawParts[1] = rawParts[1] + ',' + rawParts[1] + ',' + rawParts[1] + ',' + rawParts[1] + ',' + rawParts[1];

            //.Select(a=> new KeyValuePair(a.Split(' ')[0], )
            var groups=rawParts[1].Split(',').Select(a=>int.Parse(a)).ToList();
            var line=rawParts[0].Trim('.');
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

                // else if (line.Substring(groups.First(),1) == "#"){
                //     // Wenn EINS weiter hinen als die Länge der ersten Gruppe ein # ist, MUSS das erste ein Leerding sein, damit wie oben
                //     line=line.Substring(groups.First()+1);
                //     groups=groups.Skip(1).ToList();
                //     if (line.StartsWith('?')){
                //         line=line.Substring(1);
                //     }
                //     line=line.Trim('.');
                //     somethingfound=true;
                // }

                // else if (line.Substring(line.Length-groups.Last(),1) == "#"){
                //     line=line.Substring(0,line.Length-groups.Last()-1);
                //     groups=groups.Take(groups.Count()-1).ToList();
                //     if (line.EndsWith('?')){
                //         line=line.Substring(0,line.Length-1);
                //     }
                //     line=line.Trim('.');
                //     somethingfound=true;
                // }

                // else if (line.Substring(groups.First(),1).Equals(".") && amAnfangPassend.IsMatch(line.Substring(0,groups.First()))){
                //     line=line.Substring(groups.First());
                //     groups=groups.Skip(1).ToList();
                //     if (line.StartsWith('?')){
                //         line=line.Substring(1);
                //     }
                //     line=line.Trim('.');
                //     somethingfound=true;
                // }
            }

            if (groups.Count() == 0){
                //ok, alle unter. Keine Varianzen
                sumA++;
                continue;
            }

            line=line.Trim('.');


            // Wenn die Anzahl der GRUPPEN von Fragezeichen jetzt gleich der Anzahl in den "groups" ist, dann...
            System.Text.RegularExpressions.Regex frGrp = new("\\?+");
            var frGrpM = frGrp.Matches(line);
            // if (frGrpM.Count == groups.Count()){
            //     var zwischenres=1;
            //     for (int i = 0; i < groups.Count(); i++)
            //     {
            //         Match match = frGrpM[i];
            //         zwischenres*=(match.Length-groups[i]+1);
            //     }
            //     sumA+=zwischenres;
            //     continue;
            // }
            

            if (frGrpM.Count == 1) {
                var minZeich = groups.Count-1 + groups.Sum();
                var luft = frGrpM[0].Length-minZeich;
                var anzahlPlatzierungen = luft+groups.Count;
                var oben = 1;
                for (int i = 1; i <= anzahlPlatzierungen; i++)
                {
                    oben*=i;
                }
                var unten=1;
                for (int i = 1; i <= anzahlPlatzierungen-luft; i++)
                {
                    unten*=i;
                }
                for (int i = 1; i <= luft; i++)
                {
                    unten*=i;
                }

                sumA+=oben/unten;
            } else {
                System.Text.RegularExpressions.Regex fr = new("\\?");
                var frM = fr.Matches(line);
                var anzFragez=frM.Count;
                for (int i = 0; i < Math.Pow(2,anzFragez); i++)
                {
                    var bruteForceSrcLine=line.ToCharArray();
                    for (int j = 0; j < anzFragez; j++)
                    {
                        if ((i & (int)Math.Pow(2,j)) == (int)Math.Pow(2,j)) {
                            bruteForceSrcLine[frM[j].Index] = '#';
                        } else {
                            bruteForceSrcLine[frM[j].Index] = '.';
                        }
                    }

                    var countGrups = new string(bruteForceSrcLine).Split('.',StringSplitOptions.RemoveEmptyEntries);
                    if (countGrups.Length == groups.Count() && String.Join(",",groups.Select(a=>a.ToString())).Equals(String.Join(",",countGrups.Select(a=>a.ToString().Length)))){
                        sumA++;
                    }
                }
                Console.WriteLine($"{sumA}");
             }
        }


        Console.WriteLine($"Sum: {sumA}");
        Console.WriteLine($"SumB: {sumB}");
    }
}