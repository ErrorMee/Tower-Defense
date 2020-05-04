using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class ChamferPlane : PPBase
    {
        public float width = 2;
        public float length = 2;
        public float fillet = 0.4f;
        public int widthSegs = 10;
        public int lengthSegs = 10;
        public int filletSegs = 5;
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public bool flipNormals = false;

        private void Start()
        {
            m_mesh.name = "ChamferPlane";
        }

        protected override void CreateMesh()
        {
            length = Mathf.Clamp(length, 0.00001f, 10000.0f);
            width = Mathf.Clamp(width, 0.00001f, 10000.0f);
            float min = length < width ? length : width;
            fillet = Mathf.Clamp(fillet, 0.00001f, min / 2.0f);
            lengthSegs = Mathf.Clamp(lengthSegs, 1, 100);
            widthSegs = Mathf.Clamp(widthSegs, 1, 100);
            filletSegs = Mathf.Clamp(filletSegs, 1, 100);

            float lengthHalf = length * 0.5f;
            float widthHalf = width * 0.5f;
            float filletHalf = fillet * 0.5f;
            float lengthHalfFillet = lengthHalf - fillet;
            float widthHalfFillet = widthHalf - fillet;
            float lengthHalfFilletHalf = lengthHalf - filletHalf;
            float widthHalfFilletHalf = widthHalf - filletHalf;

            CreatePlane(Vector3.zero, Vector3.forward, Vector3.right, widthHalfFillet * 2, lengthHalfFillet * 2, widthSegs, lengthSegs, generateMappingCoords, realWorldMapSize, 
                new Vector2(filletHalf / widthHalf, filletHalf / lengthHalf), new Vector2(widthHalfFillet / widthHalf, lengthHalfFillet / lengthHalf), flipNormals);
            CreatePlane(new Vector3(0.0f, 0.0f, lengthHalfFilletHalf), Vector3.forward, Vector3.right, widthHalfFillet * 2, fillet, widthSegs, filletSegs, generateMappingCoords, realWorldMapSize,
                new Vector2(filletHalf / widthHalf, (length - fillet) / length), new Vector2(widthHalfFillet / widthHalf, fillet / length), flipNormals);
            CreatePlane(new Vector3(0.0f, 0.0f, -lengthHalfFilletHalf), Vector3.forward, Vector3.right, widthHalfFillet * 2, fillet, widthSegs, filletSegs, generateMappingCoords, realWorldMapSize,
                new Vector2(filletHalf / widthHalf, 0.0f), new Vector2(widthHalfFillet / widthHalf, fillet / length), flipNormals);
            CreatePlane(new Vector3(-widthHalfFilletHalf, 0.0f, 0.0f), Vector3.forward, Vector3.right, fillet, lengthHalfFillet * 2, filletSegs, lengthSegs, generateMappingCoords, realWorldMapSize,
                new Vector2(0.0f, filletHalf / lengthHalf), new Vector2(fillet / width, lengthHalfFillet / lengthHalf), flipNormals);
            CreatePlane(new Vector3(widthHalfFilletHalf, 0.0f, 0.0f), Vector3.forward, Vector3.right, fillet, lengthHalfFillet * 2, filletSegs, lengthSegs, generateMappingCoords, realWorldMapSize,
                new Vector2((width - fillet) / width, filletHalf / lengthHalf), new Vector2(fillet / width, lengthHalfFillet / lengthHalf), flipNormals);

            Vector2 tiling = new Vector2(fillet / widthHalf, fillet / lengthHalf);
            CreateCircle(new Vector3(widthHalf - fillet, 0.0f, lengthHalf - fillet), Vector3.forward, Vector3.right, fillet, filletSegs, filletSegs, true, 0.0f, 90.0f, generateMappingCoords, realWorldMapSize,
                new Vector2(widthHalfFillet / width, lengthHalfFillet / length), tiling, flipNormals);
            CreateCircle(new Vector3(widthHalf - fillet, 0.0f, -lengthHalfFillet), Vector3.forward, Vector3.right, fillet, filletSegs, filletSegs, true, 90.0f, 180.0f, generateMappingCoords, realWorldMapSize,
                new Vector2(widthHalfFillet / width, -lengthHalfFillet / length), tiling, flipNormals);
            CreateCircle(new Vector3(-widthHalfFillet, 0.0f, -lengthHalfFillet), Vector3.forward, Vector3.right, fillet, filletSegs, filletSegs, true, 180.0f, 270.0f, generateMappingCoords, realWorldMapSize,
                new Vector2(-widthHalfFillet / width, -lengthHalfFillet / length), tiling, flipNormals);
            CreateCircle(new Vector3(-widthHalfFillet, 0.0f, lengthHalf - fillet), Vector3.forward, Vector3.right, fillet, filletSegs, filletSegs, true, 270.0f, 360.0f, generateMappingCoords, realWorldMapSize,
                new Vector2(-widthHalfFillet / width, lengthHalfFillet / length), tiling, flipNormals);
        }
    }
}