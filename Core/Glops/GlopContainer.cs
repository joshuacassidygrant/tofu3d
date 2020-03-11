using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TofuCore.Events;
using TofuCore.Service;
using Debug = UnityEngine.Debug;

namespace TofuCore.Glops
{
    /*
     * A GLOP Container holds and manages GLOP objects (TOFU's generic object type).
     * It contains methods for storing, retrieving and updating its child glops.
     */
    public class GlopContainer<T>: AbstractService, IGlopContainer, IGlopStream {
        T Value { get; }

        protected Dictionary<int, Glop> _contents;
        [Dependency] protected IEventContext EventContext;


        public override void Initialize() {
            BindListener(EventContext.GetEvent("FrameUpdate"), OnUpdateFrame, EventContext);
        }

        public override void Prepare()
        {
            base.Prepare();
            BindListener("GlopsDeserialized", HandleGlopsDeserialized, EventContext);

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
            if (HasId(id))
            {
                return _contents[id];
            }
            return null;
        }

        public T Get(int id)
        {
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
                Debug.Log("Tried to register a null Glop!");
                return;
            }

            _contents.Add(id, glop);
        }

        private void HandleGlopsDeserialized(EventPayload payload)
        {
            Debug.Log(this);
            if (payload.ContentType != "Null") return;
            ReinitializeContents();
        }

        public virtual void ReinitializeContents()
        {
            // Called after glops loaded to trigger any internal inter-glop resolutions etc.
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
            }

            Debug.Log("No GLOP found with id " + glop.Id);
        }

        public virtual void FillFromSerializedData(Dictionary<int, JObject> jsonGlopList)
        {
            foreach (KeyValuePair<int, JObject> jsonGlop in jsonGlopList)
            {
                T val = JsonConvert.DeserializeObject<T>(jsonGlop.Value.ToString());
                RegisterFromLoad(jsonGlop.Key, val);
            }
        }
    }
}

