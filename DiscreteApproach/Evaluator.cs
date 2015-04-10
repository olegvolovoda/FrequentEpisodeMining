using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteApproach
{
    public class Evaluator : IEvaluator
    {
        private IRulesRepo _rulesRepo;

        public Evaluator(IRulesRepo rulesRepo)
        {
            _rulesRepo = rulesRepo;
        }

        public bool[] CalcEffectResults()
        {
            var confirmingRules = _rulesRepo.GetConfirmRuleSets2().Select(rules => rules.Sum(rule => rule.Weight)).ToArray();
            var result = new bool[confirmingRules.Count()];

            var threshold = 0.2;
            var indexMax = confirmingRules.IndexMax(count => count);

            if (confirmingRules.Count(rulesCount => confirmingRules[indexMax] <= rulesCount + threshold) == 1)
            {
                result[indexMax] = true;
            }

            return result;
        }
    }
}
