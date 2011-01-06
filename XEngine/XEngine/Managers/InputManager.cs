using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XEngine {
    class InputManager : GameComponent, IInputManager {

        private KeyboardState m_lastKeyboardState;

        private KeyboardState m_currentKeyboardState;

        private MouseState m_lastMouseState;

        private MouseState m_currentMouseState;

        static readonly bool m_traceEnabled = false;

        public InputManager(XEngineGame game)
            : base(game) {

        }

        public override void Update(GameTime gameTime) {
            m_lastKeyboardState = m_currentKeyboardState;
            m_currentKeyboardState = Keyboard.GetState();
            m_lastMouseState = m_currentMouseState;
            m_currentMouseState = Mouse.GetState();
        }

        public bool isKeyPressed(Keys key) {
            bool isKeyPressed = m_currentKeyboardState.IsKeyDown(key) && m_lastKeyboardState.IsKeyUp(key);
            if (isKeyPressed && m_traceEnabled) {
                traceKeyInput(key);
            }
            return isKeyPressed;
        }

        public bool isKeyDown(Keys key) {
            bool isKeyDown = m_currentKeyboardState.IsKeyDown(key);
            if (isKeyDown && m_traceEnabled) {
                traceKeyInput(key);
            }
            return isKeyDown;
        }

        public bool isMouseLeftDown() {
            bool isButtonDown = m_currentMouseState.LeftButton == ButtonState.Pressed && m_lastMouseState.LeftButton != ButtonState.Pressed;
            if (isButtonDown && m_traceEnabled) {
                traceMouseInput(m_currentMouseState);
            }
            return isButtonDown;
        }

        public bool isMouseLeftPressed() {
            bool isButtonPressed =  m_currentMouseState.LeftButton == ButtonState.Pressed && m_lastMouseState.LeftButton != ButtonState.Pressed;
            if (isButtonPressed && m_traceEnabled) {
                traceMouseInput(m_currentMouseState);
            }
            return isButtonPressed;
        }

        public bool isMouseRightDown() {
            bool isButtonDown = m_currentMouseState.RightButton == ButtonState.Pressed;
            if (isButtonDown && m_traceEnabled) {
                traceMouseInput(m_currentMouseState);
            }
            return isButtonDown;
        }

        public bool isMouseRightPressed() {
            bool isButtonPressed = m_currentMouseState.RightButton == ButtonState.Pressed && m_lastMouseState.RightButton != ButtonState.Pressed;
            if (isButtonPressed && m_traceEnabled) {
                traceMouseInput(m_currentMouseState);
            }
            return isButtonPressed;
        }

        public Vector2 getMouseMove() {
            int x = m_currentMouseState.X - m_lastMouseState.X;
            int y = m_currentMouseState.Y - m_lastMouseState.Y;
            return new Vector2(x, y);
        }

        public int getMouseScroll() {
            return m_currentMouseState.ScrollWheelValue - m_lastMouseState.ScrollWheelValue;
        }

        private void traceKeyInput(Keys key) {
            Trace.WriteLine("Key Press Detected: " + key.ToString());
        }

        private void traceMouseInput(MouseState mouseState) {
            Trace.WriteLine("Mouse Input Detected: " + mouseState.ToString() );
        }


        static public void TestInputManager() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            testGame.BindGameComponent( new Camera( testGame ), typeof( ICamera ) );
            InputManager inputManager = new InputManager( testGame );
            testGame.BindGameComponent( inputManager, typeof( IInputManager ) );
            testGame.UpdateDelegate =
                delegate( GameTime gameTime ) {
                    inputManager.isKeyPressed( Keys.A );
                    inputManager.isMouseLeftPressed();
                    inputManager.isMouseRightPressed();
                };

            testGame.Run();
        }
    }
}
