using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

static class Day13 {
    static bool isPowerOfTwo(long x) 
    {
        // First x in the below expression is
        // for the case when x is 0 
        return x != 0 && ((x & (x - 1)) == 0);
    }
 
    // function to check whether the two numbers 
    // differ at one bit position only 
    static bool differAtOneBitPos(long a, long b)
    {
        return isPowerOfTwo(a ^ b);
    }
    static bool differAtOneBitPos(string a, string b)
    {
        var aa = a.ToCharArray();
        var ba = b.ToCharArray();
        long la=0;
        long lb=0;
        for (int i = 0; i < a.Length; i++)
        {
            la += aa[i] == '#' ? (long)Math.Pow(2,i) : 0;
            lb += ba[i] == '#' ? (long)Math.Pow(2,i) : 0;
        }
        return differAtOneBitPos(la, lb);
    }

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


        //Part2
        foreach (var block in blocks)
        {
            bool found=false;
            for (int i = 1; i < block.Count(); i++)
            {
                var countOneBitDiff = 0;
                var mirroredLines=0;
                for (int j = 0; j < i; j++)
                {
                    found=true;
                    if (block.Count>i+j && !block[i-1-j].Equals(block[i+j])){
                        if (countOneBitDiff > 0) {
                            // Wenn schon eine BitKorrektur, dann ists hier zu Ende
                            countOneBitDiff++;
                            found=false;
                            break;
                        }

                        if (countOneBitDiff == 0 && differAtOneBitPos(block[i-1-j], block[i+j])) {
                            //Erste Bitkorrektur ok
                            countOneBitDiff++;
                            mirroredLines++;
                        } else {
                            found=false;
                            break;
                        }
                    } else
                        mirroredLines++;
                }
                if (countOneBitDiff == 1 && found && mirroredLines>0) {
                    Console.WriteLine(100*i);
                    sumB+=100*i;
                    break;
                } else
                    found=false;
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
                    var countOneBitDiff = 0;
                    for (int j = 0; j < i; j++)
                    {
                        found=true;
                        if (colStrings.Count>i+j && !colStrings[i-1-j].Equals(colStrings[i+j])){
                            if (countOneBitDiff > 0) {
                                // Wenn schon eine BitKorrektur, dann ists hier zu Ende
                                countOneBitDiff++;
                                found=false;
                                break;
                            }

                            if (countOneBitDiff == 0 && differAtOneBitPos(colStrings[i-1-j], colStrings[i+j])) {
                                //Erste Bitkorrektur ok
                                countOneBitDiff++;
                            } else {
                                found=false;
                                break;
                            }
                        }
                    }
                    if (countOneBitDiff == 1 && found) {
                      Console.WriteLine(i);
                        sumB+=i;
                        break;
                    } else
                        found=false;
                }
            }
            if (!found){
                Console.WriteLine("kein treffer");
            }
        }

        Console.WriteLine($"SumB: {sumB}");
    }
}