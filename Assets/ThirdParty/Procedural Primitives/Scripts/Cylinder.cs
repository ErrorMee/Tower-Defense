using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Cylinder : PPBase
    {
        public float radius = 0.5f;
        public float height = 1.0f;
        public int sides = 20;
        public int capSegs = 2;
        public int heightSegs = 2;
        public bool sliceOn = false;
        public float sliceFrom = 0.0f;
        public float sliceTo = 360.0f;
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public bool flipNormals = false;
        public bool smooth = true;

        private void Start()
        {
            m_mesh.name = "Cylinder";
        }

        protected override void CreateMesh()
        {
            radius = Mathf.Clamp(radius, 0.00001f, 10000.0f);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            sides = Mathf.Clamp(sides, 3, 100);
            capSegs = Mathf.Clamp(capSegs, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);

            float heightHalf = height * 0.5f;

            CreateCylinder(Vector3.zero, Vector3.forward, Vector3.right, height, radius, sides, heightSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals, smooth);
            CreateCircle(new Vector3(0.0f, heightHalf, 0.0f), Vector3.forward, Vector3.right, radius, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals);
            CreateCircle(new Vector3(0.0f, -heightHalf, 0.0f), Vector3.forward, Vector3.right, radius, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, !flipNormals);

            if (sliceOn)
            {
                Vector3 centerFrom = new Vector3(Mathf.Sin(sliceFrom * deg2rad), 0.0f, Mathf.Cos(sliceFrom * deg2rad)) * radius * 0.5f;
                Vector3 centerTo = new Vector3(Mathf.Sin(sliceTo * deg2rad), 0.0f, Mathf.Cos(sliceTo * deg2rad)) * radius * 0.5f;
                CreatePlane(centerFrom, Vector3.up, -centerFrom.normalized, radius, height, capSegs, heightSegs, generateMappingCoords, realWorldMapSize, Vector2.zero, new Vector2(0.5f, 1.0f), flipNormals);
                CreatePlane(centerTo, Vector3.up, centerTo.normalized, radius, height, capSegs, heightSegs, generateMappingCoords, realWorldMapSize, new Vector2(0.5f, 0.0f), new Vector2(0.5f, 1.0f), flipNormals);
            }
        }
    }
}