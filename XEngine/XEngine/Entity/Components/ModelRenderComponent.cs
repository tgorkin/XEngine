using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EntityPipeline;

namespace XEngine {
    class ModelRenderComponent : BaseComponent, IEntityComponent {

        private static readonly string COMPONENT_DATA_MODEL = "Model";

        private TransformAttribute m_transform;

        private string m_modelName;

        private Model m_model;

        public ModelRenderComponent( Entity entity )
            : base( entity ) {
           
        }

        public ModelRenderComponent( Entity entity, String modelName ) : this( entity ) {
            m_modelName = modelName;
        }

        override public void LoadFromTemplate( ComponentTemplate componentTemplate ) {
            if ( componentTemplate.ComponentData.ContainsKey( COMPONENT_DATA_MODEL ) ) {
                m_modelName = componentTemplate.ComponentData[COMPONENT_DATA_MODEL] as string;
            }
        }

        override public void Initialize() {
            m_transform = this.Entity.GetAttribute( Attributes.TRANSFORM ) as TransformAttribute;
            m_model = ServiceLocator.Content.Load<Model>( m_modelName );
        }

        override public void Draw(GameTime gameTime) {
            Matrix[] transforms = new Matrix[m_model.Bones.Count];
            m_model.CopyAbsoluteBoneTransformsTo( transforms );

            ICamera camera = ServiceLocator.Camera;
            Matrix world;
            if ( m_transform != null ) {
                world = m_transform.World;
            } else {
                world = Matrix.Identity;
            }

            foreach ( ModelMesh mesh in m_model.Meshes ) {
                foreach ( BasicEffect effect in mesh.Effects ) {
                    effect.EnableDefaultLighting();
                    effect.GraphicsDevice.BlendState = BlendState.Opaque;
                    effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    effect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.World = transforms[mesh.ParentBone.Index] * world;
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
            TransformAttribute transform = new TransformAttribute();
            transform.Scale = new Vector3(0.001f);
            entity.AddAttribute( Attributes.TRANSFORM, transform );

            // Add a model render component
            ModelRenderComponent renderComponent = new ModelRenderComponent( entity, "Models/Ship" );
            entity.AddComponent( renderComponent );
        }

        static public void AddGridTestComponent( Entity entity ) {
            // Setup a position and scale attribute
            TransformAttribute transform = new TransformAttribute();
            transform.Position = new Vector3( 0, -0.5f, 0 );
            transform.Scale = new Vector3( 0.1f );
            entity.AddAttribute( Attributes.TRANSFORM, transform );

            // Add a model render component
            ModelRenderComponent renderComponent = new ModelRenderComponent( entity, "Models/Grid" );
            entity.AddComponent( renderComponent );
        }

    }
}
