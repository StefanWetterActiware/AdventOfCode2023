using System.Text.RegularExpressions;


class Day1 {
    internal static void doit(){

        Console.WriteLine("Hello, World!");

        var lines = Helper.getInputAsLines(1);
        
        System.Text.RegularExpressions.Regex rA = new(@"(\d)");
        System.Text.RegularExpressions.Regex rB = new(@"(\d|one|two|three|four|five|six|seven|eight|nine)");
        Regex rB2 = new(@"(\d|one|two|three|four|five|six|seven|eight|nine)", RegexOptions.RightToLeft);

        var num2num = (Match a) => {
            switch (a.Value)
            {
                case "one": return 1;
                case "two": return 2;
                case "three": return 3;
                case "four": return 4;
                case "five": return 5;
                case "six": return 6;
                case "seven": return 7;
                case "eight": return 8;
                case "nine": return 9;
                default:
                return int.Parse(a.Value);
            }
        };

        var sumA = 0;
        var sumB = 0;
        foreach (string line in lines)
        {
            var matchesA = rA.Matches(line);
            var firstA = matchesA.First();
            var lastA = matchesA.Last();
            var firstB = rB.Match(line);
            var lastB = rB2.Match(line);

            var no = int.Parse($"{num2num(firstA)}{num2num(lastA)}");
            var noB = int.Parse($"{num2num(firstB)}{num2num(lastB)}");
            
            sumA += no;
            sumB += noB;
        }

        Console.WriteLine($"Sum: {sumA}");
        Console.WriteLine($"SumB: {sumB}");
    }
}