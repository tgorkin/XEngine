using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {

    class TopDownCameraController : CameraController {

        private static readonly Vector3 INITIAL_POSITION = new Vector3( 0, 15.0f, 5.0f);

        private static readonly Vector3 INITIAL_LOOK_AT = new Vector3( 0, 0, 0 );

        public TopDownCameraController( Game game )
            : base(game) {
        }

        override protected void SetupInitialCamera() {
            m_camera.Position = INITIAL_POSITION;
            m_camera.LookAt = INITIAL_LOOK_AT;
        }

        new static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();
            testGame.CameraType = CameraType.CAMERA_TYPE_TOP_DOWN;

            Origin origin = null;
            testGame.InitDelegate = delegate {
                origin = new Origin();
                origin.Initialize();
            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                origin.Draw( gameTime );
            };
            testGame.Run();
        }
    }
}
