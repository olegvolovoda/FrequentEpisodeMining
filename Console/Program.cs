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
            //string s = "axxxbcxxxd".ToLower();

            //var chain =
            //    new ChainBuilder().Build(
            //        new[]
            //            {
            //                new WordRepeat() {Word = "xaa", MinGap = 8, MaxGap = 32},
            //                new WordRepeat() {Word = "xbb", MinGap = 5, MaxGap = 28},
            //                new WordRepeat() {Word = "xcc", MinGap = 7, MaxGap = 40},
            //                new WordRepeat() {Word = "xab", MinGap = 3, MaxGap = 29},
            //                new WordRepeat() {Word = "xbc", MinGap = 5, MaxGap = 30},
            //                new WordRepeat() {Word = "xca", MinGap = 3, MaxGap = 27},
            //            }, 1500, 1);
            var chain = new ChainBuilder().Build(new WordRepeat[] { new WordRepeat() { Word = "axxb", MinGap = 8, MaxGap = 12 }, new WordRepeat() { Word = "cxxd", MinGap = 2, MaxGap = 12 } }, 1000, 3);
            //var chain = new ChainBuilder().Build(new WordRepeat[] { new WordRepeat() { Word = "house", Times = 100, MinGap = 8, MaxGap = 12 }, new WordRepeat() { Word = "window", Times = 100, MinGap = 2, MaxGap = 20 } }, 1000, 3);
            //var chain = new ChainBuilder().Build(new WordRepeat[] {}, 500, 3);
            //var chain = new ChainBuilder().Build(new WordRepeat[] { new WordRepeat() { Word = "house", Times = 50, MinGap = 8, MaxGap = 12 }}, 700, 3);
            var result = brain.PerceiveChain1(chain);

            //System.Console.Out.WriteLine(result);
            //System.Console.ReadKey();

            //System.Console.Out.WriteLine(string.Join("\n", brain.GetAllSequences().ToArray()));

            var writer = File.CreateText("result.txt");
            writer.WriteLine(result);
            writer.WriteLine(string.Join("\n", brain.GetAllSequences().ToArray()));
            writer.Close();

            //System.Console.ReadKey();
        }
    }
}
