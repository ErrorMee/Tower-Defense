using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Torus : PPBase
    {
        public float radius1 = 0.5f;
        public float radius2 = 0.1f;
        public int sides = 24;
        public int segments = 12;
        public bool sliceOn = false;
        public float sliceFrom = 0.0f;
        public float sliceTo = 360.0f;
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public bool flipNormals = false;
        public bool smooth = true;

        private void Start()
        {
            m_mesh.name = "Torus";
        }

        protected override void CreateMesh()
        {
            radius1 = Mathf.Clamp(radius1, 0.0f, 10000.0f);
            radius2 = Mathf.Clamp(radius2, 0.0f, 10000.0f);
            sides = Mathf.Clamp(sides, 3, 100);
            segments = Mathf.Clamp(segments, 3, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);

            CreateTorus(Vector3.zero, Vector3.forward, Vector3.right, radius1, radius2, sides, segments, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals, smooth);

            if (sliceOn)
            {
                Vector3 centerFrom = new Vector3(Mathf.Sin(sliceFrom * deg2rad), 0.0f, Mathf.Cos(sliceFrom * deg2rad)) * radius1;
                Vector3 centerTo = new Vector3(Mathf.Sin(sliceTo * deg2rad), 0.0f, Mathf.Cos(sliceTo * deg2rad)) * radius1;
                CreateCircle(centerFrom, Vector3.up, -centerFrom.normalized, radius2, segments, 1, generateMappingCoords, realWorldMapSize, flipNormals);
                CreateCircle(centerTo, Vector3.up, centerTo.normalized, radius2, segments, 1, generateMappingCoords, realWorldMapSize, flipNormals);
            }
        }
    }
}