using System;
using System.Collections.Generic;
using System.Linq;

namespace Game2DFramework.Animations
{
    /// <summary>
    /// Animation done from 0 to 1 in a specific time.
    /// </summary>
    public class DeltaAnimation : Animation
    {
        private readonly Action<float> _onUpdate;

        public DeltaAnimation(float durationSeconds, Action<float> onUpdate, bool loops) : base(durationSeconds, loops)
        {
            if (onUpdate == null) throw new ArgumentNullException(nameof(onUpdate));
            _onUpdate = onUpdate;
        }

        protected override void OnUpdate()
        {
            _onUpdate(Delta);
        }
    }
}