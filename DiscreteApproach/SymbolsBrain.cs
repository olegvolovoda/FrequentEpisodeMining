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
        public void Perceive_ShouldPerceive_abc_Sequence()
        {
            var brain = new SymbolsBrain();
            
            string s = "Twohouseholds,bothalikeindignity, InfairVerona,wherewelayourscene, Fromancientgrudgebreaktonewmutiny, Wherecivilbloodmakescivilhandsunclean. Fromforththefatalloinsofthesetwofoes Apairofstar-cross'dloverstaketheirlife; Whosemisadventuredpiteousoverthrows Dowiththeirdeathburytheirparents'strife. Thefearfulpassageoftheirdeath-mark'dlove, Andthecontinuanceoftheirparents'rage, Which,buttheirchildren'send,noughtcouldremove, Isnowthetwohours'trafficofourstage; Thewhichifyouwithpatientearsattend, Whathereshallmiss,ourtoilshallstrivetomend. ".ToLower().Repeat(3);
            //brain.PerceiveChain("kk" + "abcdduabbccdk".Repeat(50));// + "abrakadabra" + "abcddddd".Repeat(3));
            //brain.PerceiveChain(s);
            var result = brain.PerceiveChain("edeedeede" + "udedeuedd".Repeat(4) + "deede".Repeat(40));
            //Assert.Equal("b", result);
            Console.Out.WriteLine(result);
            Console.Out.WriteLine(string.Join("\n", brain.GetAllSequences().ToArray()));
        }
    }
}
