using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TofuPlugin.Data
{
    public class PrefabAssetLoader<T> : IAssetLoader<GameObject>
    {
        /*
         * Loads all GameObject assets in a folder and converts them to type T, returning them as a list.
         */

        public List<GameObject> Load(string path)
        {
            GameObject[] resources = Resources.LoadAll<GameObject>(path);
            return new List<GameObject>(resources).ToList();
        }
    }
}