using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscreteApproach
{
    public class Learner
    {
        private IRulesRepo _rulesRepo;
        private IEvaluator _evaluator;

        public Learner(IRulesRepo rulesRepo, IEvaluator evaluator)
        {
            _rulesRepo = rulesRepo;
            _evaluator = evaluator;
        }

        public void AjustByOutput(int truthOutput, bool learn = true)
        {
            var newExecutedRules = new List<int>();
            var confirmRuleSets = _rulesRepo.GetConfirmRuleSets2();

            var effectResult = _evaluator.CalcEffectResults();

            int prefferedOutput = 0;
            if (effectResult[0] != effectResult[1])
            {
                prefferedOutput = effectResult[0] ? 3 : 4;
            }

            if (learn)
            {
                AlignWeightsAndCreateRules(truthOutput, confirmRuleSets, prefferedOutput);
            }

            newExecutedRules.AddRange(confirmRuleSets[truthOutput - _rulesRepo.FirstBasisOutputRule].Select(rule => rule.Index));
            newExecutedRules.Sort();
            _rulesRepo.ExecutedRules = newExecutedRules;
        }

        public void AlignWeightsAndCreateRules(int truthOutput, RuleInfo[][] confirmedRuleSets, int prefferedOutput)
        {

            if (prefferedOutput == truthOutput || prefferedOutput == 0)
            {
                var succededRules = confirmedRuleSets[truthOutput - _rulesRepo.FirstBasisOutputRule];
                var highestRuleIndex = succededRules.IndexMax(rule => rule.Height);
                if (highestRuleIndex != -1)
                {
                    succededRules[highestRuleIndex].AdmitSuccess();
                }

                //foreach (var confirmedRule in succededRules)
                //{
                //    var r = _rulesRepo.GetRuleByName(confirmedRule.Index);
                //    if (r != null)
                //    {
                //        r.AdmitSuccess();
                //    }
                //}
            }
            else
            {
                foreach (var confirmedRule in confirmedRuleSets[prefferedOutput - _rulesRepo.FirstBasisOutputRule])
                {
                    var r = _rulesRepo.GetRuleByName(confirmedRule.Index);
                    if (r != null)
                    {
                        r.AdmitFailure();
                    }
                }
            }

            if (prefferedOutput == 0 || prefferedOutput != truthOutput)
            {
                foreach (var confirmedOutput in confirmedRuleSets[truthOutput - _rulesRepo.FirstBasisOutputRule].Select(rule => rule.Index).Union(new int[] { truthOutput }))
                {
                    foreach (var inputRule in _rulesRepo.ActiveRules)
                    {
                        if (_rulesRepo.IsRuleIsDuplicateEdge(inputRule, confirmedOutput))
                        {
                            if ((inputRule < 3 || _rulesRepo.GetRuleByName(inputRule).Weight > 0) 
                                && (confirmedOutput < 5 || _rulesRepo.GetRuleByName(confirmedOutput).Weight > 0) 
                                && _rulesRepo.GetRuleHeight(inputRule) == _rulesRepo.GetRuleHeight(confirmedOutput))
                            {
                                var newRule = new RuleInfo
                                {
                                    Cause = inputRule,
                                    Result = confirmedOutput,
                                    Total = 0,
                                    Successes = 0
                                };

                                _rulesRepo.AddRule(newRule);
                                newRule.Height = _rulesRepo.GetRuleHeight(newRule.Index);
                            }
                        }
                    }
                }
            }
        }


    }
}
