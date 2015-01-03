
namespace Game2DFramework.Components
{
    public abstract class GameObjectComponent
    {
        private GameObject _gameObject;

        protected GameObjectComponent(GameObject gameObject)
        {
            _gameObject = gameObject;
            IsActive = true;
        }

        public abstract void Update(float elapsedTime);

        public bool IsActive { get; set; }
    }
}
