using System;
using System.Collections.Generic;
using System.Linq;
using kameffee.unity1week202109.View;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace kameffee.unity1week202109.Presenter
{
    /// <summary>
    /// 使い回す地面.
    /// </summary>
    public sealed class RecycleGroundPresenter : IInitializable, IDisposable
    {
        private readonly IReadOnlyList<IOutroGround> grounds;
        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public RecycleGroundPresenter(IReadOnlyList<IOutroGround> grounds)
        {
            this.grounds = grounds;
        }

        public void Initialize()
        {
            foreach (var ground in grounds)
            {
                ground.OnEnterPlayer
                    .Subscribe(_ =>
                    {
                        Move();
                    })
                    .AddTo(disposable);
            }
        }
        
        /// <summary>
        /// 一番奥へ移動させる
        /// </summary>
        public void Move()
        {
            // 一番後ろの地面を持ってくる
            var last = grounds.OrderBy(ground => ground.Position.x).First(); 
            float toPositionX = last.Position.x + grounds.Sum(ground => ground.Size.x);
            last.SetPosition(new Vector2(toPositionX, 0));
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}