using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    class BoundingBoxPrimitive : BoundingVolumePrimitive {

        BoundingBox m_boundingBox;

        public BoundingBox BoundingBox {
            get { return m_boundingBox; }
        }

        public BoundingBoxPrimitive( BoundingBox boundingBox, Color color ) {
            m_boundingBox = boundingBox;
            m_primitive = GeometricPrimitive.Factory( XEngineTypes.GeometricPrimitiveType.Cube, 1.0f );
            m_primitive.Wireframe = true;
            m_primitive.Color = color;
            m_primitive.Initialize();

            Vector3 min = m_boundingBox.Min;
            Vector3 max = m_boundingBox.Max;
            Vector3 center = Vector3.Lerp( min, max, 0.5f );
            Vector3 scale = max - min;
            m_transform = Matrix.CreateScale(scale) * Matrix.CreateTranslation( center );
        }

        public BoundingBoxPrimitive( BoundingBox boundingBox )
            : this( boundingBox, Color.White ) {
        }


    }
}
