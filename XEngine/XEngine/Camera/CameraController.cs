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

        private static readonly float CAMERA_ROTATION_SPEED = 1.5f;

        ICamera m_camera;

        IInputManager m_inputManager;

        public CameraController(XEngineGame game)
            : base(game) {
        }

        public override void Initialize() {
            m_camera = (ICamera)Game.Services.GetService( typeof( ICamera ) );
            m_inputManager = (IInputManager)Game.Services.GetService( typeof( IInputManager ) );
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
            if ( m_inputManager.isKeyDown( Keys.A ) ) {
                moveDirection += -1 * Vector3.Cross(m_camera.LookDirection, m_camera.Up);
            }
            if ( m_inputManager.isKeyDown( Keys.D ) ) {
                moveDirection += Vector3.Cross(m_camera.LookDirection, m_camera.Up);
            }
            if ( m_inputManager.isKeyDown( Keys.S ) ) {
                moveDirection += -m_camera.LookDirection;
            }
            if ( m_inputManager.isKeyDown( Keys.W ) ) {
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
                rotation = Quaternion.Multiply( rotation, this.getRotationSpeed() );
                Vector3 lookDir = m_camera.LookDirection;
                Vector3 newLookDir = Vector3.Transform( lookDir, rotation );
                m_camera.LookAt = m_camera.Position + newLookDir;
            }
        }

        private Quaternion GetRotation() {
            Quaternion rotation = new Quaternion();
            if ( m_inputManager.isMouseRightDown() ) {
                Vector2 mouseMovement = m_inputManager.getMouseMove();
                if (Math.Abs(mouseMovement.X) > 0 || Math.Abs(mouseMovement.Y) > 0) {
                    // convert mouse pixel movement to yaw/pitch in radians
                    float yaw = convertPixelsToRadians(mouseMovement.X, Game.GraphicsDevice.Viewport.Width);
                    float pitch = convertPixelsToRadians(mouseMovement.Y, Game.GraphicsDevice.Viewport.Height);

                    Quaternion qYaw = Quaternion.CreateFromAxisAngle( m_camera.Up, -yaw );
                    Quaternion qPitch = Quaternion.CreateFromAxisAngle( m_camera.Right, -pitch );
                    rotation = qYaw * qPitch;
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
            XEngineComponentTest testGame = new XEngineComponentTest();
            testGame.BindGameComponent( new Camera( testGame ), typeof( ICamera ) );
            testGame.BindGameComponent( new InputManager( testGame ), typeof( IInputManager ) );

            testGame.Components.Add( new CameraController( testGame ) );
            testGame.Components.Add( new Origin( testGame ) );
            testGame.Components.Add( new DebugHUD( testGame ) );
            testGame.Run();
        }
    }
}
