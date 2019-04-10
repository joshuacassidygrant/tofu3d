﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TofuCore.Events;
using TofuCore.Service;
using Debug = UnityEngine.Debug;

namespace TofuCore.Glops
{
    /*
     * A GLOP Container holds and manages GLOP objects (TOFU's generic object type).
     * It contains methods for storing, retrieving and updating its child glops.
     */
    public class GlopContainer<T> : AbstractService, IGlopContainer
    {
        T Value { get; }

        private Dictionary<int, Glop> _contents;
        [Dependency] protected IEventContext EventContext;

        public override void Initialize() {
            BindListener(EventContext.GetEvent("FrameUpdate"), OnUpdateFrame, EventContext);
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

        public Glop GetGlopById(int id)
        {
            if (HasId(id))
            {
                return _contents[id];
            }
            return null;
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
            glop.InjectDependencies(ContentInjectables);
            glop.Initialize();
            _contents.Add(id, glop);
        }

        public int GenerateGlopId()
        {
            return ServiceContext.LastGlopId++;
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
    }
}

