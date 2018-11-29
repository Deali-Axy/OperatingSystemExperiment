using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OperatingSystemExperiment.Exp3
{
    public class Main
    {
        private int _resourceClassesCount = 0;
        private readonly List<int> _resource = new List<int>();
        private List<int> _max = new List<int>();

        private Main()
        {
        }

        private void LoadAvailableResource()
        {
            var reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "Exp3", "Available_list.txt"));
            var line = reader.ReadLine();
            _resourceClassesCount = int.Parse(line?.Trim());
            _resource.AddRange(Array.ConvertAll(line?.Split(' '), int.Parse));
            reader.Close();
        }

        private void LoadProcessMaxResource()
        {
            var reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "Exp3", "Max_list.txt"));
            var line = reader.ReadLine();
            _resourceClassesCount = int.Parse(line?.Trim());
            _max.AddRange(Array.ConvertAll(line?.Split(' '), int.Parse));
            reader.Close();
        }

        public static void Run()
        {
            new Main();
        }
    }
}