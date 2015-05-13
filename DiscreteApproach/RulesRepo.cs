using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscreteApproach
{
    public class RulesRepo : IRulesRepo
    {
        private List<int> _activeRules;
        private List<int> _postActiveRules = new List<int>();
        private List<int> executedRules;

        //private int[] _outputRules = new[] { 3, 4 };
        //private int _firstOutputRule = 3;

        private int _firstInputRule = 1;
        private int _inputRulesCount = 2;

        private int _outputRulesCount = 2;

        private readonly RulesData _rulesData;


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

        //public int[] OutputRules
        //{
        //    get { return _outputRules; }
        //    set { _outputRules = value; }
        //}

        public int FirstInputRule
        {
            get { return _firstInputRule; }
        }

        public int FirstOutputRule
        {
            get { return _firstInputRule + InputRulesCount; }
        }

        public int OutputRulesCount
        {
            get { return _outputRulesCount; }
            set { _outputRulesCount = value; }
        }

        public int InputRulesCount
        {
            get { return _inputRulesCount; }
            set { _inputRulesCount = value; }
        }

        public RulesRepo(List<RuleInfo> ruleInfos, int inputRulesCount, int outputRulesCount)
        {
            _rulesData = new RulesData();
            for (int i = 0; i < inputRulesCount + outputRulesCount; i++)
            {
                var rule = new RuleInfo() { Successes = 1, Total = 1, Height = 1 };
                AddRule(rule);
            }
            ruleInfos.ForEach(rule => AddRule(rule));
            this._inputRulesCount = inputRulesCount;
            this._outputRulesCount = outputRulesCount;
        }

        public IEnumerable<RuleInfo> GetRuleByResult(int basicRule)
        {
            return _rulesData.GetRulesByResult(basicRule);
        }

        public IEnumerable<RuleInfo> GetRuleByCause(int activeRule)
        {
            return _rulesData.GetRuleByCause(activeRule);
        }

        public void AddRule(RuleInfo newRule)
        {
            _rulesData.AddRule(newRule);
        }

        public RuleInfo GetRuleByIndex(int ruleIndex)
        {
            return _rulesData.GetRuleByIndex(ruleIndex);
        }

        public bool IsRuleNotDuplicatesEdge(int inputRule, int outputRule)
        {
            return !_rulesData.IsAnyRulesByCauseAndResult(inputRule, outputRule);
        }

        public int GetRuleHeight(int ruleIndex)
        {
            var rule = GetRuleByIndex(ruleIndex);
            if (rule != null)
            {
                if (rule.Height > 0)
                {
                    return rule.Height;
                }
            }

            int height = 1;
            while (!IsBasicRule(ruleIndex))
            {
                ruleIndex = GetRuleByIndex(ruleIndex).Cause;
                height++;
            }

            return height;
        }

        private bool IsBasicRule(int ruleIndex)
        {
            return ruleIndex <= InputRulesCount + OutputRulesCount;
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


        public int[] OutputRules {
            get { return Enumerable.Range(InputRulesCount + 1, OutputRulesCount).ToArray(); }
        }
    
        public RuleInfo[][] GetConfirmRuleSets2()
        {
            var relatedToBasisActiveRulesLists = new List<RuleInfo>[OutputRulesCount];

            foreach (var basicRule in OutputRules)
            {
                relatedToBasisActiveRulesLists[basicRule - FirstOutputRule] = GetAllExecutedRulesBasedOnResult(basicRule);
            }

            return relatedToBasisActiveRulesLists.Select(list => list.ToArray()).ToArray();
        }


        public List<int> GetSequence(int rule)
        {
            List<int> sequence;

            if (GetRuleByIndex(rule).Height > 2)
            {
                sequence = GetSequence(GetRuleByIndex(rule).Cause);
                sequence.Add(GetLastOutputCause(rule));
                return sequence;
            }
            else if (GetRuleByIndex(rule).Height == 2)
            {
                sequence = new List<int>();
                sequence.Add(GetRuleByIndex(rule).Cause);
                sequence.Add(GetRuleByIndex(rule).Result - InputRulesCount);
                return sequence;
            }
            else
            {
                return new List<int>();
            }
        }

        public SequenceInfo[] GetAllSequences()
        {
            List<SequenceInfo> sequences = new List<SequenceInfo>();

            foreach (var ruleInfo in _rulesData.AllRuleInfos())
            {
                //if (ruleInfo.Probability > 0.7 && ruleInfo.Height >= 2)
                if (ruleInfo.Height >= 2)
                {
                    sequences.Add(new SequenceInfo() { Sequence = GetSequence(ruleInfo.Index).ToArray() , Rule = ruleInfo});
                }
            }

            return sequences.OrderBy(item => item.Sequence.Sum()).ToArray();
        }

        private int GetLastOutputCause(int rule)
        {
            if (GetRuleByIndex(rule).Height > 2)
            {
                return GetLastOutputCause(GetRuleByIndex(rule).Result);
            }
            else
            {
                return GetRuleByIndex(rule).Result - InputRulesCount;
            }
        }
    }

    public class SequenceInfo
    {
        public int[] Sequence { get; set; }

        public RuleInfo Rule { get; set; }
    }
}