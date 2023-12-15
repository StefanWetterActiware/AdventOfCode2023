using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

static class Day12 {

    static bool passt(string lineCandidate, IEnumerable<int> groups) {
        var countGrups = lineCandidate.Split('.',StringSplitOptions.RemoveEmptyEntries);
        return (countGrups.Length == groups.Count() && String.Join(",",groups.Select(a=>a.ToString())).Equals(String.Join(",",countGrups.Select(a=>a.ToString().Length))));
    }

    static long getPossibilitiesBruteForce(string line, int lastgroup) {
        if (String.IsNullOrWhiteSpace(line)) return 0;

        if (line.Contains('#')) {
            if (line.LastIndexOf('#') - line.IndexOf('#') > lastgroup-1) return 0;
            if (line.LastIndexOf('#') - line.IndexOf('#') == lastgroup-1) return 1;
        }

        long BFsum = 0;
        System.Text.RegularExpressions.Regex fr = new("\\?");
        Console.WriteLine("Bruteforcing " + line);

        for (ulong i = (ulong)Math.Pow(2,lastgroup)-1; i < Math.Pow(2,line.Length); i=i<<1)
        {
            var bruteForceSrcLine=line.ToCharArray();
            for (int j = 0; j < line.Length; j++)
            {
                if (bruteForceSrcLine[j] == '?') {
                    if ((i & (ulong)Math.Pow(2,j)) == (ulong)Math.Pow(2,j)) {
                        bruteForceSrcLine[j] = '#';
                    } else {
                        bruteForceSrcLine[j] = '.';
                    }
                }
            }

            if (bruteForceSrcLine.Count(a=>a=='#') == lastgroup)
                if (passt(new string(bruteForceSrcLine),new []{lastgroup})){
                    BFsum++;
                }
        }
        return BFsum;
    }

    static Regex alleRauten = new("#", RegexOptions.Compiled);

    static Dictionary<string, long> cache=new();

    static long getPossibilities(string line, IEnumerable<int> groups) {
        var key = line + "xx" + String.Join(",", groups.Select(a => a.ToString()));
        if (cache.ContainsKey(key)) return cache[key];
        var val = getPossibilities2(line, groups);
        cache.Add(key, val);
        return val;
    }

