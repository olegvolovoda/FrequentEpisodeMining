using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteApproach
{
    public class Evaluator
    {
        private RulesRepo _rulesRepo;

        public Evaluator(RulesRepo rulesRepo)
        {
            _rulesRepo = rulesRepo;
        }

        public bool[] CalcEffectResults()
        {
            var confirmingRules = _rulesRepo.GetConfirmRuleSets2().Select(rules => rules.Sum(rule => rule.Weight)).ToArray();
            var result = new bool[confirmingRules.Count()];

            var threshold = 0;
            int indexMax = !confirmingRules.Any() ? -1 :
                confirmingRules
                .Select((value, index) => new { Value = value, Index = index })
                .Aggregate((a, b) => (a.Value > b.Value) ? a : b)
                .Index;

            if (confirmingRules.Count(rulesCount => confirmingRules[indexMax] <= rulesCount + threshold) == 1)
            {
                result[indexMax] = true;
            }

            return result;
        }
    }
}
