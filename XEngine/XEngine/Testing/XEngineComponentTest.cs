using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XEngine {

    // declare TestDelagate types
    delegate void InitDelegate();

    delegate void UpdateDelegate(GameTime gameTime);

    delegate void DrawDelegate(GameTime gameTime);

    class XEngineComponentTest : XEngineGame {
        
        private InitDelegate m_initDelegate;

        private UpdateDelegate m_updateDelegate;

        private DrawDelegate m_drawDelegate;

        private bool m_setupDefaultComponents = true;

        private CameraType m_cameraType = CameraType.CAMERA_TYPE_FREE;

        public XEngineComponentTest() : base() {
            this.IsMouseVisible = true;
        }

        public XEngineComponentTest(bool setupDefaultComponents)
            : this() {
           m_setupDefaultComponents = setupDefaultComponents;
        }

        public InitDelegate InitDelegate {
            set { m_initDelegate = value; }
        }

        public UpdateDelegate UpdateDelegate {
            set { m_updateDelegate = value; }
        }

        public DrawDelegate DrawDelegate {
            set { m_drawDelegate = value; }
        }

        public CameraType CameraType {
            set { m_cameraType = value; }
        }

        protected override void Initialize() {

            if ( m_setupDefaultComponents ) {

                // Initialize Camera
                Camera camera = new Camera( this );
                this.Components.Add( camera );
                ServiceLocator.Camera = camera;

                // Initialize InputManager
                InputManager inputManager = new InputManager( this );
                this.Components.Add( inputManager );
                ServiceLocator.InputManager = inputManager;

                // Initialize CameraController
                this.Components.Add( CameraController.Factory( m_cameraType, this) );

                // Initialize EntityManager
                EntityManager entityManager = new EntityManager( this );
                this.Components.Add( entityManager );
                ServiceLocator.EntityManager = entityManager;

                // Initialize Scenegraph
                ScenegraphManager scenegraph = new ScenegraphManager( this );
                this.Components.Add( scenegraph );
                ServiceLocator.ScenegraphManager = scenegraph;

                // Initialize DebugHUD
                DebugHUD debugHUD = new DebugHUD( this );
                this.Components.Add( new DebugHUD( this ) );
            }

            base.Initialize();

            if (m_initDelegate != null) {
                m_initDelegate();
            }
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (m_updateDelegate != null) {
                m_updateDelegate(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
            if (m_drawDelegate != null) {
                m_drawDelegate(gameTime);
            }
        }
    }
}
