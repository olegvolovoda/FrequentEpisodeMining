using System;
using System.Collections.Generic;
using System.Linq;
using DiscreteApproach;
using Xunit;

namespace Tests
{
    public class SymbolsBrainTests
    {
        private readonly ChainBuilder _chainBuilder = new ChainBuilder();

        [Fact]
        public void Perceive_ShouldPerceive_aaa_Sequence()
        {
            var brain = new SymbolsBrain();

            brain.PerceiveChain("aaaa");
            var result = brain.Perceive('a');
            Assert.Equal("a", result);
        }

        [Fact]
        public void Perceive_ShouldPerceive_deede_Sequence()
        {
            var brain = new SymbolsBrain();
            
            //string s = "Twohouseholds,bothalikeindignity, development InfairVerona,wherewelayourscene, Fromancientgrudgebreaktonewmutiny, Wherecivilbloodmakescivilhandsunclean. Fromforththefatalloinsofthesetwofoes".ToLower().Repeat(3);
            string s = "house".ToLower();
            var chain = new ChainBuilder().Build(new WordRepeat[] { new WordRepeat() { Word = s, Times = 50, MinGap = 8, MaxGap = 12 }, new WordRepeat() { Word = "window", Times = 50, MinGap = 2, MaxGap = 20 } }, 150, 3);

            //Console.Out.WriteLine(string.Join("\n", brain.GetAllSequences().ToArray()));

            var result = brain.PerceiveChain1(chain);

            result = brain.PerceiveChain("hous", false);
            //Assert.Equal("e", result);
            //Assert.Equal('e', result.Last());

            result = brain.PerceiveChain("windo", false);
            //Assert.Equal("w", result);
            //Assert.Equal('w', result.Last());
            
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

        [Fact]
        public void Perceive_RandomSequence()
        {
            var brain = new SymbolsBrain();

            //string s = "Twohouseholds,bothalikeindignity, development InfairVerona,wherewelayourscene, Fromancientgrudgebreaktonewmutiny, Wherecivilbloodmakescivilhandsunclean. Fromforththefatalloinsofthesetwofoes".ToLower().Repeat(3);
            string s = "";
            var random = new Random();
            for (int i = 0; i < 2000; i++)
            {
                s += (char)random.Next(97, 97 + 26);
            }

            var result = brain.PerceiveChain(s);

            Console.Out.WriteLine(s);
            Console.Out.WriteLine(result);
            Console.Out.WriteLine(string.Join("\n", brain.GetAllSequences().ToArray()));
        }

    }
}