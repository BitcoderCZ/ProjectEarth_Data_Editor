using ProjectEarth_Data_Editor.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemPlus;
using SystemPlus.Extensions;

namespace ProjectEarth_Data_Editor
{
    class Program
    {
        static void Main(string[] args)
        {
            string from = @"C:\Users\Tomas\Desktop\Project Earth\Api\data\buildplates\_plates";
            string to = @"C:\Users\Tomas\Desktop\Project Earth\Api\data\buildplates\plates\";

            string[] files = Directory.GetFiles(from);
            ParallelOptions options = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount, };

            // done 412
            int done = 0;
            int skipped = 0;
            int converting = 0;
            object lockObj = new object();

            List<float> times = new List<float>();

            Console.CursorVisible = false;
            Print(done, files.Length, skipped, converting, times);
            Parallel.For(0, files.Length, options, (int i) =>
            {
                converting++;
                lock (lockObj)
                    Print(done, files.Length, skipped, converting, times);
                Stopwatch watch = new Stopwatch();
                watch.Start();
                try {
                    JsonObject converted = Converter.SharedToNormal(JsonSerializer.Deserialize(File.ReadAllText(files[i])));
                    File.WriteAllText(to + converted["id"] + ".json", JsonSerializer.Serialize(converted, JsonSerializationSettings.Default));
                    done++;
                    watch.Stop();
                    times.Add((float)watch.Elapsed.TotalSeconds);
                } catch { skipped++; }
                watch.Stop();
                converting--;
                lock (lockObj)
                    Print(done, files.Length, skipped, converting, times);
            });
            Console.ReadKey(true);
        }

        private static void Print(int done, int maxDone, int skipped, int converting, List<float> times)
        {
            int top = Console.CursorTop;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Converted {done + skipped}/{maxDone} {MathPlus.Round(((float)(done + skipped) / maxDone) * 100f, 2)}%, Sucseeded: {done}, Failed: {skipped}, Working on: {converting}," +
                $" AvrgTime: {MathPlus.Round(ArrayExtensions.Avrage(times), 2)}s");
            Console.SetCursorPosition(0, top);
        }
    }
}
