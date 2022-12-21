using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemPlus;
using SystemPlus.Extensions;
using SystemPlus.UI;
using SystemPlus.Vectors;
using WK.Libraries.BetterFolderBrowserNS;
using static ProjectEarth_Data_Editor.Util;

namespace ProjectEarth_Data_Editor
{
    static class Program
    {
        private static string apiDir;

        [STAThread]
        public static void Main()
        {
            BetterFolderBrowser browser = new BetterFolderBrowser()
            {
                Title = "Select Api folder (ProjectEarth/Api)",
                Multiselect = false,
            };

            DialogResult res = browser.ShowDialog();

            if (res != DialogResult.OK)
                return;

            apiDir = browser.SelectedFolder;

            if (apiDir.Last() != '\\' && apiDir.Last() != '/')
                apiDir += '\\';

            if (!Directory.Exists(apiDir) || !Directory.Exists(apiDir + "data"))
                return;

            UI.Run(apiDir);
        }
    }
}
