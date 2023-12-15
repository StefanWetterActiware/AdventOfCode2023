using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

static class Day15 {
    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
                                           
//         input = """
// rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7
// """;

        IEnumerable<string> lines = Helper.getLines(input);

        long sumA = 0;
        long sumB = 0;

        //Go
        foreach (var block in lines.Single().Split(','))
        {
            var z = 0;
            foreach (char item in block.ToCharArray())
            {
                z+=(int)item;
                z*=17;
                z=z%256;
            }
            sumA+=z;
        }
        Console.WriteLine("a: " + sumA);

        Dictionary<string, int>[] boxes=new Dictionary<string, int>[256];
        for (int i = 0; i < 256; i++)
        {
            boxes[i]=new();
        }
        foreach (var block in lines.Single().Split(','))
        {
            var parts = block.Split("=-".ToCharArray());
            var label = parts[0];
            var focal = parts.Length > 1 && parts[1] != "" ? int.Parse(parts[1]) : 0;
            var hash=0;
            var sign = block[label.Length];
            foreach (char item in label.ToCharArray())
            {
                hash+=(int)item;
                hash*=17;
                hash=hash%256;
            }

            if (sign == '-'){
                if (boxes[hash].ContainsKey(label)){
                    boxes[hash].Remove(label);
                    //new because .net reuses empty spaces at the beginning lateron...
                    boxes[hash] = boxes[hash].ToDictionary(a=>a.Key, a=>a.Value);
                }
            } else {
                if (boxes[hash].ContainsKey(label))
                    boxes[hash][label] = focal;
                else
                    boxes[hash].Add(label,focal);
            }
            Console.WriteLine();
        }

        for (int i = 0; i < boxes.Length; i++)
        {
            var boxno=i+1;
            var slot=0;
            foreach (var item in boxes[i])
            {
                slot++;
                sumB+=(slot*boxno*item.Value);
            }
        }

        Console.WriteLine("b: " + sumB);
    }
}