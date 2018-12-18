using System;

namespace OperatingSystemExperiment.Exp4 {
    /// <summary>
    /// 内存块
    /// </summary>
    public class MemoryBlock {
        public readonly int PageIndex;
        /// <summary>
        /// 上次访问时间
        /// </summary>
        public int LastVisit;
        /// <summary>
        /// 进入时间
        /// </summary>
        public int EnterTime;

        public MemoryBlock(int pageIndex)
        {
            this.PageIndex = pageIndex;
        }

        public static MemoryBlock Parse(string str) =>
            int.TryParse(str, out var temp) ? new MemoryBlock(temp) : null;

        public override string ToString()
        {
            return Convert.ToString(PageIndex);
        }
    }
}