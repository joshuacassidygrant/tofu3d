using System.Collections.Generic;
using Newtonsoft.Json;
using TofuCore.Service;
using UnityEngine;

namespace TofuCore.Strings
{
    public class StringsService : AbstractService
    {
        private Dictionary<string, string> _strings;
        private string CurrentLanguage;
        

        public StringsService(string defaultLanguage)
        {
            SetLanguage(defaultLanguage);
        }

        public void SetLanguage(string language)
        {
            CurrentLanguage = language;

            _strings = ReadJson("Strings/" + language);
        }

        private Dictionary<string, string> ReadJson(string path)
        {
            TextAsset text = Resources.Load<TextAsset>(path);
            if (text == null)
            {
                Debug.LogWarning($"No text resource for path {path}");
            }
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(text.text);
        }

        public string Format(string id, params object[] values)
        {
            if (!_strings.ContainsKey(id))
            {
                Debug.LogWarning($"No string found for id {id}");
                return id + " " + string.Join(" ", values);
            }

            return string.Format(_strings[id], values);
        }

    }
}
