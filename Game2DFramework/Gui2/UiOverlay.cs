using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui2
{
    public class UiOverlay : UiFocusContainer
    {
        private UiCompoundElement _currentModalElement;
        private readonly List<UiElement> _uiElements;
        private bool _waitForShowed;
        private bool _waitForHidden;

        /// <summary>
        /// Is called after show() and all elements have state active.
        /// </summary>
        public event Action Showed;

        /// <summary>
        /// Is called after show() and all elements have state inactive.
        /// </summary>
        public event Action Hidden;

        public UiOverlay()
        {
            _uiElements = new List<UiElement>();
        }

        public TElement AddElement<TElement>(TElement element) where TElement : UiElement
        {
            _uiElements.Add(element);
            AddUiElement(element);
            return element;
        }

        public void Center(Rectangle bounds, bool vertical, bool horizontal)
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;

            foreach(var element in _uiElements)
            {
                minX = Math.Min(minX, element.Bounds.Left);
                maxX = Math.Max(maxX, element.Bounds.Right);
                minY = Math.Min(minY, element.Bounds.Top);
                maxY = Math.Max(maxY, element.Bounds.Bottom);
            }

            var offsetX = horizontal ? bounds.X + bounds.Width / 2 - (maxX - minX) / 2 - minX : 0;
            var offsetY = vertical ? bounds.Y + bounds.Height / 2 - (maxY - minY) / 2 - minY : 0;

            foreach (var element in _uiElements)
            {
                element.Translate(offsetX, offsetY);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (var i = 0; i < _uiElements.Count; i++)
            {
                _uiElements[i].Update(gameTime);
            }
            _currentModalElement?.Update(gameTime);

            if (_waitForShowed && _uiElements.All(e => e.State == UiState.Active))
            {
                _waitForShowed = false;
                Showed?.Invoke();
            }

            if (_waitForHidden && _uiElements.All(e => e.State == UiState.Inactive))
            {
                _waitForHidden = false;
                Hidden?.Invoke();
            }
        }

        public void ShowModal(UiCompoundElement modalElement)
        {
            _currentModalElement = modalElement;
            SetUiElements(_currentModalElement.Children, true);
            FocusFirstElement();
            _currentModalElement.Show();
        }

        public void CloseModal()
        {
            _currentModalElement?.Hide();
            SetUiElements(_uiElements, false);
            FocusFirstElement();
        }

        public void Show()
        {
            _uiElements.ForEach(e => e.Show());
            _waitForHidden = false;
            _waitForShowed = true;
        }

        public void Hide()
        {
            _uiElements.ForEach(e => e.Hide());
            _waitForHidden = true;
            _waitForShowed = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < _uiElements.Count; i++)
            {
                var uiBaseElement = _uiElements[i];
                if (uiBaseElement.State != UiState.Inactive) uiBaseElement.Draw(spriteBatch);
            }

            if (_currentModalElement != null && _currentModalElement.State != UiState.Inactive) _currentModalElement?.Draw(spriteBatch);
        }
    }
}
