using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2DFramework
{
    public class GameProperty
    {
        public const string GameResolutionXProperty = "ResolutionX";
        public const string GameResolutionYProperty = "ResolutionY";
        public const string GameIsFullScreenProperty = "IsFullScreen";

        internal string Name { get; private set; }
        internal string Value { get; set; }

        internal GameProperty(string name)
        {
            Name = name;
            Value = String.Empty;
        }
    }
}
