using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XEngine {
    interface IInputManager {

        bool isKeyPressed( Keys key );

        bool isKeyDown( Keys key );

        bool isMouseLeftDown();

        bool isMouseLeftPressed();

        bool isMouseRightDown();

        bool isMouseRightPressed();

        Vector2 getMouseMove();

        int getMouseScroll();
    }
}
