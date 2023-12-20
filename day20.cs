using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Text.RegularExpressions;

abstract class relay{
    abstract public void handleSignal(signal sig, ref Queue<signal> queue, ref SortedDictionary<string, relay> allRelays);
    protected IEnumerable<relay> inputs {get;set;}
    public string name{get;set;}
    public List<string> outputs {get;set;}
    protected void sendToAllFollowers(bool high, ref Queue<signal> queue, ref SortedDictionary<string, relay> allRelays){
        foreach (var item in outputs)
        {
            if (allRelays.ContainsKey(item))
                queue.Enqueue(new signal(){from=this, to=allRelays[item], high=high});
            else
                queue.Enqueue(new signal(){from=this, high=high});
        }
    }
}
class flipflop:relay{
    private bool state = false;
    public override void handleSignal(signal sig, ref Queue<signal> queue, ref SortedDictionary<string, relay> allRelays){
        if (sig.high) return;
        state=!state;
        this.sendToAllFollowers(state, ref queue, ref allRelays);
    }
}
class conj:relay{
    private SortedDictionary<string, bool> states = null;
    private void initStates(ref SortedDictionary<string, relay> allRelays){
        if (states == null){
            states=new();
            this.inputs = allRelays.Values.Where(a=> a.outputs.Contains(this.name));
            foreach (var item in this.inputs)
            {
                states.Add(item.name, false);
            }
        }
    }
    public override void handleSignal(signal sig, ref Queue<signal> queue, ref SortedDictionary<string, relay> allRelays){
        initStates(ref allRelays);
        states[sig.from.name]=sig.high;
        this.sendToAllFollowers(!states.Values.All(a=>a==true), ref queue, ref allRelays);
    }
}
class broadcaster:relay{
    public override void handleSignal(signal sig, ref Queue<signal> queue, ref SortedDictionary<string, relay> allRelays){
        this.sendToAllFollowers(false, ref queue, ref allRelays);
    }
}

class signal{
    public bool high{get;set;}
    public relay from {get;set;}
    public relay to{get;set;}
}

static class Day20 {
    static List<string> lines;
    static Queue<signal> sigQueue = new();

    internal static void doit() {
        Regex dayNoR = new(@"\d*$");

        var input = Helper.getInput(int.Parse(dayNoR.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Value));

//        input = """
// broadcaster -> a, b, c
// %a -> b
// %b -> c
// %c -> inv
// &inv -> a
// """;

        broadcaster starter = null;
        SortedDictionary<string, relay> allRelays = new();
        lines = Helper.getLines(input);
        for (int i = 0; i < lines.Count; i++)
        {
            var parts = lines[i].Split("->", StringSplitOptions.TrimEntries);
            var ziele = parts[1].Split(",",StringSplitOptions.TrimEntries);
            relay r = null;
            switch (lines[i][0])
            {
                case '%':
                    r = new flipflop();
                    parts[0] = parts[0][1..];
                    break;
                case '&':
                    r = new conj();
                    parts[0] = parts[0][1..];
                    break;
                default:
                    r = new broadcaster();
                    break;
            }
            
            r.outputs = new List<string>(ziele);
            r.name=parts[0];
            if (r is broadcaster rbroad) {
                starter=rbroad;
                continue;
            }

            allRelays.Add(r.name, r);
        }

        var lowCounter = 0L;
        var highCounter = 0L;

        for (int i = 0; true; i++)
        {
            var rxCounter=0;
            starter.handleSignal(new signal(){ high=false}, ref sigQueue, ref allRelays);
            lowCounter++;
            while (sigQueue.Count>0) {
                var sig = sigQueue.Dequeue();
                if (sig.high)
                    highCounter++;
                else
                    lowCounter++;

                //Console.WriteLine((sig.from.name??"") + " -" + (sig.high ? "high" : "low") + "-> " + sig.to.name ?? "" );
                if (sig.to != null) sig.to.handleSignal(sig,ref sigQueue, ref allRelays); else rxCounter++;
            }

            if (i == 1000) {
               Console.WriteLine("a: " + lowCounter*highCounter);
            }

            if (rxCounter == 1) {
                Console.WriteLine("PartB: " + i+1);
            }
        }

        long sumB=0;

        // Console.WriteLine("b: " + sumB);
    }
}