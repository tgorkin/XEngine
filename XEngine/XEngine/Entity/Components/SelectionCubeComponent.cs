using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XEngineTypes;

namespace XEngine {
    class SelectionCubeComponent : BaseComponent {

        GeometricPrimitive m_selectionPrimitive;

        Transform m_entityTransform;

        Transform m_localTransform = new Transform();

        BoundingSphere m_boundingSphere;

        public SelectionCubeComponent( Entity entity )
            : base( entity ) {
        }

        override public void Initialize() {
            m_entityTransform = this.Entity.GetAttribute<Transform>( Attributes.TRANSFORM );
            m_boundingSphere = this.Entity.GetAttribute<BoundingSphere>( Attributes.BOUNDING_SPHERE );
            m_localTransform.Position = m_boundingSphere.Center;
            m_selectionPrimitive = GeometricPrimitive.Factory( GeometricPrimitiveType.Sphere, m_boundingSphere.Radius*2 );
            m_selectionPrimitive.Color = Color.Yellow;
            m_selectionPrimitive.Wireframe = true;
            m_selectionPrimitive.Initialize();
        }

        override public void Draw( GameTime gameTime ) {
            Matrix world;
            if ( m_entityTransform != null ) {
                world = m_localTransform.Local * m_entityTransform.World;
            } else {
                world = m_localTransform.Local;
            }
            m_selectionPrimitive.Draw( world );
        }

        static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            Entity entity1 = null;
            testGame.InitDelegate = delegate {
                EntityFactory entityFactory = new EntityFactory();
                entityFactory.LoadEntityTemplates( "Data/EntityTemplates" );
                entity1 = entityFactory.CreateEntity( "Ship" );
                entity1.AddComponent( new SelectionCubeComponent( entity1 ) );
                entity1.Initialize();
            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                entity1.Draw( gameTime );
            };
            testGame.Run();
        }
    }
}
