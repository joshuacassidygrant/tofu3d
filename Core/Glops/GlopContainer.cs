using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TofuConfig;
using TofuCore.Events;
using TofuCore.Service;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace TofuCore.Glops
{
    /*
     * A GLOP Container holds and manages GLOP objects (TOFU's generic object type).
     * It contains methods for storing, retrieving and updating its child glops.
     */
    public class GlopContainer<T>: AbstractService, IGlopContainer, IGlopStream {
        T Value { get; }
        public T Default = default(T);

        protected Dictionary<int, Glop> _contents;
        [Dependency] protected IEventContext EventContext;

        public override void Initialize() {
            base.Initialize();
            BindListener(EventKey.FrameUpdate, OnUpdateFrame, EventContext);
        }

        public override void Prepare()
        {
            base.Prepare();
            BindListener(EventKey.GlopsDeserialized, HandleGlopsDeserialized, EventContext);
        }

        public void SetDefault(T def)
        {
            Default = def;
        }

        public void OnUpdateFrame(EventPayload payload) {

            if (payload.ContentType != "Float") return;

            List<Glop> garbageCollection = new List<Glop>();

            foreach (Glop g in _contents.Values) {
                if (g.Garbage)
                {
                    garbageCollection.Add(g);
                } else
                {
                    g.Update((float)payload.GetContent());
                }
            }

            if (garbageCollection.Count > 0)
            {
                foreach (Glop g in garbageCollection)
                {
                    _contents.Remove(g.Id);
                }
            }


        }

        public override void Build()
        {
            base.Build();
            _contents = new Dictionary<int, Glop>();
        }

        public int CountActive() {
            return _contents.Count;
        }

        public List<Glop> GetContents()
        {
            return _contents.Values.ToList();
        }


        public virtual Dictionary<int, Glop> GetContentsIndexed()
        {
            return _contents;
        }

        public Glop GetGlopById(int id)
        {
            if (id == 0) {
                return (Glop)(object)Default;
            }

            if (HasId(id))
            {
                return _contents[id];
            }
            return null;
        }

        public T Get(int id)
        {
            if (id == 0)
            {
                return Default;
            }

            if (!_contents.ContainsKey(id))
            {
                Debug.LogWarning($"Couldn't find {id} in container.");
                return default(T);
            }

            return (T)(object)_contents[id];
        }

        public List<T> GetAll(List<int> ids) {
            List<T> vals = new List<T>();
            try {
                ids.ForEach(i => vals.Add((T)(object)_contents[i]));
            } catch (Exception e) {
                Debug.LogWarning(e);
            }

            return vals;
        }


        public List<T> GetAll()
        {
            return _contents.Values.Cast<T>().ToList();
        }

        public bool HasId(int id)
        {
            return _contents.ContainsKey(id);
        }

        /**
         * Register gives the GLOP an Id, injects its dependencies, triggers initialization, and adds it to the GlopContainer.
         */
        public void Register(Glop glop)
        {
            if (glop == null)
            {
                Debug.Log("Tried to register a null Glop!");
                return;
            }

            int id = GenerateGlopId();
            glop.Id = id;
            glop.Initialize();
            _contents.Add(id, glop);
        }

        public virtual void RegisterFromLoad(int id, T val)
        {
            Glop glop = val as Glop;

            if (glop == null) {
                Debug.Log($"Tried to register a null Glop to id {id} in {this}!");
                return;
            }

            if (_contents.ContainsKey(id))
            {
                Debug.Log($"Already registered a glop with id {id} in {this}");
                return;
            }

            _contents.Add(id, glop);
        }

        private void HandleGlopsDeserialized(EventPayload payload)
        {
            if (payload.ContentType != "Null") return;
            ReinitializeContents();
        }

        public virtual void ReinitializeContents()
        {
            foreach (Glop glop in _contents.Values) {
                glop.ResolveAfterDeserialize(ServiceContext);
            }
            EventContext.TriggerEvent(EventKey.ContainerResolved);
        }

        public int GenerateGlopId()
        {
            return ServiceContext.LastGlopId++;
        }

        public List<T> ResolveGlopList(List<int> ids)
        {
            return ids.Select(x => _contents[x]).Cast<T>().ToList();
        }

        public void Destroy(Glop glop)
        {
            if (HasId(glop.Id))
            {
                glop.Die();
                _contents.Remove(glop.Id);
                return;
            }

            Debug.Log("No GLOP found with id " + glop.Id);
        }

        public void DestroyAllByIds(List<int> ids)
        {
            foreach (int id in ids)
            {
                if (HasId(id)) {
                    _contents[id].Die();
                    _contents.Remove(id);
                }
            }
        }

        public void FlushAll()
        {
            DestroyAllByIds(_contents.Keys.ToList());
        }

        public void Resolve(ref T item)
        {
            Glop glop = item as Glop;
            if (glop == null) return;
            item = Get(glop.Id);
        }

        public void ResolveAll(ref List<T> items)
        {
            items = ResolveGlopList(items.Select(i => (i as Glop).Id).ToList());
        }

        public virtual void FillFromSerializedData(Dictionary<string, JObject> jsonGlopList)
        {
            foreach (KeyValuePair<string, JObject> jsonGlop in jsonGlopList)
            {
                int id = -1;
                bool succ = int.TryParse(jsonGlop.Key, out id);
                if (!succ)
                {
                    Debug.LogWarning($"Can't parse id to int: {jsonGlop.Key}");
                }
                T val = JsonConvert.DeserializeObject<T>(jsonGlop.Value.ToString());
                RegisterFromLoad(id, val);
            }
        }
    }
}

