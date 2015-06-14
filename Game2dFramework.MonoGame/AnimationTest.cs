using Game2DFramework.Drawing;
using Game2DFramework.Interaction;
using Game2DFramework.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.MonoGame
{
    class AnimationTest : InitializableState
    {
        private Animator _iconAnimator;
        private Sprite _iconSprite;

        protected override void OnEntered(object enterInformation)
        {
            _iconAnimator.PlayAnimation("BlendIn");
        }

        protected override void OnInitialize(object enterInformation)
        {
            _iconAnimator = new Animator();
            _iconAnimator.AddAnimation("BlendIn", new Animation(1.0f, AnimateBlendIn));
            _iconSprite = new Sprite(Game.Content.Load<Texture2D>("Textures/MonoGameLogo"))
            {
                Position = new Vector2(Game.ScreenWidth*0.5f, Game.ScreenHeight*0.5f),
                Alpha = 0.0f
            };
        }

        private void AnimateBlendIn(float delta)
        {
            _iconSprite.Rotation = MathHelper.SmoothStep(0.0f, 1.0f, delta) * MathHelper.TwoPi;
            _iconSprite.SetScale(MathHelper.SmoothStep(0.0f, 1.0f, delta));
            _iconSprite.Alpha = delta;
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
            _iconSprite.Draw(Game.SpriteBatch);
        }
    }
}
