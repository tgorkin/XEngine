using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XEngineTypes;

namespace XEngine {
    class ScenegraphManager : GameComponent {

        private ScenegraphNode m_root;

        public ScenegraphManager( Game game ) : base( game ) {
            m_root = new ScenegraphNode( new Transform() );
        }

        override public void Update(GameTime gameTime) {
            m_root.Update();
        }

        public void AddEntity( Entity entity, Entity parent ) {
            Transform transform = ( entity.GetAttribute( Attributes.TRANSFORM ) as EntityAttribute<Transform> ).Value;
            ScenegraphNode node = new ScenegraphNode( transform );
            if ( parent != null ) {
                Transform parentTransform = ( parent.GetAttribute( Attributes.TRANSFORM ) as EntityAttribute<Transform> ).Value;
                ScenegraphNode parentNode = m_root.FindByTransform( parentTransform );
                if ( parentNode != null ) {
                    parentNode.AddChild( node );
                } else {
                    m_root.AddChild( node );
                }
            } else {
                m_root.AddChild( node );
            }
        }

        static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            ScenegraphManager scenegraph = new ScenegraphManager( testGame ); ;
            Entity entity1 = null;
            Entity entity2 = null;
            Entity entity3 = null;
            testGame.InitDelegate = delegate {
                scenegraph.Initialize();

                entity1 = new Entity();
                PrimitiveRenderComponent.AddTestComponent( entity1, GeometricPrimitiveType.Sphere );
                MoveComponent.AddTestComponent( entity1 );
                entity1.Initialize();
                scenegraph.AddEntity( entity1, null );

                entity2 = new Entity();
                PrimitiveRenderComponent.AddTestComponent( entity2, GeometricPrimitiveType.Cube );
                entity2.Initialize();
                Transform transform2 = ( entity2.GetAttribute( Attributes.TRANSFORM ) as EntityAttribute<Transform> ).Value;
                transform2.Position = new Vector3( 0, 1.0f, 0 );
                transform2.Rotation = Matrix.CreateRotationY( MathHelper.PiOver4 );
                transform2.Scale = new Vector3( 0.5f );
                scenegraph.AddEntity( entity2, entity1 );

                entity3 = new Entity();
                PrimitiveRenderComponent.AddTestComponent( entity3, GeometricPrimitiveType.Cube );
                entity3.Initialize();
                Transform transform3 = ( entity3.GetAttribute( Attributes.TRANSFORM ) as EntityAttribute<Transform> ).Value;
                transform3.Position = new Vector3( 1.5f, 0, 0 );
                scenegraph.AddEntity( entity3, entity2 );

            };
            testGame.UpdateDelegate = delegate( GameTime gameTime ) {
                entity1.Update( gameTime );
                entity2.Update( gameTime );
                entity3.Update( gameTime );
                scenegraph.Update( gameTime );
            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                entity1.Draw( gameTime );
                entity2.Draw( gameTime );
                entity3.Draw( gameTime );
            };
            testGame.Run();
        }
    }
}
