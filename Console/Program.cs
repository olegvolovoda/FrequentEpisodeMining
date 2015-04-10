using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscreteApproach;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //var brain = new BinaryBrain(new Reasoner(new List<RuleInfo>()));

            //System.Console.WriteLine("Enter '0' and '1' sequence:");
            //string input;
            //do
            //{
            //    input = System.Console.ReadLine();
            //    var output = brain.PerceiveChain(input);
            //    System.Console.WriteLine(" " + output.Replace('D', '0').Replace('U', '1'));
            //} 
            //while (input != string.Empty);

            var brain = new SymbolsBrain();
            string text = File.ReadAllText("d:\\text\\text.txt");
            string s = text.ToLower().Substring(0, 1000).Repeat(8);
            var result = brain.PerceiveChain(s);
            System.Console.WriteLine(string.Join("\n", brain.GetAllSequences().ToArray()));
            System.Console.ReadKey();
        }
    }
}
