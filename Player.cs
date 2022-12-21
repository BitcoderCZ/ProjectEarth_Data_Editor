using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using static ProjectEarth_Data_Editor.Util;

namespace ProjectEarth_Data_Editor
{
    public class Player
    {
        public string Path;
        public string Id;

        public BuildPlate buildPlate;

        public Player(string _path)
        {
            if (_path[_path.Length - 1] != '\\' && _path[_path.Length - 1] != '/')
                Path = _path + '\\';
            else
                Path = _path;

            string __path = Path.Substring(0, Path.Length - 1);
            Id = __path.Substring(__path.LastIndexOf('\\') + 1);

            Load();
        }

        public static Player CreateAsk(string apiDir)
        {
            string playersPath = apiDir + "data\\players\\";
            if (File.Exists(playersPath + ".gitkeep"))
                File.Delete(playersPath + ".gitkeep");
            string[] _players = Directory.GetDirectories(playersPath);
            Player[] players = _players.For(playerPath => new Player(playerPath));
            string[] ids = players.For(p => p.Id);

            ids = ids.Add("Cancel");

            int selected = 0;
            ShowMenu("Select player:", ids, ref selected);

            if (selected == ids.Length - 1)
                return null;
            else
                return players[selected];
        }

        public void AddBuildPlates(string[] buildPlates)
        {
            Load();

            for (int i = 0; i < buildPlates.Length; i++) {
                if (buildPlate.LockedBuildplates.Contains(buildPlates[i]))
                    buildPlate.LockedBuildplates.Remove(buildPlates[i]);
                if (!buildPlate.UnlockedBuildplates.Contains(buildPlates[i]))
                    buildPlate.UnlockedBuildplates.Add(buildPlates[i]);
            }

            Save();
        }

        public void RemoveBuildPlates(string[] buildPlates, bool lockPlates = false)
        {
            Load();

            for (int i = 0; i < buildPlates.Length; i++) {
                if (buildPlate.UnlockedBuildplates.Contains(buildPlates[i]))
                    buildPlate.UnlockedBuildplates.Remove(buildPlates[i]);
                if (lockPlates && !buildPlate.LockedBuildplates.Contains(buildPlates[i]))
                    buildPlate.LockedBuildplates.Add(buildPlates[i]);
            }

            Save();
        }

        public void Load()
        {
            buildPlate = LoadJson<BuildPlate>(Path + "buildplates.json");
        }

        public void Save()
        {
            SaveJson(Path + "buildplates.json", buildPlate);
        }

        [Serializable]
        public class BuildPlate
        {
            public List<string> UnlockedBuildplates { get; set; }
            public List<string> LockedBuildplates { get; set; }
        }
    }
}
