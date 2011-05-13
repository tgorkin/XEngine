using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XEngineTypes;

namespace XEngine {
    class ScenegraphManager {

        public ScenegraphNode Root;

        public void Initialize() {
            Root = new ScenegraphNode( new Transform() );
        }

        public void Update() {
            Root.Update();
        }

        static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            ScenegraphManager scenegraph = new ScenegraphManager(); ;
            Entity entity1 = null;
            Entity entity2 = null;
            testGame.InitDelegate = delegate {
                scenegraph.Initialize();

                entity1 = new Entity();
                ModelRenderComponent.AddShipTestComponent( entity1 );
                MoveComponent.AddTestComponent( entity1 );
                entity1.Initialize();

                Transform transform1 = ( entity1.GetAttribute( Attributes.TRANSFORM ) as EntityAttribute<Transform> ).Value;
                ScenegraphNode node1 = new ScenegraphNode( transform1 );
                scenegraph.Root.AddChild( node1 );

                entity2 = new Entity();
                PrimitiveRenderComponent.AddSphereTestComponent( entity2 );
                entity2.Initialize();

                Transform transform2 = ( entity2.GetAttribute( Attributes.TRANSFORM ) as EntityAttribute<Transform> ).Value;
                transform2.Scale = new Vector3( 1000f );
                transform2.Position = new Vector3( 0, 2.0f, 0 );
                node1.AddChild( new ScenegraphNode( transform2 ) );

            };
            testGame.UpdateDelegate = delegate( GameTime gameTime ) {
                entity1.Update( gameTime );
                entity2.Update( gameTime );
                scenegraph.Update();
            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                entity1.Draw( gameTime );
                entity2.Draw( gameTime );
            };
            testGame.Run();
        }
    }
}
