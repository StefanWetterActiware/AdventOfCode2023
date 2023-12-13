using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

static class Day13 {
    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));
                                           
//         input = """

// """;

        var blocks = Helper.getBlocks(input);

        var sumA = 0;
        var sumB = 0;

        foreach (var block in blocks)
        {
            bool found=false;
            for (int i = 1; i < block.Count(); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    found=true;
                    if (block.Count>i+j && !block[i-1-j].Equals(block[i+j])){
                        found=false;
                        break;
                    }
                }
                if (found) {
                    sumA+=100*i;
                    break;
                }
            }
            if(!found){
                //column-Suche
                var colStrings = new List<string>();
                for (int colNo = 0; colNo < block.First().Length; colNo++)
                {
                    colStrings.Add(String.Join("",block.Select(a=>a.Substring(colNo,1))));
                }
                for (int i = 1; i < colStrings.Count(); i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        found=true;
                        if (colStrings.Count>i+j && !colStrings[i-1-j].Equals(colStrings[i+j])){
                            found=false;
                            break;
                        }
                    }
                    if (found) {
                        sumA+=i;
                        break;
                    }
                }
            }
        }


        Console.WriteLine($"Sum: {sumA}");
        Console.WriteLine($"SumB: {sumB}");
    }
}