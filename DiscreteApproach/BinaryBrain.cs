﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DiscreteApproach
{
    public class BinaryBrain
    {
        private Reasoner reasoner;

        public BinaryBrain(Reasoner reasoner)
        {
            this.reasoner = reasoner;
        }

        public string Perceive(string info)
        {
            reasoner.ApplyTruthRule(info == "0" ? 3 : 4);
            reasoner.InitNextGeneration();
            reasoner.AddSensorInfo(info == "0" ? 1 : 2);
            reasoner.NextLogicStep();
            var effectResults = reasoner.CalcEffectResults();

            if (Math.Abs(effectResults[0] - effectResults[1]) < 0.001) return "_";
            return effectResults[0] > effectResults[1] ? "D" : "U";
        }

        public string PerceiveChain(string chain)
        {
            string result = "";

            foreach (var item in chain)
            {
                result += Perceive(item.ToString());
            }

            return result;
        }
    }

    public class BinaryBrainTests
    {
        [Fact]
        public void Perceive_ShouldRecognize010Sequence()
        {
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
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
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
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
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
                    new Rule(){Name = 9, Cause = 5, Result = 5},
                };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.PerceiveChain("010100");

            Assert.Equal('D', reaction.Last());
        }

        [Fact]
        public void PerceiveChain_ShouldRecognize000010Sequence()
        {
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
                    new Rule(){Name = 9, Cause = 5, Result = 5},
                };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.PerceiveChain("000010");

            Assert.Equal('U', reaction.Last());
        }

        [Fact]
        public void PerceiveChain_ShouldRecognize01SequenceAlternative()
        {
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 4},
                    
                };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.PerceiveChain("01");

            Assert.Equal('D', reaction.Last());
        }

        [Fact]
        public void Perceive_ShouldRecognize0As00Sequence()
        {
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3, Weight = 0.1}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4, Weight = 0}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3, Weight = 0}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6, Weight = 0},
                    new Rule(){Name = 9, Cause = 5, Result = 5, Weight = 0},
                };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.Perceive("0");

            Assert.Equal("D", reaction);
        }

        [Fact]
        public void Perceive_ShouldLearn0Sequence()
        {
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3, Weight = 0}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4, Weight = 0}, 
                };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            binaryBrain.Perceive("0");
            
            var reaction = binaryBrain.Perceive("0");

            Assert.Equal("D", reaction);
        }

        [Fact]
        public void Perceive_ShouldLearn01Sequence()
        {
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3, Weight = 0}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4, Weight = 0}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3, Weight = 0}, 
                    new Rule(){Name = 8, Cause = 2, Result = 4, Weight = 0},
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
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3, Weight = 0}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4, Weight = 0}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3, Weight = 0}, 
                    new Rule(){Name = 8, Cause = 2, Result = 4, Weight = 0},
                    new Rule(){Name = 9, Cause = 7, Result = 6, Weight = 0},
                    new Rule(){Name = 10, Cause = 5, Result = 5, Weight = 0},
                };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.PerceiveChain("000000101010101000001010100");

            Assert.Equal('D', reaction.Last());
        }

        [Fact]
        public void Perceive_ShouldLearn0SequenceFromScratch()
        {
            var rules = new List<Rule>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.PerceiveChain("0000");

            Assert.Equal('D', reaction.Last());
        }

        [Fact]
        public void Perceive_ShouldLearn0And01SequencesFromScratch()
        {
            var rules = new List<Rule>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.PerceiveChain("0000000010101010101010100000000010101010000010");

            Assert.Equal('U', reaction.Last());

            reaction = binaryBrain.PerceiveChain("000");

            Assert.Equal('D', reaction.Last());
        }

        [Fact]
        public void Perceive_ShouldLearn110SequencesFromScratch()
        {
            var rules = new List<Rule>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            binaryBrain.PerceiveChain("110110110110110110110110110110110110110");
            var reaction = binaryBrain.PerceiveChain("110");

            Assert.Equal("UDU", reaction);
        }

        [Fact]
        public void Perceive_ShouldLearn110And1110SequencesFromScratch()
        {
            var rules = new List<Rule>();

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            binaryBrain.PerceiveChain("1110".Repeat(10));
            binaryBrain.PerceiveChain("110".Repeat(10));
            binaryBrain.PerceiveChain("1110".Repeat(10));
            binaryBrain.PerceiveChain("110".Repeat(10));
            var reaction = binaryBrain.PerceiveChain("110");
            Assert.Equal("UDU", reaction);
            binaryBrain.PerceiveChain("1110".Repeat(3));
            reaction = binaryBrain.PerceiveChain("1110");
            Assert.Equal("UUDU", reaction);
        }
    }
}
