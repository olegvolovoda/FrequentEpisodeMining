using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DiscreteApproach
{
    public class SymbolsBrain
    {
        private Reasoner reasoner;

        public SymbolsBrain()
        {
            this.reasoner = new Reasoner(new List<RuleInfo>(), 26, 26);
        }

        public string Perceive(string info)
        {
            reasoner.ApplyTruthRule((int)info.First() - 96 + 26);
            reasoner.InitNextGeneration();
            reasoner.AddSensorInfo((int)info.First() - 96);
            reasoner.NextLogicStep();
            var effectResults = reasoner.CalcEffectResults();

            if (effectResults.Count(result => result) == 1)
            {
                return ((char)(effectResults.IndexMax(result => result ? 1 : 0) + 97)).ToString();
            }
            else
            {
                return "_";
            }
        }

        public string PerceiveChain(string chain, bool learn = true)
        {
            string result = "";

            foreach (var item in chain)
            {
                if (Char.IsLower(item))
                {
                    result += Perceive(item.ToString());
                }
            }

            return result;
        }

        public List<string> GetAllSequences()
        {
            var strings = new List<string>();

            var sequences = reasoner.GetAllSequences();
            foreach (var sequence in sequences)
            {
                var s = new string(sequence.Sequence.Select(item => (char)(item + 96)).ToArray()) + " \t" + sequence.Rule.Weight; 
                strings.Add(s);
            }

            return strings;
        }
    }

    public class SymbolsBrainTests
    {
        [Fact]
        public void Perceive_ShouldPerceive_aaa_Sequence()
        {
            var brain = new SymbolsBrain();

            brain.PerceiveChain("aaaa");
            var result = brain.Perceive("a");
            Assert.Equal("a", result);
        }

        [Fact]
        public void Perceive_ShouldPerceive_deede_Sequence()
        {
            var brain = new SymbolsBrain();
            
            string s = "Twohouseholds,bothalikeindignity, development InfairVerona,wherewelayourscene, Fromancientgrudgebreaktonewmutiny, Wherecivilbloodmakescivilhandsunclean. Fromforththefatalloinsofthesetwofoes".ToLower().Repeat(3);
            //brain.PerceiveChain("kk" + "abcdduabbccdk".Repeat(50));// + "abrakadabra" + "abcddddd".Repeat(3));
            //brain.PerceiveChain(s);
            var result = brain.PerceiveChain(s.Repeat(6));
            //Assert.Equal("b", result);
            Console.Out.WriteLine(result);
            Console.Out.WriteLine(string.Join("\n", brain.GetAllSequences().ToArray()));
        }

        [Fact]
        public void Perceive_ShouldPerceive_abc_Sequence()
        {
            var brain = new SymbolsBrain();
            //string s =
            //    "Twohouseholds,bothalikeindignity, development InfairVerona,wherewelayourscene,development  Fromancientgrudgebreaktonewmutiny, Wherecivilbloodmakescivilhandsunclean.development Fromforththefatalloinsofthesetwofoes Apairofstar-cross'dloverstaketheirlife; Whosemisadventuredpiteousoverthrows Dowiththeirdeathburytheirparents'strife. Thefearfulpassageoftheirdeath-mark'dlove, Andthecontinuanceoftheirparents'rage, Which,buttheirchildren'send,noughtcouldremove"
            //        .ToLower().Repeat(8);
            string s = "aaaaaa" +
                       String.Format(
                           //"{0}kkkkkkkkkkkkk{1}asdfandf{0}asdfasdfaldfddcd{1}kdiekdiekdhfghg{0}jdje{1}gdgstdgd{0}rcrsd{1}kdie{0}dsaf{1}cveve{0}sdkdd{1}ss{0}aa{1}".Repeat(3),
                           "{0}kkkkkkkkkkkkk{1}kkkk{0}kkkkkkkkkkkk{1}kkkkkkkkkk{0}kkkk{1}kkkkkkk{0}kkkkk{1}kkk{0}kkkk{1}kkkkk{0}kkkkk{1}kk{0}kk{0}".Repeat(7),
                           "bazazac", "dazazae");
            //brain.PerceiveChain("kk" + "abcdduabbccdk".Repeat(50));// + "abrakadabra" + "abcddddd".Repeat(3));
            //brain.PerceiveChain(s);
            var result = brain.PerceiveChain(s);
            //Assert.Equal("b", result);
            Console.Out.WriteLine(result);
            Console.Out.WriteLine(string.Join("\n", brain.GetAllSequences().ToArray()));
        }

    }
}
