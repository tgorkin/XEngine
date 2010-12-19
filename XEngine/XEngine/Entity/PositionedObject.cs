using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    public class PositionedObject : DrawableGameComponent {

        private Vector3 m_position;

        private Matrix m_rotation = Matrix.Identity;

        private Vector3 m_scales;

        public PositionedObject( Game game )
            : base( game ) {
        }

        public PositionedObject( Game game, Vector3 position )
            : base( game ) {
                m_position = position;
        }

        public Vector3 position {
            get { return m_position; }
            set { m_position = value; }
        }

        public Matrix rotation {
            get { return m_rotation; }
            set { m_rotation = value; }
        }

        public Vector3 scales {
            get { return m_scales; }
            set { m_scales = value; }
        }

        public float scaleX {
            get { return m_scales.X; }
            set { m_scales.X = value; }
        }

        public float scaleY {
            get { return m_scales.Y; }
            set { m_scales.Y = value; }
        }

        public float scaleZ {
            get { return m_scales.Z; }
            set { m_scales.Z = value; }
        }


        protected Matrix getWorld() {
            return Matrix.Identity * ScaleMatrix() * RotationMatrix() * TranslationMatrix();
        }

        private Matrix TranslationMatrix() {
            return Matrix.CreateTranslation( m_position );
        }

        private Matrix RotationMatrix() {
            return m_rotation;
        }

        private Matrix ScaleMatrix() {
            Matrix result = Matrix.Identity;
            if ( m_scales.LengthSquared() > 0 ) {
                result = Matrix.CreateScale( m_scales );
            }
            return result;
        }
    }
}
