using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystemExperiment.Exp1 {
    enum ProcessStatus {
        Ready,
        Run,
        Finish
    }

    /// <summary>
    /// 进程控制块 PCB
    /// </summary>
    class ProcessControlBlock {
        /// <summary>
        /// 进程号
        /// </summary>
        public int ID;

        /// <summary>
        /// 进程状态
        /// </summary>
        public ProcessStatus Status;

        /// <summary>
        /// 进程到达时间
        /// </summary>
        public int ArriveTime;

        /// <summary>
        /// 估计运行时间
        /// </summary>
        public int Time;

        /// <summary>
        /// 已运行时间
        /// </summary>
        public int RunTime = 0;

        /// <summary>
        /// 等待时间
        /// </summary>
        public int WaitTime;

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority;

        /// <summary>
        /// 链接指针
        /// </summary>
        public ProcessControlBlock Next;

        /// <summary>
        /// 开始时间
        /// </summary>
        public int StartTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        public int FinishTime;

        public void Run()
        {
            this.Status = ProcessStatus.Run;

            if (RunTime >= Time) {
                this.Status = ProcessStatus.Finish;
                return;
            }

            this.RunTime++;
        }

        public void Wait()
        {
            this.WaitTime++;
        }

        public override string ToString() => String.Format("{0} {1} {2}", ID, StartTime, FinishTime);
    }
}