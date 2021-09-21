using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public class BackgroundView : MonoBehaviour
    {
        [SerializeField]
        private Camera camera;

        [SerializeField]
        private Vector2 ratio = Vector2.one;

        private Vector3 lastPos;

        private Renderer renderer;
        MaterialPropertyBlock materialPropertyBlock;
        private static readonly int Offset = Shader.PropertyToID("_Offset");

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            if (camera == null)
            {
                this.camera = Camera.main;
            }

            lastPos = this.camera.transform.position;
            materialPropertyBlock = new MaterialPropertyBlock();

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    var offset = this.camera.transform.position - lastPos;
                    AddScroll(offset * ratio);
                    lastPos = camera.transform.position;
                }).AddTo(this);
        }

        public void SetCamera(Camera camera)
        {
            this.camera = camera;
        }

        public void AddScroll(Vector2 vector2)
        {
            renderer.GetPropertyBlock(materialPropertyBlock);
            var offset = materialPropertyBlock.GetVector(Offset);
            offset.x += vector2.x;
            offset.y += vector2.y;
            materialPropertyBlock.SetVector(Offset, offset);
            renderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}