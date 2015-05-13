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

        public void AjustByOutput(int[] truthOutputs, bool learn = true)
        {
            var confirmRuleSets = _rulesRepo.GetConfirmRuleSets2();

            if (learn)
            {
                AlignWeightsAndCreateRules(truthOutputs.ToArray(), confirmRuleSets);
            }

            var confirmedExecutedRules = new List<int>();
            foreach (var truthOutput1 in truthOutputs)
            {
                confirmedExecutedRules.AddRange(confirmRuleSets[truthOutput1 - _rulesRepo.FirstOutputRule].Select(rule => rule.Index));    
            }
            
            confirmedExecutedRules.Sort();
            _rulesRepo.ExecutedRules = confirmedExecutedRules;
        }

        public void AlignWeightsAndCreateRules(int[] truthOutputs, RuleInfo[][] confirmedRuleSets)
        {
            
            foreach (var outputRule in _rulesRepo.OutputRules)
            {
                var executedRules = confirmedRuleSets[outputRule - _rulesRepo.FirstOutputRule];
                if (truthOutputs.Contains(outputRule))
                {
                    executedRules.ToList().ForEach(rule => rule.AdmitSuccess());
                }
                else
                {
                    executedRules.ToList().ForEach(rule => rule.AdmitFailure());
                }
            }

            var reliableOutput = _evaluator.CalcReliableOutput(0.85);
            foreach (var outputRule in _rulesRepo.OutputRules)
            {
                if (truthOutputs.Contains(outputRule) && !reliableOutput[outputRule - _rulesRepo.FirstOutputRule])
                {
                    bool anyRuleCreated = false;

                    foreach (
                        var confirmedOutput in
                            confirmedRuleSets[outputRule - _rulesRepo.FirstOutputRule].Select(rule => rule.Index)
                                                                                        .Union(new int[] {outputRule})
                        )
                    {
                        foreach (var inputRule in _rulesRepo.ActiveRules)
                        {
                            if (
                                /*(
                                    _rulesRepo.GetRuleByIndex(confirmedOutput).Height == 1 || 
                                 (_rulesRepo.GetRuleByIndex(confirmedOutput).Total > 2 && _rulesRepo.GetRuleByIndex(inputRule).Total > 2)
                                 ) && */
                                CreateRule(inputRule, confirmedOutput))
                            //if (CreateRule(inputRule, confirmedOutput))
                            {
                                anyRuleCreated = true;
                            }
                        }
                    }

                    if (!anyRuleCreated)
                    {
                        _rulesRepo.ActiveRules.Select(rule => _rulesRepo.GetRuleByIndex(rule))
                                    .MaxItems(rule => rule.Height)
                                    .ToList()
                                    .ForEach(rule => rule.RequestExpand());
                    }
                }
            }

            foreach (var outputRule in _rulesRepo.OutputRules)
            {
                if (truthOutputs.Contains(outputRule) && reliableOutput[outputRule - _rulesRepo.FirstOutputRule])
                {
                    foreach (
                        var outputNeedsExpand in
                            confirmedRuleSets[outputRule - _rulesRepo.FirstOutputRule].Where(
                                rule => rule.IsNeedExpand()).Select(rule => rule.Index))
                    {
                        bool ruleCreated = false;

                        foreach (var inputRule in _rulesRepo.ActiveRules)
                        {
                            if (CreateRule(inputRule, outputNeedsExpand))
                            {
                                ruleCreated = true;
                            }
                        }

                        if (ruleCreated)
                        {
                            _rulesRepo.GetRuleByIndex(outputNeedsExpand).MarkExpanded();
                        }
                        else
                        {
                            _rulesRepo.ActiveRules.Select(rule => _rulesRepo.GetRuleByIndex(rule))
                                      .MaxItems(rule => rule.Height)
                                      .ToList()
                                      .ForEach(rule => rule.RequestExpand());
                        }
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
