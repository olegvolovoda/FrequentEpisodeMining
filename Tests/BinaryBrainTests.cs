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
                new RuleInfo(){Name = 5, Cause = 1, Result = 3}, 
                new RuleInfo(){Name = 6, Cause = 1, Result = 4}, 
                new RuleInfo(){Name = 7, Cause = 2, Result = 3}, 
                new RuleInfo(){Name = 8, Cause = 7, Result = 6},
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
        public void PerceiveChain_ShouldRecognize010100Sequence()
        {
            var rules = new List<RuleInfo>
            {
                //new RuleInfo(){Name = 5, Cause = 1, Result = 3}, 
                //new RuleInfo(){Name = 6, Cause = 1, Result = 4}, 
                //new RuleInfo(){Name = 7, Cause = 2, Result = 3}, 
                //new RuleInfo(){Name = 8, Cause = 7, Result = 6},
                //new RuleInfo(){Name = 9, Cause = 5, Result = 5},
            };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.PerceiveChain("010100");

            Assert.Equal('D', reaction.Last());
        }

        [Fact]
        public void PerceiveChain_ShouldRecognize000010Sequence()
        {
            var rules = new List<RuleInfo>
            {
                //new RuleInfo(){Name = 5, Cause = 1, Result = 3}, 
                //new RuleInfo(){Name = 6, Cause = 1, Result = 4}, 
                //new RuleInfo(){Name = 7, Cause = 2, Result = 3}, 
                //new RuleInfo(){Name = 8, Cause = 7, Result = 6},
                //new RuleInfo(){Name = 9, Cause = 5, Result = 5},
            };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.PerceiveChain("000010");

            Assert.Equal('U', reaction.Last());
        }

        [Fact]
        public void PerceiveChain_ShouldRecognize01SequenceAlternative()
        {
            var rules = new List<RuleInfo>
            {
                new RuleInfo(){Name = 5, Cause = 1, Result = 3}, 
                new RuleInfo(){Name = 6, Cause = 1, Result = 4}, 
                new RuleInfo(){Name = 7, Cause = 2, Result = 3}, 
                new RuleInfo(){Name = 8, Cause = 7, Result = 4},
                    
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
                new RuleInfo(){Name = 5, Cause = 1, Result = 3, Weight = 0.5}, 
                new RuleInfo(){Name = 6, Cause = 1, Result = 4, Weight = 0}, 
                new RuleInfo(){Name = 7, Cause = 2, Result = 3, Weight = 0}, 
                new RuleInfo(){Name = 8, Cause = 7, Result = 6, Weight = 0},
                new RuleInfo(){Name = 9, Cause = 5, Result = 5, Weight = 0},
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
                new RuleInfo(){Name = 5, Cause = 1, Result = 3, Weight = 0}, 
                new RuleInfo(){Name = 6, Cause = 1, Result = 4, Weight = 0}, 
                new RuleInfo(){Name = 7, Cause = 2, Result = 3, Weight = 0}, 
                new RuleInfo(){Name = 8, Cause = 2, Result = 4, Weight = 0},
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
                new RuleInfo(){Name = 5, Cause = 1, Result = 3, Weight = 0.5}, 
                new RuleInfo(){Name = 6, Cause = 1, Result = 4, Weight = 0.5}, 
                new RuleInfo(){Name = 7, Cause = 2, Result = 3, Weight = 0.5}, 
                new RuleInfo(){Name = 8, Cause = 2, Result = 4, Weight = 0.5},
                new RuleInfo(){Name = 9, Cause = 7, Result = 6, Weight = 0.5},
                new RuleInfo(){Name = 10, Cause = 5, Result = 5, Weight = 0.5},
            };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.PerceiveChain("000000101010101000001010100");

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

            binaryBrain.PerceiveChain("001".Repeat(5));
            binaryBrain.PerceiveChain("110".Repeat(5));
            binaryBrain.PerceiveChain("010".Repeat(5));

            binaryBrain.PerceiveChain("001".Repeat(2));
            var reaction = binaryBrain.PerceiveChain("001");
            Assert.Equal("DUD", reaction);

            binaryBrain.PerceiveChain("110".Repeat(3));
            reaction = binaryBrain.PerceiveChain("110");
            Assert.Equal("UDU", reaction);

            binaryBrain.PerceiveChain("010".Repeat(2));
            reaction = binaryBrain.PerceiveChain("010");
            Assert.Equal("UDD", reaction);
        }

        [Fact]
        public void Perceive_ShouldLearn1110SequenceFromScratch()
        {
            var rules = new List<RuleInfo>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            binaryBrain.PerceiveChain("01".Repeat(10));
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
        public void Perceive_ShouldLearnSandbox()
        {
            var rules = new List<RuleInfo>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            ////var reaction = binaryBrain.PerceiveChain("0000011111010101010101110110110110110110110".Repeat(2));
            var reaction = binaryBrain.PerceiveChain("00000000000001111111111111111100000000000010101010101010100000000000101010111110101010101");

            Assert.Equal("", reaction);
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
