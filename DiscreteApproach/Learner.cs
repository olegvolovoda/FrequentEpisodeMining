using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscreteApproach
{
    public class Learner
    {
        private RulesRepo _rulesRepo;
        private Evaluator _evaluator;

        public Learner(RulesRepo rulesRepo, Evaluator evaluator)
        {
            _rulesRepo = rulesRepo;
            _evaluator = evaluator;
        }

        public void AjustByOutput(int truthOutput)
        {
            var newExecutedRules = new List<int>();
            var confirmRuleSets = _rulesRepo.GetConfirmRuleSets2();

            var effectResult = _evaluator.CalcEffectResults();

            int prefferedOutput = 0;
            if (effectResult[0] != effectResult[1])
            {
                prefferedOutput = effectResult[0] ? 3 : 4;
            }
            AlignWeightsAndCreateRules(truthOutput, confirmRuleSets, prefferedOutput);

            newExecutedRules.AddRange(confirmRuleSets[truthOutput - _rulesRepo.FirstBasisOutputRule].Select(rule => rule.Index));
            newExecutedRules.Sort();
            _rulesRepo.ExecutedRules = newExecutedRules;
        }

        public void AlignWeightsAndCreateRules(int truthOutput, RuleInfo[][] confirmedRuleSets, int prefferedOutput)
        {

            if (prefferedOutput == truthOutput || prefferedOutput == 0)
            {
                foreach (var confirmedRule in confirmedRuleSets[truthOutput - _rulesRepo.FirstBasisOutputRule])
                {
                    var r = _rulesRepo.GetRuleByName(confirmedRule.Index);
                    if (r != null)
                    {
                        r.Weight += 0.2;
                        r.Weight = Math.Min(1, r.Weight);
                    }
                }
            }
            else
            {
                foreach (var confirmedRule in confirmedRuleSets[prefferedOutput - _rulesRepo.FirstBasisOutputRule])
                {
                    var r = _rulesRepo.GetRuleByName(confirmedRule.Index);
                    if (r != null)
                    {
                        r.Weight -= 0.2;
                        r.Weight = Math.Max(0, r.Weight);
                    }
                }
            }

            if (prefferedOutput == 0 || prefferedOutput != truthOutput)
            {
                foreach (var confirmedOutput in confirmedRuleSets[truthOutput - _rulesRepo.FirstBasisOutputRule].Select(rule => rule.Index).Union(new int[] { truthOutput }))
                {
                    foreach (var inputRule in _rulesRepo.InputRules)
                    {
                        if (_rulesRepo.IsRuleIsDuplicateEdge(inputRule, confirmedOutput))
                        {
                            if (_rulesRepo.GetRuleHeight(inputRule) == _rulesRepo.GetRuleHeight(confirmedOutput))
                            {
                                var newRule = new RuleInfo
                                {
                                    Cause = inputRule,
                                    Result = confirmedOutput,
                                    Weight = 0.2
                                };

                                _rulesRepo.AddRule(newRule);
                            }
                        }
                    }
                }
            }
        }


    }
}
