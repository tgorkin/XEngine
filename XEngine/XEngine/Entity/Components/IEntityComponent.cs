using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XEngineTypes;

namespace XEngine {
    interface IEntityComponent {

        void LoadData( ComponentData data );

        void Initialize();

        void Update( GameTime gameTime );

        void Draw( GameTime gameTime );
    }
}
