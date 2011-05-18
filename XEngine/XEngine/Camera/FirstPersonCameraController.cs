using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XEngine {
    class FirstPersonCameraController : CameraController {

        private static readonly Vector3 INITIAL_POSITION = new Vector3( 0, 2.0f, 4.0f );

        private static readonly Vector3 INITIAL_LOOK_DIR = new Vector3( 0, 0, -1.0f );

        public FirstPersonCameraController( XEngineGame game )
            : base(game) {
        }

        override protected void SetupInitialCamera() {
            m_camera.Position = INITIAL_POSITION;
            m_camera.LookAt = INITIAL_POSITION + INITIAL_LOOK_DIR;
        }

        override protected Quaternion GetRotation() {
            Quaternion rotation = new Quaternion();
            Vector2 mouseMovement = m_inputManager.getMouseMove();
            if ( Math.Abs( mouseMovement.X ) > 0 || Math.Abs( mouseMovement.Y ) > 0 ) {
                // convert mouse pixel movement to yaw/pitch in radians
                float yaw = convertPixelsToRadians( mouseMovement.X, Game.GraphicsDevice.Viewport.Width );
                float pitch = convertPixelsToRadians( mouseMovement.Y, Game.GraphicsDevice.Viewport.Height );

                Quaternion qYaw = Quaternion.CreateFromAxisAngle( m_camera.Up, -yaw );
                Quaternion qPitch = Quaternion.CreateFromAxisAngle( m_camera.Right, -pitch );
                rotation = qYaw * qPitch;
            }
            return rotation;
        }

        new static public void ComponentTest() {
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

            testGame.Components.Add( new FirstPersonCameraController( testGame ) );

            testGame.Run();
        }
    }
}
