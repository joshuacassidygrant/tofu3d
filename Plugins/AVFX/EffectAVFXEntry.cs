using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class EffectAVFXEntry
{
    [JsonProperty] public string EffectId;
    [JsonProperty] public float EffectDelay;
    [JsonProperty] public EffectAVFXTarget EffectTarget;
    [JsonProperty] public Dictionary<string, string> Config;
}
