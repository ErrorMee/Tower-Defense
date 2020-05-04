using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Plane : PPBase
    {
        public float width = 2;
        public float length = 2;
        public int widthSegs = 10;
        public int lengthSegs = 10;
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public bool flipNormals = false;

        private void Start()
        {
            m_mesh.name = "Plane";
        }

        protected override void CreateMesh()
        {
            length = Mathf.Clamp(length, 0.00001f, 10000.0f);
            width = Mathf.Clamp(width, 0.00001f, 10000.0f);
            lengthSegs = Mathf.Clamp(lengthSegs, 1, 100);
            widthSegs = Mathf.Clamp(widthSegs, 1, 100);
            
            CreatePlane(Vector3.zero, Vector3.forward, Vector3.right, width, length, widthSegs, lengthSegs, generateMappingCoords, realWorldMapSize, Vector2.zero, Vector2.one, flipNormals);
        }
    }
}