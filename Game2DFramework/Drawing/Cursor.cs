using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Drawing
{
    public class Cursor : GameObject
    {
        private readonly Dictionary<string, CursorType> _cursorTypes;
        private CursorType _currentCursor;
        private Vector2 _currentPosition;

        public bool IsActive { get; set; }

        public Cursor(Game2D game) : base(game)
        {
            _cursorTypes = new Dictionary<string, CursorType>();
            IsActive = true;
        }

        public void AddCursorType(string name, CursorType cursorType)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Invalid Name", "name");
            if (_cursorTypes.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("Cursor already registered: {0}", name), "name");
            }

            _cursorTypes.Add(name, cursorType);

            if (_cursorTypes.Count == 1)
            {
                _currentCursor = cursorType;
            }
        }

        public void SetCursorType(string name)
        {
            if (!_cursorTypes.ContainsKey(name)) throw new ArgumentException(string.Format("Unknown cursor type: {0}", name), "name");

            _currentCursor = _cursorTypes[name];
        }

        internal void Update()
        {
            if (!IsActive || _currentCursor == null) return;
            _currentPosition = new Vector2(Game.Mouse.X, Game.Mouse.Y);
        }

        internal void Draw()
        {
            if (!IsActive || _currentCursor == null) return;
            _currentCursor.Draw(Game.SpriteBatch, _currentPosition);
        }
    }
}
