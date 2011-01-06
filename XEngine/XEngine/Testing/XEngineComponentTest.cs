using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {

    // declare TestDelagate types
    delegate void InitDelegate();

    delegate void UpdateDelegate(GameTime gameTime);

    delegate void DrawDelegate(GameTime gameTime);

    class XEngineComponentTest : XEngineGame {
        
        private InitDelegate m_initDelegate;

        private UpdateDelegate m_updateDelegate;

        private DrawDelegate m_drawDelegate;

        private bool m_addDebugComponents;

        public XEngineComponentTest() {
            this.IsMouseVisible = true;
        }

        public XEngineComponentTest(bool addDebugComponents) : base() {
            this.IsMouseVisible = true;
            m_addDebugComponents = addDebugComponents;
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

        protected override void Initialize() {
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

        protected override void ConfigureGameComponents() {
            if ( this.m_addDebugComponents ) {
                this.BindGameComponent( new Camera( this ), typeof( ICamera ) );
                this.BindGameComponent( new InputManager( this ), typeof( IInputManager ) );

                this.Components.Add( new CameraController( this ) );
                this.Components.Add( new Origin( this ) );
                this.Components.Add( new DebugHUD( this ) );
            }
        }
    }
}
