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
                    var highestRuleIndex = succededRules.Where(rule => rule.Weight > 0).IndexMax(rule => rule.Height);
                    if (highestRuleIndex != -1)
                    {
                        var maxHeight = succededRules[highestRuleIndex].Height;
                        succededRules.Where(rule => rule.Height <= maxHeight).ToList().ForEach(rule => rule.AdmitSuccess());
                        //succededRules[highestRuleIndex].AdmitSuccess();
                    }
                    else
                    {
                        succededRules.ToList().ForEach(rule => rule.AdmitSuccess());    
                    }
                }

                var failedOutputs = _rulesRepo.OutputRules.Except(new []{truthOutput}).ToArray();
                foreach (var failedOutput in failedOutputs)
                {
                    var failedRules = confirmedRuleSets[failedOutput - _rulesRepo.FirstOutputRule];
                    failedRules.ToList().ForEach(rule => rule.AdmitFailure());

                    //var highestRuleIndex = succededRules.IndexMax(rule => rule.Height);
                    //if (highestRuleIndex != -1)
                    //{
                    //    var maxHeight = succededRules[highestRuleIndex].Height;
                    //    failedRules.Where(rule => rule.Height == maxHeight).ToList().ForEach(rule => rule.AdmitFailure());
                    //    //succededRules[highestRuleIndex].AdmitSuccess();
                    //}
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
                bool anyRuleCreated = false;

                foreach (var confirmedOutput in confirmedRuleSets[truthOutput - _rulesRepo.FirstOutputRule].Select(rule => rule.Index).Union(new int[] { truthOutput }))                    
                {
                    foreach (var inputRule in _rulesRepo.ActiveRules)
                    {
                        if (CreateRule(inputRule, confirmedOutput))
                        {
                            anyRuleCreated = true;
                        }
                    }
                }

                if (!anyRuleCreated)
                {
                    _rulesRepo.ActiveRules.Select(rule => _rulesRepo.GetRuleByIndex(rule)).MaxItems(rule => rule.Height).ToList().ForEach(rule => rule.RequestExpand());
                }
            }

            if (prefferedOutput == truthOutput)
            {
                foreach (var confirmedOutput in confirmedRuleSets[truthOutput - _rulesRepo.FirstOutputRule].Where(rule => rule.IsNeedExpand()).Select(rule => rule.Index))
                {
                    bool ruleCreated = false;

                    foreach (var inputRule in _rulesRepo.ActiveRules)
                    {
                        if (CreateRule(inputRule, confirmedOutput))
                        {
                            ruleCreated = true;
                        }
                    }

                    if (ruleCreated)
                    {
                        _rulesRepo.GetRuleByIndex(confirmedOutput).MarkExpanded();
                    }
                    else
                    {
                        _rulesRepo.ActiveRules.Select(rule => _rulesRepo.GetRuleByIndex(rule)).MaxItems(rule => rule.Height).ToList().ForEach(rule => rule.RequestExpand());
                    }
                }
            }
        }

        private bool CreateRule(int inputRule, int outputRule)
        {
            if (_rulesRepo.IsRuleIsDuplicateEdge(inputRule, outputRule))
            {
                if (
                    //((_rulesRepo.GetRuleByIndex(inputRule).Weight > 0 && _rulesRepo.GetRuleByIndex(confirmedOutput).Weight > 0) || _rulesRepo.GetRuleHeight(inputRule) < 0)
                    //&&
                    _rulesRepo.GetRuleHeight(inputRule) == _rulesRepo.GetRuleHeight(outputRule))
                {
                    var newRule = new RuleInfo
                        {
                            Cause = inputRule,
                            Result = outputRule,
                            Total = 0,
                            Successes = 0
                        };

                    _rulesRepo.AddRule(newRule);
                    newRule.Height = _rulesRepo.GetRuleHeight(newRule.Index);
                    return true;
                }
            }

            return false;
        }
    }
}
