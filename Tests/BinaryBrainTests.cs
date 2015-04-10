using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscreteApproach;
using Xunit;

namespace Tests
{
    public class BinaryBrainTests
    {
        [Fact]
        public void Perceive_ShouldRecognize010Sequence()
        {
            var rules = new List<RuleInfo>
            {
                new RuleInfo(){Index = 5, Cause = 1, Result = 3}, 
                new RuleInfo(){Index = 6, Cause = 1, Result = 4}, 
                new RuleInfo(){Index = 7, Cause = 2, Result = 3}, 
                new RuleInfo(){Index = 8, Cause = 7, Result = 6},
            };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            binaryBrain.Perceive("0");
            var reaction = binaryBrain.Perceive("1");

            Assert.Equal("D", reaction);
        }

        [Fact]
        public void Perceive_ShouldRecognize0101Sequence()
        {
            var rules = new List<RuleInfo>
            {
            };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            binaryBrain.Perceive("0");
            binaryBrain.Perceive("1");
            var reaction = binaryBrain.Perceive("0");

            Assert.Equal("U", reaction);
        }

        [Fact]
        public void PerceiveChain_ShouldRecognize01SequenceAlternative()
        {
            var rules = new List<RuleInfo>
            {
                new RuleInfo(){Index = 5, Cause = 1, Result = 3}, 
                new RuleInfo(){Index = 6, Cause = 1, Result = 4}, 
                new RuleInfo(){Index = 7, Cause = 2, Result = 3}, 
                new RuleInfo(){Index = 8, Cause = 7, Result = 4},
                    
            };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.PerceiveChain("01");

            Assert.Equal('D', reaction.Last());
        }

        [Fact]
        public void Perceive_ShouldRecognize0As00Sequence()
        {
            var rules = new List<RuleInfo>
            {
                new RuleInfo(){Index = 5, Cause = 1, Result = 3, Total = 3, Successes = 2}, 
                new RuleInfo(){Index = 6, Cause = 1, Result = 4, Total = 1, Successes = 0}, 
                new RuleInfo(){Index = 7, Cause = 2, Result = 3, Total = 1, Successes = 0}, 
                new RuleInfo(){Index = 8, Cause = 7, Result = 6, Total = 1, Successes = 0},
                new RuleInfo(){Index = 9, Cause = 5, Result = 5, Total = 1, Successes = 0},
            };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.Perceive("0");

            Assert.Equal("D", reaction);
        }

        [Fact]
        public void Perceive_ShouldLearn0Sequence()
        {
            var rules = new List<RuleInfo>();
            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            binaryBrain.Perceive("0");

            var reaction = binaryBrain.Perceive("0");

            Assert.Equal("D", reaction);
        }

        [Fact]
        public void Perceive_ShouldLearn01Sequence()
        {
            var rules = new List<RuleInfo>
            {
                new RuleInfo(){Index = 5, Cause = 1, Result = 3, Total = 1, Successes = 0}, 
                new RuleInfo(){Index = 6, Cause = 1, Result = 4, Total = 1, Successes = 0}, 
                new RuleInfo(){Index = 7, Cause = 2, Result = 3, Total = 1, Successes = 0}, 
                new RuleInfo(){Index = 8, Cause = 2, Result = 4, Total = 1, Successes = 0},
            };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            binaryBrain.Perceive("0");
            binaryBrain.Perceive("1");
            binaryBrain.Perceive("0");
            binaryBrain.Perceive("1");

            var reaction = binaryBrain.Perceive("0");

            Assert.Equal("U", reaction);

            reaction = binaryBrain.Perceive("1");
            Assert.Equal("D", reaction);
        }

        [Fact]
        public void Perceive_ShouldLearn01And0Sequences()
        {
            var rules = new List<RuleInfo>
            {
                new RuleInfo(){Index = 5, Cause = 1, Result = 3, Total = 1, Successes = 1}, 
                new RuleInfo(){Index = 6, Cause = 1, Result = 4, Total = 1, Successes = 1}, 
                new RuleInfo(){Index = 7, Cause = 2, Result = 3, Total = 1, Successes = 1}, 
                new RuleInfo(){Index = 8, Cause = 2, Result = 4, Total = 1, Successes = 1},
                new RuleInfo(){Index = 9, Cause = 7, Result = 6, Total = 1, Successes = 1},
                new RuleInfo(){Index = 10, Cause = 5, Result = 5, Total = 1, Successes = 1},
            };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.PerceiveChain("0000001010101010000010101000");

            Assert.Equal('D', reaction.Last());
        }

        [Fact]
        public void Perceive_ShouldLearn0SequenceFromScratch()
        {
            var rules = new List<RuleInfo>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.PerceiveChain("0000");

            Assert.Equal('D', reaction.Last());
        }

        [Fact]
        public void Perceive_ShouldLearn0And01SequencesFromScratch()
        {
            var rules = new List<RuleInfo>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.PerceiveChain("0000000010101010101010100000000010101010000010".Repeat(10));
            //var reaction = binaryBrain.PerceiveChain("01010101010100101010101010000001111110101010111110000000");

            //Assert.Equal("U", reaction);
            Assert.Equal('U', reaction.Last());

            reaction = binaryBrain.PerceiveChain("000");

            Assert.Equal('D', reaction.Last());
        }

