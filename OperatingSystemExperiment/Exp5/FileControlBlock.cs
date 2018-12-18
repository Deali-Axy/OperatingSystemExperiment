namespace OperatingSystemExperiment.Exp5
{
    /// <summary>
    /// 文件控制块
    /// </summary>
    public class FileControlBlock
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName;

        /// <summary>
        /// 文件大小
        /// </summary>
        public int Size;

        /// <summary>
        /// 直接索引地址 存放文件数据的物理块号
        /// </summary>
        public int[] IAddr = new int[Main.DirectIndexNum];

        /// <summary>
        /// 一级索引地址 存放一级索引块的物理块号
        /// </summary>
        public int SingleIndirect = -1;

        /// <summary>
        /// 二级索引地址 
        /// </summary>
        public int DoubleIndirect = -1;

        /// <summary>
        /// 下一个文件控制块
        /// </summary>
        public FileControlBlock next;

        public FileControlBlock(string fileName, int size)
        {
            this.FileName = fileName;
            this.Size = size;
        }
    }
}