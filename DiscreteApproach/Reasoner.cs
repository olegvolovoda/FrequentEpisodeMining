using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscreteApproach
{
    public class Reasoner
    {
        public readonly IRulesRepo _rulesRepo;
        private IEvaluator _evaluator;

        public Reasoner(List<RuleInfo> ruleInfos)
        {
            _rulesRepo = new RulesRepo(ruleInfos);
            _rulesRepo.ActiveRules = new List<int>();
            _rulesRepo.ExecutedRules = new List<int>();
            _evaluator = new Evaluator(_rulesRepo);
        }

        public void NextLogicStep()
        {
            new RuleExecutor(_rulesRepo).Run();
        }

        public void ApplyTruthRule(int truthOutput, bool learn = true)
        {
            new Learner(_rulesRepo, _evaluator).AjustByOutput(truthOutput, learn);
        }

        public void InitNextGeneration()
        {
            _rulesRepo.ActiveRules = _rulesRepo.ExecutedRules;
            _rulesRepo.ExecutedRules = new List<int>();
        }

        public void AddSensorInfo(int i)
        {
            _rulesRepo.ActiveRules.Add(i);
        }

        public bool[] CalcEffectResults()
        {
            return _evaluator.CalcEffectResults();
        }
    }
}
