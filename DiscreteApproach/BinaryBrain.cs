using System;

namespace DiscreteApproach
{
    public class BinaryBrain
    {
        private Reasoner reasoner;
        public bool SelfFeedbackMode = false;

        public BinaryBrain(Reasoner reasoner)
        {
            this.reasoner = reasoner;
        }

        public string Perceive(string info, bool learn = true)
        {
            reasoner.ApplyTruthRule(info == "0" ? 3 : 4, learn);    
            reasoner.InitNextGeneration();
            reasoner.AddSensorInfo(info == "0" ? 1 : 2);
            reasoner.NextLogicStep();
            var effectResults = reasoner.CalcEffectResults();

            return effectResults[0] ? "D" : effectResults[1] ? "U" : "_";
        }

        public string PredictNext()
        {
            var effectResults1 = reasoner.CalcEffectResults();
            if (effectResults1[0] || effectResults1[1])
            {
                reasoner.ApplyTruthRule(effectResults1[0] ? 3 : 4, false);
            }
            reasoner.InitNextGeneration();

            if (effectResults1[0] || effectResults1[1])
            {
                reasoner.AddSensorInfo(effectResults1[0] ? 1 : 2);
            }
            reasoner.NextLogicStep();

            var effectResults = reasoner.CalcEffectResults();

            return effectResults1[0] ? "0" : effectResults1[1] ? "1" : "_";
        }

        public string PredictNext(int times)
        {
            string s = "";
            for (int i = 0; i < times; i++)
            {
                s = s + PredictNext();
            }

            return s;
        }

        public string PerceiveChain(string chain, bool learn = true)
        {
            string result = "";

            foreach (var item in chain)
            {
                result += Perceive(item.ToString(), learn);
            }

            return result;
        }
    }

 }
