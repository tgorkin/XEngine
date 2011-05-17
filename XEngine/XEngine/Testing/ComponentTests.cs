using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XEngineTypes;

namespace XEngine {
    class ComponentTests {

        static public void LoadLevelData() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            testGame.InitDelegate = delegate {
                LevelData levelData = ServiceLocator.Content.Load<LevelData>( "Data/Level_Test" );
                System.Diagnostics.Debugger.Break();
            };
            testGame.Run();
        }
    }
}
