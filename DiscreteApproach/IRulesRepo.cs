using System.Collections.Generic;

namespace DiscreteApproach
{
    public interface IRulesRepo
    {
        List<int> InputRules { get; set; }
        List<int> OutputRules { get; set; }
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
    }
}