        [Fact]
        public void Perceive_ShouldLearn110SequencesFromScratch()
        {
            var rules = new List<RuleInfo>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            binaryBrain.PerceiveChain("110110110110110110110110110110110110110");
            var reaction = binaryBrain.PerceiveChain("110");

            Assert.Equal("UDU", reaction);
        }

        [Fact]
        public void Perceive_ShouldLearnThreeDigitSequencesFromScratch()
        {
            var rules = new List<RuleInfo>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            binaryBrain.PerceiveChain("001".Repeat(10));
            binaryBrain.PerceiveChain("110".Repeat(5));
            //binaryBrain.PerceiveChain("010".Repeat(5));

            binaryBrain.PerceiveChain("001".Repeat(1));
            var reaction = binaryBrain.PerceiveChain("001");
            Assert.Equal("DUD", reaction);

            binaryBrain.PerceiveChain("110".Repeat(2));
            reaction = binaryBrain.PerceiveChain("110");
            Assert.Equal("UDU", reaction);

            binaryBrain.PerceiveChain("010".Repeat(1));
            reaction = binaryBrain.PerceiveChain("010");
            Assert.Equal("UDD", reaction);
        }

        [Fact]
        public void Perceive_ShouldLearn1110SequenceFromScratch()
        {
            var rules = new List<RuleInfo>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            //binaryBrain.PerceiveChain("01".Repeat(10));
            binaryBrain.PerceiveChain("1110".Repeat(5));
            var reaction = binaryBrain.PerceiveChain("1110");

            Assert.Equal("UUDU", reaction);
        }

        [Fact]
        public void Perceive_ShouldLearn1100Sequence()
        {
            var rules = new List<RuleInfo>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            binaryBrain.PerceiveChain("1100".Repeat(20));
            var reaction = binaryBrain.PerceiveChain("1100");

            Assert.Equal("UDDU", reaction);
        }

        [Fact]
        public void Perceive_ShouldLearnLongSequencesSequence()
        {
            var rules = new List<RuleInfo>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var longSequence0 = "10010";
            var longSequence1 = "100101";
            var longSequence2 = "010001";
            //var longSequence1 = "110011";
            //var longSequence2 = "111100";

            //binaryBrain.PerceiveChain(longSequence0.Repeat(7));
            binaryBrain.PerceiveChain(longSequence1.Repeat(7));
            binaryBrain.PerceiveChain(longSequence2.Repeat(7));
            binaryBrain.PerceiveChain(longSequence1.Repeat(7));
            binaryBrain.PerceiveChain(longSequence2.Repeat(7));
            binaryBrain.PerceiveChain(longSequence1.Repeat(7));
            binaryBrain.PerceiveChain(longSequence2.Repeat(7));
            binaryBrain.PerceiveChain(longSequence1.Repeat(7));
            binaryBrain.PerceiveChain(longSequence2.Repeat(7));
            binaryBrain.PerceiveChain(longSequence1.Repeat(7));
            binaryBrain.PerceiveChain(longSequence2.Repeat(7));
            binaryBrain.PerceiveChain(longSequence1.Repeat(7));
            binaryBrain.PerceiveChain(longSequence2.Repeat(7));
            binaryBrain.PerceiveChain(longSequence1.Repeat(7));
            binaryBrain.PerceiveChain(longSequence2.Repeat(7));

            binaryBrain.PerceiveChain(longSequence1);
            var reaction = binaryBrain.PerceiveChain(longSequence1);

            //Assert.Equal("UDDUUU", reaction);
            Assert.Equal("DDUDUU", reaction);
        }

        [Fact]
        public void Perceive_ShouldLearnSandbox()
        {
            var rules = new List<RuleInfo>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            ////var reaction = binaryBrain.PerceiveChain("0000011111010101010101110110110110110110110".Repeat(2));
            var reaction = binaryBrain.PerceiveChain("00000000000001111111111111111100000000000010101010101010100000000000101010111110101010101");

            Assert.Equal("", reaction);
        }

        [Fact]
        public void Temp()
        {
            //Sample 1:00010101010100000000001010101000000000000010
            var rules = new List<RuleInfo>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            //var reaction = binaryBrain.PerceiveChain("00010101010100000000001010101000000000000010");
            //var reaction = binaryBrain.PerceiveChain("0001".Repeat(20) + "1101".Repeat(20) + "0001".Repeat(3) + "1101".Repeat(2));

            var sequence = "000110101";
            binaryBrain.PerceiveChain(sequence.Repeat(70) + "0011101".Repeat(20));

            binaryBrain.PerceiveChain(sequence.Repeat(10));
            var reaction = binaryBrain.PerceiveChain(sequence);
            Assert.Equal("DDUUDUDUD", reaction);
        }

