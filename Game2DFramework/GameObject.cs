
using System.Collections.Generic;
using System.Linq;
using Game2DFramework.Components;

namespace Game2DFramework
{
    public abstract class GameObject
    {
        private readonly List<GameObjectComponent> _components; 

        protected GameObject(Game2D game)
        {
            Game = game;
            _components = new List<GameObjectComponent>();
        }

        public Game2D Game { get; private set; }

        public virtual void Update(float elapsedTime)
        {
            _components.ForEach(c => c.Update(elapsedTime));
        }

        public virtual void Draw()
        {
            _components.ForEach(c => c.Draw());
        }

        public virtual void OnCollide(GameObject other)
        {
            
        }

        public virtual void ApplyDamage(int amount)
        {
            
        }

        public void AddComponent<TComponent>(TComponent component) where TComponent : GameObjectComponent
        {
            _components.Add(component);
        }

        public TComponent GetComponent<TComponent>() where TComponent : GameObjectComponent
        {
            return _components.OfType<TComponent>().FirstOrDefault();
        }
    }
}
