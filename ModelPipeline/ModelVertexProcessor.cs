using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using XEngineTypes;

namespace ModelPipeline {
   
    [ContentProcessor( DisplayName = "CustomModelProcessor" )]
    public class CustomModelProcessor : ModelProcessor {
        public override ModelContent Process( NodeContent input, ContentProcessorContext context ) {
            ModelContent model = base.Process( input, context );

            //List<Vector3> vertices = new List<Vector3>();
            //vertices = AddVerticesToList( input, vertices );
            //model.Tag = vertices.ToArray();

            Dictionary<string, List<Vector3>> meshVertexDictionary = new Dictionary<string, List<Vector3>>();
            meshVertexDictionary = GenerateMeshVertexDictionary( input, meshVertexDictionary );

            foreach ( ModelMeshContent mesh in model.Meshes ) {
                List<Vector3> meshPoints = meshVertexDictionary[mesh.Name];
                CustomMeshData meshData = new CustomMeshData();
                meshData.BoundingSphere = BoundingVolumeHelper.GenerateBestBoundingSphere( meshPoints );
                meshData.BoundingBox = BoundingBox.CreateFromPoints( meshPoints );
                mesh.Tag = meshData;
            }

            return model;
        }

        private List<Vector3> AddVerticesToList(NodeContent node, List<Vector3> vertList) {
            MeshContent mesh = node as MeshContent;
            if ( mesh != null ) {
                Matrix meshTransform = mesh.AbsoluteTransform;
                foreach ( GeometryContent geometry in mesh.Geometry ) {
                    foreach ( int index in geometry.Indices ) {
                        Vector3 vertex = geometry.Vertices.Positions[index];
                        Vector3 transformedVertex = Vector3.Transform( vertex, meshTransform );
                        vertList.Add( transformedVertex );
                    }
                }
            }
            foreach ( NodeContent child in node.Children ) {
                vertList = AddVerticesToList( child, vertList );
            }

            return vertList;
        }

        private Dictionary<string, List<Vector3>> GenerateMeshVertexDictionary( NodeContent node, Dictionary<string, List<Vector3>> meshVertexDictionary ) {
            foreach ( NodeContent child in node.Children ) {
                meshVertexDictionary = GenerateMeshVertexDictionary( child, meshVertexDictionary );
            }
            MeshContent mesh = node as MeshContent;
            if ( mesh != null ) {
                List<Vector3> nodeVertices = new List<Vector3>();
                foreach ( GeometryContent geometry in mesh.Geometry ) {
                    VertexContent vertices = geometry.Vertices;
                    foreach ( Vector3 position in vertices.Positions ) {
                        nodeVertices.Add( position );
                    }
                }
                meshVertexDictionary.Add(mesh.Name, nodeVertices);
            }

            return meshVertexDictionary;
        }

        private List<Vector3[]> AddModelMeshVertexArrayToList( NodeContent node, List<Vector3[]> modelVertices ) {
            foreach ( NodeContent child in node.Children ) {
                modelVertices = AddModelMeshVertexArrayToList( child, modelVertices );
            }

            MeshContent mesh = node as MeshContent;
            if ( mesh != null ) {
                List<Vector3> nodeVertices = new List<Vector3>();
                foreach ( GeometryContent geometry in mesh.Geometry ) {
                    VertexContent vertices = geometry.Vertices;
                    foreach ( Vector3 position in vertices.Positions ) {
                        nodeVertices.Add( position );
                    }
                }
                modelVertices.Add( nodeVertices.ToArray() );
            }
            return modelVertices;
        }

    }
}