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

        public bool[] CalcReliableOutput(double reliableRate)
        {
            return CalcProbabilities().Select(p => p >= reliableRate).ToArray();
        }

        public double[] CalcProbabilities()
        {
            var probabilities = _rulesRepo.GetConfirmRuleSets2().Select(rules => rules.Any() ? rules.Max(rule => rule.Total > 2 ? rule.Probability : 0) : 0).ToArray();

            return probabilities;
        }
    }
}
