using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemPlus.Extensions;
using SystemPlus.Json;
using SystemPlus.UI;
using SystemPlus.Vectors;
using WK.Libraries.BetterFolderBrowserNS;

using static ProjectEarth_Data_Editor.Util;

namespace ProjectEarth_Data_Editor
{
    public static class UI
    {
        private static string apiDir;

        public static IMenuAction menu = new SMenu("PE Data Editor".ToBIG(), LastVal.Exit, new IMenuAction[,]
        {
            { 
                new SMenu("Build plate", LastVal.Back, new IMenuAction[,] 
                { 
                    { new SAction(BuildPlate_ShaderToNormal) } 
                }, "Shared to normal") 
            },
            { 
                new SMenu("Player", LastVal.Back, new IMenuAction[,] 
            { 
                { new SMenu("Player/Build plate", LastVal.Back, new IMenuAction[,] 
                    {
                        { new SAction(Player_BuildPlate_Add) },
                        { new SAction(Player_BuildPlate_Remove) },
                        { new SAction(Player_BuildPlate_List) },
                    }, "Add", "Remove", "List")  } 
                }, "Build plate") 
            },
            { 
                new SAction(ServerConfig)
            },
            {
                new SMenu("", LastVal.Back, new IMenuAction[,]
                {

                }, "")
            },
        }, "Build plate", "Player", "ServerConfig");

        public static void Run(string _apiDir)
        {
            apiDir = _apiDir;
            Console.InputEncoding = Encoding.Unicode;
            Run(menu);
        }

        private static MenuResult BuildPlate_ShaderToNormal()
        {
            string[] files = AskForBuildPlates("Select build plate shared files");
            BetterFolderBrowser browser = new BetterFolderBrowser()
            {
                Title = "Select folder to place converted files to",
                Multiselect = false,
            };
            DialogResult res = browser.ShowDialog();
            if (res != DialogResult.OK || !Directory.Exists(browser.SelectedFolder))
                return MenuResult.Stay;
            string folder = browser.SelectedFolder;
            if (folder[folder.Length - 1] != '/' && folder[folder.Length - 1] != '\\')
                folder += '\\';
            Converter.SharedToNormal(files.ToArray(), folder);

            return MenuResult.Stay;
        }

        private static MenuResult Player_BuildPlate_Add()
        {
            Player p = Player.CreateAsk(apiDir);
            if (p == null)
                return MenuResult.Back;

            string[] files = AskForBuildPlates($"Select build plate files to add to player {p.Id}");

            // make sure unlocked build plates can be loaded
            string buildPlatesPath = apiDir + "data\\buildplates\\";
            for (int i = 0; i < files.Length; i++)
                if (!File.Exists(buildPlatesPath + Path.GetFileName(files[i])))
                    File.Copy(files[i], buildPlatesPath + Path.GetFileName(files[i]), true);

            files = files.For((string s) => Path.GetFileNameWithoutExtension(s));

            p.AddBuildPlates(files);

            PAKC("Done");

            return MenuResult.Stay;
        }
        private static MenuResult Player_BuildPlate_Remove()
        {
            Player p = Player.CreateAsk(apiDir);
            if (p == null)
                return MenuResult.Back;

            string[] files = AskForBuildPlates($"Select build plate files to remove from player {p.Id}");

            files = files.For((string s) => Path.GetFileNameWithoutExtension(s));

            p.RemoveBuildPlates(files);

            PAKC("Done");

            return MenuResult.Stay;
        }
        private static MenuResult Player_BuildPlate_List()
        {
            Player p = Player.CreateAsk(apiDir);
            if (p == null)
                return MenuResult.Back;

            Console.WriteLine($"Player: {p.Id}");
            Console.WriteLine("Unlocked:");
            for (int i = 0; i < p.buildPlate.UnlockedBuildplates.Count; i++)
                Console.WriteLine($" - {p.buildPlate.UnlockedBuildplates[i]}");
            Console.WriteLine("Locked:");
            for (int i = 0; i < p.buildPlate.LockedBuildplates.Count; i++)
                Console.WriteLine($" - {p.buildPlate.LockedBuildplates[i]}");

            PAKC(string.Empty);

            return MenuResult.Stay;
        }

