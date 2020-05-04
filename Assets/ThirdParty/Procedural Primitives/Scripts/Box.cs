using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Box : PPBase
    {
        public float width = 1.0f;
        public float length = 1.0f;
        public float height = 1.0f;
        public int widthSegs = 2;
        public int lengthSegs = 2;
        public int heightSegs = 2;
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public bool flipNormals = false;

        private void Start()
        {
            m_mesh.name = "Box";
        }

        protected override void CreateMesh()
        {
            length = Mathf.Clamp(length, 0.00001f, 10000.0f);
            width = Mathf.Clamp(width, 0.00001f, 10000.0f);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            lengthSegs = Mathf.Clamp(lengthSegs, 1, 100);
            widthSegs = Mathf.Clamp(widthSegs, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);

            float lengthHalf = length * 0.5f;
            float widthHalf = width * 0.5f;
            float heightHalf = height * 0.5f;
            
            CreatePlane(new Vector3(0.0f, 0.0f, -lengthHalf), Vector3.up,      Vector3.right,   width,  height, widthSegs,  heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane(new Vector3(0.0f, 0.0f, lengthHalf),  Vector3.up,      Vector3.left,    width,  height, widthSegs,  heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane(new Vector3(-widthHalf, 0.0f, 0.0f),  Vector3.up,      Vector3.back,    length, height, lengthSegs, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane(new Vector3(widthHalf, 0.0f, 0.0f),   Vector3.up,      Vector3.forward, length, height, lengthSegs, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane(new Vector3(0.0f, heightHalf, 0.0f),  Vector3.forward, Vector3.right,   width,  length, widthSegs,  lengthSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane(new Vector3(0.0f, -heightHalf, 0.0f), Vector3.forward, Vector3.left,    width,  length, widthSegs,  lengthSegs, generateMappingCoords, realWorldMapSize, flipNormals);
        }
    }
}