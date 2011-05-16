using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XEngine {

    class Origin {

        protected List<VertexPositionColor> m_vertices = new List<VertexPositionColor>();

        private float m_size;

        public Origin( float size ) 
            : base() {
            m_size = size;
        }
    }
}
