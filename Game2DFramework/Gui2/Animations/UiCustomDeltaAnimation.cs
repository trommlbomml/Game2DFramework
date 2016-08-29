using System;
using System.Collections.Generic;
using System.Linq;

namespace Game2DFramework.Gui2.Animations
{
    public class UiCustomDeltaAnimation : UiAnimation
    {
        private readonly Action<UiElement, float> _onDeltaUpdate;

        public UiCustomDeltaAnimation(float durationSeconds, Action<UiElement, float> onDeltaUpdate) : base(durationSeconds)
        {
            if (onDeltaUpdate == null) throw new ArgumentNullException(nameof(onDeltaUpdate));
            _onDeltaUpdate = onDeltaUpdate;
        }

        protected override void OnUpdate()
        {
            _onDeltaUpdate(Owner, Delta);
        }
    }
}
