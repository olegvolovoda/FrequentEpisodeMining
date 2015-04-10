using System.Collections.Generic;
using System.Linq;

namespace DiscreteApproach
{
    public class RulesRepo : IRulesRepo
    {
        private List<int> _activeRules;
        private List<int> _postActiveRules = new List<int>();
        private List<int> executedRules;

        private int[] _outputRules = new[] { 3, 4 };
        private int _firstOutputRule = 3;
        private int _outputRulesCount = 2;

        private int _firstInputRule = 1;
        private int _inputRulesCount = 2;
        
        private List<RuleInfo> _ruleInfos;

        public List<int> ActiveRules
        {
            get { return _activeRules; }
            set { _activeRules = value; }
        }

        public List<int> PostActiveRules
        {
            get { return _postActiveRules; }
            set { _postActiveRules = value; }
        }

        public List<int> ExecutedRules
        {
            get { return executedRules; }
            set { executedRules = value; }
        }

        public int[] OutputRules
        {
            get { return _outputRules; }
            set { _outputRules = value; }
        }

        public int FirstOutputRule
        {
            get { return _firstOutputRule; }
            set { _firstOutputRule = value; }
        }

        public int OutputRulesCount
        {
            get { return _outputRulesCount; }
            set { _outputRulesCount = value; }
        }

        public int FirstInputRule
        {
            get { return _firstInputRule; }
            set { _firstInputRule = value; }
        }

        public int InputRulesCount
        {
            get { return _inputRulesCount; }
            set { _inputRulesCount = value; }
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

        public bool IsRuleIsDuplicateEdge(int inputRule, int outputRule)
        {
            return !_ruleInfos.Any(rule => rule.Cause == inputRule && rule.Result == outputRule)
                   && !_ruleInfos.Any(rule => rule.Cause == inputRule && rule.Index == outputRule)
                   && !_ruleInfos.Any(rule => rule.Index == inputRule && rule.Result == outputRule);
        }

        public int GetRuleHeight(int ruleIndex)
        {
            var rule = GetRuleByName(ruleIndex);
            if (rule != null)
            {
                if (rule.Height > 0)
                {
                    return rule.Height;
                }
            }

            int height = 1;
            while (ruleIndex > 4)
            {
                ruleIndex = GetRuleByName(ruleIndex).Cause;
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

            foreach (var basicRule in _outputRules)
            {
                relatedToBasisActiveRulesLists[basicRule - _firstOutputRule] = GetAllExecutedRulesBasedOnResult(basicRule);
            }

            return relatedToBasisActiveRulesLists.Select(list => list.ToArray()).ToArray();
        }


        public string GetSequence(int rule)
        {
            if (GetRuleByName(rule).Height > 2)
            {
                return GetSequence(GetRuleByName(rule).Cause) + GetLastOutputCause(rule);
            }
            else
            {
                return (GetRuleByName(rule).Cause - 1).ToString() + (GetRuleByName(rule).Result - 3).ToString();
            }
        }

        public string[] GetAllSequences()
        {
            List<string> sequences = new List<string>();

            foreach (var ruleInfo in _ruleInfos)
            {
                if (ruleInfo.Weight > 0)
                {
                    sequences.Add(GetSequence(ruleInfo.Index));
                }
            }

            return sequences.ToArray();
        }

        private int GetLastOutputCause(int rule)
        {
            if (GetRuleByName(rule).Height > 2)
            {
                return GetLastOutputCause(GetRuleByName(rule).Result);
            }
            else
            {
                return GetRuleByName(rule).Result - 3;
            }
        }
    }
}