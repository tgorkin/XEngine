using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EntityPipeline;

namespace XEngine {
    interface IEntityComponent {

        void LoadFromTemplate( ComponentTemplate componentTemplate );

        void Initialize();

        void Update( GameTime gameTime );

        void Draw( GameTime gameTime );
    }
}
