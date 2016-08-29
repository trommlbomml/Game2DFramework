﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game2DFramework.Extensions;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Animations
{
    /// <summary>
    /// Animator-class for handling animations and animation series.
    /// </summary>
    public class Animator
    {
        private readonly Dictionary<string, Animation> _animations;
        private readonly Dictionary<string, string> _transitions;
        private string _currentAnimationName;

        /// <summary>
        /// Gets called when an animation has finished.
        /// </summary>
        public event Action<string,bool> AnimationFinished;

        /// <summary>
        /// Gets called when an animation has started.
        /// </summary>
        public event Action<string> AnimationStarted;

        /// <summary>
        /// Gets called when the last animation of animator has finished.
        /// </summary>
        public event Action AnimatorFinished;

        /// <summary>
        /// Currently active animation.
        /// </summary>
        public Animation CurrentAnimation { get; private set; }

        /// <summary>
        /// Creates Animator.
        /// </summary>
        public Animator()
        {
            _animations = new Dictionary<string, Animation>();
            _transitions = new Dictionary<string, string>();
        }

        /// <summary>
        /// Sets current animation and starts it.
        /// </summary>
        /// <param name="name">Name of animation</param>
        /// <param name="playReversed">Plays the animation backwards.</param>
        public void SetAnimation(string name, bool playReversed = false)
        {
            if (_currentAnimationName == name && CurrentAnimation != null)
            {
                if (CurrentAnimation.PlayReversed == playReversed) return;
                CurrentAnimation.Start(playReversed);
            }
            else
            {
                _currentAnimationName = name;
                CurrentAnimation = _animations[name];
                CurrentAnimation.Start(playReversed);
                AnimationStarted?.Invoke(name);
            }
            
        }

        /// <summary>
        /// Registeres new animation.
        /// </summary>
        /// <param name="name">name of animation</param>
        /// <param name="animation">Object</param>
        public void AddAnimation(string name, Animation animation)
        {
            _animations.Add(name, animation);
        }

        /// <summary>
        /// Removes an Animation by name.
        /// </summary>
        /// <param name="sourceName"></param>
        public void RemoveAnimation(string sourceName)
        {
            _animations.Remove(sourceName);
        }

        /// <summary>
        /// Registers transition between animation
        /// </summary>
        /// <param name="sourceName">Source Animation Name</param>
        /// <param name="targetName">Target Animation Name</param>
        public void AddTransition(string sourceName, string targetName)
        {
            _transitions.Add(sourceName, targetName);
        }

        /// <summary>
        /// Adds a list of Animations to play in a list.
        /// </summary>
        /// <param name="sourceName">Source Animation Name</param>
        /// <param name="targetName">Target Animation Name</param>
        /// <param name="additionalTargets">Following targets</param>
        public void AddTransitionChain(string sourceName, string targetName, params string[] additionalTargets)
        {
            _transitions.Add(sourceName, targetName);

            if (additionalTargets != null)
            {
                var currentTarget = targetName;
                foreach (var newTarget in additionalTargets)
                {
                    _transitions.Add(currentTarget, newTarget);
                    currentTarget = newTarget;
                }
            }
        }

        public void Stop()
        {
            CurrentAnimation = null;
        }

        /// <summary>
        /// Updates Animator.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (CurrentAnimation == null) return;

            CurrentAnimation.Update(gameTime.GetSeconds());
            if (CurrentAnimation.IsFinished)
            {
                if (CurrentAnimation.IsLoop)
                {
                    CurrentAnimation.Start();
                }
                else
                {
                    AnimationFinished?.Invoke(_currentAnimationName, CurrentAnimation.PlayReversed);

                    string newAnimation;
                    if (_transitions.TryGetValue(_currentAnimationName, out newAnimation))
                    {
                        SetAnimation(newAnimation);
                    }
                    else
                    {
                        CurrentAnimation = null;
                        AnimatorFinished?.Invoke();
                    }
                }
            }
        }
    }
}
