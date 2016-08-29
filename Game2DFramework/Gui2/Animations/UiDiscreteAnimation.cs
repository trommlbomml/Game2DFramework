using System;

namespace Game2DFramework.Gui2.Animations
{
    public class UiDiscreteAnimation<T> : UiAnimation
    {
        private readonly Action<T> _onUpdate;
        private readonly T[] _animationSteps;
        private readonly float _stepTime;

        public UiDiscreteAnimation(float durationSeconds, T[] animationSteps, Action<T> onUpdate, bool loops)
            : base(durationSeconds, loops)
        {
            _onUpdate = onUpdate;
            _animationSteps = animationSteps;
            _stepTime = durationSeconds / animationSteps.Length;
        }

        protected override void OnUpdate()
        {
            var currentAnimationStep = (int)(ElapsedSeconds / _stepTime);
            if (currentAnimationStep >= _animationSteps.Length) currentAnimationStep = 0;
            _onUpdate?.Invoke(_animationSteps[currentAnimationStep]);
        }
    }
}
