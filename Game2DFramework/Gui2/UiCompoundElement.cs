using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui2
{
    /// <summary>
    /// UiElement which holds a list of elements. It should be used for complex UI Elements with multiple child elements.
    /// </summary>
    public abstract class UiCompoundElement : UiElement
    {
        private readonly List<UiElement> _children = new List<UiElement>();

        public ReadOnlyCollection<UiElement> Children { get; }

        protected UiCompoundElement()
        {
            Children = new ReadOnlyCollection<UiElement>(_children);
        }

        /// <summary>
        /// Adds a child control to compound element.
        /// </summary>
        /// <param name="element"></param>
        protected void AddChildElement(UiElement element)
        {
            _children.Add(element);
        }

        protected void RemoveChild(UiElement element)
        {
            _children.Remove(element);
        }

        protected void ClearChildren()
        {
            _children.Clear();
        }

        public override void Show()
        {
            State = UiState.Active;
            foreach (var uiElement in _children)
            {
                uiElement.Show();
            }   
        }

        public override void Hide()
        {
            State = UiState.Inactive;
            foreach (var uiElement in _children)
            {
                uiElement.Hide();
            }
        }

        public override void Focus()
        {
            foreach (var uiElement in _children)
            {
                uiElement.Hide();
            }
        }

        public override void Unfocus()
        {
            foreach (var uiElement in _children)
            {
                uiElement.Unfocus();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var uiElement in _children)
            {
                uiElement.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach (var uiElement in _children)
            {
                uiElement.Update(time);
            }
        }
    }
}