﻿using System;
using System.Collections;
using System.Collections.Generic;
using TofuConfig;
using TofuCore.Events;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Renderable
{
    public class AbstractRenderer : MonoBehaviour, IListener
    {

        protected IRenderable Renderable;
        protected SpriteRenderer SpriteRenderer;
        protected IServiceContext ServiceContext;
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

        public void Initialize(IRenderable renderable, IServiceContext context)
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

        public void BindListener(EventKey key, Action<EventPayload> action, IEventContext evntContext)
        {
            BindListener(evntContext.GetEvent(key), action, evntContext);
        }

        public void BindListener(TofuEvent evnt, Action<EventPayload> action, IEventContext evntContext)
        {
            if (_boundListeners == null) _boundListeners = new Dictionary<TofuEvent, List<Action<EventPayload>>>();

            if (!_boundListeners.ContainsKey(evnt)) _boundListeners.Add(evnt, new List<Action<EventPayload>>());

            evntContext.ContextBindEventListener(evnt, this);
            _boundListeners[evnt].Add(action);
        }

        public void UnbindListener(TofuEvent evnt, Action<EventPayload> action, IEventContext evntContext)
        {
            evntContext.ContextRemoveEventListener(evnt, this);
            if (_listenersToUnbind == null) _listenersToUnbind = new Dictionary<TofuEvent, Action<EventPayload>>();
            if (!_listenersToUnbind.ContainsKey(evnt))
            {
                _listenersToUnbind.Add(evnt, action);
            }

        }

        private void FlushListeners()
        {
            if (_listenersToUnbind == null) return;
            foreach (KeyValuePair<TofuEvent, Action<EventPayload>> entry in _listenersToUnbind)
            {
                if (_boundListeners.ContainsKey(entry.Key))
                {
                    _boundListeners[entry.Key].Remove(entry.Value);
                }
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

        public void UnbindListenerDeferred(EventKey key, Action<EventPayload> action, IEventContext evntContext) {
            if (_listenersToUnbind != null) {
                _listenersToUnbind.Add(evntContext.GetEvent(key), action);
            } else {
                UnbindListener(evntContext.GetEvent(key), action, evntContext);
            }
        }
    }
}
