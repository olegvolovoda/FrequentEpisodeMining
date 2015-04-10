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

            if (effectResult.Count(result => result) == 1)
            {
                prefferedOutput = effectResult.IndexMax(result => result ? 1 : 0) + _rulesRepo.FirstOutputRule;
            }

            if (learn)
            {
                AlignWeightsAndCreateRules(truthOutput, confirmRuleSets, prefferedOutput);
            }

            newExecutedRules.AddRange(confirmRuleSets[truthOutput - _rulesRepo.FirstOutputRule].Select(rule => rule.Index));
            newExecutedRules.Sort();
            _rulesRepo.ExecutedRules = newExecutedRules;
        }

        public void AlignWeightsAndCreateRules(int truthOutput, RuleInfo[][] confirmedRuleSets, int prefferedOutput)
        {

            if (prefferedOutput == truthOutput || prefferedOutput == 0)
            {
                var succededRules = confirmedRuleSets[truthOutput - _rulesRepo.FirstOutputRule];
                {
                    var highestRuleIndex = succededRules.IndexMax(rule => rule.Height);
                    if (highestRuleIndex != -1)
                    {
                        var maxHeight = succededRules[highestRuleIndex].Height;
                        succededRules.Where(rule => rule.Height == maxHeight).ToList().ForEach(rule => rule.AdmitSuccess());
                        //succededRules[highestRuleIndex].AdmitSuccess();
                    }
                }

                var failedOutputs = _rulesRepo.OutputRules.Except(new []{truthOutput}).ToArray();
                foreach (var failedOutput in failedOutputs)
                {
                    var failedRules = confirmedRuleSets[failedOutput - _rulesRepo.FirstOutputRule];

                    var highestRuleIndex = succededRules.IndexMax(rule => rule.Height);
                    if (highestRuleIndex != -1)
                    {
                        var maxHeight = succededRules[highestRuleIndex].Height;
                        failedRules.Where(rule => rule.Height == maxHeight).ToList().ForEach(rule => rule.AdmitFailure());
                        //succededRules[highestRuleIndex].AdmitSuccess();
                    }
                }
            }
            else
            {
                foreach (var confirmedRule in confirmedRuleSets[prefferedOutput - _rulesRepo.FirstOutputRule])
                {
                    var r = _rulesRepo.GetRuleByIndex(confirmedRule.Index);
                    if (r != null)
                    {
                        r.AdmitFailure();
                    }
                }
            }

            if (prefferedOutput == 0 || prefferedOutput != truthOutput)
            {
                foreach (var confirmedOutput in confirmedRuleSets[truthOutput - _rulesRepo.FirstOutputRule].Select(rule => rule.Index).Union(new int[] { truthOutput }))
                {
                    foreach (var inputRule in _rulesRepo.ActiveRules)
                    {
                        if (_rulesRepo.IsRuleIsDuplicateEdge(inputRule, confirmedOutput))
                        {
                            if (
                                //((_rulesRepo.GetRuleByIndex(inputRule).Weight > 0 && _rulesRepo.GetRuleByIndex(confirmedOutput).Weight > 0) || _rulesRepo.GetRuleHeight(inputRule) < 0)
                                //&&
                                _rulesRepo.GetRuleHeight(inputRule) == _rulesRepo.GetRuleHeight(confirmedOutput))
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
