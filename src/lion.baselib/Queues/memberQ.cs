using OdinSdk.BaseLib.Queue;

namespace LottoLion.BaseLib.Queues
{
    /// <summary>
    /// 
    /// </summary>
    public class MemberQ : WorkerQ
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queue_name"></param>
        public MemberQ(string queue_name = "lottolion_memberQ")
            : base(p_queueName: queue_name)
        {
        }
    }
}