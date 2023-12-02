using System.Text.RegularExpressions;

class Day2{
    internal static void doit() {
        Regex idR = new(@"\d+");
        Regex showR = new(@"[^;]*");
        Regex blocksR = new(@"(\d+) (blue|green|red)");

        var lines = System.IO.File.ReadAllLines("input/day2");

        Dictionary<int, List<Dictionary<string, int>>> games = new();

        var lineReader = (string line) => {
            var id = int.Parse(idR.Match(line).Value);
            var line2 = line.Split(":")[1];
            List<Dictionary<string, int>> shows = new();
            foreach(Match showMtch in showR.Matches(line2)){
                Dictionary<string, int> blocks = new();
                foreach(Match blockM in blocksR.Matches(showMtch.Value)){
                    blocks.Add(blockM.Groups[2].Value, int.Parse(blockM.Groups[1].Value));
                }
                shows.Add(blocks);
            }
            games.Add(id, shows);
        };

        foreach (string line in lines.Where(a => !String.IsNullOrWhiteSpace(a)))
        {
            lineReader(line);    
        }

        var sum = 0;
        var sum2=0;

        foreach (var game in games)
        {
            var succ=true;
            var minBlue=0;
            var minRed=0;
            var minGreen=0;
            foreach (var show in game.Value)
            {
                if (show.ContainsKey("red") && show["red"] > 12) succ=false;
                if (show.ContainsKey("green") && show["green"] > 13) succ=false;
                if (show.ContainsKey("blue") && show["blue"] > 14) succ=false;
                if (show.ContainsKey("red")) minRed = Math.Max(minRed, show["red"]);
                if (show.ContainsKey("green")) minGreen = Math.Max(minGreen, show["green"]);
                if (show.ContainsKey("blue")) minBlue = Math.Max(minBlue, show["blue"]);
            }
            if (succ) sum += game.Key;
            sum2 += (minRed*minGreen*minBlue);
        }

        Console.WriteLine($"Sum: {sum}");
        Console.WriteLine($"Sum2: {sum2}");
    }
}
