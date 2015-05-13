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

            //var brain = new SymbolsBrain();
            //string text = File.ReadAllText("d:\\text\\text.txt");
            //string s = text.ToLower().Substring(0, 500).Repeat(25);
            //var result = brain.PerceiveChain(s);
            //System.Console.WriteLine(result);
            ////System.Console.WriteLine(string.Join("\n", brain.GetAllSequences().ToArray()));
     

            //var writer = File.CreateText("result.txt");
            //writer.WriteLine(result);
            //writer.WriteLine(string.Join("\n", brain.GetAllSequences().ToArray().Reverse()));
            //writer.Close();

            //System.Console.ReadKey();


            var brain = new SymbolsBrain();

            //string s = "Twohouseholds,bothalikeindignity, development InfairVerona,wherewelayourscene, Fromancientgrudgebreaktonewmutiny, Wherecivilbloodmakescivilhandsunclean. Fromforththefatalloinsofthesetwofoes".ToLower().Repeat(3);
            //string s = "house".ToLower();
            string s = "axxbcxxd".ToLower();

            var result = brain.PerceiveChain1(s.Repeat(30));

            System.Console.Out.WriteLine(result);
            System.Console.ReadKey();

            System.Console.Out.WriteLine(string.Join("\n", brain.GetAllSequences().ToArray()));

            var writer = File.CreateText("result.txt");
            writer.WriteLine(result);
            writer.WriteLine(string.Join("\n", brain.GetAllSequences().ToArray()));
            writer.Close();

            System.Console.ReadKey();
        }
    }
}
