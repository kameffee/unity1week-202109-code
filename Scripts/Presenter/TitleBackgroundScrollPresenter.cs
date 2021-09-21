using System.Collections.Generic;
using kameffee.unity1week202109.View;
using UnityEngine;
using VContainer.Unity;

namespace kameffee.unity1week202109.Presenter
{
    public class TitleBackgroundScrollPresenter: ITickable
    {
        private readonly IReadOnlyList<IScrollBackground> scrollBackgrounds;

        public TitleBackgroundScrollPresenter(IReadOnlyList<IScrollBackground> scrollBackgrounds)
        {
            this.scrollBackgrounds = scrollBackgrounds;
        }

        public void Tick()
        {
            foreach (var scrollBackground in scrollBackgrounds)
            {
                scrollBackground.AddScroll(new Vector2(1f, 0f));
            }
        }
    }
}