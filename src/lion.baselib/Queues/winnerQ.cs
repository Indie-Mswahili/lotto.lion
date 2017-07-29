using OdinSdk.BaseLib.Queue;

namespace LottoLion.BaseLib.Queues
{
    /// <summary>
    /// 
    /// </summary>
    public class WinnerQ : WorkerQ
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queue_name"></param>
        public WinnerQ(string queue_name = "lottolion_winnerQ")
            : base(p_queueName: queue_name)
        {
        }
    }
}