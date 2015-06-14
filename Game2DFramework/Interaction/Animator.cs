using System;
using System.Collections.Generic;

namespace Game2DFramework.Interaction
{
    public class Animator
    {
        private string _currentAnimationId;
        private readonly Dictionary<string, Animation> _animations = new Dictionary<string, Animation>();
        private readonly Dictionary<string, string> _transitions = new Dictionary<string, string>(); 

        public Animation CurrentAnimation { get; private set; }

        public event Action AnimationFinished;
        public event Action<string> AnimationClipStarted;
        public event Action<string> AnimationClipFinished;

        public void AddAnimation(string id, Animation animation)
        {
            _animations.Add(id, animation);
        }

        public void PlayAnimation(string id)
        {
            CurrentAnimation = _animations[id];
            _currentAnimationId = id;
            CurrentAnimation.Start();
            if (AnimationClipStarted != null) AnimationClipStarted(_currentAnimationId);
        }

        public void AddTransition(string sourceId, string targetId)
        {
            _transitions.Add(sourceId, targetId);
        }

        public void Update(float elapsedTime)
        {
            if (CurrentAnimation == null) return;

            CurrentAnimation.Update(elapsedTime);
            if (CurrentAnimation.Delta >= 1.0f)
            {
                if (AnimationClipFinished != null) AnimationClipFinished(_currentAnimationId);
                
                string nextAnimation;
                if (_transitions.TryGetValue(_currentAnimationId, out nextAnimation))
                {
                    PlayAnimation(nextAnimation);
                }
                else
                {
                    CurrentAnimation = null;
                    _currentAnimationId = null;
                    if (AnimationFinished != null) AnimationFinished();
                }
            }
        }
    }
}
