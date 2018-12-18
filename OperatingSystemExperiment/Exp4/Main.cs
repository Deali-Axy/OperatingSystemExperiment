using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace OperatingSystemExperiment.Exp4
{
    public class Main
    {
        private readonly int[] _sequence; // 页面访问序列
        private readonly List<MemoryBlock> _blocks; // 内存中的块
        private readonly int _blocksCount;

        public Main()
        {
            Console.WriteLine("请输入分配给进程的物理块数:");
            _blocksCount = int.Parse(Console.ReadLine());
            // 初始化内存物理块
            _blocks = new List<MemoryBlock>(_blocksCount);

            // 从文件读取访问序列
            using (var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, "Exp4", "input", "sequence.txt")))
            {
                var line = sr.ReadLine();
                _sequence = Array.ConvertAll(line?.Split("  "), int.Parse);
                Console.WriteLine("读入的页面流：{0}", string.Join(" ", _sequence));
                Console.Write("请选择页面置换算法 1-FIFO 2-LRU:");
                var option = Console.ReadLine();
                Console.WriteLine();

                switch (option)
                {
                    case "1":
                        FIFO();
                        break;
                    case "2":
                        LRU();
                        break;
                    default:
                        Console.WriteLine("输入错误！");
                        break;
                }
            }
        }

        private void FIFO()
        {
            var pageFault = 0;
            foreach (var pageIndex in _sequence)
            {
                var oldest = GetOldestIndex();
                var blockIndex = getBlockIndex(pageIndex);

                if (blockIndex == -1)
                {
                    if (_blocks.Count < _blocksCount)
                    {
                        // 内存还没装满
                        _blocks.Add(new MemoryBlock(pageIndex));
                        pageFault++;
                    }
                    else
                    {
                        // 内存已经装满了
                        _blocks[oldest] = new MemoryBlock(pageIndex);
                        _blocks[oldest].EnterTime = 0;
                        pageFault++;
                    }
                }

                // 刷新物理块调用时间
                for (var i = 0; i < _blocks.Count; i++)
                {
                    if (i != blockIndex)
                        _blocks[i].EnterTime++;
                }

                // 再次计算最久未使用页面
                oldest = GetOldestIndex();

                // 输出当前内存物理块状态
                var blockState = "";
                for (var i = 0; i < _blocks.Count; i++)
                {
                    if (i == oldest && _blocks.Count == _blocksCount)
                        blockState += _blocks[i].PageIndex + "* ";
                    else
                        blockState += _blocks[i].PageIndex + "  ";
                }

                Console.WriteLine("{0}: {1}", pageIndex, blockState);
            }

            Console.WriteLine($"总缺页次数：{pageFault}");
        }

        private void LRU()
        {
            var pageFault = 0;
            foreach (var pageIndex in _sequence)
            {
                var oldest = GetLeastUsedIndex();
                var blockIndex = getBlockIndex(pageIndex);

                if (blockIndex > -1)
                {
                    // 页面已经调入内存，上次访问时间设置0
                    _blocks[blockIndex].LastVisit = 0;
                    pageFault++;
                }
                else
                {
                    if (_blocks.Count < _blocksCount)
                    {
                        // 内存还没装满
                        _blocks.Add(new MemoryBlock(pageIndex));
                        pageFault++;
                    }
                    else
                    {
                        // 内存已经装满了
                        _blocks[oldest] = new MemoryBlock(pageIndex);
                        _blocks[oldest].LastVisit = 0;
                        pageFault++;
                    }
                }

                // 刷新物理块调用时间
                for (var i = 0; i < _blocks.Count; i++)
                {
                    if (i != blockIndex)
                        _blocks[i].LastVisit++;
                }

                // 再次计算最久未使用页面
                oldest = GetLeastUsedIndex();

                // 输出当前内存物理块状态
                var blockState = "";
                for (var i = 0; i < _blocks.Count; i++)
                {
                    if (i == oldest && _blocks.Count == _blocksCount)
                        blockState += _blocks[i].PageIndex + "* ";
                    else
                        blockState += _blocks[i].PageIndex + "  ";
                }

                Console.WriteLine("{0}: {1}", pageIndex, blockState);
            }

            Console.WriteLine($"总缺页次数：{pageFault}");
        }

        /// <summary>
        /// 获取最久未访问页面的index
        /// </summary>
        /// <returns></returns>
        private int GetLeastUsedIndex()
        {
            var max = 0;
            var index = -1;
            for (var i = 0; i < _blocks.Count; i++)
            {
                if (_blocks[i].LastVisit > max)
                {
                    max = _blocks[i].LastVisit;
                    index = i;
                }
            }

            return index;
        }

        private int GetOldestIndex()
        {
            var max = 0;
            var index = -1;
            for (var i = 0; i < _blocks.Count; i++)
            {
                if (_blocks[i].EnterTime > max)
                {
                    max = _blocks[i].EnterTime;
                    index = i;
                }
            }

            return index;
        }

        /// <summary>
        /// 获取指定页面在物理块中的编号，找不到则返回-1
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int getBlockIndex(int index)
        {
            foreach (var item in _blocks)
            {
                if (item.PageIndex == index)
                {
                    return _blocks.IndexOf(item);
                }
            }

            return -1;
        }

        public static void Run()
        {
            new Main();
        }
    }
}