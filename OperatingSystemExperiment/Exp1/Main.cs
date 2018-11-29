namespace OperatingSystemExperiment.Exp1
{
    public class Main
    {
        public static void Run()
        {
            CentralProcessUnit.GenerateProcessList(5);
            new CentralProcessUnit().Run();
        }
    }
}