﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteApproach
{
    public class SymbolsBrain
    {
        private Reasoner reasoner;

        public SymbolsBrain()
        {
            this.reasoner = new Reasoner(new List<RuleInfo>(), 26, 26);
        }

        public string Perceive(char info)
        {
            return Perceive(new char[]{info});
        }

        public string Perceive(char[] infos)
        {
            reasoner.ApplyTruthRule(infos.Select(info => info - 96 + 26).ToArray());
            reasoner.InitNextGeneration();
            foreach (var info in infos)
            {
                reasoner.AddSensorInfo((int) info - 96);
            }
            reasoner.NextLogicStep();
            var effectResults = reasoner.CalcProbabilities();
            var dic = new Dictionary<int, double>();
            for (int i = 0; i < effectResults.Count(); i++)
            {
                if (effectResults[i] > 0.7)
                {
                    dic.Add(i, effectResults[i]);
                }
            }
            dic = dic.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            string s = "[";
            int shift = 0;
            foreach (var ch in infos)
            {
                s += ch;
                shift++;
                if (shift == 1)
                    s += "   ";
            }
            s += "]\n";

            s += "[";
            foreach (var item in dic)
            {
                s += (char)(item.Key + 97);
            }
            s += "]      ";

            
            return s;

            //if (effectResults.Count(result => result) == 1)
            //{
            //    return ((char)(effectResults.IndexMax(result => result ? 1 : 0) + 97)).ToString();
            //}
            //else
            //{
            //    return "_";
            //}
        }

        public string PerceiveChain(string chain, bool learn = true)
        {
            string result = "";

            foreach (var item in chain)
            {
                if (Char.IsLower(item))
                {
                    result += Perceive(item);
                }
            }

            return result;
        }

        public string PerceiveChain1(char[][] chain, bool learn = true)
        {
            string result = "";

            foreach (var chars in chain)
            {
                result += Perceive(chars);
            }

            return result;
        }

        public List<string> GetAllSequences()
        {
            var strings = new List<string>();

            var sequences = reasoner.GetAllSequences().OrderByDescending(sequence => sequence.Rule.Successes);
            foreach (var sequence in sequences)
            {
                var s = new string(sequence.Sequence.Select(item => (char)(item + 96)).ToArray()) + " \t" + sequence.Rule.Probability + " \t" + sequence.Rule.Index + " " + sequence.Rule.Successes + "\\" + sequence.Rule.Total; 
                strings.Add(s);
            }

            return strings;
        }
    }
}
