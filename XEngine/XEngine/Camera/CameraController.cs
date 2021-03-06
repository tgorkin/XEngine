﻿using System;
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

        private static readonly Vector3 INITIAL_POSITION = new Vector3( 0, 2.0f, 4.0f );

        private static readonly Vector3 INITIAL_LOOK_DIR = new Vector3( 0 );

        protected ICamera m_camera;

        protected IInputManager m_inputManager;

        public CameraController(Game game)
            : base(game) {
        }

        public override void Initialize() {
            m_camera = ServiceLocator.Camera;
            m_inputManager = ServiceLocator.InputManager;
            SetupInitialCamera();
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

        virtual protected void SetupInitialCamera() {
            m_camera.Position = INITIAL_POSITION;
            m_camera.LookAt = INITIAL_LOOK_DIR;
        }
        
        virtual protected Vector3 getMoveDirection() {
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

        virtual protected Quaternion GetRotation() {
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

        protected float convertPixelsToRadians(float pixelDelta, int maxPixels) {
            // approximation
            return pixelDelta / maxPixels;
        }

        private float getRotationSpeed() {
            return CAMERA_ROTATION_SPEED;
        }

        static public CameraController Factory( CameraType type, Game game ) {
            CameraController cameraController = null;
            switch ( type ) {
                case CameraType.CAMERA_TYPE_FIRST_PERSON:
                    cameraController = new FirstPersonCameraController( game );
                    break;
                case CameraType.CAMERA_TYPE_TOP_DOWN:
                    cameraController = new TopDownCameraController( game );
                    break;
                case CameraType.CAMERA_TYPE_FREE:
                default:
                    cameraController = new CameraController( game );
                    break;
            }
            return cameraController;
        }

        static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest( false );

            // Initialize Camera
            Camera camera = new Camera( testGame );
            testGame.Components.Add( camera );
            ServiceLocator.Camera = camera;

            // Initialize InputManager
            InputManager inputManager = new InputManager( testGame );
            testGame.Components.Add( inputManager );
            ServiceLocator.InputManager = inputManager;

            // Initialize DebugHUD
            testGame.Components.Add( new DebugHUD( testGame ) );

            testGame.Components.Add( new CameraController( testGame ) );

            testGame.Run();
        }
    }
}
