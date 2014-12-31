using System;
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
            var confirmRuleSets = reasoner.GetConfirmRuleSets();
            return confirmRuleSets[0].Count() > confirmRuleSets[1].Count() ? "D" : "U";
        }

        public string PerceiveChain(string chain)
        {
            string result = "";

            foreach (var item in chain)
            {
                result = Perceive(item.ToString());
            }

            return result;
        }
    }

    public class BinaryBrainTests
    {
        [Fact]
        public void Perceive_ShouldRecognize010Sequence()
        {
            var rules = new Rule[]
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
            var rules = new Rule[]
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
            var rules = new Rule[]
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

            Assert.Equal("D", reaction);
        }

        [Fact]
        public void PerceiveChain_ShouldRecognize000010Sequence()
        {
            var rules = new Rule[]
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

            Assert.Equal("U", reaction);
        }

        [Fact]
        public void PerceiveChain_ShouldRecognize01SequenceAlternative()
        {
            var rules = new Rule[]
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 4},
                    
                };

            var reasoner = new Reasoner(rules);

            var binaryBrain = new BinaryBrain(reasoner);

            var reaction = binaryBrain.PerceiveChain("01");

            Assert.Equal("D", reaction);
        }
    }
}
