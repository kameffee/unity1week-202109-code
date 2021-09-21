using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public interface IOutroGround
    {
        IObservable<Unit> OnEnterPlayer { get; }
        
        Vector2 Size { get; }

        Vector3 Position { get; }

        void SetPosition(Vector2 position);
    }

    /// <summary>
    /// エンディングで使う地面
    /// </summary>
    public class OutroGround : MonoBehaviour, IOutroGround
    {
        [SerializeField]
        private Vector2 size;

        public Vector2 Size => size;

        public Vector3 Position => transform.position;

        public IObservable<Unit> OnEnterPlayer => this.OnTriggerEnter2DAsObservable().AsUnitObservable();

        public void SetPosition(Vector2 position) => transform.position = position;
    }
}