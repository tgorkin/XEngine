using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XEngineTypes;

namespace XEngine {
    class ModelBoneControllerComponent : BaseComponent {

        private string m_boneName;

        private ModelBone m_bone;

        private TransformType m_transformType;

        private Matrix m_originalTransform;

        private float m_speed;

        private float m_currentTransformAmount;

        public ModelBoneControllerComponent( Entity entity )
            : base( entity ) {
           
        }

        override public void LoadData( ComponentData componentData ) {
            ModelBoneControllerData data = componentData as ModelBoneControllerData;
            if ( data != null ) {
                m_boneName = data.BoneName;
                m_transformType = data.TransformType;
                m_speed = data.Speed;
            }
        }

        override public void Initialize() {
            ModelRenderComponent modelRenderComponent = Entity.GetComponentOfType( typeof( ModelRenderComponent ) ) as ModelRenderComponent;
            if ( modelRenderComponent != null ) {
                Model model = modelRenderComponent.Model;
                if ( model.Bones.TryGetValue( m_boneName, out m_bone ) ) {
                    m_originalTransform = m_bone.Transform;
                } else {
                    throw new Exception( this.GetType().ToString() + ": Unable to find bone named " + m_boneName );
                }
            } else {
                throw new Exception( this.GetType().ToString() + ": Unable to find ModelRenderComponent" );
            }
        }

        override public void Update( GameTime gameTime ) {
            if ( m_bone != null ) {
                IInputManager inputManager = ServiceLocator.InputManager;
                if(inputManager.isKeyDown( Keys.Q ) ) {
                    m_currentTransformAmount += m_speed;
                } else if (inputManager.isKeyDown( Keys.E )) {
                    m_currentTransformAmount -= m_speed;
                }
                Transform transform = Transform.CreateFromType( this.m_transformType, this.m_currentTransformAmount );

                m_bone.Transform = transform.Local * m_originalTransform;
            }
        }
    }
}
