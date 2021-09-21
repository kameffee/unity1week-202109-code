using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public sealed class StartPoint : MonoBehaviour, IStartPoint
    {
        public Vector3 Position => transform.position;
    }
}