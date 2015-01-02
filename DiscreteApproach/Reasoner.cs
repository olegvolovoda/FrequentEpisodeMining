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
        private List<Rule> _rules;

        private List<int> inputRules;
        private List<int> outputRules = new List<int>();
        private List<int> executedRules;
        private int[] BasicOutputRules = new[] { 3, 4 };
        private static int FirstBasisOutputRule = 3;

        public Reasoner(List<Rule> rules)
        {
            inputRules = new List<int>();
            this._rules = rules;
            ExecutedRules = new List<int>();
        }

        public List<int> InputRules
        {
            get { return inputRules; }
            set { inputRules = value; }
        }

        public List<int> ExecutedRules
        {
            get { return executedRules; }
            set { executedRules = value; }
        }

        public List<int> OutputRules
        {
            get { return outputRules; }
            set { outputRules = value; }
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

            var higherActiveRules = _rules.Where(rule => rule.Result == basicRule && OutputRules.Contains(basicRule) && executedRules.Contains(rule.Name));
            foreach (var higherActiveRule in higherActiveRules)
            {
                upperRules.Add(higherActiveRule);
                upperRules.AddRange(GetAllUpperRules(higherActiveRule.Name));
            }

            return upperRules;
        }

        public void NextLogicStep()
        {
            var outputRules = new List<int>();
            executedRules = new List<int>();
            foreach (var activeRule in InputRules)
            {
                var allUpperRules = _rules.Where(rule => rule.Cause == activeRule).ToList();
                foreach (var upperRule in allUpperRules)
                {
                    outputRules.Add(upperRule.Result);    
                    executedRules.Add(upperRule.Name);
                }
            }

            outputRules.Sort();
            executedRules.Sort();
            this.outputRules = outputRules;
        }

        public void ApplyTruthRule(int truthOutput)
        {
            var newExecutedRules = new List<int>();
            var confirmRuleSets = GetConfirmRuleSets2();

            var effectResult = this.CalcEffectResults();

            int prefferedOutput = 0;
            if (effectResult[0] != effectResult[1])
            {
                prefferedOutput = effectResult[0] > effectResult[1] ? 3 : 4;
            }
            AlignWeights(truthOutput, confirmRuleSets, prefferedOutput);

            newExecutedRules.AddRange(confirmRuleSets[truthOutput - FirstBasisOutputRule].Select(rule => rule.Name));
            newExecutedRules.Sort();
            executedRules = newExecutedRules;

            Console.Out.WriteLine(_rules.Count);
        }

        public void AlignWeights(int truthOutput, Rule[][] confirmedRuleSets, int prefferedOutput)
        {

            if (prefferedOutput == truthOutput || prefferedOutput == 0)
            {
                foreach (var confirmedRule in confirmedRuleSets[truthOutput - FirstBasisOutputRule])
                {
                    var r = _rules.FirstOrDefault(rule => rule.Name == confirmedRule.Name);
                    if (r != null)
                    {
                        r.Weight += 0.2;
                        r.Weight = Math.Min(1, r.Weight);
                    }
                }
            }
            else
            {
                foreach (var confirmedRule in confirmedRuleSets[prefferedOutput - FirstBasisOutputRule])
                {
                    var r = _rules.FirstOrDefault(rule => rule.Name == confirmedRule.Name);
                    if (r != null)
                    {
                        r.Weight -= 0.2;
                        r.Weight = Math.Max(0, r.Weight);
                    }
                }
            }

            if (prefferedOutput == 0 || prefferedOutput != truthOutput)
            {
                foreach (var confirmedOutput in confirmedRuleSets[truthOutput - FirstBasisOutputRule].Select(rule => rule.Name).Union(new int[]{truthOutput}))
                {
                    foreach (var inputRule in inputRules)
                    {
                        if (RuleIsNotDuplicateEdge(inputRule, confirmedOutput))
                        {
                            //if (RulesAreOnTheSameHeight(inputRule, confirmedOutput))
                            {
                                _rules.Add(new Rule
                                    {
                                        Cause = inputRule,
                                        Name = _rules.Any() ? _rules.Max(rule => rule.Name) + 1 : 5,
                                        Result = confirmedOutput,
                                        Weight = 0.2
                                    });
                            }
                        }
                    }
                }
            }
        }

        private bool RuleIsNotDuplicateEdge(int inputRule, int confirmedOutput)
        {
            return !_rules.Any(rule => rule.Cause == inputRule && rule.Result == confirmedOutput)
                   && !_rules.Any(rule => rule.Cause == inputRule && rule.Name == confirmedOutput)
                   && !_rules.Any(rule => rule.Name == inputRule && rule.Result == confirmedOutput);
        }

        public void InitNextGeneration()
        {
            inputRules = executedRules;
            executedRules = new List<int>();
        }

        public void AddSensorInfo(int i)
        {
            InputRules.Add(i);
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
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
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
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);

            reasoner.InputRules = new int[] {1, 7}.ToList();
            reasoner.NextLogicStep();

            Assert.Equal(new int[] { 3, 4, 6 }, reasoner.OutputRules);
            Assert.Equal(new int[] { 5, 6, 8 }, reasoner.ExecutedRules);
        }

        [Fact]
        public void GetBasisRules_ShouldGoToBasis()
        {
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
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
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);
            reasoner.OutputRules = new[] { 3, 6, 4 }.ToList();
            reasoner.ExecutedRules = new[] {5, 8, 6}.ToList();

            var basisConsequences = reasoner.GetConfirmRuleSets();

            Assert.Equal(1, basisConsequences[0].Count());
            Assert.Equal(2, basisConsequences[1].Count());
        }

        [Fact]
        public void GetBasisRules_ShouldGiveInfoAboutActiveRelatedToBasisRules()
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
            reasoner.OutputRules = new[] { 5, 3, 6, 4 }.ToList();
            reasoner.ExecutedRules = new[] { 9, 5, 8, 6 }.ToList();

            var relatedToBasisActiveRules = reasoner.GetConfirmRuleSets();

            Assert.Equal(new int[] { 3, 5 }, relatedToBasisActiveRules[0]);
            Assert.Equal(new int[] { 4, 6 }, relatedToBasisActiveRules[1]);
        }

        [Fact]
        public void ApplyTruthBasis_ShouldLeftOnlyNodesThatLeadsToTruth()
        {
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
                };

            var reasoner = new Reasoner(rules);
            reasoner.OutputRules = new[] { 7, 5, 3, 6, 4 }.ToList();
            reasoner.ExecutedRules = new[] {5, 6, 8 }.ToList();

            reasoner.ApplyTruthRule(4);

            Assert.Equal(new[] { 7, 5, 3, 6, 4 }, reasoner.OutputRules);
            Assert.Equal(new[] { 6, 8 }, reasoner.ExecutedRules);
        }

        [Fact]
        public void ApplyTruthBasis_ShouldLeftOnlyNodesThatLeadsToTruth2()
        {
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6},
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
            var rules = new List<Rule>
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

            reasoner.InputRules.Add(1);
            reasoner.NextLogicStep();
            var confirmRuleSets = reasoner.GetConfirmRuleSets();

            Assert.Equal(new int[] { 3 }, confirmRuleSets[0]);
            Assert.Equal(new int[] { 4, 6}, confirmRuleSets[1]);
        }

        [Fact]
        public void ApplyTruthBasis_ShouldRecognize0Sequence()
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
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3, Weight = 0}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4, Weight = 0}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3, Weight = 0}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6, Weight = 1},
                    new Rule(){Name = 9, Cause = 5, Result = 5, Weight = 1},
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
            var rules = new List<Rule>
                {
                    new Rule(){Name = 5, Cause = 1, Result = 3, Weight = 0.2}, 
                    new Rule(){Name = 6, Cause = 1, Result = 4, Weight = 0.2}, 
                    new Rule(){Name = 7, Cause = 2, Result = 3, Weight = 0.2}, 
                    new Rule(){Name = 8, Cause = 7, Result = 6, Weight = 1},
                    new Rule(){Name = 9, Cause = 5, Result = 5, Weight = 1},
                };

            var reasoner = new Reasoner(rules);

            reasoner.OutputRules = new[] { 3, 4, 5, 6 }.ToList();
            reasoner.ExecutedRules = new[] { 5, 6, 7, 8, 9 }.ToList();

            double[] effectResults = reasoner.CalcEffectResults();

            Assert.Equal(1.4, effectResults[0]);
            Assert.Equal(1.2, effectResults[1]);
        }
    }
}
