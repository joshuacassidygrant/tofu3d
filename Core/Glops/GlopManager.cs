using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TofuCore.Events;
using TofuCore.Service;
using UnityEngine;

namespace TofuCore.Glops
{
    public class GlopManager : AbstractService
    {

        protected Dictionary<int, Glop> Contents;
        [Dependency] protected EventContext EventContext;

        public override void Initialize() {
            BindListener(EventContext.GetEvent("FrameUpdate"), OnUpdateFrame, EventContext);
        }

        public void OnUpdateFrame(EventPayload payload) {

            if (payload.ContentType != "Float") return;

            List<Glop> garbageCollection = new List<Glop>();

            foreach (Glop g in Contents.Values) {
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
                    Contents.Remove(g.Id);
                }
            }


        }

        public override void Build()
        {
            base.Build();
            Contents = new Dictionary<int, Glop>();
        }

        public int CountActive() {
            return Contents.Count;
        }

        public List<Glop> GetContents()
        {
            return Contents.Values.ToList();
        }

        public Glop GetGlopById(int id)
        {
            if (HasId(id))
            {
                return Contents[id];
            }
            return null;
        }

        public bool HasId(int id)
        {
            return Contents.ContainsKey(id);
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
                Contents.Remove(glop.Id);
            }

            Debug.Log("No GLOP found with id " + glop.Id);
        }
    }
}

