using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    class ComponentTests {

        static public void TankTurretTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            EntityFactory entityFactory = new EntityFactory();
            Entity entity = null;
            testGame.InitDelegate = delegate {
                entityFactory.LoadEntityTemplates( "Data/EntityTemplates" );
                entity = entityFactory.CreateEntity( "Tank" );
                entity.Initialize();
            };
            testGame.UpdateDelegate = delegate( GameTime gameTime ) {
                entity.Update( gameTime );
            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                entity.Draw( gameTime );
            };
            testGame.Run();
        }
    }
}
