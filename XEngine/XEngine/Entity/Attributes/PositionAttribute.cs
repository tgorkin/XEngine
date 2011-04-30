using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EntityPipeline;

namespace XEngine {
    class PositionAttribute : IEntityAttribute {

        public PositionAttribute() : this(new Vector3(), Matrix.Identity, new Vector3(1.0f)) { }

        public PositionAttribute( Vector3 position, Matrix rotation, Vector3 scale ) {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public Vector3 Position {
            get;
            set;
        }

        public Matrix Rotation {
            get;
            set;
        }

        public Vector3 Scale {
            get;
            set;
        }

        public Matrix World {
            get { return Matrix.Identity * Matrix.CreateScale( Scale ) * Rotation * Matrix.CreateTranslation( Position ); }
        }

        public void LoadData( object data ) {
            PositionAttributeData positionAttributeData = data as PositionAttributeData;
            if ( positionAttributeData != null ) {
                this.Position = positionAttributeData.Position;
                this.Rotation = positionAttributeData.Rotation;
                if ( this.Rotation == new Matrix() ) {
                    this.Rotation = Matrix.Identity;
                }
                this.Scale = positionAttributeData.Scale;
                if ( this.Scale == Vector3.Zero ) {
                    this.Scale = new Vector3( 1.0f );
                }
            }
        }
    }
}
