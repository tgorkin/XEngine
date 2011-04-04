using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XEngine {
    class ModelRenderComponent : BaseComponent, IEntityComponent {

        private PositionAttribute m_position;

        private Model m_model;

        public ModelRenderComponent( Entity entity, String modelName ) : base( entity ) {
            m_position = entity.getAttribute( Attributes.POSITION ) as PositionAttribute;
            LoadModel( modelName );
        }

        private void LoadModel( String modelName ) {
            m_model = ServiceLocator.Content.Load<Model>( modelName );
        }

        override public void Draw(GameTime gameTime) {
            Matrix[] transforms = new Matrix[m_model.Bones.Count];
            m_model.CopyAbsoluteBoneTransformsTo( transforms );

            ICamera camera = ServiceLocator.Camera;
            Matrix world;
            if ( m_position != null ) {
                world = m_position.World;
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

            Entity entity = null;
            testGame.InitDelegate = delegate {
                entity = new Entity();
                AddShipTestComponent( entity );
                AddGridTestComponent( entity );

            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                entity.Draw( gameTime );
            };
            testGame.Run();
        }

        static public void AddShipTestComponent(Entity entity) {
            // Setup a position attribute
            PositionAttribute position = new PositionAttribute();
            position.Scale = new Vector3( 0.001f );
            entity.addAttribute( Attributes.POSITION , position );

            // Add a model render component
            ModelRenderComponent renderComponent = new ModelRenderComponent( entity, "Models/Ship" );
            entity.AddComponent( renderComponent );
        }

        static public void AddGridTestComponent( Entity entity ) {
            // Setup a position attribute
            PositionAttribute position = new PositionAttribute();
            position.Position = new Vector3( 0, -5.0f, 0 );
            position.Scale = new Vector3( 0.2f );
            entity.addAttribute( Attributes.POSITION, position );

            // Add a model render component
            ModelRenderComponent renderComponent = new ModelRenderComponent( entity, "Models/Grid" );
            entity.AddComponent( renderComponent );
        }

    }
}
