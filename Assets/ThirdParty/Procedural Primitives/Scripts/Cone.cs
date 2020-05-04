using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Cone : PPBase
    {
        public float radius1 = 0.5f;
        public float radius2 = 0.3f;
        public float height = 1.0f;
        public int sides = 20;
        public int capSegs = 2;
        public int heightSegs = 5;
        public bool sliceOn = false;
        public float sliceFrom = 0.0f;
        public float sliceTo = 360.0f;
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public bool flipNormals = false;
        public bool smooth = true;

        private void Start()
        {
            m_mesh.name = "Cone";
        }

        protected override void CreateMesh()
        {
            radius1 = Mathf.Clamp(radius1, 0.00001f, 10000.0f);
            radius2 = Mathf.Clamp(radius2, 0.00001f, 10000.0f);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            sides = Mathf.Clamp(sides, 3, 100);
            capSegs = Mathf.Clamp(capSegs, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);

            float heightHalf = height * 0.5f;
            Vector3 centerUp = new Vector3(0.0f, heightHalf, 0.0f);
            Vector3 centerDown = new Vector3(0.0f, -heightHalf, 0.0f);

            CreateCone(Vector3.zero, Vector3.forward, Vector3.right, height, radius1, radius2, sides, heightSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals, smooth);
            CreateCircle(centerUp, Vector3.forward, Vector3.right, radius2, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals);
            CreateCircle(centerDown, Vector3.forward, Vector3.right, radius1, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, !flipNormals);

            if (sliceOn)
            {
                float offset = (radius1 - radius2) * 0.5f;
                Vector3 centerFrom = new Vector3(Mathf.Sin(sliceFrom * deg2rad), 0.0f, Mathf.Cos(sliceFrom * deg2rad)) * radius1 * 0.5f;
                Vector3 centerTo = new Vector3(Mathf.Sin(sliceTo * deg2rad), 0.0f, Mathf.Cos(sliceTo * deg2rad)) * radius1 * 0.5f;
                CreateTrapezoid(centerFrom, Vector3.up, -centerFrom.normalized, radius1, radius2, height, offset, capSegs, heightSegs, generateMappingCoords, realWorldMapSize, Vector2.zero, new Vector2(0.5f, 1.0f), flipNormals);
                CreateTrapezoid(centerTo, Vector3.up, centerTo.normalized, radius1, radius2, height, -offset, capSegs, heightSegs, generateMappingCoords, realWorldMapSize, new Vector2(0.5f, 0.0f), new Vector2(0.5f, 1.0f), flipNormals);
            }
        }
    }
}