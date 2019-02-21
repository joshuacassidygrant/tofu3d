using System;
using System.Collections;
using System.Collections.Generic;
using TofuCore.Events;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Renderable
{
    public class AbstractRenderer : MonoBehaviour, IListener
    {

        protected IRenderable Renderable;
        protected SpriteRenderer SpriteRenderer;
        protected ServiceContext ServiceContext;
        protected EventContext EventContext;
        protected bool ToDestroy;

        private Dictionary<TofuEvent, List<Action<EventPayload>>> _boundListeners;
        private Dictionary<TofuEvent, Action<EventPayload>> _listenersToUnbind;

        protected RuntimeAnimatorController AnimatorController;
        protected Animator Anim;


        public void Render()
        {
            if (Renderable == null) return;
            transform.position = Renderable.Position;
            
            if (Anim.runtimeAnimatorController == null && SpriteRenderer != null)
            {
                SpriteRenderer.sprite = Renderable.Sprite;
            }
            else
            {
                UpdateAnimationStates(Renderable.GetAnimationStateBools());
            }

        }

        private void Update()
        {
            Render();

            if (ToDestroy)
            {
                enabled = false;
                Destroy(gameObject);
            }
        }

        public void Initialize(IRenderable renderable, ServiceContext context)
        {
            Renderable = renderable;
            SpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            ServiceContext = context;
            Anim = gameObject.AddComponent<Animator>();
            EventContext = context.Fetch("EventContext");
            Render();
        }


        protected void SetToPosition()
        {
            transform.position = Renderable.Position;
        }

        protected void SetLayer(string layerName)
        {
            SpriteRenderer.sortingLayerName = layerName;
        }

        public void ReceiveEvent(TofuEvent evnt, EventPayload payload)
        {
            FlushListeners();
            if (_boundListeners == null) return;

            if (!_boundListeners.ContainsKey(evnt))
            {
                Debug.Log("Expected event " + evnt + " but found no action bound");
            }

            foreach (Action<EventPayload> action in _boundListeners[evnt])
            {
                action.Invoke(payload);
            }
            FlushListeners();
        }

        public void BindListener(TofuEvent evnt, Action<EventPayload> action, EventContext evntContext)
        {
            if (_boundListeners == null) _boundListeners = new Dictionary<TofuEvent, List<Action<EventPayload>>>();

            if (!_boundListeners.ContainsKey(evnt)) _boundListeners.Add(evnt, new List<Action<EventPayload>>());

            evntContext.HelperBindEventListener(evnt, this);
            _boundListeners[evnt].Add(action);
        }

        public void UnbindListener(TofuEvent evnt, Action<EventPayload> action, EventContext evntContext)
        {
            evntContext.RemoveEventListener(evnt, this);
            if (_listenersToUnbind == null) _listenersToUnbind = new Dictionary<TofuEvent, Action<EventPayload>>();
            _listenersToUnbind.Add(evnt, action);

        }

        private void FlushListeners()
        {
            if (_listenersToUnbind == null) return;
            foreach (KeyValuePair<TofuEvent, Action<EventPayload>> entry in _listenersToUnbind)
            {
                _boundListeners[entry.Key].Remove(entry.Value);
            }
        }

        private void UpdateAnimationStates(Dictionary<string, bool> paramBools)
        {
            foreach (KeyValuePair<string, bool> entry in paramBools)
            {
                 Anim.SetBool(entry.Key, entry.Value);
            }
        }

        public IRenderable GetRenderable()
        {
            return Renderable;
        }
    }
}
