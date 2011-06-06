using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XEngineTypes;

namespace XEngine {
    class ModelRenderComponent : BaseComponent, IEntityComponent {

        private string m_modelName;

        private XEngineModel m_model;

        private Transform m_entityTransform;

        private Transform m_localTransform;

        public ModelRenderComponent( Entity entity )
            : base( entity ) {
                m_localTransform = new Transform();
        }

        public string ModelName {
            set { m_modelName = value; }
        }

        public Model Model {
            get { return m_model.Model; }
        }

        override public void LoadData( ComponentData componentData ) {
            ModelRenderData data = componentData as ModelRenderData;
            if ( data != null ) {
                m_modelName = data.Model;
                m_localTransform = data.Transform;
            }
        }

        override public void Initialize() {
            m_model = new XEngineModel( m_modelName );
            m_model.Initialize();
            m_entityTransform = this.Entity.GetAttribute<Transform>( Attributes.TRANSFORM );

            //BoundingSphere boundingSphere = ModelUtils.GetGlobalBoundingSphere( m_model );
            //BoundingSphere boundingSphere = m_model.Meshes[1].BoundingSphere;
            //boundingSphere = boundingSphere.Transform( m_localTransform.Local );
            //this.Entity.AddAttribute( Attributes.BOUNDING_SPHERE, boundingSphere );
        }

        override public void Draw(GameTime gameTime) {
            Matrix world;
            if ( m_entityTransform != null ) {
                world = m_localTransform.Local * m_entityTransform.World;
            } else {
                world = m_localTransform.Local;
            }
            m_model.Draw( world );
        }

        static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            Entity entity1 = null;
            Entity entity2 = null;
            testGame.InitDelegate = delegate {
                entity1 = new Entity();
                AddShipTestComponent( entity1 );
                entity1.AddAttribute( Attributes.TRANSFORM, new Transform( new Vector3( 0, 1.0f, 0 ) ) );
                entity1.Initialize();

                entity2 = new Entity();
                AddGridTestComponent( entity2 );
                entity2.Initialize();
            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                entity1.Draw( gameTime );
                entity2.Draw( gameTime );
            };
            testGame.Run();
        }

        static public void AddShipTestComponent(Entity entity) {
            ModelRenderComponent component = new ModelRenderComponent( entity );

            ModelRenderData componentData = new ModelRenderData();
            componentData.Model = "Models/Ship";
            Transform modelTransform = new Transform();
            modelTransform.Scale = new Vector3( 0.001f );
            componentData.Transform = modelTransform;

            component.LoadData( componentData );
            entity.AddComponent( component );
        }

        static public void AddGridTestComponent( Entity entity ) {
            ModelRenderComponent component = new ModelRenderComponent( entity );

            ModelRenderData componentData = new ModelRenderData();
            componentData.Model = "Models/Grid";
            Transform modelTransform = new Transform();
            modelTransform.Scale = new Vector3( 0.1f );
            componentData.Transform = modelTransform;

            component.LoadData( componentData );
            entity.AddComponent( component );
        }
    }
}
