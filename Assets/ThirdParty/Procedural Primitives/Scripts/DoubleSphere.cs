using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class DoubleSphere : PPBase
    {
        public float radius1 = 0.5f;
        public float radius2 = 0.3f;
        public int segments = 24;
        public bool sliceOn = true;
        public float sliceFrom = 90.0f;
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
            m_mesh.name = "DoubleSphere";
        }

        protected override void CreateMesh()
        {
            radius1 = Mathf.Clamp(radius1, 0.00001f, 10000.0f);
            radius2 = Mathf.Clamp(radius2, 0.00001f, radius1);
            segments = Mathf.Clamp(segments, 4, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);
            cutFrom = Mathf.Clamp01(cutFrom);
            cutTo = Mathf.Clamp(cutTo, cutFrom, 1.0f);
            
            float min = (radius1 - radius2) / (radius1 * 2);
            float max = (radius1 + radius2) / (radius1 * 2);
            float cutFrom2 = Mathf.Clamp01((cutFrom - min) / (max - min));
            float cutTo2 = Mathf.Clamp01((cutTo - min) / (max - min));
            float cf = pi - Mathf.Acos((cutFrom - 0.5f) * 2.0f);
            float ct = pi - Mathf.Acos((cutTo - 0.5f) * 2.0f);
            float cf2 = pi - Mathf.Acos((cutFrom2 - 0.5f) * 2.0f);
            float ct2 = pi - Mathf.Acos((cutTo2 - 0.5f) * 2.0f);

            CreateSphere(Vector3.zero, Vector3.forward, Vector3.right, radius1, segments, segments / 2, sliceOn, sliceFrom, sliceTo, hemiSphere, cf, ct, generateMappingCoords, realWorldMapSize, flipNormals, smooth);
            CreateSphere(Vector3.zero, Vector3.forward, Vector3.right, radius2, segments, segments / 2, sliceOn, sliceFrom, sliceTo, hemiSphere, cf2, ct2, generateMappingCoords, realWorldMapSize, !flipNormals, smooth);

            if (sliceOn)
            {
                Vector3 centerFrom = new Vector3(Mathf.Sin(sliceFrom * deg2rad), 0.0f, Mathf.Cos(sliceFrom * deg2rad)) * radius1 * 0.5f;
                Vector3 centerTo = new Vector3(Mathf.Sin(sliceTo * deg2rad), 0.0f, Mathf.Cos(sliceTo * deg2rad)) * radius1 * 0.5f;
                CreateHemiRing(Vector3.zero, Vector3.up, centerFrom.normalized, radius1, radius2, segments / 2, 1, hemiSphere, cf, ct, generateMappingCoords, realWorldMapSize, flipNormals);
                CreateHemiRing(Vector3.zero, Vector3.up, centerTo.normalized, radius1, radius2, segments / 2, 1, hemiSphere, cf, ct, generateMappingCoords, realWorldMapSize, Vector2.zero, new Vector2(-1.0f, 1.0f), !flipNormals);
            }

            if (hemiSphere)
            {
                Vector2 sincosFrom = new Vector2(Mathf.Sin(cf), -Mathf.Cos(cf));
                Vector2 sincosTo = new Vector2(Mathf.Sin(ct), -Mathf.Cos(ct));
                Vector2 sincosFrom2 = new Vector2(Mathf.Sin(cf2), -Mathf.Cos(cf2));
                Vector2 sincosTo2 = new Vector2(Mathf.Sin(ct2), -Mathf.Cos(ct2));
                CreateRing(new Vector3(0.0f, sincosFrom.y * radius1, 0.0f), Vector3.forward, Vector3.right, radius1 * sincosFrom.x, radius2 * sincosFrom2.x, segments, 1, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, !flipNormals);
                CreateRing(new Vector3(0.0f, sincosTo.y * radius1, 0.0f), Vector3.forward, Vector3.right, radius1 * sincosTo.x, radius2 * sincosTo2.x, segments, 1, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, flipNormals);
            }
        }
    }
}