// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var lines = System.IO.File.ReadAllLines("input/day1");
var sum = 0;
System.Text.RegularExpressions.Regex r = new(@"\d");

foreach (string line in lines)
{
    var first = r.Match(line);
    var last = r.Match(String.Join(' ', line.Reverse()));

    var no = int.Parse(first.Value + last.Value);
    sum += no;
}

Console.WriteLine($"Sum: {sum}");
