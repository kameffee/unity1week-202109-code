using Cinemachine;
using DG.Tweening;
using UnityEngine;
using VContainer.Unity;

namespace kameffee.unity1week202109.View
{
    public sealed class PlayerCamera : MonoBehaviour, IInitializable, IPlayerCamera
    {
        [SerializeField]
        private CinemachineVirtualCamera targetCamera;

        [SerializeField]
        private float maxSize = 11f;

        [SerializeField]
        private float YOffsetMax = -1.5f;

        // ベース画角
        private float baseSize;

        private CinemachineFramingTransposer transposer;

        public void Initialize()
        {
            transposer = targetCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            baseSize = transposer.m_CameraDistance;
        }

        public void SetFollowPlayer(Transform target)
        {
            targetCamera.Follow = target;
        }

        public void ZoomOut(float value)
        {
            DOVirtual.Float(transposer.m_CameraDistance,
                Mathf.Clamp(maxSize, baseSize, baseSize * value),
                1f,
                f => transposer.m_CameraDistance = f);
        }

        public void ZoomReset()
        {
            DOVirtual.Float(transposer.m_CameraDistance,
                baseSize,
                3f,
                f => transposer.m_CameraDistance = f);
        }
    }
}