using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ModelPipeline {
    class BoundingVolumeHelper {

        public static BoundingSphere GenerateBestBoundingSphere( List<Vector3> points ) {
            BoundingSphere boundingSphereFromBox = GenerateBoundingSphereUsingBox( points );
            BoundingSphere boundingSphereFromPoints = BoundingSphere.CreateFromPoints( points );
            BoundingSphere result = boundingSphereFromBox;
            if ( boundingSphereFromPoints.Radius < boundingSphereFromBox.Radius ) {
                result = boundingSphereFromPoints;
            }
            return result;
        }

        private static BoundingSphere GenerateBoundingSphereUsingBox( List<Vector3> points ) {
            BoundingBox boundingBox = BoundingBox.CreateFromPoints( points );
            Vector3 center = 0.5f * ( boundingBox.Max + boundingBox.Min );

            float maxDistance = Vector3.DistanceSquared( center, points[0] );
            for ( int i = 1; i < points.Count; i++ ) {
                float distance = Vector3.DistanceSquared( center, points[i] );
                if ( distance > maxDistance ) {
                    maxDistance = distance;
                }
            }
            float radius = (float)Math.Sqrt(maxDistance);
            return new BoundingSphere( center, radius );
        }
    }
}
