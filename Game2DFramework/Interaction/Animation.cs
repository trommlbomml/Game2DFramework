using System;

namespace Game2DFramework.Interaction
{
    public class Animation
    {
        private readonly float _animationTime;
        private readonly Action _finished;
        private readonly Action<float> _animate;
        
        public float AnimatedTime { get; private set; }

        public Animation(float animationTime, Action<float> animate, Action finished = null)
        {
            _animationTime = animationTime;
            _finished = finished;
            _animate = animate;
        }

        public float Delta 
        {
            get { return Math.Min(1.0f, AnimatedTime/_animationTime); }
        }

        public void Start()
        {
            AnimatedTime = 0;
        }

        public void Update(float elapsedTime)
        {
            AnimatedTime += elapsedTime;
            if (AnimatedTime >= _animationTime && _finished != null)
            {
                _finished();
            }
            else
            {
                _animate(Delta);
            }
        }
    }
}