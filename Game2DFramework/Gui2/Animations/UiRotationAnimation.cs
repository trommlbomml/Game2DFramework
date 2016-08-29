using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Gui2.Animations
{
    public class UiRotationAnimation : UiAnimation
    {
        private readonly float _startRotation;
        private readonly float _endRotation;

        public UiRotationAnimation(float durationSeconds, float startRotation, float endRotation) : base(durationSeconds)
        {
            _startRotation = startRotation;
            _endRotation = endRotation;
        }

        protected override void OnUpdate()
        {
            Owner.Rotation = MathHelper.Lerp(_startRotation, _endRotation, Delta);
        }
    }
}
