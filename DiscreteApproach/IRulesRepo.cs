using System.Collections.Generic;

namespace DiscreteApproach
{
    public interface IRulesRepo
    {
        List<int> ActiveRules { get; set; }
        int[] OutputRules { get; }
        List<int> PostActiveRules { get; set; }
        List<int> ExecutedRules { get; set; }
        int InputRulesCount { get; set; }
        int OutputRulesCount { get; set; }
        //int[] OutputRules { get; set; }
        int FirstInputRule { get; }
        int FirstOutputRule { get; }
        IEnumerable<RuleInfo> GetRuleByResult(int basicRule);
        IEnumerable<RuleInfo> GetRuleByCause(int activeRule);
        void AddRule(RuleInfo newRule);
        RuleInfo GetRuleByIndex(int ruleName);
        bool IsRuleNotDuplicatesEdge(int inputRule, int confirmedOutput);
        int GetRuleHeight(int rule);
        List<RuleInfo> GetAllExecutedRulesBasedOnResult(int basicRule);
        int[][] GetConfirmRuleSets();
        RuleInfo[][] GetConfirmRuleSets2();
        List<int> GetSequence(int rule);
        SequenceInfo[] GetAllSequences();
    }
}