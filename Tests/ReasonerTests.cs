using System;
using System.Collections.Generic;
using System.Linq;
using DiscreteApproach;
using Xunit;

namespace Tests
{
    public class ReasonerTests
    {
        [Fact]
        public void CalcConsequences_SholdProcessRule()
        {
            var rules = new List<RuleInfo>
                {
                    new RuleInfo(){Name = 5, Cause = 1, Result = 3}, 
                };

            var reasoner = new Reasoner(rules);
            reasoner.InputRules = new int[] { 1 }.ToList();

            reasoner.NextLogicStep();

            Assert.Equal(new int[] { 3 }, reasoner.OutputRules);
            Assert.Equal(new int[] { 5 }, reasoner.ExecutedRules);
        }

        [Fact]
        public void CalcConsequences_ShouldProcessFewRules()
        {
            var rules = new List<RuleInfo>
                {
                    new RuleInfo(){Name = 5, Cause = 1, Result = 3}, 
                    new RuleInfo(){Name = 6, Cause = 1, Result = 4}, 
                    new RuleInfo(){Name = 7, Cause = 2, Result = 3}, 
                    new RuleInfo(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);

            reasoner.InputRules = new int[] { 1, 7 }.ToList();
            reasoner.NextLogicStep();

            Assert.Equal(new int[] { 3, 4, 6 }, reasoner.OutputRules);
            Assert.Equal(new int[] { 5, 6, 8 }, reasoner.ExecutedRules);
        }

        [Fact]
        public void GetBasisRules_ShouldGoToBasis()
        {
            var rules = new List<RuleInfo>
                {
                    new RuleInfo(){Name = 5, Cause = 1, Result = 3}, 
                    new RuleInfo(){Name = 6, Cause = 1, Result = 4}, 
                    new RuleInfo(){Name = 7, Cause = 2, Result = 3}, 
                    new RuleInfo(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);
            reasoner.OutputRules = new[] { 3, 6 }.ToList();
            reasoner.ExecutedRules = new[] { 5, 8 }.ToList();

            var basisConsequences = reasoner.GetConfirmRuleSets();

            Assert.Equal(1, basisConsequences[0].Count());
            Assert.Equal(0, basisConsequences[1].Count());
        }

        [Fact]
        public void GetBasisRules_ShouldGoToBasis2()
        {
            var rules = new List<RuleInfo>
                {
                    new RuleInfo(){Name = 5, Cause = 1, Result = 3}, 
                    new RuleInfo(){Name = 6, Cause = 1, Result = 4}, 
                    new RuleInfo(){Name = 7, Cause = 2, Result = 3}, 
                    new RuleInfo(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);
            reasoner.OutputRules = new[] { 3, 6, 4 }.ToList();
            reasoner.ExecutedRules = new[] { 5, 8, 6 }.ToList();

            var basisConsequences = reasoner.GetConfirmRuleSets();

            Assert.Equal(1, basisConsequences[0].Count());
            Assert.Equal(2, basisConsequences[1].Count());
        }

        [Fact]
        public void GetBasisRules_ShouldGiveInfoAboutActiveRelatedToBasisRules()
        {
            var rules = new List<RuleInfo>
                {
                    new RuleInfo(){Name = 5, Cause = 1, Result = 3}, 
                    new RuleInfo(){Name = 6, Cause = 1, Result = 4}, 
                    new RuleInfo(){Name = 7, Cause = 2, Result = 3}, 
                    new RuleInfo(){Name = 8, Cause = 7, Result = 6},
                    new RuleInfo(){Name = 9, Cause = 5, Result = 5},
                };

            var reasoner = new Reasoner(rules);
            reasoner.OutputRules = new[] { 5, 3, 6, 4 }.ToList();
            reasoner.ExecutedRules = new[] { 9, 5, 8, 6 }.ToList();

            var relatedToBasisActiveRules = reasoner.GetConfirmRuleSets();

            Assert.Equal(new int[] { 3, 5 }, relatedToBasisActiveRules[0]);
            Assert.Equal(new int[] { 4, 6 }, relatedToBasisActiveRules[1]);
        }

        [Fact]
        public void ApplyTruthBasis_ShouldLeftOnlyNodesThatLeadsToTruth()
        {
            var rules = new List<RuleInfo>
                {
                    new RuleInfo(){Name = 5, Cause = 1, Result = 3}, 
                    new RuleInfo(){Name = 6, Cause = 1, Result = 4}, 
                    new RuleInfo(){Name = 7, Cause = 2, Result = 3}, 
                    new RuleInfo(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);
            reasoner.OutputRules = new[] { 7, 5, 3, 6, 4 }.ToList();
            reasoner.ExecutedRules = new[] { 5, 6, 8 }.ToList();

            reasoner.ApplyTruthRule(4);

            Assert.Equal(new[] { 7, 5, 3, 6, 4 }, reasoner.OutputRules);
            Assert.Equal(new[] { 6, 8 }, reasoner.ExecutedRules);
        }

        [Fact]
        public void ApplyTruthBasis_ShouldLeftOnlyNodesThatLeadsToTruth2()
        {
            var rules = new List<RuleInfo>
                {
                    new RuleInfo(){Name = 5, Cause = 1, Result = 3}, 
                    new RuleInfo(){Name = 6, Cause = 1, Result = 4}, 
                    new RuleInfo(){Name = 7, Cause = 2, Result = 3}, 
                    new RuleInfo(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);
            reasoner.InputRules = new[] { 7, 5, 3, 6, 4 }.ToList();
            reasoner.ExecutedRules = new[] { 5, 6, 8 }.ToList();

            reasoner.InitNextGeneration();

            Assert.Equal(new[] { 5, 6, 8 }, reasoner.InputRules);
            Assert.Equal(new int[] { }, reasoner.ExecutedRules);
        }

        [Fact]
        public void ApplyTruthBasis_ShouldRecognize10Sequence()
        {
            var rules = new List<RuleInfo>
                {
                    new RuleInfo(){Name = 5, Cause = 1, Result = 3}, 
                    new RuleInfo(){Name = 6, Cause = 1, Result = 4}, 
                    new RuleInfo(){Name = 7, Cause = 2, Result = 3}, 
                    new RuleInfo(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);

            reasoner.AddSensorInfo(2);
            reasoner.NextLogicStep();
            reasoner.ApplyTruthRule(3);
            reasoner.InitNextGeneration();

            reasoner.InputRules.Add(1);
            reasoner.NextLogicStep();
            var confirmRuleSets = reasoner.GetConfirmRuleSets();

            Assert.Equal(new int[] { 3 }, confirmRuleSets[0]);
            Assert.Equal(new int[] { 4, 6 }, confirmRuleSets[1]);
        }

        [Fact]
        public void ApplyTruthBasis_ShouldRecognize0Sequence()
        {
            var rules = new List<RuleInfo>
                {
                    new RuleInfo(){Name = 5, Cause = 1, Result = 3}, 
                    new RuleInfo(){Name = 6, Cause = 1, Result = 4}, 
                    new RuleInfo(){Name = 7, Cause = 2, Result = 3}, 
                    new RuleInfo(){Name = 8, Cause = 7, Result = 6},
                    new RuleInfo(){Name = 9, Cause = 5, Result = 5},
                };

            var reasoner = new Reasoner(rules);
            reasoner.AddSensorInfo(1);
            reasoner.NextLogicStep();
            reasoner.ApplyTruthRule(3);
            reasoner.InitNextGeneration();

            reasoner.AddSensorInfo(1);
            reasoner.NextLogicStep();
            var confirmRuleSets = reasoner.GetConfirmRuleSets();

            Assert.Equal(new int[] { 3, 5 }, confirmRuleSets[0]);
            Assert.Equal(new int[] { 4 }, confirmRuleSets[1]);
        }

        [Fact]
        public void GetConfirmRuleSets2_()
        {
            var rules = new List<RuleInfo>
                {
                    new RuleInfo(){Name = 5, Cause = 1, Result = 3, Weight = 0}, 
                    new RuleInfo(){Name = 6, Cause = 1, Result = 4, Weight = 0}, 
                    new RuleInfo(){Name = 7, Cause = 2, Result = 3, Weight = 0}, 
                    new RuleInfo(){Name = 8, Cause = 7, Result = 6, Weight = 1},
                    new RuleInfo(){Name = 9, Cause = 5, Result = 5, Weight = 1},
                };

            var reasoner = new Reasoner(rules);

            reasoner.OutputRules = new[] { 3, 4, 5, 6 }.ToList();
            reasoner.ExecutedRules = new[] { 5, 6, 8, 9 }.ToList();

            var confirmRuleSets = reasoner.GetConfirmRuleSets2();

            Assert.Equal(5, confirmRuleSets[0][0].Name);
            Assert.Equal(9, confirmRuleSets[0][1].Name);
            Assert.Equal(6, confirmRuleSets[1][0].Name);
        }

        [Fact]
        public void GetConfirmRuleSets2_ShouldCalcEffectResults()
        {
            var rules = new List<RuleInfo>
                {
                    new RuleInfo(){Name = 5, Cause = 1, Result = 3, Weight = 0.2}, 
                    new RuleInfo(){Name = 6, Cause = 1, Result = 4, Weight = 0.2}, 
                    new RuleInfo(){Name = 7, Cause = 2, Result = 3, Weight = 0.2}, 
                    new RuleInfo(){Name = 8, Cause = 7, Result = 6, Weight = 1},
                    new RuleInfo(){Name = 9, Cause = 5, Result = 5, Weight = 1},
                };

            var reasoner = new Reasoner(rules);

            reasoner.OutputRules = new[] { 3, 4, 5, 6 }.ToList();
            reasoner.ExecutedRules = new[] { 5, 6, 7, 8, 9 }.ToList();

            double[] effectResults = reasoner.CalcEffectResults();

            Assert.Equal(1, effectResults[0]);
            Assert.Equal(1, effectResults[1]);
        }
    }

}
