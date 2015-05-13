using System;

namespace DiscreteApproach
{
    public class RuleInfo
    {
        public RuleInfo()
        {
            Successes = 1;
            Total = 1;
        }

        public int Index;
        public int Cause;
        public int Result;
        public int Successes;
        public int Total;
        private int _expand;

        public double Probability
        {
            get
            {
                return (double)Successes / Total;
            }
        }

        public void AdmitSuccess()
        {
            Total++;
            Successes++;
        }

        public void AdmitFailure()
        {
            Total++;
        }

        public void RequestExpand()
        {
            _expand++;
        }

        public bool IsNeedExpand()
        {
            return _expand > 0;
        }

        public void MarkExpanded()
        {
            _expand = 0;
        }

        public int Height { get; set; }

        public int BasicOutput{ get; private set; }
    }
}