using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public interface IScrollBackground
    {
        void AddScroll(Vector2 scroll);
    }
    
    
    public class ScrollBackground : MonoBehaviour, IScrollBackground
    {
        [SerializeField]
        private Vector2 speed = Vector2.one;

        private Renderer renderer;

        MaterialPropertyBlock materialPropertyBlock;
        private static readonly int Offset = Shader.PropertyToID("_Offset");

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
        }

        public void AddScroll(Vector2 scroll)
        {
            var vector2 = scroll * speed;
            renderer.GetPropertyBlock(materialPropertyBlock);
            var offset = materialPropertyBlock.GetVector(Offset);
            offset.x += vector2.x;
            offset.y += vector2.y;
            materialPropertyBlock.SetVector(Offset, offset);
            renderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}