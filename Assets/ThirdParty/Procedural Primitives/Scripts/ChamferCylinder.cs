using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class ChamferCylinder : PPBase
    {
        public float radius = 0.5f;
        public float height = 1.0f;
        public float fillet = 0.1f;
        public int sides = 20;
        public int capSegs = 2;
        public int heightSegs = 2;
        public int filletSegs = 3;
        public bool sliceOn = false;
        public float sliceFrom = 0.0f;
        public float sliceTo = 360.0f;
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public bool flipNormals = false;
        public bool smooth = true;

        private void Start()
        {
            m_mesh.name = "ChamferCylinder";
        }

        protected override void CreateMesh()
        {
            radius = Mathf.Clamp(radius, 0.00001f, 10000.0f);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            float min = radius < height / 2.0f ? radius : height / 2.0f;
            fillet = Mathf.Clamp(fillet, 0.00001f, min);
            sides = Mathf.Clamp(sides, 3, 100);
            capSegs = Mathf.Clamp(capSegs, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);
            filletSegs = Mathf.Clamp(filletSegs, 1, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);

            float heightHalf = height * 0.5f;
            float heightHalfFillet = heightHalf - fillet;
            float radiusFillet = radius - fillet;
            
            CreateCylinder(Vector3.zero, Vector3.forward, Vector3.right, heightHalfFillet * 2, radius, sides, heightSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals, smooth);
            CreateCircle(new Vector3(0.0f, heightHalf, 0.0f), Vector3.forward, Vector3.right, radiusFillet, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals);
            CreateCircle(new Vector3(0.0f, -heightHalf, 0.0f), Vector3.forward, Vector3.right, radiusFillet, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, !flipNormals);
            CreateTorus(new Vector3(0.0f, heightHalfFillet, 0.0f), Vector3.forward, Vector3.right, radiusFillet, fillet, sides, filletSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals, smooth, 180.0f, 270.0f);
            CreateTorus(new Vector3(0.0f, -heightHalfFillet, 0.0f), Vector3.forward, Vector3.right, radiusFillet, fillet, sides, filletSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals, smooth, 90.0f, 180.0f);

            if (sliceOn)
            {
                float filletHalf = fillet * 0.5f;
                float heightHalfFilletHalf = heightHalf - filletHalf;
                float radiusFilletHalf = radius - filletHalf;
                float rfh = radiusFillet * 0.5f;
                Vector3 centerFrom = new Vector3(Mathf.Sin(sliceFrom * deg2rad), 0.0f, Mathf.Cos(sliceFrom * deg2rad));
                Vector3 centerTo = new Vector3(Mathf.Sin(sliceTo * deg2rad), 0.0f, Mathf.Cos(sliceTo * deg2rad));
                Vector2 tiling = new Vector2(fillet / radius, fillet / heightHalf);

                CreatePlane(centerFrom * rfh, Vector3.up, -centerFrom.normalized, radiusFillet, heightHalfFillet * 2, capSegs, heightSegs, generateMappingCoords, realWorldMapSize,
                    new Vector2(filletHalf / radius, filletHalf / heightHalf), new Vector2(rfh / radius, heightHalfFillet / heightHalf), flipNormals);
                CreatePlane(new Vector3(centerFrom.x * rfh, heightHalfFilletHalf, centerFrom.z * rfh), Vector3.up, -centerFrom.normalized, radiusFillet, fillet, capSegs, filletSegs, generateMappingCoords, realWorldMapSize,
                    new Vector2(filletHalf / radius, (height - fillet) / height), new Vector2(rfh / radius, fillet / height), flipNormals);
                CreatePlane(new Vector3(centerFrom.x * rfh, -heightHalfFilletHalf, centerFrom.z * rfh), Vector3.up, -centerFrom.normalized, radiusFillet, fillet, capSegs, filletSegs, generateMappingCoords, realWorldMapSize,
                    new Vector2(filletHalf / radius, 0.0f), new Vector2(rfh / radius, fillet / height), flipNormals);
                CreatePlane(centerFrom * radiusFilletHalf, Vector3.up, -centerFrom.normalized, fillet, heightHalfFillet * 2, filletSegs, heightSegs, generateMappingCoords, realWorldMapSize,
                    new Vector2(0.0f, filletHalf / heightHalf), new Vector2(filletHalf / radius, heightHalfFillet / heightHalf), flipNormals);
                
                CreateCircle(new Vector3(centerFrom.x * radiusFillet, heightHalfFillet, centerFrom.z * radiusFillet), Vector3.up, -centerFrom.normalized, fillet, filletSegs, filletSegs, true, 270.0f, 360.0f, generateMappingCoords, realWorldMapSize,
                    new Vector2(-radiusFillet / (radius * 2), heightHalfFillet / height), tiling, flipNormals);
                CreateCircle(new Vector3(centerFrom.x * radiusFillet, -heightHalfFillet, centerFrom.z * radiusFillet), Vector3.up, -centerFrom.normalized, fillet, filletSegs, filletSegs, true, 180.0f, 270.0f, generateMappingCoords, realWorldMapSize,
                    new Vector2(-radiusFillet / (radius * 2), -heightHalfFillet / height), tiling, flipNormals);

                CreatePlane(centerTo * rfh, Vector3.up, centerTo.normalized, radiusFillet, heightHalfFillet * 2, capSegs, heightSegs, generateMappingCoords, realWorldMapSize,
                    new Vector2(0.5f, filletHalf / heightHalf), new Vector2(rfh / radius, heightHalfFillet / heightHalf), flipNormals);
                CreatePlane(new Vector3(centerTo.x * rfh, heightHalfFilletHalf, centerTo.z * rfh), Vector3.up, centerTo.normalized, radiusFillet, fillet, capSegs, filletSegs, generateMappingCoords, realWorldMapSize,
                    new Vector2(0.5f, (height - fillet) / height), new Vector2(rfh / radius, fillet / height), flipNormals);
                CreatePlane(new Vector3(centerTo.x * rfh, -heightHalfFilletHalf, centerTo.z * rfh), Vector3.up, centerTo.normalized, radiusFillet, fillet, capSegs, filletSegs, generateMappingCoords, realWorldMapSize,
                    new Vector2(0.5f, 0.0f), new Vector2(rfh / radius, fillet / height), flipNormals);
                CreatePlane(centerTo * radiusFilletHalf, Vector3.up, centerTo.normalized, fillet, heightHalfFillet * 2, filletSegs, heightSegs, generateMappingCoords, realWorldMapSize,
                    new Vector2(radiusFilletHalf / radius, filletHalf / heightHalf), new Vector2(filletHalf / radius, heightHalfFillet / heightHalf), flipNormals);

                CreateCircle(new Vector3(centerTo.x * radiusFillet, heightHalfFillet, centerTo.z * radiusFillet), Vector3.up, centerTo.normalized, fillet, filletSegs, filletSegs, true, 0.0f, 90.0f, generateMappingCoords, realWorldMapSize,
                    new Vector2(radiusFillet / (radius * 2), heightHalfFillet / height), tiling, flipNormals);
                CreateCircle(new Vector3(centerTo.x * radiusFillet, -heightHalfFillet, centerTo.z * radiusFillet), Vector3.up, centerTo.normalized, fillet, filletSegs, filletSegs, true, 90.0f, 180.0f, generateMappingCoords, realWorldMapSize,
                    new Vector2(radiusFillet / (radius * 2), -heightHalfFillet / height), tiling, flipNormals);
            }
        }
    }
}