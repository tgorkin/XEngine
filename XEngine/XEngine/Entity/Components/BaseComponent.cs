using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EntityPipeline;

namespace XEngine {
    class BaseComponent : IEntityComponent {

        private Entity m_entity;

        public Entity Entity { 
            get {return m_entity;} 
        }

        public BaseComponent( Entity entity ) {
            this.m_entity = entity;
        }

        virtual public void LoadFromTemplate(ComponentTemplate componentTemplate) { }

        virtual public void Initialize() { }

        virtual public void Update( GameTime gameTime ) {}

        virtual public void Draw( GameTime gameTime ) {}
    }
}
