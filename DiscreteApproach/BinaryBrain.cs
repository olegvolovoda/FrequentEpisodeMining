using System;

namespace DiscreteApproach
{
    public class BinaryBrain
    {
        private Reasoner reasoner;

        public BinaryBrain(Reasoner reasoner)
        {
            this.reasoner = reasoner;
        }

        public string Perceive(string info)
        {
            reasoner.ApplyTruthRule(info == "0" ? 3 : 4);
            reasoner.InitNextGeneration();
            reasoner.AddSensorInfo(info == "0" ? 1 : 2);
            reasoner.NextLogicStep();
            var effectResults = reasoner.CalcEffectResults();

            return effectResults[0] ? "D" : effectResults[1] ? "U" : "_";
        }

        public string PerceiveChain(string chain)
        {
            string result = "";

            foreach (var item in chain)
            {
                result += Perceive(item.ToString());
            }

            return result;
        }
    }

 }