    static long getPossibilities2(string line, IEnumerable<int> groups) {
        if (!groups.Any()) return 1;
        if (groups.Any() && String.IsNullOrWhiteSpace(line)) return 0;

        line=line.Trim('.');
        
        var minL = groups.Sum()+groups.Count()-1;
        if (line.Length<minL) return 0;

        var anzRauten=alleRauten.Matches(line).Count();
        if (anzRauten > groups.Sum()) return 0;
        if (anzRauten == groups.Sum()){
            if (passt(line.Replace('?','.'), groups))
                return 1;
            else if (line.StartsWith('#')) // Dann gibts keinen Treffer. Und abschneidern und so geht dann auch nicht
                return 0;
        }

        if (groups.Count() == 1) {      
            // Wenn zwischen 2 Rauten ein Punkt ist, return 0, kann nicht sein
            if (line.IndexOf('#') != line.LastIndexOf('#') && line.IndexOf('.')>-1) {
                if (line[(line.IndexOf('#')+1)..line.LastIndexOf('#')].Contains('.'))
                    return 0;
            }

            // Wenn nur Fragezeichen:
            if (line.Distinct().Count() == 1 && line[0] == '?')
                return line.Length - groups.Single() + 1;

            if (line.LastIndexOf('#') - line.IndexOf('#')>=groups.Single())
                return 0;

            if (line.LastIndexOf('#') > groups.Single()+1)
                line=line[(line.LastIndexOf('#')-groups.Single()+1)..];

            long lastDing=0;
            foreach (var teil in line.Split('.').Where(a=> a.Length >= groups.Single()))
            {
                // Wenn es Rauten gibt, dann müssen wir nur die Rauten-Parts untersuchen. Sonst false positives
                if (line.Contains('#') && !teil.Contains('#')) continue;

                if (teil.Length == groups.Single()) {
                    lastDing++;
                    continue;
                }

                lastDing+=getPossibilitiesBruteForce(teil,groups.Single());
            }
            return lastDing;
        }
        
        if (line.StartsWith('#')) {
            // Es gibt hier eigentlich nur 2 Zustände: Wenn die erste Gruppe NICHT an den Anfang passt, gibt es für die gesuchte Kombi
            // keinen Treffer: return 0
            // andernfalls PASST das erste Objekt NUR an den Anfang (weil 0->#), also return findPoss vom Rest

            if (line[..groups.First()].Contains('.')) return 0;
            if (line[groups.First()] == '#') return 0;

            // var anzRauten=alleRauten.Matches(line).Count();
            // if (anzRauten > groups.Sum()) return 0;
            // if (anzRauten == groups.Sum()){
            //     if (passt(line.Replace('?','.'), groups))
            //         return 1;
            //     else
            //         return 0;
            // }
            // if (line[groups.First()] == '#') return 0;

            // if (!passt(line[..groups.First()].Replace('?','#'), groups.Take(1))) return 0;

            line=line.Substring(groups.First()+1);

            //Nur eine Möglichkeit bis hierher, also einfach weglassen...
            return getPossibilities(line,groups.Skip(1));
        }

        if (line.EndsWith('#')) {
            //Logik siehe Anfang
            if (line[^groups.Last()..].Contains('.')) return 0;
            if (line[^(groups.Last()+1)] == '#') return 0;
            

            // var anzRauten=alleRauten.Matches(line).Count();
            // if (anzRauten > groups.Sum()) return 0;
            // if (anzRauten == groups.Sum()){
            //     if (passt(line.Replace('?','.'), groups))
            //         return 1;
            //     else
            //         return 0;
            // }
            // if (line[^(groups.Last()+1)] == '#') return 0;

            // if (!passt(line[^groups.Last()..].Replace('?','#'), groups.TakeLast(1))) return 0;

            line=line[..^(groups.Last()+1)];

            //Nur eine Möglichkeit bis hierher, also einfach weglassen...
            return getPossibilities(line,groups.SkipLast(1));
        }

        //     //Nur eine Möglichkeit bis hierher, also einfach weglassen...
        //     return getPossibilities(line,groups.Take(groups.Count()-1));
        // }

        // vorn eins abschneiden, wie DIREKT NACH der gewünschten Größe eine Raute kommt, dann ist nicht genug Platz "mich" _VOR_ der Raute zu platzieren
        if (line.Substring(groups.First(),1) == "#") {
            return getPossibilities(line[1..], groups);
        }
        //Selbes hinten
        if (line.Substring(line.Length-groups.Last()-1,1) == "#") {
            return getPossibilities(line[..^1], groups);
        }

        // Vor erstem Punkt weg, wenn zu kurz für die erste Gruppe
        var ersterPunkt = line.IndexOf('.');
        if (ersterPunkt > -1 && ersterPunkt < groups.First()) {
            if (line[..ersterPunkt].Contains('#')) return 0;
            return getPossibilities(line[ersterPunkt..] ,groups);
        }
        var letzterPunkt = line.LastIndexOf('.');
        if (letzterPunkt > -1 && line.Length - letzterPunkt -1 < groups.Last() -1) {
            if (line[letzterPunkt..].Contains('#')) return 0;
            return getPossibilities(line[..letzterPunkt] ,groups);
        }

        // Wenn len+1 Leer ist, dann daran splitten und vorn ists entweder genau eine möglichkeit oder halt gar keine...
        // if (line[groups.First()] == '.') {
        //     if (line.Substring(0,groups.First()).Contains('.')) {
        //         return getPossibilities(line.Substring(groups.First()),groups);
        //     } else {
        //         return passt(line.Substring(groups.First()).PadLeft(line.Length, '#'), groups) ? getPossibilities(line.Substring(groups.First()),groups.Skip(1)) : 0
        //                 + getPossibilities(line.Substring(groups.First()),groups);
        //     }
        // }

        // if (passt(line.Substring(0,groups.First()),groups.Take(1))){
        //     // Wenn der erste auf die ersten x Buchstaben passt, dann
        //     // return alle Ergebnisse für den REST der Zeile PLUS
        //     // alle Ergebnisse für "Zeile weniger erster Buchstabe"
        //     return getPossibilities(line.Substring(groups.First()+1), groups.Skip(1)) +
        //             getPossibilities(line.Substring(1),groups);
        // }

        return (line[0] == '?' ? getPossibilities("." + line[1..], groups) : 0)
                + getPossibilities("#" + line[1..], groups);

        // return getPossibilitiesBruteForce(line, groups);
    }

    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
                                           
//         input = """
// ???#??#?#?#?.. 1,7
// """;

        var lines = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        long sumA = 0;
        long sumB = 0;

        Regex amAnfangPassend = new("^[\\?#]*$");

        foreach (var lineRaw in lines)
        {
            var rawParts = lineRaw.Split(' ');

            var groups=rawParts[1].Split(',').Select(a=>int.Parse(a)).ToList();
            var line=rawParts[0].Trim('.');
            
            var thisA = getPossibilities(line,groups);
            sumA+=thisA;

            //Teil2
            line = rawParts[0] + '?' + rawParts[0] + '?' + rawParts[0] + '?' + rawParts[0] + '?' + rawParts[0];
            line=line.Trim('.');
            groups = (rawParts[1] + ',' + rawParts[1] + ',' + rawParts[1] + ',' + rawParts[1] + ',' + rawParts[1]).Split(',').Select(a=>int.Parse(a)).ToList();

            var thisB = getPossibilities(line,groups);
            sumB+=thisB;
        }


        Console.WriteLine($"Sum: {sumA}");
        Console.WriteLine($"SumB: {sumB}");
    }
}