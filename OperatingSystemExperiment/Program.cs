using System;
using OperatingSystemExperiment.Exp1;

namespace OperatingSystemExperiment {
    public static partial class Program {
        private static void Main(string[] args)
        {
            CentralProcessUnit.GenerateProcessList(5);
            new CentralProcessUnit().Run();
        }
    }
}