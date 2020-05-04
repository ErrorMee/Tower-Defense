using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Prism : PPBase
    {
        public float width = 1;
        public float length = 1;
        public float height = 1.0f;
        public float offset = 0;
        public int sideSegs = 2;
        public int heightSegs = 2;
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public bool flipNormals = false;

        private void Start()
        {
            m_mesh.name = "Prism";
        }

        protected override void CreateMesh()
        {
            width = Mathf.Clamp(width, 0.00001f, 10000.0f);
            length = Mathf.Clamp(length, 0.00001f, 10000.0f);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            offset = Mathf.Clamp(offset, -10000.0f, 10000.0f);
            sideSegs = Mathf.Clamp(sideSegs, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);

            float lengthHalf = length * 0.5f;
            float widthHalf = width * 0.5f;
            float heightHalf = height * 0.5f;

            CreateTriangle(new Vector3(0.0f, heightHalf, 0.0f), Vector3.forward, Vector3.right, width, length, offset, sideSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreateTriangle(new Vector3(0.0f, -heightHalf, 0.0f), Vector3.forward, Vector3.right, width, length, offset, sideSegs, generateMappingCoords, realWorldMapSize, !flipNormals);

            Vector3 p0 = new Vector3(-widthHalf, 0.0f, -lengthHalf);
            Vector3 p1 = new Vector3(offset, 0.0f, lengthHalf);
            Vector3 p2 = new Vector3(widthHalf, 0.0f, -lengthHalf);
            Vector3 vLeft = p0 - p1;
            Vector3 vRight = p1 - p2;
            CreatePlane(new Vector3(0.0f, 0.0f, -lengthHalf), Vector3.up, Vector3.right, width, height, sideSegs, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane((p0 + p1) * 0.5f, Vector3.up, vLeft.normalized, vLeft.magnitude, height, sideSegs, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane((p1 + p2) * 0.5f, Vector3.up, vRight.normalized, vRight.magnitude, height, sideSegs, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
        }
    }
}