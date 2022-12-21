using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemPlus.UI;
using SystemPlus.Vectors;

namespace ProjectEarth_Data_Editor
{
    public static class Util
    {
        public static T LoadJson<T>(string path) => JsonSerializer.Deserialize<T>(File.ReadAllText(path));
        public static void SaveJson<T>(string path, T val) => File.WriteAllText(path, JsonSerializer.Serialize<T>(val));

        public static void ShowMenu(string title, string[] values, ref int selected)
        {
            SystemPlus.UI.Menu menu = new SystemPlus.UI.Menu(title, values);
            menu.SetSelected(selected);
            Console.Clear();
            Console.WriteLine("(Up, Down, Enter)");
            selected = menu.Show(Vector2Int.Up);
            Console.Clear();
        }

        public static string AddRestConsole(this string s) => s + new string(' ', Console.BufferWidth - s.Length - 1);

        public static char Last(this string s)
        {
            if (s.Length == 0)
                return '\0';
            else
                return s[s.Length - 1];
        }

        public static void PAKC(string message)
        {
            if (message == string.Empty)
                Console.WriteLine("Press any key to continue");
            else
                Console.WriteLine($"{message}, press any key to continue");

            Console.ReadKey(true);
        }

        public static string[] AskForBuildPlates(string title)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                DefaultExt = ".json",
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Build plate|*.json",
                Multiselect = true,
                Title = title,
                ShowHelp = false,
            };
            DialogResult res = dialog.ShowDialog();
            if (res != DialogResult.OK)
                return new string[0];
            List<string> files = dialog.FileNames.ToList();
            for (int i = 0; i < files.Count; i++)
                if (!File.Exists(files[i])) {
                    files.RemoveAt(i);
                    i--;
                }
            return files.ToArray();
        }

        public static T2[] For<T1, T2>(this T1[] array, Func<T1, T2> func) where T2 : class
        {
            List<T2> list = new List<T2>();
            for (int i = 0; i < array.Length; i++) {
                T2 t = func(array[i]);
                if (t != null)
                    list.Add(t);
            }

            return list.ToArray();
        }

        public static T[] Add<T>(this T[] array, T value) => array.Concat(new List<T>() { value }).ToArray();
    }
}
