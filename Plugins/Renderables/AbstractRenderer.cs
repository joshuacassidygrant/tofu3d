using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Renderable
{
    public class AbstractRenderer : MonoBehaviour
    {

        private IRenderable _renderable;
        protected SpriteRenderer SpriteRenderer;

        public void Render()
        {
            if (_renderable == null) return;
            transform.position = _renderable.Position;
            SpriteRenderer.sprite = _renderable.Sprite;
        }

        private void Update()
        {
            Render();

        }

        public void Initialize(IRenderable renderable)
        {
            _renderable = renderable;
            SpriteRenderer = gameObject.AddComponent<SpriteRenderer>();

            Render();
        }

        protected void SetToPosition()
        {
            transform.position = _renderable.Position;
        }

        protected void SetLayer(string layerName)
        {
            SpriteRenderer.sortingLayerName = layerName;
        }


    }
}
