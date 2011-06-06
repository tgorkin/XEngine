using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    class BoundingSpherePrimitive : BoundingVolumePrimitive {

        private static readonly Color DEFAULT_COLOR_BOUNDING_SPHERE = Color.White;

        BoundingSphere m_boundingSphere;

        public BoundingSphere BoundingSphere {
            get { return m_boundingSphere; }
        }

        public BoundingSpherePrimitive( BoundingSphere boundingSphere, Color color ) {
            m_boundingSphere = boundingSphere;
            m_primitive = GeometricPrimitive.Factory( XEngineTypes.GeometricPrimitiveType.Sphere, m_boundingSphere.Radius );
            m_primitive.Wireframe = true;
            m_primitive.Color = color;
            m_primitive.Initialize();
            m_transform = Matrix.CreateTranslation( m_boundingSphere.Center );
            
        }

        public BoundingSpherePrimitive( BoundingSphere boundingSphere )
            : this( boundingSphere, DEFAULT_COLOR_BOUNDING_SPHERE ) {
        }

    }
}
