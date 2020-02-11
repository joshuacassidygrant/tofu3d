using System.Collections.Generic;
using TofuCore.Service;
using UnityEngine;

namespace TofuCore.Strings
{
    public class StringsService : AbstractService
    {
        private Dictionary<string, string> _strings;
        private string CurrentLanguage;

        public override void Initialize()
        {
            base.Initialize();
            _strings = new Dictionary<string, string>();

            //test
            _strings.Add("PNAMES_turn", "{0}'s turn.");
        }

        public void SetLanguage(string language)
        {
            CurrentLanguage = language;
            // TODO:
            // load in new language to _strings dicto
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
