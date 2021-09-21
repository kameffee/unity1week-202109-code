using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.View;
using UniRx;
using VContainer.Unity;

namespace kameffee.unity1week202109.Presenter
{
    public class OutroPlayerPresenter : IInitializable
    {
        private readonly PlayerModel playerModel;
        private readonly Player view;
        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public OutroPlayerPresenter(PlayerModel playerModel, Player view)
        {
            this.playerModel = playerModel;
            this.view = view;
        }
        
        public void Initialize()
        {
            // 初期化
            playerModel.SetMoveSpeed(view.MoveSpeed);

            // スピード変化
            playerModel.MoveSpeed.Subscribe(speed => view.SetMoveSpeed(speed)).AddTo(disposable);
            // 宙返り成功
            playerModel.OnSuccessSpin.Subscribe(count => view.OnSpeedUp(count)).AddTo(disposable);
            // 自動移動
            playerModel.IsActive.Subscribe(autoMove => view.SetAutoMove(autoMove)).AddTo(disposable);

            view.Speed.Subscribe(speed => playerModel.UpdateSpeed(speed)).AddTo(disposable);
            view.IsGround.Subscribe(isGround => playerModel.SetIsGround(isGround)).AddTo(disposable);
        }
    }
}