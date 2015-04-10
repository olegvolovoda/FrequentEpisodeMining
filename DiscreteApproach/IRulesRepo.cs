using System.Collections.Generic;

namespace DiscreteApproach
{
    public interface IRulesRepo
    {
        List<int> ActiveRules { get; set; }
        List<int> PostActiveRules { get; set; }
        List<int> ExecutedRules { get; set; }
        int[] BasicOutputRules { get; set; }
        int FirstBasisOutputRule { get; set; }
        IEnumerable<RuleInfo> GetRuleByResult(int basicRule);
        IEnumerable<RuleInfo> GetRuleByCause(int activeRule);
        void AddRule(RuleInfo newRule);
        RuleInfo GetRuleByName(int ruleName);
        bool IsRuleIsDuplicateEdge(int inputRule, int confirmedOutput);
        int GetRuleHeight(int rule);
        List<RuleInfo> GetAllExecutedRulesBasedOnResult(int basicRule);
        int[][] GetConfirmRuleSets();
        RuleInfo[][] GetConfirmRuleSets2();
        string GetSequence(int rule);
        string[] GetAllSequences();
    }
}