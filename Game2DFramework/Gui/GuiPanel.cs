using System.Collections.Generic;

namespace Game2DFramework.Gui
{
    public class GuiPanel : GameObject
    {
        private readonly List<GuiElement> _elements = new List<GuiElement>();

        private GuiElement _currentMouseOverElement;
        private GuiElement _focusedElement;
        
        public GuiPanel(Game2D game) : base(game)
        {
        }

        public void AddElement(GuiElement element)
        {
            _elements.Add(element);
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);

            foreach (var element in _elements)
            {
                UpdateElementUiEvents(element);
                element.Update(elapsedTime);
            }

            if (Game.Mouse.IsLeftButtonClicked() && _currentMouseOverElement != null)
            {
                _currentMouseOverElement.OnClick();
                _currentMouseOverElement.OnGotFocus();
                if (_currentMouseOverElement != _focusedElement)
                {
                    if (_focusedElement != null) _focusedElement.OnFocusLost();
                    _focusedElement = _currentMouseOverElement;
                }
            }
        }

        private void UpdateElementUiEvents(GuiElement element)
        {
            var x = Game.Mouse.X;
            var y = Game.Mouse.Y;

            HandleMouseOver(element, x, y);
            HandleFocus(element, x, y);
        }

        private void HandleFocus(GuiElement element, float x, float y)
        {
            if (!Game.Mouse.IsLeftButtonClicked()) return;

            GuiElement newFocusedElement = null;
            if (element.Bounds.Contains(x, y))
            {
                newFocusedElement = element.OnGotFocus();
            }

            if (newFocusedElement != _focusedElement)
            {
                if (_focusedElement != null) _focusedElement.OnFocusLost();
                _currentMouseOverElement = newFocusedElement;
            }
        }

        private void HandleMouseOver(GuiElement element, float x, float y)
        {
            GuiElement newMouseOverElement = null;
            if (element.Bounds.Contains(x, y))
            {
                newMouseOverElement = element.OnMouseOver();
            }

            if (newMouseOverElement != _currentMouseOverElement)
            {
                if (_currentMouseOverElement != null) _currentMouseOverElement.OnMouseLeft();
                _currentMouseOverElement = newMouseOverElement;
            }
        }

        public override void Draw()
        {
            base.Draw();
            _elements.ForEach(e => e.Draw());
        }
    }
}
