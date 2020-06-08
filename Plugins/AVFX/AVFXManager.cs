using System;
using System.Collections.Generic;
using TofuConfig;
using TofuCore.Configuration;
using TofuCore.Events;
using TofuCore.Service;
using UnityEngine;

namespace Scripts.AVFX
{
    public class AVFXManager : AbstractMonoService
    {

        [Dependency] protected EventContext EventContext;
        [Dependency] protected AVFXLibrary AVFXLibrary;

        public List<AVFXInstance> ActiveInstances;

        public override void Initialize()
        {
            base.Initialize();
            BindListener(EventKey.AVFXRequest, HandleAVFXRequest, EventContext);
            ActiveInstances = new List<AVFXInstance>();
        }

        public AVFXInstance SpawnAVFXInstance(string id, Vector3 position, Vector3 end, Configuration config = null)
        {
            AVFXInstance instance = SpawnAVFXInstance(id, position, config);
            instance.SetProjectileTarget(end);

            return instance;
        }

        public AVFXInstance SpawnAVFXInstance(string id, Vector3 position, Configuration config = null)
        {
            if (!AVFXLibrary.ContainsKey(id))
            {
                Debug.LogWarning($"No AVFX registered with id {id}");
                return null;
            }

            AVFXInstance instance = Instantiate(AVFXLibrary.Get(id), transform);
            instance.transform.position = position;
            instance.BindManager(this);
            ActiveInstances.Add(instance);
            return instance;
        }

        private void HandleAVFXRequest(EventPayload payload)
        {
            if (payload.ContentType != "AVFXRequest") return;

            AVFXRequest req = payload.GetContent();

            Vector3 target = req.Target;
            Vector3 origin = req.Origin;
            List<EffectAVFXEntry> fxList = req.Entries;

            bool projectile = false;
            Vector3 end = Vector3.zero;

            foreach (EffectAVFXEntry fx in fxList) {
                //TODO: afvx delays
                //TODO: afvx configs

                Vector3 pos = target;
                switch(fx.EffectTarget)
                {
                    case EffectAVFXTarget.ORIGIN:
                        pos = origin;
                        break;
                    case EffectAVFXTarget.TARGET:
                        pos = target;
                        break;
                    case EffectAVFXTarget.GLOBAL:
                        pos = Vector3.zero;
                        break;
                    case EffectAVFXTarget.PROJ_SELF_TO_TARGET:
                        pos = origin + Vector3.up;
                        projectile = true;
                        end = target;
                        break;
                    case EffectAVFXTarget.PROJ_TARGET_TO_SELF:
                        pos = target + Vector3.up;
                        projectile = true;
                        end = origin;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (projectile)
                {
                    SpawnAVFXInstance(fx.EffectId, pos, end);
                }
                else
                {
                    SpawnAVFXInstance(fx.EffectId, pos);
                }

            }


        }

    }
}
