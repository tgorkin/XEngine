using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XEngineTypes {
    public enum TransformType {
        RotationX,
        RotationY,
        RotationZ,
        TranslationX,
        TranslationY,
        TranslationZ,
        ScaleX,
        ScaleY,
        ScaleZ
    }

    public class Transform {

        private Transform m_parent;

        private Vector3 m_position = new Vector3();

        private Matrix m_rotation = Matrix.Identity;

        private Vector3 m_scale = new Vector3( 1.0f );

        private Matrix m_world = Matrix.Identity;

        [ContentSerializerIgnore]
        public Transform Parent {
            set { m_parent = value; }
        }

        [ContentSerializer( Optional = true )]
        public Vector3 Position {
            get {
                return m_position;
            }
            set {
                m_position = value;
            }
        }

        [ContentSerializer( Optional = true )]
        public Matrix Rotation {
            get {
                return m_rotation;
            }
            set {
                m_rotation = value;
            }
        }

        [ContentSerializer( Optional = true )]
        public Vector3 Scale {
            get {
                return m_scale;
            }
            set {
                m_scale = value;
            }
        }

        [ContentSerializerIgnore]
        public Matrix Local {
            get {
                return Matrix.CreateScale( m_scale ) * m_rotation * Matrix.CreateTranslation( m_position );
            }
        }

        [ContentSerializerIgnore]
        public Matrix World {
            get {
                Matrix parent = Matrix.Identity;
                if ( m_parent != null ) {
                    parent = m_parent.World;
                }
                return Local * parent;
            }
        }

        public static Transform CreateFromType( TransformType type, float value ) {
            Transform result = new Transform();
            switch ( type ) {
                case TransformType.RotationX:
                    result.Rotation = Matrix.CreateRotationX( value );
                    break;
                case TransformType.RotationY:
                    result.Rotation = Matrix.CreateRotationY( value );
                    break;
                case TransformType.RotationZ:
                    result.Rotation = Matrix.CreateRotationZ( value );
                    break;
                case TransformType.TranslationX:
                    result.Position = new Vector3(value, 0, 0);
                    break;
                case TransformType.TranslationY:
                    result.Position = new Vector3(0, value, 0);
                    break;
                case TransformType.TranslationZ:
                    result.Position = new Vector3(0, 0, value);
                    break;
                case TransformType.ScaleX:
                    result.Scale = new Vector3( value, 1, 1 );
                    break;
                case TransformType.ScaleY:
                    result.Scale = new Vector3( 1, value, 1 );
                    break;
                case TransformType.ScaleZ:
                    result.Scale = new Vector3( 1, 1, value );
                    break;
               
            }
            return result;
        }
    }
}
