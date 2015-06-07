namespace DiscreteApproach
{
    public interface IEvaluator
    {
        double[] CalcProbabilities();
        bool[] CalcReliableOutput(double reliableRate);
        bool[] CalcReliableOutput1(double reliableRate);
    }
}