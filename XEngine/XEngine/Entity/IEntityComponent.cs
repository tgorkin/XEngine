using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    interface IEntityComponent {

        void Update( GameTime gameTime );

        void Draw( GameTime gameTime );
    }
}
