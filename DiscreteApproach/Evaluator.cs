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

        public bool[] CalcReliableOutput1(double reliableRate)
        {
            return CalcProbabilities().Select(p => p >= reliableRate).ToArray();
        }

        public double[] CalcProbabilities()
        {
            var probabilities =
                _rulesRepo.GetConfirmRuleSets2()
                          .Select(
                              rules =>
                              rules.Any()
                                  ? rules.Max(
                                      rule =>
                                        rule.Total > 3 ? Math.Pow(rule.Probability, 1 + ((double) rule.Height - 1)/4) : 0)
                                  : 0)
                          .ToArray();

            return probabilities;
        }
    }
}
