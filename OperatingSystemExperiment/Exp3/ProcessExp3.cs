namespace OperatingSystemExperiment.Exp3 {
    public class ProcessExp3 {
        public int Id;

        /// <summary>
        /// 进程最大（各类）资源需求数：信用额度
        /// </summary>
        public int[] Max;

        /// <summary>
        /// 已分配给进程的资源：贷款
        /// </summary>
        public int[] Allocation;

        /// <summary>
        /// 进程还需要的资源：信用额度 - 贷款
        /// </summary>
        public int[] Need;

        public ProcessExp3(int id) => this.Id = id;


        public void EvaluateNeedResource()
        {
            Need = new int[Max.Length];
            for (var i = 0; i < Max.Length; i++) {
                Need[i] = Max[i] - Allocation[i];
            }
        }
    }
}