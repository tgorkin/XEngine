using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XEngine {
    class CameraController : GameComponent {

        private static readonly float CAMERA_MOVE_SPEED = 0.01f;

        private static readonly float CAMERA_ROTATION_SPEED = 0.1f;

        Camera m_camera;

        InputManager m_inputManager;

        public CameraController(XEngineGame game)
            : base(game) {
        }

        public override void Initialize() {
            m_camera = Camera.Instance;
            m_inputManager = InputManager.Instance;
        }

        public override void Update(GameTime gameTime) {
            MoveCamera(gameTime);
            RotateCamera(gameTime);
        }

        private void MoveCamera(GameTime gameTime) {
            Vector3 moveDirection = getMoveDirection();
            if (moveDirection.LengthSquared() > 0) {
                moveDirection = moveDirection * gameTime.ElapsedGameTime.Milliseconds * getMoveSpeed();
                m_camera.Position += moveDirection;
                m_camera.LookAt += moveDirection;
            }
        }
        
        private Vector3 getMoveDirection() {
            Vector3 moveDirection = new Vector3();

            // check for input from WASD keys
            if (InputManager.Instance.isKeyDown(Keys.A)) {
                moveDirection += -1 * Vector3.Cross(m_camera.LookDirection, m_camera.Up);
            }
            if (InputManager.Instance.isKeyDown(Keys.D)) {
                moveDirection += Vector3.Cross(m_camera.LookDirection, m_camera.Up);
            }
            if (InputManager.Instance.isKeyDown(Keys.S)) {
                moveDirection += -m_camera.LookDirection;
            }
            if (InputManager.Instance.isKeyDown(Keys.W)) {
                moveDirection += m_camera.LookDirection;
            }

            // zero out any movement in the y-direction so the camera doesn't fly and normalize
            moveDirection.Y = 0;
            moveDirection.Normalize();
            return moveDirection;
        }

        private float getMoveSpeed() {
            return CAMERA_MOVE_SPEED;
        }

        private void RotateCamera(GameTime gameTime) {
            Quaternion rotation = GetRotation();
            if (rotation.LengthSquared() > 0) {
                float rotationScaleFactor = gameTime.ElapsedGameTime.Milliseconds * getRotationSpeed();
                rotation = Quaternion.Multiply(rotation, rotationScaleFactor);
                Vector3 newLook = Vector3.Transform(m_camera.LookDirection, rotation);
                m_camera.LookAt = m_camera.Position + newLook;
            }
        }

        private Quaternion GetRotation() {
            Quaternion rotation = new Quaternion();
            if (InputManager.Instance.isMouseRightDown()) {
                Vector2 mouseMovement = InputManager.Instance.getMouseMove();
                if (Math.Abs(mouseMovement.X) > 0 || Math.Abs(mouseMovement.Y) > 0) {
                    // convert mouse pixel movement to yaw/pitch in radians
                    float yaw = convertPixelsToRadians(mouseMovement.X, Game.GraphicsDevice.Viewport.Width);
                    float pitch = convertPixelsToRadians(mouseMovement.Y, Game.GraphicsDevice.Viewport.Height);
                    // convert to quaternion
                    rotation = Quaternion.CreateFromYawPitchRoll(-yaw, -pitch, 0);
                }
            }
            return rotation;
        }

        private float convertPixelsToRadians(float pixelDelta, int maxPixels) {
            // approximation
            return pixelDelta / maxPixels;
        }

        private float getRotationSpeed() {
            return CAMERA_ROTATION_SPEED;
        }

        static public void TestCameraController() {
            
            XEngineComponentTest.TestGame.Components.Add( new InputManager(XEngineComponentTest.TestGame) );
            XEngineComponentTest.TestGame.Components.Add( new CameraController(XEngineComponentTest.TestGame) );
            XEngineComponentTest.TestGame.Components.Add( new Origin(XEngineComponentTest.TestGame) );
            XEngineComponentTest.TestGame.Components.Add( new DebugHUD(XEngineComponentTest.TestGame) );
            XEngineComponentTest.StartTest();
        }
    }
}
