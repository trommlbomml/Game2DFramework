using System;
using System.Runtime.InteropServices;
using Game2DFramework.Interaction;
using Game2DFramework.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.MonoGame
{
    class AnimationTest : InitializableState
    {
        private Animator _iconAnimator;
        private Texture2D _iconTexture2D;
        private float _rotation;
        private float _alpha;
        private Vector2 _position;

        protected override void OnEntered(object enterInformation)
        {
            _iconAnimator.PlayAnimation("BlendIn");
        }

        protected override void OnInitialize(object enterInformation)
        {
            _iconAnimator = new Animator();
            _iconAnimator.AddAnimation("BlendIn", new Animation(1.0f, AnimateBlendIn));

            _iconTexture2D = Game.Content.Load<Texture2D>("smilie");
            _rotation = 0.0f;
            _alpha = 0.0f;
        }

        private void AnimateBlendIn(float delta)
        {
            _rotation = delta * delta * MathHelper.TwoPi;
            _alpha = delta;
            _position = new Vector2(Game.ScreenWidth * 0.25f, Game.ScreenHeight * 0.8f) + new Vector2(Game.ScreenWidth * 0.25f, -Game.ScreenHeight * 0.3f) * delta;
        }

        public override void OnLeave()
        {
        }

        public override StateChangeInformation OnUpdate(float elapsedTime)
        {
            _iconAnimator.Update(elapsedTime);

            return StateChangeInformation.Empty;
        }

        public override void OnDraw(float elapsedTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            Game.SpriteBatch.Draw(_iconTexture2D, _position, null, Color.White * _alpha, _rotation, new Vector2(64, 64), 1.0f, SpriteEffects.None, 0);
        }
    }
}
