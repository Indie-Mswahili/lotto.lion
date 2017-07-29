using OdinSdk.BaseLib.Queue;

namespace LottoLion.BaseLib.Queues
{
    /// <summary>
    /// 
    /// </summary>
    public class AnalysisQ : WorkerQ
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queue_name"></param>
        public AnalysisQ(string queue_name = "lottolion_analysisQ")
            : base(p_queueName: queue_name)
        {
        }
    }
}