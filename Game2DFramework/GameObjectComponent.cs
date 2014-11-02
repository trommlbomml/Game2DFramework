using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2DFramework
{
    public interface GameObjectComponent
    {
        void Update(float elapsedTime, Game2D game);
    }
}
