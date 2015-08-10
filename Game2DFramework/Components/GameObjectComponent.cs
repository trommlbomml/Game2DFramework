
namespace Game2DFramework.Components
{
    public abstract class GameObjectComponent
    {
        protected GameObject GameObject;

        protected GameObjectComponent(GameObject gameObject)
        {
            GameObject = gameObject;
            IsActive = true;
        }

        public abstract void Update(float elapsedTime);

        public bool IsActive { get; set; }

        public virtual void Draw()
        {
        }
    }
}
