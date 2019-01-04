using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TofuCore.Events;
using TofuCore.Service;
using UnityEngine;

namespace TofuCore.Glop
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

            foreach (Glop g in Contents.Values) {
                g.Update((float)payload.GetContent());
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

        public int GenerateGlopId()
        {
            return ServiceContext.LastGlopId++;
        }
    }
}

