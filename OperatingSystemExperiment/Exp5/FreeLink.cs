namespace OperatingSystemExperiment.Exp5
{
    /// <summary>
    /// 空闲盘区 一组连续的空闲物理块组成一个盘区
    /// </summary>
    public class FreeLink
    {
        /// <summary>
        /// 空闲盘区的起始物理块号
        /// </summary>
        public int Start;
        /// <summary>
        /// 空闲盘区的物理块个数
        /// </summary>
        public int Num;

        /// <summary>
        /// 下一个空闲盘区
        /// </summary>
        public FreeLink next;
    }
}