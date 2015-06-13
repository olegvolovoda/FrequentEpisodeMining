using System.Collections.Generic;

namespace DiscreteApproach
{
    public interface IRulesRepo
    {
        List<int> ActiveRules { get; set; }
        int[] OutputRules { get; }
        List<int> PostActiveRules { get; set; }
        ICollection<int> ExecutedRules { get; set; }
        int InputRulesCount { get; set; }
        int OutputRulesCount { get; set; }
        //int[] OutputRules { get; set; }
        int FirstInputRule { get; }
        int FirstOutputRule { get; }
        IEnumerable<RuleInfo> GetRulesByResult(int basicRule);
        IEnumerable<RuleInfo> GetRulesByCause(int activeRule);
        void AddRule(RuleInfo newRule);
        RuleInfo GetRuleByIndex(int ruleName);
        bool IsRuleNotDuplicatesEdge(int inputRule, int confirmedOutput);
        int GetRuleHeight(int rule);
        List<RuleInfo> GetAllExecutedRulesBasedOnResult(int basicRule);
        int[][] GetConfirmRuleSets();
        RuleInfo[][] GetConfirmRuleSets2();
        IList<int> GetSequence(int rule);
        SequenceInfo[] GetAllSequences();
        void RemoveUnsufficientRules(int height);
    }
}