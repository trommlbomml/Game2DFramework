﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Game2DFramework.Gui2
{
    /// <summary>
    /// Defining how to control the elements.
    /// </summary>
    public interface OverlayInputController
    {
        /// <summary>
        /// Called once per frame.
        /// </summary>
        void Update(UiFocusContainer container);

        /// <summary>
        /// This action should be called when an element should execute it's clicked action.
        /// </summary>
        event Action<UiElement> OnAction;

        /// <summary>
        /// Fire this event when to move to the next element by tab index.
        /// </summary>
        event Action MoveToNextElement;

        /// <summary>
        /// Fire this event when to move to the previous element by tab index.
        /// </summary>
        event Action MoveToPreviousElement;
    }
}
