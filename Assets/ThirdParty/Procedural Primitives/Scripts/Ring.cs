using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Ring : PPBase
    {
        public float radius1 = 1;
        public float radius2 = 0.5f;
        public int sides = 20;
        public int segments = 3;
        public bool sliceOn = false;
        public float sliceFrom = 0.0f;
        public float sliceTo = 360.0f;
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public bool flipNormals = false;

        private void Start()
        {
            m_mesh.name = "Ring";
        }

        protected override void CreateMesh()
        {
            radius1 = Mathf.Clamp(radius1, 0.00001f, 10000.0f);
            radius2 = Mathf.Clamp(radius2, 0.00001f, radius1);
            segments = Mathf.Clamp(segments, 1, 100);
            sides = Mathf.Clamp(sides, 3, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);
            
            CreateRing(Vector3.zero, Vector3.forward, Vector3.right, radius1, radius2, sides, segments, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals);
        }
    }
}