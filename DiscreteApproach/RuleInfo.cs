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

        public double Weight
        {
            get { 
                var rate = (double)Successes/Total;
                var threshold = 0.7;
                if (rate > threshold)
                {
                    return (rate - threshold) * (1 / (1 - threshold));
                }
                else
                {
                    return 0;
                }
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
    }
}