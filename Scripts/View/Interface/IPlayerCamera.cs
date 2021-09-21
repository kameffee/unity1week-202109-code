using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public interface IPlayerCamera
    {
        void SetFollowPlayer(Transform target);

        void ZoomOut(float value);

        void ZoomReset();
    }
}