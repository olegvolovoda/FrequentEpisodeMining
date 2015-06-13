using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscreteApproach
{
    public class Reasoner
    {
        public readonly IRulesRepo _rulesRepo;
        public int _time = 1;
        private IEvaluator _evaluator;

        public Reasoner() : this(new List<RuleInfo>())
        {
        }

        public Reasoner(List<RuleInfo> ruleInfos) : this(ruleInfos, 2, 2)
        {
        }

        public Reasoner(List<RuleInfo> ruleInfos, int inputRulesCount, int outputRulesCount)
        {
            _rulesRepo = new RulesRepo(ruleInfos, inputRulesCount, outputRulesCount);
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
            ApplyTruthRule(new [] {truthOutput}, learn);
        }

        public void ApplyTruthRule(int[] truthOutputs, bool learn = true)
        {
            new Learner(_rulesRepo, _evaluator).AjustByOutput(truthOutputs, learn);
        }

        public void InitNextGeneration()
        {
            _rulesRepo.ActiveRules = _rulesRepo.ExecutedRules.ToList();
            _rulesRepo.ExecutedRules = new List<int>();

            _time++;

            for (int i = 2; i < 6; i++)
            {
                if (_time % (int)(Math.Pow(_rulesRepo.InputRulesCount, i) / 30) == 0)
                {
                    _rulesRepo.RemoveUnsufficientRules(i);
                }   
            }
        }

        public void AddSensorInfo(int i)
        {
            _rulesRepo.ActiveRules.Add(i);
        }

        public bool[] CalcEffectResults()
        {
            return _evaluator.CalcReliableOutput(0.7);
        }

        public double[] CalcProbabilities()
        {
            return _evaluator.CalcProbabilities();
        }

        public SequenceInfo[] GetAllSequences()
        {
            return _rulesRepo.GetAllSequences();
        }
    }
}
