using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XEngine {
    class ServiceLocator {

        static private ServiceLocator m_instance;

        private GraphicsDevice m_graphics;

        private ContentManager m_content;

        private ICamera m_camera;

        private IInputManager m_inputManager;

        private ServiceLocator() { }

        static public void Initialize( Game game ) {
            m_instance = new ServiceLocator();
            m_instance.m_content = game.Content;
        }

        static public GraphicsDevice Graphics {
            get { return m_instance.m_graphics; }
            set { m_instance.m_graphics = value; }
        }

        static public ContentManager Content {
            get { return m_instance.m_content; }
        }

        static public ICamera Camera {
            get { return m_instance.m_camera; }
            set { m_instance.m_camera = value; }
        }

        static public IInputManager InputManager {
            get { return m_instance.m_inputManager; }
            set { m_instance.m_inputManager = value; }
        }
    }

}

