using System.Collections.Generic;
using System.Linq;

namespace DiscreteApproach
{
    public class RulesRepo
    {
        private List<RuleInfo> _ruleInfos;

        public RulesRepo(List<RuleInfo> ruleInfos)
        {
            _ruleInfos = ruleInfos;
        }

        public IEnumerable<RuleInfo> GetRuleByResult(int basicRule)
        {
            return _ruleInfos.Where(rule => rule.Result == basicRule);
        }

        public IEnumerable<RuleInfo> GetRuleByCause(int activeRule)
        {
            return _ruleInfos.Where(rule => rule.Cause == activeRule);
        }

        public void AddRule(RuleInfo newRule)
        {
            newRule.Name = _ruleInfos.Any() ? _ruleInfos.Max(rule => rule.Name) + 1 : 5;
            _ruleInfos.Add(newRule);
        }

        public RuleInfo GetRuleByName(int ruleName)
        {
            return _ruleInfos.FirstOrDefault(rule => rule.Name == ruleName);
        }

        public bool IsRuleIsDuplicateEdge(int inputRule, int confirmedOutput)
        {
            return !_ruleInfos.Any(rule => rule.Cause == inputRule && rule.Result == confirmedOutput)
                   && !_ruleInfos.Any(rule => rule.Cause == inputRule && rule.Name == confirmedOutput)
                   && !_ruleInfos.Any(rule => rule.Name == inputRule && rule.Result == confirmedOutput);
        }

        public int GetRuleHeight(int rule)
        {
            int height = 1;
            while (rule > 4)
            {
                rule = GetRuleByName(rule).Cause;
                height++;
            }

            return height;
        }
    }
}