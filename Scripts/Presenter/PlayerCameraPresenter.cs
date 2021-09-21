using System;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.View;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace kameffee.unity1week202109
{
    public sealed class PlayerCameraPresenter : IInitializable, IDisposable
    {
        private readonly PlayerModel playerModel;
        private readonly IPlayerCamera playerCamera;
        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public PlayerCameraPresenter(PlayerModel playerModel, IPlayerCamera playerCamera)
        {
            this.playerModel = playerModel;
            this.playerCamera = playerCamera;
        }

        public void Initialize()
        {
            playerModel.Velocity.Subscribe(speed => playerCamera.ZoomOut(Mathf.Max(1f, speed.magnitude / 10f)));
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}