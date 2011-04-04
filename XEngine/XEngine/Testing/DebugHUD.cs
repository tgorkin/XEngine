using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XEngine {
    class DebugHUD : DrawableGameComponent {

        static readonly String FONT_NAME = "DefaultFont";

        static readonly Vector2 CAMERA_POS_TEXT_LOC = new Vector2(0, 0);

        static readonly Vector2 CAMERA_LOOK_AT_TEXT_LOC = new Vector2(0, 16);

        static readonly Vector2 FPS_TEXT_LOC = new Vector2(0, 32);

        static readonly int FPS_MEASURE_INTERVAL_SECONDS = 1;

        private SpriteBatch m_spriteBatch;

        private SpriteFont m_debugFont;

        private int m_frameCount;

        private float m_fpsFrameTime;

        private string m_fpsString;

        public DebugHUD(XEngineGame game)
            : base(game) {

        }

        public override void Initialize() {
            m_spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            m_debugFont = Game.Content.Load<SpriteFont>("Fonts\\" + FONT_NAME);
        }

        private void TraceCameraPosition(ICamera camera) {
            string positionString = StringUtils.Vec3ToString( camera.Position );
            m_spriteBatch.DrawString(m_debugFont, "Position: " + positionString, CAMERA_POS_TEXT_LOC, Color.White);
        }

        private void TraceCameraLookAt(ICamera camera) {
            string lookAtString = StringUtils.Vec3ToString( camera.LookAt );
            m_spriteBatch.DrawString(m_debugFont, "LookAt: " + lookAtString, CAMERA_LOOK_AT_TEXT_LOC, Color.White);
        }

        private void PrintFPS(GameTime gameTime) {
            m_frameCount++;
            m_fpsFrameTime += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
            m_spriteBatch.DrawString(m_debugFont, "FPS: " + m_fpsString, FPS_TEXT_LOC, Color.White);
            if ((m_fpsFrameTime > FPS_MEASURE_INTERVAL_SECONDS)) {
                m_fpsString = (m_frameCount / m_fpsFrameTime).ToString(); ;
                m_frameCount = 0;
                m_fpsFrameTime = 0;
            }
        }

        public override void Draw(GameTime gameTime) {
            ICamera camera = ServiceLocator.Camera;
            m_spriteBatch.Begin();
            TraceCameraPosition(camera);
            TraceCameraLookAt(camera);
            PrintFPS(gameTime);
            m_spriteBatch.End();
        }

        static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest(false);

            // Init Camera
            Camera camera = new Camera( testGame );
            testGame.Components.Add( camera );
            ServiceLocator.Camera = camera;

            testGame.Components.Add( new DebugHUD( testGame ) );

            testGame.Run();
        }
    }
}
