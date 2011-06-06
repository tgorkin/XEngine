using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    class BoundingVolumePrimitive {

        protected GeometricPrimitive m_primitive;

        protected Matrix m_transform;

        public BoundingVolumePrimitive() { }

        public void Draw( Matrix world ) {
            m_primitive.Draw( m_transform * world );
        }
    }
}
