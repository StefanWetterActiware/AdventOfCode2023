using System.Text.RegularExpressions;


class Day1 {
    internal static void doit(){

        Console.WriteLine("Hello, World!");

        var lines = System.IO.File.ReadAllLines("input/day1");
        System.Text.RegularExpressions.Regex r = new(@"(\d|one|two|three|four|five|six|seven|eight|nine)");

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

        var sum = 0;
        foreach (string line in lines)
        {
            var matches = r.Matches(line);
            var first = matches.First();
            var last = matches.Last();

            var no = int.Parse($"{num2num(first)}{num2num(last)}");
            Console.WriteLine(no);
            sum += no;
        }

        Console.WriteLine($"Sum: {sum}");
    }
}