        private static MenuResult ServerConfig()
        {
            JsonObject config = JsonSerializer.Deserialize(File.ReadAllText(apiDir + "data\\config\\apiconfig.json"));
            MenuSettings s = new MenuSettings("ServerConfig".ToBIG(), new IMenuSettingsItem[]
                {
                    new MSIStringValue("BaseServerIP", config["baseServerIP"].ToString(), 28),
                    new MSIBool("UseBaseServerIP", bool.Parse(config["useBaseServerIP"].ToString())) { dispType = MSIBool.Checkmark },
                    new MSIStringValue("TileServerUrl", config["tileServerUrl"].ToString(), 28),
                    new MSIIntSlider("MixTappableSpawnAmount", int.Parse(config["mixTappableSpawnAmount"].ToString()), 0, 100),
                    new MSIIntSlider("MaxTappableSpawnAmount", int.Parse(config["maxTappableSpawnAmount"].ToString()), 0, 100),
                    new MSIFloatValue("TappableSpawnRadius", float.Parse(config["tappableSpawnRadius"].ToString(), CultureInfo.InvariantCulture), 0.0001f, 10f) { step = 0.01f },
                });
            Console.Clear();
            Console.WriteLine("(Up, Down, Enter)");
            MenuSettings.STATUS status = s.Show(Vector2Int.Up);
            if (status == MenuSettings.STATUS.OK) {
                config["baseServerIP"] = s.lastValues[0].GetValue();
                config["useBaseServerIP"] = s.lastValues[1].GetValue();
                config["tileServerUrl"] = s.lastValues[2].GetValue();
                config["mixTappableSpawnAmount"] = s.lastValues[3].GetValue();
                config["maxTappableSpawnAmount"] = s.lastValues[4].GetValue();
                config["tappableSpawnRadius"] = s.lastValues[5].GetValue();
                File.WriteAllText(apiDir + "data\\config\\apiconfig.json", JsonSerializer.Serialize(config, JsonSerializationSettings.Default));
            }
            return MenuResult.Back;
        }

        private static void Run(IMenuAction menu)
        {
            MenuResult res = menu.Run();
            if (res.menuAction == MenuAction.Stay)
                Run(menu);
            else if (res.menuAction == MenuAction.Forward) {
                for (int i = 0; i < menu.Inner.GetLength(1); i++)
                    Run(menu.Inner[res.selected, i]);
                Run(menu);
            }
        }
    }

    public interface IMenuAction
    {
        IMenuAction[,] Inner { get; }
        MenuResult Run();
    }

    public class SAction : IMenuAction
    {
        public IMenuAction[,] Inner { get; private set; }

        public Func<MenuResult> Action;

        public MenuResult Run() => Action();

        public SAction(Func<MenuResult> _action)
        {
            Action = _action;
        }
    }

    public class SMenu : IMenuAction
    {
        public IMenuAction[,] Inner { get; private set; }

        public string Title;
        public string[] Values;
        public LastVal lastVal;

        public SMenu(string _title, LastVal _lastVal, IMenuAction[,] _inner, params string[] _values)
        {
            Title = _title;
            lastVal = _lastVal;
            Values = _values;
            Inner = _inner;
        }

        public MenuResult Run()
        {
            List<string> values = Values.ToList();
            if (lastVal == LastVal.Back)
                values.Add("Back");
            else if (lastVal == LastVal.Exit)
                values.Add("Exit");

            SystemPlus.UI.Menu m = new SystemPlus.UI.Menu(Title, values.ToArray());

            Console.Clear();
            Console.WriteLine("(Up, Down, Enter)");
            int selected = m.Show(Vector2Int.Up);
            Console.Clear();

            if (lastVal != LastVal.None && selected == values.Count - 1)
                return new MenuResult(selected, MenuAction.BackOrExit);
            else
                return new MenuResult(selected, MenuAction.Forward);
        }
    }

    public struct MenuResult
    {
        public static readonly MenuResult Back = new MenuResult(-1, MenuAction.BackOrExit);
        public static readonly MenuResult Stay = new MenuResult(-1, MenuAction.Stay);

        public int selected;
        public MenuAction menuAction;

        public MenuResult(int _selected, MenuAction _menuAction)
        {
            selected = _selected;
            menuAction = _menuAction;
        }
    }

    public enum MenuAction
    {
        Forward,
        Stay,
        BackOrExit,
    }

    public enum LastVal
    {
        None,
        Back,
        Exit,
    }
}
