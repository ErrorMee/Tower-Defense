using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Sphere : PPBase
    {
        public float radius = 0.5f;
        public int segments = 24;
        public bool sliceOn = false;
        public float sliceFrom = 0.0f;
        public float sliceTo = 360.0f;
        public bool hemiSphere = false;
        public float cutFrom = 0.0f;
        public float cutTo = 1.0f;
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public bool flipNormals = false;
        public bool smooth = true;

        private void Start()
        {
            m_mesh.name = "Sphere";
        }

        protected override void CreateMesh()
        {
            radius = Mathf.Clamp(radius, 0.00001f, 10000.0f);
            segments = Mathf.Clamp(segments, 4, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);
            cutFrom = Mathf.Clamp01(cutFrom);
            cutTo = Mathf.Clamp(cutTo, cutFrom, 1.0f);

            float cf = pi - Mathf.Acos((cutFrom - 0.5f) * 2.0f);
            float ct = pi - Mathf.Acos((cutTo - 0.5f) * 2.0f);

            CreateSphere(Vector3.zero, Vector3.forward, Vector3.right, radius, segments, segments / 2, sliceOn, sliceFrom, sliceTo, hemiSphere, cf, ct, generateMappingCoords, realWorldMapSize, flipNormals, smooth);

            if (sliceOn)
            {
                Vector3 centerFrom = new Vector3(Mathf.Sin(sliceFrom * deg2rad), 0.0f, Mathf.Cos(sliceFrom * deg2rad)) * radius * 0.5f;
                Vector3 centerTo = new Vector3(Mathf.Sin(sliceTo * deg2rad), 0.0f, Mathf.Cos(sliceTo * deg2rad)) * radius * 0.5f;
                CreateHemiCircle(Vector3.zero, Vector3.up, centerFrom.normalized, radius, segments / 2, 1, hemiSphere, cf, ct, generateMappingCoords, realWorldMapSize, flipNormals);
                CreateHemiCircle(Vector3.zero, Vector3.up, centerTo.normalized, radius, segments / 2, 1, hemiSphere, cf, ct, generateMappingCoords, realWorldMapSize, Vector2.zero, new Vector2(-1.0f, 1.0f), !flipNormals);
            }

            if (hemiSphere)
            {
                Vector2 sincosFrom = new Vector2(Mathf.Sin(cf), -Mathf.Cos(cf));
                Vector2 sincosTo = new Vector2(Mathf.Sin(ct), -Mathf.Cos(ct));
                CreateCircle(new Vector3(0.0f, sincosFrom.y * radius, 0.0f), Vector3.forward, Vector3.right, radius * sincosFrom.x, segments, 1, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, !flipNormals);
                CreateCircle(new Vector3(0.0f, sincosTo.y * radius, 0.0f), Vector3.forward, Vector3.right, radius * sincosTo.x, segments, 1, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals);
            }
        }
    }
}