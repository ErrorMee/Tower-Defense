using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Arrow : PPBase
    {
        public float width1 = 0.5f;
        public float width2 = 0.3f;
        public float width3 = 1.0f;
        public float length1 = 1.0f;
        public float length2 = 0.5f;
        public float height = 0.5f;
        public int widthSegs1 = 2;
        public int lengthSegs1 = 2;
        public int widthSegs2 = 4;
        public int lengthSegs2 = 2;
        public int heightSegs = 2;
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public bool flipNormals = false;

        private void Start()
        {
            m_mesh.name = "Arrow";
        }

        protected override void CreateMesh()
        {
            width1 = Mathf.Clamp(width1, 0.00001f, 10000.0f);
            width2 = Mathf.Clamp(width2, 0.00001f, 10000.0f);
            width3 = Mathf.Clamp(width3, width2, 10000.0f);
            length1 = Mathf.Clamp(length1, 0.00001f, 10000.0f);
            length2 = Mathf.Clamp(length2, 0.00001f, 10000.0f);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            lengthSegs1 = Mathf.Clamp(lengthSegs1, 1, 100);
            widthSegs1 = Mathf.Clamp(widthSegs1, 1, 100);
            lengthSegs2 = Mathf.Clamp(lengthSegs2, 1, 100);
            widthSegs2 = Mathf.Clamp(widthSegs2, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);

            float widthHalf1 = width1 * 0.5f;
            float widthHalf2 = width2 * 0.5f;
            float widthHalf3 = width3 * 0.5f;
            float heightHalf = height * 0.5f;
            float lengthHalf1 = length1 * 0.5f;
            float lengthHalf2 = length2 * 0.5f;

            CreateTriangle(new Vector3(0.0f, heightHalf, lengthHalf1), Vector3.forward, Vector3.right, width3, length2, 0.0f, widthSegs2, lengthSegs2, generateMappingCoords, realWorldMapSize, flipNormals);
            CreateTriangle(new Vector3(0.0f, -heightHalf, lengthHalf1), Vector3.forward, Vector3.right, width3, length2, 0.0f, widthSegs2, lengthSegs2, generateMappingCoords, realWorldMapSize, !flipNormals);
            Vector3 p0 = new Vector3(-widthHalf3, 0.0f, lengthHalf1 - lengthHalf2);
            Vector3 p1 = new Vector3(0.0f, 0.0f, lengthHalf1 + lengthHalf2);
            Vector3 p2 = new Vector3(widthHalf3, 0.0f, lengthHalf1 - lengthHalf2);
            Vector3 vLeft = p0 - p1;
            Vector3 vRight = p1 - p2;
            CreatePlane(new Vector3(0.0f, 0.0f, lengthHalf1 - lengthHalf2), Vector3.up, Vector3.right, width3, height, widthSegs2, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane((p0 + p1) * 0.5f, Vector3.up, vLeft.normalized, vLeft.magnitude, height, lengthSegs2, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane((p1 + p2) * 0.5f, Vector3.up, vRight.normalized, vRight.magnitude, height, lengthSegs2, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);

            CreateTrapezoid(new Vector3(0.0f, heightHalf, -lengthHalf2), Vector3.forward, Vector3.right, width1, width2, length1, 0.0f, widthSegs1, lengthSegs1, generateMappingCoords, realWorldMapSize, flipNormals);
            CreateTrapezoid(new Vector3(0.0f, -heightHalf, -lengthHalf2), Vector3.forward, Vector3.right, width1, width2, length1, 0.0f, widthSegs1, lengthSegs1, generateMappingCoords, realWorldMapSize, !flipNormals);
            Vector3 pp0 = new Vector3(-widthHalf1, 0.0f, -lengthHalf2 - lengthHalf1);
            Vector3 pp1 = new Vector3(-widthHalf2, 0.0f, -lengthHalf2 + lengthHalf1);
            Vector3 pp2 = new Vector3(widthHalf2, 0.0f, -lengthHalf2 + lengthHalf1);
            Vector3 pp3 = new Vector3(widthHalf1, 0.0f, -lengthHalf2 - lengthHalf1);
            Vector3 vvLeft = pp0 - pp1;
            Vector3 vvRight = pp2 - pp3;
            CreatePlane(new Vector3(0.0f, 0.0f, -lengthHalf2 - lengthHalf1), Vector3.up, Vector3.right, width1, height, widthSegs1, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane((pp0 + pp1) * 0.5f, Vector3.up, vvLeft.normalized, vvLeft.magnitude, height, lengthSegs1, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane((pp2 + pp3) * 0.5f, Vector3.up, vvRight.normalized, vvRight.magnitude, height, lengthSegs1, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
        }
    }
}