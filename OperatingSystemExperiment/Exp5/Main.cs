using System;

namespace OperatingSystemExperiment.Exp5
{
    public class Main
    {
        /// <summary>
        /// 磁盘物理块个数
        /// </summary>
        public const int BlockNum = 8000;

        /// <summary>
        /// 直接地址索引个数
        /// </summary>
        public const int DirectIndexNum = 10;

        /// <summary>
        /// 索引块的索引项个数
        /// </summary>
        public const int IndexBlockNum = 100;

        private int[] Blocks = new int[BlockNum];
        private IndexBlock[] Indexes = new IndexBlock[BlockNum];


        public Main()
        {
            string[] command;
            var blocks = new int[BlockNum];
            while (true)
            {
                Console.WriteLine("请输入命令,命令格式");
                Console.WriteLine("创建文件：create [文件名] [文件大小(kb)]");
                Console.WriteLine("删除文件：delete [文件名]");
                Console.WriteLine("退出系统：exit");
                var input = Console.ReadLine();
                command = input.Split(" ");
                switch (command[0])
                {
                    case "create":
                        if (command.Length < 3)
                        {
                            Console.WriteLine("参数不够！");
                            continue;
                        }

                        var filename = command[1];
                        var filesize = int.Parse(command[2]);
                        var fcb = new FileControlBlock(filename, filesize);

                        if (filesize <= 10)
                        {
                            for (var i = 0; i < 10; i++)
                            {
                                fcb.IAddr[i] = GetNextFreeBlock();
                            }
                        }
                        else if (filesize > 10 && filesize <= 100)
                        {
                            fcb.SingleIndirect = GetNextFreeIndex();
                            for (var si = 0; si < filesize - 10; si++)
                            {
                                var index = new IndexBlock();
                                index.Addr[si] = GetNextFreeBlock();
                                Indexes[si] = index;
                            }
                        }
                        else
                        {
                            fcb.DoubleIndirect = GetNextFreeIndex();
                            for (var di = 0; di < filesize - 10; di++)
                            {
                                var index = new IndexBlock();
                                index.Addr[di] = GetNextFreeBlock();
                                Indexes[di] = index;
                            }
                        }

                        Console.WriteLine($"文件创建成功 文件名：{filename} 文件大小：{filesize}k");
                        for (var i = 0; i < 10; i++)
                        {
                            Console.WriteLine($"iaddr[{i}]={fcb.IAddr[i]}");
                        }

                        Console.WriteLine($"SingleIndirect={fcb.SingleIndirect}");
                        Console.WriteLine($"DoubleIndirect={fcb.DoubleIndirect}");

                        break;
                    case "delete":
                        Console.WriteLine("删除文件");
                        if (command.Length < 3)
                        {
                            Console.WriteLine("参数不够！");
                            continue;
                        }

                        filename = command[1];
                        break;
                    case "exit":
                        return;
                    default:
                        Console.WriteLine("命令错误！");
                        break;
                }

                Console.WriteLine("----------------------------------------");
            }
        }

        /// <summary>
        /// 获取下一个空闲块，没有则返回-1
        /// </summary>
        /// <returns></returns>
        private int GetNextFreeBlock()
        {
            for (var i = 0; i < Blocks.Length; i++)
            {
                if (Blocks[i] == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        private int GetNextFreeIndex()
        {
            for (var i = 0; i < Indexes.Length; i++)
            {
                if (Indexes[i] == null)
                    return i;
            }

            return -1;
        }

        public static void Run()
        {
            new Main();
        }
    }
}