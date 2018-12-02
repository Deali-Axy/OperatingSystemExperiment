using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace OperatingSystemExperiment.Exp3 {
    public class Main {
        private int _resourceClassesCount = 0;
        private int _processCount = 0;

        private List<ProcessExp3> _processes = new List<ProcessExp3>();

        /// <summary>
        /// 系统全部可分配资源：银行流动资金
        /// </summary>
        private List<int> _resource = new List<int>();

        /// <summary>
        /// 系统剩余可分配资源
        /// </summary>
        private List<int> _available = new List<int>();

        private Main()
        {
            // 获取当前系统资源分配状态
            LoadAllAvailableResource();
            LoadProcessMaxResource();

            var continueFlag = true;
            while (continueFlag) {
                mainLoop:
                // 获取各进程已分配资源
                LoadAllocationResource();
                // 评估每个进程还需要的资源
                EvaluateNeedResource();

                // 打印各数据结构当前值
                PrintStatus();
                Console.Write("请输入要操作的进程号：");
                if (!int.TryParse(Console.ReadLine(), out var processId)) {
                    Console.WriteLine("\n请输入数字！");
                    continue;
                }

                if (processId < 0 || processId >= _processes.Count) {
                    Console.WriteLine("\n不存在进程号为 {0} 的进程！", processId);
                    continue;
                }

                Console.Write("请输入资源请求向量：");
                var request = Console.ReadLine();
                var requestVector = Array.ConvertAll(request?.Split(' '), int.Parse);

                // 检查资源请求是否合理
                var proc = _processes[processId];
                Console.WriteLine("银行家算法检验中...");
                for (var i = 0; i < requestVector.Length; i++) {
                    if (requestVector[i] > proc.Need[i]) {
                        Console.WriteLine("分配失败！资源类型 {0}，请求数量 {1}，超过进程所需数量 {2}",
                            i, requestVector[i], proc.Need[i]);
                        if (QueryExit()) Environment.Exit(0);
                        else goto mainLoop;
                    }

                    if (requestVector[i] > proc.Max[i]) {
                        Console.WriteLine("分配失败！资源类型 {0}，请求数量 {1}，超过进程最大资源数量 {2}",
                            i, requestVector[i], proc.Need[i]);
                        if (QueryExit()) Environment.Exit(0);
                        else goto mainLoop;
                    }
                }


                continueFlag = !QueryExit();
            }
        }

        /// <summary>
        /// 安全性算法
        /// </summary>
        private void SecurityEvaluate()
        {
            var work = _available;
            var finish = new bool[_processCount];

            for (var i = 0; i < _processCount; i++) {
                if (!finish[i]) {
                    for (var t = 0; t < work.Count; t++) {
                        var proc = _processes[i];
                        if (proc.Need[t] < work[t]) {
                            work[t] += proc.Allocation[t];
                            finish[i] = true;
                        }
                    }
                }
            }
        }

        private bool QueryExit()
        {
            Console.Write("是否退出(y/n)？");
            var option = Console.ReadLine()?.ToLower();
            return option == "y";
        }

        /// <summary>
        /// 评估进程所需资源
        /// </summary>
        private void EvaluateNeedResource()
        {
            foreach (var p in _processes) {
                p.EvaluateNeedResource();
                // 计算系统还剩下多少资源
                for (var i = 0; i < _resourceClassesCount; i++) {
                    _available[i] -= p.Allocation[i];
                }
            }
        }

        /// <summary>
        /// 打印出当前状态
        /// </summary>
        private void PrintStatus()
        {
            Console.WriteLine("-------------------------银行家算法-------------------------");
            Console.WriteLine("系统进程数量：{0}；资源种类数量：{1}", _processCount, _resourceClassesCount);
            Console.WriteLine("可用资源向量 Available：");
            foreach (var i in _resource) {
                Console.Write("{0} ", i);
            }

            Console.WriteLine("\n最大需求矩阵 Max：");
            foreach (var p in _processes) {
                foreach (var t in p.Max) {
                    Console.Write("{0} ", t);
                }

                Console.WriteLine("");
            }

            Console.WriteLine("已分配矩阵 Allocation：");
            foreach (var p in _processes) {
                foreach (var t in p.Allocation) {
                    Console.Write("{0} ", t);
                }

                Console.WriteLine("");
            }

            Console.WriteLine("需求矩阵 Need：");
            foreach (var p in _processes) {
                foreach (var t in p.Need) {
                    Console.Write("{0} ", t);
                }

                Console.WriteLine("");
            }
        }

        /// <summary>
        /// 加载系统所有可用的资源数量
        /// </summary>
        private void LoadAllAvailableResource()
        {
            var reader =
                new StreamReader(Path.Combine(Environment.CurrentDirectory, "Exp3", "input", "Available_list.txt"));
            var line = reader.ReadLine();
            // 获取各类型资源数量
            _resourceClassesCount = int.Parse(line?.Trim());
            line = reader.ReadLine();
            _resource.AddRange(Array.ConvertAll(line?.Split(' '), int.Parse));
            _available.AddRange(Array.ConvertAll(line?.Split(' '), int.Parse));
            reader.Close();
        }

        /// <summary>
        /// 加载所有进程以及他们需要的最大资源
        /// </summary>
        private void LoadProcessMaxResource()
        {
            var reader =
                new StreamReader(Path.Combine(Environment.CurrentDirectory, "Exp3", "input", "Max_list.txt"));
            var line = reader.ReadLine();
            var index = 0;
            _processCount = int.Parse(line?.Trim());
            while (!reader.EndOfStream) {
                line = reader.ReadLine();
                var tempProc = new ProcessExp3(index++) {
                    Max = Array.ConvertAll(line?.Split(' '), int.Parse)
                };
                _processes.Add(tempProc);
            }

            reader.Close();
        }

        /// <summary>
        /// 获取所有进程已经分配的资源
        /// </summary>
        private void LoadAllocationResource()
        {
            var reader = new StreamReader(Path.Combine(
                Environment.CurrentDirectory, "Exp3", "input", "Allocation_list.txt"));
            var index = 0;
            do {
                var line = reader.ReadLine();
                _processes[index++].Allocation = Array.ConvertAll(line?.Split(' '), int.Parse);
            } while (!reader.EndOfStream);

            reader.Close();
        }

        private void EvaluateProcessNeedResource() { }

        public static void Run()
        {
            new Main();
        }
    }
}