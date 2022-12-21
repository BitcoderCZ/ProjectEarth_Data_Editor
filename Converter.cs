using ProjectEarth_Data_Editor.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SystemPlus;
using SystemPlus.Extensions;

namespace ProjectEarth_Data_Editor
{
    public static class Converter
    {
        public static void SharedToNormal(string[] files, string toPath)
        {
            ParallelOptions options = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount, };

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
                    SharedBuildPlate sbp = JsonSerializer.Deserialize<Json.SharedBuildPlate>(File.ReadAllText(files[i]),
                        new JsonSerializerOptions() { AllowTrailingCommas = true });
                    BuildPlate bp = sharedToNormal(sbp);
                    File.WriteAllText(toPath + bp.id.ToString() + ".json", JsonSerializer.Serialize<BuildPlate>(bp));
                    done++;
                    watch.Stop();
                    times.Add((float)watch.Elapsed.TotalSeconds);
                }
                catch (Exception e) { 
                    Console.WriteLine($"Failed to convert {Path.GetFileName(files[i])}"); 
                    skipped++; 
                }
                watch.Stop();
                converting--;
                lock (lockObj)
                    Print(done, files.Length, skipped, converting, times);
            });
            Console.WriteLine("Done, press any key to continue");
            Console.ReadKey(true);
        }
        private static BuildPlate sharedToNormal(SharedBuildPlate _bp)
        {
            SharedBuildPlateData bpd = _bp.result.buildplateData;

            BuildPlate buildPlate = new BuildPlate();
            buildPlate.blocksPerMeter = bpd.blocksPerMeter;
            buildPlate.dimension = bpd.dimension;
            buildPlate.eTag = "dsasdasda";
            buildPlate.id = Guid.NewGuid();
            buildPlate.isModified = false;
            buildPlate.lastUpdated = "2022-04-13T12:42:24Z";
            buildPlate.locked = false;
            buildPlate.model = bpd.model;
            buildPlate.numberOfBlocks = 0;
            buildPlate.offset = bpd.offset;
            buildPlate.order = bpd.order;
            buildPlate.requiredLevel = 0;
            buildPlate.surfaceOrientation = bpd.surfaceOrientation;
            buildPlate.templateId = "00000000-0000-0000-0000-000000000000";
            buildPlate.type = bpd.type;
            return buildPlate;
        }
        private static void Print(int done, int maxDone, int skipped, int converting, List<float> times)
        {
            int top = Console.CursorTop;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Converted {done + skipped}/{maxDone} {MathPlus.Round(((float)(done + skipped) / maxDone) * 100f, 2)}%, Sucseeded: {done}, Failed: {skipped}, Working on: {converting}," +
                $" AvrgTime: {MathPlus.Round(ArrayExtensions.Avrage(times), 2)}s".AddRestConsole());
            Console.SetCursorPosition(0, top);
        }
    }
}