        [Fact]
        public void Temp2()
        {
            var rules = new List<RuleInfo>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            //var reaction = binaryBrain.PerceiveChain("00010101010100000000001010101000000000000010");
            //var reaction = binaryBrain.PerceiveChain("0001".Repeat(20) + "1101".Repeat(20) + "0001".Repeat(3) + "1101".Repeat(2));

            var sequence = "00011010";
            binaryBrain.PerceiveChain("00011010" + "00111010" + "00111010" + "00011010" + "00011010" + "00111010" + "00011010" + "00111010");

            //binaryBrain.PerceiveChain(sequence.Repeat(10));
            var reaction = binaryBrain.PerceiveChain("00011010");
            Assert.Equal("", reaction);
        }

        [Fact]
        public void TestPrediction()
        {
            var rules = new List<RuleInfo>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            //var reaction = binaryBrain.PerceiveChain("00010101010100000000001010101000000000000010");
            //var reaction = binaryBrain.PerceiveChain("0001".Repeat(20) + "1101".Repeat(20) + "0001".Repeat(3) + "1101".Repeat(2));

            //var random = "000111000001001010101110110001000000001111110100101111110001011100000100 01111111 10111000 11110100 10001001 00001001 10111001 111101100" +
            //             "0111100 11101111 11100111 00110011 11100011 00011111 00001101 01010010 ";

            var random = "00011100 00010010 10101110 11000100 00000011 11110100 10111111 00010111 01010111 01011100 10000111 00100111 01011110 11001010 1001011" +
                         " 11010110 00110000 00000100 01111111 10111000 11110100 10001001 00001001 10111001 11110110 00111100 11101111 11100111 00110011 1001011" +
                         " 11100011 00011111 00001101 01010010 10101110 10100101 01100000 10100100 10111101 11010101 01000001 10000100 11010101 01011101 1001011" +
                         " 00110000 11110101 00001101 01010111 00011110 10111001 10011100 00111101 10011001 11100100 10111100 10100110 10010111 10111000 1001011" +
                         " 11011000 01001001 01000010 11111010 00101001 01010110 11101101 00111001 00100100 00010111 10101101 00011100 11000110 11100010 11011011 00100011 1001011" +
                         "11000101 11001001 01101101 11110111 10100111 00111000 10011001 11011111 10011001 01001000 00111110 00110011 10100101 10000101 1001011" +
                         "11110001 01101110 00010010 01000010 11100111 00100000  1001011 10010011 11001100 11001000 10001010 00110110 11010011 10111100 11001010 ".Replace(" ", "");
            var sequence = "".Replace(" ", "");
            //binaryBrain.PerceiveChain(sequence);
            
            //binaryBrain.PerceiveChain("1011".Repeat(10));
            //binaryBrain.PerceiveChain("1000".Repeat(10));
            //binaryBrain.PerceiveChain("1011".Repeat(3));
            //binaryBrain.PerceiveChain("10".Repeat(10));
            //binaryBrain.PerceiveChain("10".Repeat(10));
            //binaryBrain.PerceiveChain("1001".Repeat(10));
            //var reaction = binaryBrain.PerceiveChain("1011", false);
            //Assert.Equal("", reaction);

            //binaryBrain.PerceiveChain(sequence.Repeat(10));
            var prediction = binaryBrain.PerceiveChain(random);
            //var prediction = binaryBrain.PredictNext(20);
            Assert.Equal("", prediction);
        }

        [Fact]
        public void RandomTest()
        {
            var rules = new List<RuleInfo>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            binaryBrain.PerceiveChain("0001".Repeat(10));
            binaryBrain.PerceiveChain("0011".Repeat(10));
            binaryBrain.PerceiveChain("1101".Repeat(10));

            binaryBrain.PerceiveChain("0011".Repeat(2));
            var reaction = binaryBrain.PerceiveChain("0011");
            Assert.Equal("DUUD", reaction);

            binaryBrain.PerceiveChain("0001".Repeat(3));
            reaction = binaryBrain.PerceiveChain("0001");
            //Assert.Equal("DDUD", reaction);

            binaryBrain.PerceiveChain("1101".Repeat(2));
            reaction = binaryBrain.PerceiveChain("1101");
            Assert.Equal("UDUU", reaction);

        }


        //[Fact]
        //public void Perceive_ShouldLearn110And1110SequencesFromScratch()
        //{
        //    var rules = new List<RuleInfo>();

        //    var reasoner = new Reasoner(rules);

        //    var binaryBrain = new BinaryBrain(reasoner);

        //    binaryBrain.PerceiveChain("1".Repeat(10));
        //    binaryBrain.PerceiveChain("0".Repeat(10));
        //    binaryBrain.PerceiveChain("10".Repeat(10));
        //    binaryBrain.PerceiveChain("1110".Repeat(10));
        //    binaryBrain.PerceiveChain("110".Repeat(10));
        //    binaryBrain.PerceiveChain("1110".Repeat(10));
        //    binaryBrain.PerceiveChain("110".Repeat(10));
        //    var reaction = binaryBrain.PerceiveChain("110");
        //    Assert.Equal("UDU", reaction);
        //    binaryBrain.PerceiveChain("1110".Repeat(3));
        //    reaction = binaryBrain.PerceiveChain("1110");
        //    Assert.Equal("UUDU", reaction);
        //}
    }
}
