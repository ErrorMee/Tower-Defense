using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Tube : PPBase
    {
        public float radius1 = 0.5f;
        public float radius2 = 0.3f;
        public float height = 1.0f;
        public int sides = 20;
        public int capSegs = 2;
        public int heightSegs = 2;
        public bool cap1 = false;
        public float capThickness1 = 0.2f;
        public bool cap2 = false;
        public float capThickness2 = 0.2f;
        public bool sliceOn = false;
        public float sliceFrom = 0.0f;
        public float sliceTo = 360.0f;
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public bool flipNormals = false;
        public bool smooth = true;

        private void Start()
        {
            m_mesh.name = "Tube";
        }

        protected override void CreateMesh()
        {
            radius1 = Mathf.Clamp(radius1, 0.00001f, 10000.0f);
            radius2 = Mathf.Clamp(radius2, 0.00001f, radius1);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            if (cap1)
            {
                capThickness1 = Mathf.Clamp(capThickness1, 0.00001f, height);
                capThickness2 = Mathf.Clamp(capThickness2, 0.00001f, height - capThickness1);
            }
            else
            {
                capThickness1 = Mathf.Clamp(capThickness1, 0.00001f, height);
                capThickness2 = Mathf.Clamp(capThickness2, 0.00001f, height);
            }
            sides = Mathf.Clamp(sides, 3, 100);
            capSegs = Mathf.Clamp(capSegs, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);

            float heightHalf = height * 0.5f;
            float hDown = cap1 ? capThickness1 : 0.0f;
            float hUp = cap2 ? capThickness2 : 0.0f;
            float dif = radius1 - radius2;

            Vector3 centerDown = new Vector3(0.0f, -heightHalf, 0.0f);
            Vector3 centerUp = new Vector3(0.0f, heightHalf, 0.0f);
            Vector3 centerDown2 = new Vector3(0.0f, -heightHalf + hDown, 0.0f);
            Vector3 centerUp2 = new Vector3(0.0f, heightHalf - hUp, 0.0f);

            CreateCylinder(Vector3.zero, Vector3.forward, Vector3.right, height, radius1, sides, heightSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals, smooth);
            CreateCylinder(new Vector3(0.0f, (hDown - hUp) * 0.5f, 0.0f), Vector3.forward, Vector3.right, height - hUp - hDown, radius2, sides, heightSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, !flipNormals, smooth);
            if (cap1)
            {
                CreateCircle(centerDown, Vector3.forward, Vector3.right, radius1, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, !flipNormals);
                CreateCircle(centerDown2, Vector3.forward, Vector3.right, radius2, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals);
            }
            else
            {
                CreateRing(centerDown, Vector3.forward, Vector3.right, radius1, radius2, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, !flipNormals);
            }
            if (cap2)
            {
                CreateCircle(centerUp, Vector3.forward, Vector3.right, radius1, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals);
                CreateCircle(centerUp2, Vector3.forward, Vector3.right, radius2, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, !flipNormals);
            }
            else
            {
                CreateRing(centerUp, Vector3.forward, Vector3.right, radius1, radius2, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals);
            }
            if (sliceOn)
            {

                Vector3 centerFrom = new Vector3(Mathf.Sin(sliceFrom * deg2rad), 0.0f, Mathf.Cos(sliceFrom * deg2rad));
                Vector3 centerTo = new Vector3(Mathf.Sin(sliceTo * deg2rad), 0.0f, Mathf.Cos(sliceTo * deg2rad));
                CreatePlane(centerFrom * (radius2 + dif * 0.5f), Vector3.up, -centerFrom.normalized, dif, height, capSegs, heightSegs, generateMappingCoords, realWorldMapSize, Vector2.zero, new Vector2(0.5f, 1.0f), flipNormals);
                CreatePlane(centerTo * (radius2 + dif * 0.5f), Vector3.up, centerTo.normalized, dif, height, capSegs, heightSegs, generateMappingCoords, realWorldMapSize, new Vector2(0.5f, 0.0f), new Vector2(0.5f, 1.0f), flipNormals);
                if (cap1)
                {
                    CreatePlane(new Vector3(centerFrom.x * radius2 * 0.5f, -heightHalf + capThickness1 * 0.5f, centerFrom.z * radius2 * 0.5f), Vector3.up, -centerFrom.normalized, radius2, capThickness1, capSegs, 1, generateMappingCoords, realWorldMapSize, Vector2.zero, new Vector2(0.5f, 1.0f), flipNormals);
                    CreatePlane(new Vector3(centerTo.x * radius2 * 0.5f, -heightHalf + capThickness1 * 0.5f, centerTo.z * radius2 * 0.5f), Vector3.up, centerTo.normalized, radius2, capThickness1, capSegs, 1, generateMappingCoords, realWorldMapSize, new Vector2(0.5f, 0.0f), new Vector2(0.5f, 1.0f), flipNormals);
                }
                if (cap2)
                {
                    CreatePlane(new Vector3(centerFrom.x * radius2 * 0.5f, heightHalf - capThickness2 * 0.5f, centerFrom.z * radius2 * 0.5f), Vector3.up, -centerFrom.normalized, radius2, capThickness2, capSegs, 1, generateMappingCoords, realWorldMapSize, Vector2.zero, new Vector2(0.5f, 1.0f), flipNormals);
                    CreatePlane(new Vector3(centerTo.x * radius2 * 0.5f, heightHalf - capThickness2 * 0.5f, centerTo.z * radius2 * 0.5f), Vector3.up, centerTo.normalized, radius2, capThickness2, capSegs, 1, generateMappingCoords, realWorldMapSize, new Vector2(0.5f, 0.0f), new Vector2(0.5f, 1.0f), flipNormals);
                }
            }
        }
    }
}