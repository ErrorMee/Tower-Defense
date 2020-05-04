using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Pyramid : PPBase
    {
        public float width1 = 1.0f;
        public float length1 = 1.0f;
        public float width2 = 0.5f;
        public float length2 = 0.5f;
        public float height = 1.0f;
        public int widthSegs = 2;
        public int lengthSegs = 2;
        public int heightSegs = 4;
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public bool flipNormals = false;

        private void Start()
        {
            m_mesh.name = "Pyramid";
        }

        protected override void CreateMesh()
        {
            length1 = Mathf.Clamp(length1, 0.00001f, 10000.0f);
            width1 = Mathf.Clamp(width1, 0.00001f, 10000.0f);
            length2 = Mathf.Clamp(length2, 0.00001f, 10000.0f);
            width2 = Mathf.Clamp(width2, 0.00001f, 10000.0f);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            lengthSegs = Mathf.Clamp(lengthSegs, 1, 100);
            widthSegs = Mathf.Clamp(widthSegs, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);

            float lengthHalf1 = length1 * 0.5f;
            float widthHalf1 = width1 * 0.5f;
            float lengthHalf2 = length2 * 0.5f;
            float widthHalf2 = width2 * 0.5f;
            float heightHalf = height * 0.5f;

            float w = (widthHalf1 + widthHalf2) * 0.5f;
            float l = (lengthHalf1 + lengthHalf2) * 0.5f;
            Vector3 forward = new Vector3(0.0f, heightHalf, lengthHalf2) - new Vector3(0.0f, -heightHalf, lengthHalf1);
            Vector3 right = new Vector3(widthHalf2, heightHalf, 0.0f) - new Vector3(widthHalf1, -heightHalf, 0.0f);
            float lengthForward = forward.magnitude;
            float lengthRight = right.magnitude;
            forward = forward.normalized;
            right = right.normalized;
            Vector3 back = new Vector3(0.0f, forward.y, -forward.z);
            Vector3 left = new Vector3(-right.x, right.y, 0.0f);

            CreateTrapezoid(new Vector3(0.0f, 0.0f, l),  forward, Vector3.left,    width1,  width2, lengthForward, 0.0f, widthSegs, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreateTrapezoid(new Vector3(0.0f, 0.0f, -l),    back, Vector3.right,   width1,  width2, lengthForward, 0.0f, widthSegs, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreateTrapezoid(new Vector3(-w, 0.0f, 0.0f),    left, Vector3.back,    length1, length2, lengthRight, 0.0f, lengthSegs, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreateTrapezoid(new Vector3(w, 0.0f, 0.0f) ,   right, Vector3.forward, length1, length2, lengthRight, 0.0f, lengthSegs, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);

            CreatePlane(new Vector3(0.0f, heightHalf, 0.0f), Vector3.forward, Vector3.right, width2, length2, widthSegs, lengthSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane(new Vector3(0.0f, -heightHalf, 0.0f), Vector3.forward, Vector3.left, width1, length1, widthSegs, lengthSegs, generateMappingCoords, realWorldMapSize, flipNormals);
        }
    }
}