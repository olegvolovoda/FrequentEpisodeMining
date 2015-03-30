using System.Collections.Generic;
using System.Linq;

namespace DiscreteApproach
{
    public class RulesRepo
    {
        private List<int> inputRules;
        private List<int> outputRules = new List<int>();
        private List<int> executedRules;

        public int[] BasicOutputRules = new[] { 3, 4 };
        public int FirstBasisOutputRule = 3;
        
        private List<RuleInfo> _ruleInfos;

        public List<int> InputRules
        {
            get { return inputRules; }
            set { inputRules = value; }
        }

        public List<int> OutputRules
        {
            get { return outputRules; }
            set { outputRules = value; }
        }

        public List<int> ExecutedRules
        {
            get { return executedRules; }
            set { executedRules = value; }
        }

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
            newRule.Index = _ruleInfos.Any() ? _ruleInfos.Max(rule => rule.Index) + 1 : 5;
            _ruleInfos.Add(newRule);
        }

        public RuleInfo GetRuleByName(int ruleName)
        {
            return _ruleInfos.FirstOrDefault(rule => rule.Index == ruleName);
        }

        public bool IsRuleIsDuplicateEdge(int inputRule, int confirmedOutput)
        {
            return !_ruleInfos.Any(rule => rule.Cause == inputRule && rule.Result == confirmedOutput)
                   && !_ruleInfos.Any(rule => rule.Cause == inputRule && rule.Index == confirmedOutput)
                   && !_ruleInfos.Any(rule => rule.Index == inputRule && rule.Result == confirmedOutput);
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

        public List<RuleInfo> GetAllExecutedRulesBasedOnResult(int basicRule)
        {
            var upperRules = new List<RuleInfo>();

            var higherActiveRules = GetRuleByResult(basicRule).Where(rule => ExecutedRules.Contains(rule.Index));
            foreach (var higherActiveRule in higherActiveRules)
            {
                upperRules.Add(higherActiveRule);
                upperRules.AddRange(GetAllExecutedRulesBasedOnResult(higherActiveRule.Index));
            }

            return upperRules;
        }

        public int[][] GetConfirmRuleSets()
        {
            return GetConfirmRuleSets2().Select(rules => rules.Select(rule => rule.Result).ToArray()).ToArray();
        }

        public RuleInfo[][] GetConfirmRuleSets2()
        {
            var relatedToBasisActiveRulesLists = new List<RuleInfo>[2];

            foreach (var basicRule in BasicOutputRules)
            {
                relatedToBasisActiveRulesLists[basicRule - FirstBasisOutputRule] = GetAllExecutedRulesBasedOnResult(basicRule);
            }

            return relatedToBasisActiveRulesLists.Select(list => list.ToArray()).ToArray();
        }

    }
}