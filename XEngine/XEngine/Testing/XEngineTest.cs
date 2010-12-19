using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    class XEngineTest : XEngineGame {

        protected override void Initialize() {
            this.Components.Add(new Camera(this));
            this.Components.Add(new InputManager(this));
            this.Components.Add(new CameraController(this));
            this.Components.Add(new Origin(this));
            this.Components.Add(new DebugHUD(this));
            base.Initialize();
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
        }

        public static void StartTest() {
            using (XEngineTest testGame = new XEngineTest()) {
                testGame.Run();
            }
        }

    }
}
