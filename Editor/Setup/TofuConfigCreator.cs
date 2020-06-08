using System;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TofuCore.Editor.Setup
{

    public class TofuConfigCreator {
        public static string FolderName = "tofuConfig";
        public static string CoreFolder = "Assets/tofu3d/Core";
        public static string PluginsFolder = "Assets/tofu3d/Plugins";
        public static string ScriptsFolder = "Assets/Scripts";
        public static string EventsConfigFileName = "eventList.txt";

        /**
         * Run this to create/regenerate a separate tofuconfig folder with an asmdef,
         * then federate all content from eventList.txt files under tofu3d/Core, tofu3d/Plugins, Scripts
         * into a single Events enum.
         */
        [MenuItem("Assets/Generate Tofu3d Scripts", false, -1)]
        static void Create() {
            DeleteConfigFolder();
            string newFolderPath = CreateFolderAndAsmdef();

            GenerateEventsEnum(newFolderPath + "/EventName.cs");
        }

        private static string CreateFolderAndAsmdef() {
            string guid = AssetDatabase.CreateFolder("Assets", FolderName);
            string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);

            string asmPath = newFolderPath + "/TofuConfig.asmdef";

            string asmContent = @"
            {
	            ""name"": ""TofuConfig""
            }
        ";
            File.WriteAllText(asmPath, asmContent);
            AssetDatabase.ImportAsset(asmPath);
            return newFolderPath;
        }

        public static void GenerateEventsEnum(string path) {
            string[] eventListPaths = { CoreFolder + "/" + EventsConfigFileName, PluginsFolder + "/" + EventsConfigFileName, ScriptsFolder + "/" + EventsConfigFileName };

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
namespace TofuConfig
{
    public enum EventName
    {
");

            HashSet<string> eventKeySet = new HashSet<string>();

            foreach (string eventListPath in eventListPaths) {
                StreamReader input = new StreamReader(eventListPath);
                while (!input.EndOfStream) {
                    string key = input.ReadLine();
                    if (key == null) break;
                    key = key.Replace("\n", "").Replace("\r", "");
                    if (!LegalEnumCheck(key)) {
                        Debug.LogWarning("Can't read enum name for events:" + key);
                    } else if (eventKeySet.Contains(key)) {
                        Debug.LogWarning("Ignoring duplicate event key value: " + key);
                    } else {
                        eventKeySet.Add(key);
                        sb.Append(key);
                        sb.Append(",");
                        sb.Append(Environment.NewLine);
                    }
                }
                input.Close();
            }


            sb.Append("}}");

            File.WriteAllText(path, sb.ToString());
            AssetDatabase.ImportAsset(path);
        }

        public static void DeleteConfigFolder() {
            AssetDatabase.DeleteAsset("Assets/" + FolderName);
        }

        public static bool LegalEnumCheck(string name) {
            if (name == "") return false;
            Regex rg = new Regex("[^a-zA-Z0-9_]");
            return !rg.IsMatch(name);
        }
    }

}
