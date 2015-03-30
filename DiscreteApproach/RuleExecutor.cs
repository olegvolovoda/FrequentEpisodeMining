using System.Collections.Generic;
using System.Linq;

namespace DiscreteApproach
{
    public class RuleExecutor
    {
        private RulesRepo _rulesRepo;

        public RuleExecutor(RulesRepo rulesRepo)
        {
            _rulesRepo = rulesRepo;
        }

        public void Run()
        {
            var outputRules = new List<int>();
            _rulesRepo.ExecutedRules = new List<int>();
            foreach (var activeRule in _rulesRepo.InputRules)
            {
                var allUpperRules = _rulesRepo.GetRuleByCause(activeRule).ToList();
                foreach (var upperRule in allUpperRules)
                {
                    outputRules.Add(upperRule.Result);
                    _rulesRepo.ExecutedRules.Add(upperRule.Index);
                }
            }

            outputRules.Sort();
            _rulesRepo.ExecutedRules.Sort();
            _rulesRepo.OutputRules = outputRules;
        }
    }
}