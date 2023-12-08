using System.Linq.Expressions;
using System.Text.RegularExpressions;


static class Day8 {
    static long gcf(long a, long b)
{
    while (b != 0)
    {
        long temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

static long lcm(long a, long b)
{
    return (a / gcf(a, b)) * b;
}
    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var lines = Helper.getInputAsLines(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value),
                                           test:false);



        var rl = lines.First().Replace("R","1").Replace("L","0").ToCharArray().Select(a=>int.Parse(a.ToString()));
        var rllen=rl.Count();
        var inst = new Dictionary<string, string[]>();
        foreach (var line in lines.Skip(1).Where(a=>!String.IsNullOrWhiteSpace(a)))
        {
            var split = line.Split("=", StringSplitOptions.TrimEntries);
            var linerl = split[1].Trim("()".ToCharArray()).Split(",",StringSplitOptions.TrimEntries);
            inst.Add(split[0], linerl);
        }

        var next="AAA";
        var i = 0;
        while (next != "ZZZ"){
            var myRL = rl.Skip(i++%rllen).First();
            next=inst[next][myRL];
        }

        var nextB = inst.Keys.Where(a=>a.EndsWith("A")).ToList();
        long steps = 1;
        for (int k = 0; k < nextB.Count; k++)
        {
            var j=0;
            next=nextB[k];
            while(!next.EndsWith("Z")){
                var myRL = rl.Skip(j++%rllen).First();
                next=inst[next][myRL];
            }
            steps=lcm(steps,j);
        }
        

        Console.WriteLine($"Sum: {i}");
        Console.WriteLine($"SumB: {steps}");
    }
}