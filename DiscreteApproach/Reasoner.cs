using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DiscreteApproach
{
    public class Reasoner
    {
        private Rule[] _rules;

        private List<int> activeRules;
        private List<int> executedRules;
        private int[] BasicOutputRules = new[] { 3, 4 };
        private static int FirstBasisOutputRule = 3;

        public Reasoner(Rule[] rules)
        {
            activeRules = new List<int>();
            this._rules = rules;
            ExecutedRules = new List<int>();
        }

        public List<int> ActiveRules
        {
            get { return activeRules; }
            set { activeRules = value; }
        }

        public List<int> ExecutedRules
        {
            get { return executedRules; }
            set { executedRules = value; }
        }

        public int[][] GetConfirmRuleSets()
        {
            return GetConfirmRuleSets2().Select(rules => rules.Select(rule => rule.Result).ToArray()).ToArray();
        }

        public Rule[][] GetConfirmRuleSets2()
        {
            var relatedToBasisActiveRulesLists = new List<Rule>[2];

            foreach (var basicRule in BasicOutputRules)
            {
                relatedToBasisActiveRulesLists[basicRule - FirstBasisOutputRule] = GetAllUpperRules(basicRule);
            }

            return relatedToBasisActiveRulesLists.Select(list => list.ToArray()).ToArray();
        }

        private List<Rule> GetAllUpperRules(int basicRule)
        {
            var upperRules = new List<Rule>();

            if (BasicOutputRules.Contains(basicRule))
            {
                if (ActiveRules.Contains(basicRule))
                {
                    //upperRules.Add(new Rule(){Name = 0, Cause = 0, Result = basicRule, Weight = 0});
                }
                else
                {
                    //return new List<Rule>();
                }
            }

            var higherActiveRules = _rules.Where(rule => rule.Result == basicRule && ActiveRules.Contains(basicRule) && executedRules.Contains(rule.Name));
            foreach (var higherActiveRule in higherActiveRules)
            {
                upperRules.Add(higherActiveRule);
                upperRules.AddRange(GetAllUpperRules(higherActiveRule.Name));
            }

            return upperRules;
        }

        public void NextLogicStep()
        {
            var newActiveRules = new List<int>();
            executedRules = new List<int>();
            foreach (var activeRule in ActiveRules)
            {
                var allUpperRules = _rules.Where(rule => rule.Cause == activeRule).ToList();
                foreach (var upperRule in allUpperRules)
                {
                    newActiveRules.Add(upperRule.Result);    
                    executedRules.Add(upperRule.Name);
                }
            }

            newActiveRules.Sort();
            executedRules.Sort();
            activeRules = newActiveRules;
        }

        public void ApplyTruthRule(int truthRule)
        {
            var newExecutedRules = new List<int>();
            var confirmRuleSets = GetConfirmRuleSets();

            foreach (var confirmedConsequence in confirmRuleSets[truthRule-FirstBasisOutputRule])
            {
                var confirmedRules = _rules.Where(rule => rule.Result == confirmedConsequence && executedRules.Contains(rule.Name)).Select(rule => rule.Name).ToList();
                newExecutedRules.AddRange(confirmedRules);
            }

            newExecutedRules.Sort();
            executedRules = newExecutedRules;
        }

        public void InitNextGeneration()
        {
            activeRules = executedRules;
            executedRules = new List<int>();
        }

        public void AddSensorInfo(int i)
        {
            ActiveRules.Add(i);
        }

        public double[] CalcEffectResults()
        {
            return GetConfirmRuleSets2().Select(rules => rules.Sum(rule => rule.Weight)).ToArray();
        }
    }

    public class Rule
    {
        public Rule()
        {
            Weight = 1;
        }

        public int Name;
        public int Cause;
        public int Result;
        public double Weight;
    }

    public class ReasonerTests
    {
        [Fact]
        public void CalcConsequences_SholdProcessRule()
        {
            var rules = new Rule[]
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                };

            var reasoner = new Reasoner(rules);
            reasoner.ActiveRules = new int[] { 1 }.ToList();

            reasoner.NextLogicStep();

            Assert.Equal(new int[] { 3 }, reasoner.ActiveRules);
            Assert.Equal(new int[] { 5 }, reasoner.ExecutedRules);
        }

        [Fact]
        public void CalcConsequences_ShouldProcessFewRules()
        {
            var rules = new Rule[]
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);

            reasoner.ActiveRules = new int[] {1, 7}.ToList();
            reasoner.NextLogicStep();

            Assert.Equal(new int[]{3, 4, 6}, reasoner.ActiveRules);
            Assert.Equal(new int[] { 5, 6, 8 }, reasoner.ExecutedRules);
        }

        [Fact]
        public void GetBasisRules_ShouldGoToBasis()
        {
            var rules = new Rule[]
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);
            reasoner.ActiveRules = new[] { 3, 6 }.ToList();
            reasoner.ExecutedRules = new[] { 5, 8 }.ToList();

            var basisConsequences = reasoner.GetConfirmRuleSets();

            Assert.Equal(1, basisConsequences[0].Count());
            Assert.Equal(0, basisConsequences[1].Count());
        }

        [Fact]
        public void GetBasisRules_ShouldGoToBasis2()
        {
            var rules = new Rule[]
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);
            reasoner.ActiveRules = new[] {3, 6, 4}.ToList();
            reasoner.ExecutedRules = new[] {5, 8, 6}.ToList();

            var basisConsequences = reasoner.GetConfirmRuleSets();

            Assert.Equal(1, basisConsequences[0].Count());
            Assert.Equal(2, basisConsequences[1].Count());
        }

        [Fact]
        public void GetBasisRules_ShouldGiveInfoAboutActiveRelatedToBasisRules()
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
            reasoner.ActiveRules = new[] { 5, 3, 6, 4 }.ToList();
            reasoner.ExecutedRules = new[] { 9, 5, 8, 6 }.ToList();

            var relatedToBasisActiveRules = reasoner.GetConfirmRuleSets();

            Assert.Equal(new int[] { 3, 5 }, relatedToBasisActiveRules[0]);
            Assert.Equal(new int[] { 4, 6 }, relatedToBasisActiveRules[1]);
        }

        [Fact]
        public void ApplyTruthBasis_ShouldLeftOnlyNodesThatLeadsToTruth()
        {
            var rules = new Rule[]
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);
            reasoner.ActiveRules = new[] { 7, 5, 3, 6, 4 }.ToList();
            reasoner.ExecutedRules = new[] {5, 6, 8 }.ToList();

            reasoner.ApplyTruthRule(4);

            Assert.Equal(new[] { 7, 5, 3, 6, 4 }, reasoner.ActiveRules);
            Assert.Equal(new[] { 6, 8 }, reasoner.ExecutedRules);
        }

        [Fact]
        public void ApplyTruthBasis_ShouldLeftOnlyNodesThatLeadsToTruth2()
        {
            var rules = new Rule[]
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);
            reasoner.ActiveRules = new[] { 7, 5, 3, 6, 4 }.ToList();
            reasoner.ExecutedRules = new[] { 5, 6, 8 }.ToList();

            reasoner.InitNextGeneration();

            Assert.Equal(new[] { 5, 6, 8 }, reasoner.ActiveRules);
            Assert.Equal(new int[] { }, reasoner.ExecutedRules);
        }

        [Fact]
        public void ApplyTruthBasis_ShouldRecognize10Sequence()
        {
            var rules = new Rule[]
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);
            
            reasoner.AddSensorInfo(2);
            reasoner.NextLogicStep();
            reasoner.ApplyTruthRule(3);
            reasoner.InitNextGeneration(); 

            reasoner.ActiveRules.Add(1);
            reasoner.NextLogicStep();
            var confirmRuleSets = reasoner.GetConfirmRuleSets();

            Assert.Equal(new int[] { 3 }, confirmRuleSets[0]);
            Assert.Equal(new int[] { 4, 6}, confirmRuleSets[1]);
        }

        [Fact]
        public void ApplyTruthBasis_ShouldRecognize0Sequence()
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
            var rules = new Rule[]
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3, Weight = 0}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4, Weight = 0}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3, Weight = 0}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6, Weight = 1},
                    new Rule(){Name = 9, Cause = 5, Result = 5, Weight = 1},
                };

            var reasoner = new Reasoner(rules);

            reasoner.ActiveRules = new[] { 3, 4, 5, 6 }.ToList();
            reasoner.ExecutedRules = new[] { 5, 6, 8, 9 }.ToList();

            var confirmRuleSets = reasoner.GetConfirmRuleSets2();

            Assert.Equal(5, confirmRuleSets[0][0].Name);
            Assert.Equal(9, confirmRuleSets[0][1].Name);
            Assert.Equal(6, confirmRuleSets[1][0].Name);
        }

        [Fact]
        public void GetConfirmRuleSets2_ShouldCalcEffectResults()
        {
            var rules = new Rule[]
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3, Weight = 0.2}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4, Weight = 0.2}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3, Weight = 0.2}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6, Weight = 1},
                    new Rule(){Name = 9, Cause = 5, Result = 5, Weight = 1},
                };

            var reasoner = new Reasoner(rules);

            reasoner.ActiveRules = new[] { 3, 4, 5, 6 }.ToList();
            reasoner.ExecutedRules = new[] { 5, 6, 7, 8, 9 }.ToList();

            double[] effectResults = reasoner.CalcEffectResults();

            Assert.Equal(1.4, effectResults[0]);
            Assert.Equal(1.2, effectResults[1]);
        }
    }
}
