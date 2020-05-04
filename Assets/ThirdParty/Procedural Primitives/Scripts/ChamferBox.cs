using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class ChamferBox : PPBase
    {
        public float width = 1.0f;
        public float length = 1.0f;
        public float height = 1.0f;
        public float fillet = 0.1f;
        public int widthSegs = 2;
        public int lengthSegs = 2;
        public int heightSegs = 2;
        public int filletSegs = 3;
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public bool flipNormals = false;
        public bool smooth = true;

        private void Start()
        {
            m_mesh.name = "Chamfer Box";
        }

        protected override void CreateMesh()
        {
            length = Mathf.Clamp(length, 0.00001f, 10000.0f);
            width = Mathf.Clamp(width, 0.00001f, 10000.0f);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            float min = length < width ? length : width;
            min = min < height ? min : height;
            fillet = Mathf.Clamp(fillet, 0.00001f, min / 2.0f);
            lengthSegs = Mathf.Clamp(lengthSegs, 1, 100);
            widthSegs = Mathf.Clamp(widthSegs, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);
            filletSegs = Mathf.Clamp(filletSegs, 1, 100);

            float lengthHalf = length * 0.5f;
            float widthHalf = width * 0.5f;
            float heightHalf = height * 0.5f;
            float angle = pi * 2.0f / (filletSegs * 4);
            float lengthHalfFillet = lengthHalf - fillet;
            float widthHalfFillet = widthHalf - fillet;
            float heightHalfFillet = heightHalf - fillet;
            float lengthFillet = lengthHalfFillet * 2.0f;
            float widthFillet = widthHalfFillet * 2.0f;
            float heightFillet = heightHalfFillet * 2.0f;

            Vector2 tilingCylinder = new Vector2(0.25f, 1.0f);
            Vector2 tilingSphere = new Vector2(0.25f, 1.0f);

            CreatePlane(new Vector3(0.0f, 0.0f, -lengthHalf), Vector3.up,      Vector3.right,   widthFillet,  heightFillet, widthSegs,  heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane(new Vector3(0.0f, 0.0f, lengthHalf),  Vector3.up,      Vector3.left,    widthFillet,  heightFillet, widthSegs,  heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane(new Vector3(-widthHalf, 0.0f, 0.0f),  Vector3.up,      Vector3.back,    lengthFillet, heightFillet, lengthSegs, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane(new Vector3(widthHalf, 0.0f, 0.0f),   Vector3.up,      Vector3.forward, lengthFillet, heightFillet, lengthSegs, heightSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane(new Vector3(0.0f, heightHalf, 0.0f),  Vector3.forward, Vector3.right,   widthFillet,  lengthFillet, widthSegs,  lengthSegs, generateMappingCoords, realWorldMapSize, flipNormals);
            CreatePlane(new Vector3(0.0f, -heightHalf, 0.0f), Vector3.forward, Vector3.left,    widthFillet,  lengthFillet, widthSegs,  lengthSegs, generateMappingCoords, realWorldMapSize, flipNormals);

            CreateCylinder(new Vector3(widthHalfFillet, 0.0f, lengthHalfFillet),   Vector3.forward, Vector3.right, heightFillet, fillet, filletSegs, heightSegs, true, 0.0f,   90.0f,  generateMappingCoords, realWorldMapSize, new Vector2(0.0f, 0.0f),  tilingCylinder, flipNormals, smooth);
            CreateCylinder(new Vector3(widthHalfFillet, 0.0f, -lengthHalfFillet),  Vector3.forward, Vector3.right, heightFillet, fillet, filletSegs, heightSegs, true, 90.0f,  180.0f, generateMappingCoords, realWorldMapSize, new Vector2(0.25f, 0.0f), tilingCylinder, flipNormals, smooth);
            CreateCylinder(new Vector3(-widthHalfFillet, 0.0f, -lengthHalfFillet), Vector3.forward, Vector3.right, heightFillet, fillet, filletSegs, heightSegs, true, 180.0f, 270.0f, generateMappingCoords, realWorldMapSize, new Vector2(0.5f, 0.0f),  tilingCylinder, flipNormals, smooth);
            CreateCylinder(new Vector3(-widthHalfFillet, 0.0f, lengthHalfFillet),  Vector3.forward, Vector3.right, heightFillet, fillet, filletSegs, heightSegs, true, 270.0f, 360.0f, generateMappingCoords, realWorldMapSize, new Vector2(0.75f, 0.0f), tilingCylinder, flipNormals, smooth);

            CreateCylinder(new Vector3(widthHalfFillet,  -heightHalfFillet, 0.0f), Vector3.down, Vector3.right, lengthFillet, fillet, filletSegs, lengthSegs, true, 0.0f,   90.0f,  generateMappingCoords, realWorldMapSize, new Vector2(0.0f, 0.0f),  tilingCylinder, flipNormals, smooth);
            CreateCylinder(new Vector3(widthHalfFillet,  heightHalfFillet, 0.0f),  Vector3.down, Vector3.right, lengthFillet, fillet, filletSegs, lengthSegs, true, 90.0f,  180.0f, generateMappingCoords, realWorldMapSize, new Vector2(0.25f, 0.0f), tilingCylinder, flipNormals, smooth);
            CreateCylinder(new Vector3(-widthHalfFillet, heightHalfFillet, 0.0f),  Vector3.down, Vector3.right, lengthFillet, fillet, filletSegs, lengthSegs, true, 180.0f, 270.0f, generateMappingCoords, realWorldMapSize, new Vector2(0.5f, 0.0f),  tilingCylinder, flipNormals, smooth);
            CreateCylinder(new Vector3(-widthHalfFillet, -heightHalfFillet, 0.0f), Vector3.down, Vector3.right, lengthFillet, fillet, filletSegs, lengthSegs, true, 270.0f, 360.0f, generateMappingCoords, realWorldMapSize, new Vector2(0.75f, 0.0f), tilingCylinder, flipNormals, smooth);

            CreateCylinder(new Vector3(0.0f, -heightHalfFillet, lengthHalfFillet),  Vector3.forward, Vector3.down, widthFillet, fillet, filletSegs, widthSegs, true, 0.0f,   90.0f,  generateMappingCoords, realWorldMapSize, new Vector2(0.0f, 0.0f),  tilingCylinder, flipNormals, smooth);
            CreateCylinder(new Vector3(0.0f, -heightHalfFillet, -lengthHalfFillet), Vector3.forward, Vector3.down, widthFillet, fillet, filletSegs, widthSegs, true, 90.0f,  180.0f, generateMappingCoords, realWorldMapSize, new Vector2(0.25f, 0.0f), tilingCylinder, flipNormals, smooth);
            CreateCylinder(new Vector3(0.0f, heightHalfFillet, -lengthHalfFillet),  Vector3.forward, Vector3.down, widthFillet, fillet, filletSegs, widthSegs, true, 180.0f, 270.0f, generateMappingCoords, realWorldMapSize, new Vector2(0.5f, 0.0f),  tilingCylinder, flipNormals, smooth);
            CreateCylinder(new Vector3(0.0f, heightHalfFillet, lengthHalfFillet),   Vector3.forward, Vector3.down, widthFillet, fillet, filletSegs, widthSegs, true, 270.0f, 360.0f, generateMappingCoords, realWorldMapSize, new Vector2(0.75f, 0.0f), tilingCylinder, flipNormals, smooth);

            CreateSphere(new Vector3(widthHalfFillet,  heightHalfFillet, lengthHalfFillet),  Vector3.forward, Vector3.right, fillet, filletSegs, filletSegs * 2, true, 0.0f,   90.0f,  true, pi * 0.5f, pi, generateMappingCoords, realWorldMapSize, new Vector2(0.0f,  0.0f), tilingSphere, flipNormals, smooth);
            CreateSphere(new Vector3(widthHalfFillet,  heightHalfFillet, -lengthHalfFillet), Vector3.forward, Vector3.right, fillet, filletSegs, filletSegs * 2, true, 90.0f,  180.0f, true, pi * 0.5f, pi, generateMappingCoords, realWorldMapSize, new Vector2(0.25f, 0.0f), tilingSphere, flipNormals, smooth);
            CreateSphere(new Vector3(-widthHalfFillet, heightHalfFillet, -lengthHalfFillet), Vector3.forward, Vector3.right, fillet, filletSegs, filletSegs * 2, true, 180.0f, 270.0f, true, pi * 0.5f, pi, generateMappingCoords, realWorldMapSize, new Vector2(0.5f,  0.0f), tilingSphere, flipNormals, smooth);
            CreateSphere(new Vector3(-widthHalfFillet, heightHalfFillet, lengthHalfFillet),  Vector3.forward, Vector3.right, fillet, filletSegs, filletSegs * 2, true, 270.0f, 360.0f, true, pi * 0.5f, pi, generateMappingCoords, realWorldMapSize, new Vector2(0.75f, 0.0f), tilingSphere, flipNormals, smooth);
            
            CreateSphere(new Vector3(widthHalfFillet,  -heightHalfFillet, lengthHalfFillet),  Vector3.forward, Vector3.right, fillet, filletSegs, filletSegs * 2, true, 0.0f,   90.0f,  true, 0.0f, pi * 0.5f, generateMappingCoords, realWorldMapSize, new Vector2(0.0f,  0.0f), tilingSphere, flipNormals, smooth);
            CreateSphere(new Vector3(widthHalfFillet,  -heightHalfFillet, -lengthHalfFillet), Vector3.forward, Vector3.right, fillet, filletSegs, filletSegs * 2, true, 90.0f,  180.0f, true, 0.0f, pi * 0.5f, generateMappingCoords, realWorldMapSize, new Vector2(0.25f, 0.0f), tilingSphere, flipNormals, smooth);
            CreateSphere(new Vector3(-widthHalfFillet, -heightHalfFillet, -lengthHalfFillet), Vector3.forward, Vector3.right, fillet, filletSegs, filletSegs * 2, true, 180.0f, 270.0f, true, 0.0f, pi * 0.5f, generateMappingCoords, realWorldMapSize, new Vector2(0.5f,  0.0f), tilingSphere, flipNormals, smooth);
            CreateSphere(new Vector3(-widthHalfFillet, -heightHalfFillet, lengthHalfFillet),  Vector3.forward, Vector3.right, fillet, filletSegs, filletSegs * 2, true, 270.0f, 360.0f, true, 0.0f, pi * 0.5f, generateMappingCoords, realWorldMapSize, new Vector2(0.75f, 0.0f), tilingSphere, flipNormals, smooth);
        }
    }
}