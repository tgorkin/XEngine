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

        protected Model m_model;

        private EntityAttribute<Transform> m_transform;

        protected Matrix[] m_absoluteBoneTransforms;

        public ModelRenderComponent( Entity entity )
            : base( entity ) {
        }

        public string ModelName {
            set { m_modelName = value; }
        }

        public Model Model {
            get { return m_model; }
        }

        override public void LoadData( ComponentData componentData ) {
            ModelRenderData data = componentData as ModelRenderData;
            if ( data != null ) {
                m_modelName = data.Model;
            }
        }

        override public void Initialize() {
            m_transform = this.Entity.GetAttribute( Attributes.TRANSFORM ) as EntityAttribute<Transform>;
            m_model = ServiceLocator.Content.Load<Model>( m_modelName );
            m_absoluteBoneTransforms = new Matrix[m_model.Bones.Count];
            
        }

        override public void Draw(GameTime gameTime) {
            ICamera camera = ServiceLocator.Camera;
            Matrix world;
            if ( m_transform != null ) {
                world = m_transform.Value.World;
            } else {
                world = Matrix.Identity;
            }
            m_model.CopyAbsoluteBoneTransformsTo( m_absoluteBoneTransforms );
            foreach ( ModelMesh mesh in m_model.Meshes ) {
                foreach ( BasicEffect effect in mesh.Effects ) {
                    effect.EnableDefaultLighting();
                    effect.GraphicsDevice.BlendState = BlendState.Opaque;
                    effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    effect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.World = m_absoluteBoneTransforms[mesh.ParentBone.Index] * world;
                }
                mesh.Draw();
            }
        }

        static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            Entity entity1 = null;
            Entity entity2 = null;
            testGame.InitDelegate = delegate {
                entity1 = new Entity();
                entity2 = new Entity();
                AddShipTestComponent( entity1 );
                AddGridTestComponent( entity2 );
                entity1.Initialize();
                entity2.Initialize();
            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                entity1.Draw( gameTime );
                entity2.Draw( gameTime );
            };
            testGame.Run();
        }

        static public void AddShipTestComponent(Entity entity) {
            // Setup a position and scale attribute
            EntityAttribute<Transform> transform = new EntityAttribute<Transform>();
            transform.Value = new Transform();
            transform.Value.Scale = new Vector3(0.001f);
            entity.AddAttribute( Attributes.TRANSFORM, transform );

            // Add a model render component
            ModelRenderComponent renderComponent = new ModelRenderComponent( entity );
            renderComponent.ModelName = "Models/Ship";
            entity.AddComponent( renderComponent );
        }

        static public void AddGridTestComponent( Entity entity ) {
            // Setup a position and scale attribute
            EntityAttribute<Transform> transform = new EntityAttribute<Transform>();
            transform.Value = new Transform();
            transform.Value.Position = new Vector3( 0, -0.5f, 0 );
            transform.Value.Scale = new Vector3( 0.1f );
            entity.AddAttribute( Attributes.TRANSFORM, transform );

            // Add a model render component
            ModelRenderComponent renderComponent = new ModelRenderComponent( entity );
            renderComponent.ModelName = "Models/Grid";
            entity.AddComponent( renderComponent );
        }
    }
}
