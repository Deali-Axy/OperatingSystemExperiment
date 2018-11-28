using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OperatingSystemExperiment.Exp1 {
    class CentralProcessUnit {
        private List<ProcessControlBlock> PCBList = new List<ProcessControlBlock>();
        private Queue<ProcessControlBlock> FinishQueue = new Queue<ProcessControlBlock>();
        private Queue<ProcessControlBlock> ReadyQueue = new Queue<ProcessControlBlock>();

        public CentralProcessUnit()
        {
            LoadPcbList();
        } 

        /// <summary>
        /// 生成进程列表
        /// </summary>
        /// <param name="count">进程数量</param>
        public static void GenerateProcessList(int count)
        {
            var processListFile = Path.Combine(Environment.CurrentDirectory, "process_list.txt");
            var writer = new StreamWriter(processListFile);
            var rnd = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < count; i++) {
                var runTime = rnd.Next(1, 10);
                writer.WriteLine("{0} {1} {2} {3}", i, Math.Pow(2, i), runTime, rnd.Next(0, 4));
            }

            writer.Close();
        }

        /// <summary>
        /// 加载PCB列表
        /// </summary>
        private void LoadPcbList()
        {
            var processListFile = Path.Combine(Environment.CurrentDirectory, "process_list.txt");
            var reader = new StreamReader(processListFile);
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                var procInfo = line.Split(' ');
                PCBList.Add(new ProcessControlBlock {
                    ID = int.Parse(procInfo[0]),
                    ArriveTime = int.Parse(procInfo[1]),
                    Time = int.Parse(procInfo[2]),
                    Priority = int.Parse(procInfo[3])
                });
            }
        }

        /// <summary>
        /// CPU运行
        /// </summary>
        public void Run()
        {
            var times = 0;
            while (true) {
                // 如果所有进程运行完，则退出循环
                if (FinishQueue.Count == PCBList.Count) {
                    break;
                }

                // 遍历所有进程列表
                foreach (var p in PCBList) {
                    // 根据进程到达时间判定是否有新进程加入，然后将进程状态设置为就绪
                    if (p.ArriveTime == times++) {
                        Console.WriteLine("时间：{0},进程 {1} 到达", times, p.ID);
                        p.Status = ProcessStatus.Ready;
                    }

                    // 讲就绪状态进程加入就绪列表
                    if (p.Status == ProcessStatus.Ready) {
//                        Console.WriteLine("时间：{0}，进程 {1} 加入就绪列表", times, p.ID);
                        ReadyQueue.Enqueue(p);
                    }

                    // 如果就绪队列为空则进入下一次循环
                    if (ReadyQueue.Count == 0) {
//                        Console.WriteLine("时间：{0}，没有就绪进程，进入下一个循环", times);
                        continue;
                    }

                    // 从就绪队列中取出一个进程运行
                    var currentProcess = ReadyQueue.Dequeue();
                    Console.WriteLine("时间：{0}，运行进程 {1}", times, p.ID);
                    currentProcess.Run();

                    // 将运行完毕进程加入完成列表
                    if (currentProcess.Status == ProcessStatus.Finish) {
                        Console.WriteLine("时间：{0}，进程 {1} 运行完毕，总运行时间：{2}", times, p.ID, p.RunTime);
                        FinishQueue.Enqueue(currentProcess);
                    }
                    else
                        currentProcess.Status = ProcessStatus.Ready;
                }
            }
        }
    }
}