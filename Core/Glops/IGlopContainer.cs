using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TofuCore.Glops;
using UnityEngine;

public interface IGlopContainer
{
    List<Glop> GetContents();
    Dictionary<int, Glop> GetContentsIndexed();
    Glop GetGlopById(int id);
    bool HasId(int id);
    void FillFromSerializedData(Dictionary<int, JObject> jsonGlopList);
    void ReinitializeContents();
    void FlushAll();
}
