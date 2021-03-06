﻿using Microsoft.Xna.Framework.Input;

namespace Game2DFramework.Input
{
    public class MouseEx
    {
        private MouseState _lastMouseState;
        private MouseState _currentMouseState;
        private float _elapsedLeftButtonDownTime;

        public float X { get; private set; }
        public float Y { get; private set; }

        public MouseEx()
        {
            _currentMouseState = Mouse.GetState();
        }

        public void Update(float elapsedTime)
        {
            _elapsedLeftButtonDownTime += elapsedTime;
            _lastMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            X = _currentMouseState.X;
            Y = _currentMouseState.Y;
        }

        public bool IsLeftButtonClicked()
        {
            return _currentMouseState.LeftButton == ButtonState.Released && _lastMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsLeftButtonDown()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsLeftButtonDownOnce()
        {
            return IsLeftButtonDown() && _lastMouseState.LeftButton == ButtonState.Released;
        }

        public bool IsRightButtonDown()
        {
            return _currentMouseState.RightButton == ButtonState.Pressed;
        }

        public bool IsRightButtonDownOnce()
        {
            return IsRightButtonDown() && _lastMouseState.RightButton == ButtonState.Released;
        }

        public bool MouseMoved()
        {
            return _lastMouseState.X != _currentMouseState.X ||
                   _lastMouseState.Y != _currentMouseState.Y;
        }

        public bool IsLeftMouseDownFreq(float shootFrequency)
        {
            if (IsLeftButtonDown() && _elapsedLeftButtonDownTime >= shootFrequency)
            {
                _elapsedLeftButtonDownTime = 0;
                return true;
            }

            return false;
        }
    }
}
