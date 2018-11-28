using System;
using System.Dynamic;
using System.Threading;

namespace OperatingSystemExperiment.Exp2 {
    /// <summary>
    /// 生产者-消费者问题
    /// 使用 P-V 操作解决同步和互斥问题
    /// 本实验要求设计并实现一个进程，该进程拥有3个生产者线程和1个消费者线程，它们使用10个不同的缓冲区。
    /// </summary>
    public static class Main {
        private static int[] _buffer = new int[10];

        /// <summary>
        /// 是否继续运行
        /// </summary>
        private static bool _continueRun = true;

        /// <summary>
        /// 是否锁定缓冲区
        /// </summary>
        private static bool _isLock = false;

        /// <summary>
        /// 产品号
        /// </summary>
        private static int _productId = 0;

        /// <summary>
        /// 信号枚举
        /// </summary>
        private enum SemaphoreEnum {
            /// <summary>
            /// 互斥信号量，用以阻止生产者线程和消费者线程同时操作缓冲区队列
            /// </summary>
            Mutex,

            /// <summary>
            /// 当生产者线程生产出一个物品时可以用它向消费者线程发出信号
            /// </summary>
            Full,

            /// <summary>
            /// 消费者线程释放出一个空缓冲区时可以用它向生产者线程发出信号
            /// </summary>
            Empty
        }

        public static void Do()
        {
            ThreadStart producer = () => {
                while (_continueRun) {
                    Thread.Sleep(1200);
                    // 请求空缓冲区
                    var emptyBufferId = GetEmptyBuffer();
                    // 没有空缓冲区，继续等
                    if (emptyBufferId == -1) continue;
                    // 缓冲区锁定，等待
                    if (_isLock) continue;
                    P(SemaphoreEnum.Empty);
                    P(SemaphoreEnum.Mutex);
                    Console.WriteLine("生产线程 {0} 工作中", Thread.CurrentThread.ManagedThreadId);
                    AddToBuffer(emptyBufferId, ++_productId);
                    Console.WriteLine("Produce the {0} product to buffer.", _productId);
                    // 输出缓冲区内容
                    var nextIn = GetEmptyBuffer();
                    var nextOut = GetFullBuffer();
                    for (var i = 0; i < _buffer.Length; i++) {
                        if (i == nextOut)
                            Console.WriteLine("{0}: {1} <- 下一个可取出产品消费的地方", i, _buffer[i]);
                        else if (i == nextIn)
                            Console.WriteLine("{0}: {1} <- 可放下一个产品的位置", i, _buffer[i]);
                        else
                            Console.WriteLine("{0}: {1}", i, _buffer[i]);
                    }

                    V(SemaphoreEnum.Mutex);
                    // 用信号通知一个消费者线程有一个满的缓冲区
                    V(SemaphoreEnum.Full);
                }
            };

            // 1个监视线程
            new Thread(() => {
                if (_productId > 20)
                    _continueRun = false;
            }).Start();
            // 3个生产者
            new Thread(producer).Start();
            new Thread(producer).Start();
            new Thread(producer).Start();
            // 1个消费者
            new Thread(() => {
                while (_continueRun) {
                    Thread.Sleep(200);
                    // 请求一个满的缓冲区
                    var fullBufferId = GetFullBuffer();
                    if (fullBufferId == -1) continue;
                    // 缓冲区锁定则继续等待
                    if (_isLock) continue;
                    P(SemaphoreEnum.Full);
                    // 操作缓冲区池
                    P(SemaphoreEnum.Mutex);
                    Console.WriteLine("消费者线程 {0} 工作", Thread.CurrentThread.ManagedThreadId);
                    var productId = TakeFromBuffer(fullBufferId);
                    Console.WriteLine("正在消费产品 {0}", productId);
                    V(SemaphoreEnum.Mutex);
                    // 用信号通知一个空的缓冲区
                    V(SemaphoreEnum.Empty);
                }
            }).Start();
        }

        /// <summary>
        /// 生产者把新生产的产品放入缓冲区
        /// </summary>
        /// <returns>是否成功放入，没有空缓冲区的时候不成功</returns>
        private static bool AddToBuffer(int position, int product)
        {
            if (_buffer[position] != 0) return false;
            _buffer[position] = product;
            return true;
        }

        /// <summary>
        /// 获取一个空的缓冲区，都是满的则返回-1
        /// </summary>
        /// <returns>空缓冲区的编号</returns>
        private static int GetEmptyBuffer()
        {
            for (var i = 0; i < _buffer.Length; i++) {
                if (_buffer[i] == 0) {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 获取一个满的缓冲区，都是空的则返回-1
        /// </summary>
        /// <returns>满缓冲区的编号</returns>
        private static int GetFullBuffer()
        {
            for (var i = 0; i < _buffer.Length; i++) {
                if (_buffer[i] != 0) {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 消费者从缓冲区中取出一个产品
        /// </summary>
        /// <returns>产品id</returns>
        private static int TakeFromBuffer(int position)
        {
            var temp = _buffer[position];
            _buffer[position] = 0;
            return temp;
        }

        /// <summary>
        /// 申请资源操作
        /// </summary>
        /// <param name="s"></param>
        private static void P(SemaphoreEnum s)
        {
            switch (s) {
                case SemaphoreEnum.Mutex:
                    _isLock = true;
                    break;
                case SemaphoreEnum.Full:
                    break;
                case SemaphoreEnum.Empty:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(s), s, null);
            }
        }

        /// <summary>
        /// 释放资源操作
        /// </summary>
        /// <param name="s"></param>
        private static void V(SemaphoreEnum s)
        {
            switch (s) {
                case SemaphoreEnum.Mutex:
                    _isLock = false;
                    break;
                case SemaphoreEnum.Full:
                    break;
                case SemaphoreEnum.Empty:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(s), s, null);
            }
        }
    }
}