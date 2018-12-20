using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Renderable
{
    public class AbstractRenderer : MonoBehaviour
    {

        private IRenderable _renderable;
        private SpriteRenderer _spriteRenderer;

        public void Render()
        {
            if (_renderable == null) return;
            transform.position = _renderable.Position;
            _spriteRenderer.sprite = _renderable.Sprite;
        }

        private void Update()
        {
            Render();

        }

        public void Initialize(IRenderable renderable)
        {
            _renderable = renderable;
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

            Render();
        }

        protected void SetToPosition()
        {
            transform.position = _renderable.Position;
        }

        protected void SetLayer(string layerName)
        {
            _spriteRenderer.sortingLayerName = layerName;
        }


    }
}
