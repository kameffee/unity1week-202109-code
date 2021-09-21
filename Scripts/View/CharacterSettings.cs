using UnityEngine;

namespace kameffee.unity1week202109.View
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "PlayerSettings")]
    public class CharacterSettings : ScriptableObject
    {
        [SerializeField]
        private float moveSpeed = 15;

        [SerializeField]
        private float maxBootSpeed = 10;

        [SerializeField]
        private float rotateSpeed = 1;

        [SerializeField]
        private float jumpPower = 12;

        public float MoveSpeed => moveSpeed;

        public float MaxBootSpeed => maxBootSpeed;

        public float RotateSpeed => rotateSpeed;

        public float JumpPower => jumpPower;
    }
}