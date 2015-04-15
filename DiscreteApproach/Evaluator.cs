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
            var confirmingRulesWeights = _rulesRepo.GetConfirmRuleSets2().Select(rules => rules.Sum(rule => rule.Weight)).ToArray();
            var result = new bool[confirmingRulesWeights.Count()];

            var threshold = 0.2;
            var indexMax = confirmingRulesWeights.IndexMax(count => count);

            if (confirmingRulesWeights.Count(rulesCount => confirmingRulesWeights[indexMax] <= rulesCount + threshold) == 1)
            {
                result[indexMax] = true;
            }

            return result;
        }
    }
}
