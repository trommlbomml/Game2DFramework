using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2DFramework
{
    public interface Behavior
    {
        void Update(float elapsedTime, Game2D game);
    }

    public abstract class ConfigurableBehavior<TConfigurationParameter> : Behavior
    {
        protected abstract void Configure(TConfigurationParameter parameter);
        public abstract void Update(float elapsedTime, Game2D game);
    }
}
