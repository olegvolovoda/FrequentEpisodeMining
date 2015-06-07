using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscreteApproach
{
    public class RulesData
    {
        private List<RuleInfo> _ruleInfos = new List<RuleInfo>();

        private readonly IndexDictionary<int, RuleInfo> _byResultIndex = new IndexDictionary<int, RuleInfo>();
        private readonly IndexDictionary<int, RuleInfo> _byCauseIndex = new IndexDictionary<int, RuleInfo>();
        private readonly Dictionary<int, RuleInfo> _byIndexIndex = new Dictionary<int, RuleInfo>();
        private readonly Dictionary<Tuple<int, int>, RuleInfo> _byCauseAndResultIndex = new Dictionary<Tuple<int, int>, RuleInfo>();

        private int _maxIndex = 0;

        public void AddRule(RuleInfo newRule)
        {
            newRule.Index = ++_maxIndex;
            _ruleInfos.Add(newRule);

            _byResultIndex.Add(newRule.Result, newRule);
            _byCauseIndex.Add(newRule.Cause, newRule);
            _byIndexIndex.Add(newRule.Index, newRule);
            if (newRule.Cause > 0)
            {
                _byCauseAndResultIndex.Add(new Tuple<int, int>(newRule.Cause, newRule.Result), newRule);
            }
        }

        public IEnumerable<RuleInfo> GetRulesByResult(int resultRule)
        {
            return _byResultIndex.GetValue(resultRule);
        }

        public IEnumerable<RuleInfo> GetRuleByCause(int causeRule)
        {
            return _byCauseIndex.GetValue(causeRule);
        }

        public RuleInfo GetRuleByIndex(int ruleIndex)
        {
            return _byIndexIndex.ContainsKey(ruleIndex) ? _byIndexIndex[ruleIndex] : null;
        }

        public bool IsAnyRulesByCauseAndResult(int inputRule, int outputRule)
        {
            return _byCauseAndResultIndex.ContainsKey(new Tuple<int, int>(inputRule, outputRule));
        }

        public List<RuleInfo> AllRuleInfos()
        {
            return _ruleInfos;
        }

        public void GetRulesByHeightAndMaxSuccesses(int height, int maxSuccesses)
        {
            _ruleInfos.RemoveAll(rule => rule.Height == height && rule.Successes <= maxSuccesses);
            var t = _ruleInfos.Count(rule => rule.Height == height);
        }
    }
}