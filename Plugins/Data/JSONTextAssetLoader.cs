using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace TofuPlugin.Data
{
    public class JSONTextAssetLoader<T>: IAssetLoader<T>
    {
        /*
         * Loads all JSON assets in a folder and converts them to type T, returning them as a list.
         */

        public List<T> Load(string path)
        {
            List<T> assetList = new List<T>();
            TextAsset[] assets = Resources.LoadAll<TextAsset>(path);
            foreach (TextAsset textAsset in assets)
            {
                string json = textAsset.text;
                List<T> objects = JsonConvert.DeserializeObject<List<T>>(json);
                assetList = assetList.Concat(objects).ToList();
            }

            return assetList;
        }
    }


}