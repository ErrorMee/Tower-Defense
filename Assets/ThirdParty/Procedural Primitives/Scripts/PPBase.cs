using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ProceduralPrimitivesUtil
{
    [ExecuteInEditMode]
    public class PPBase : MonoBehaviour
    {
        public const float pi = 3.14159f;
        public const float halfPi = pi * 0.5f;
        public const float deg2rad = pi / 180.0f;

        protected List<Vector3> m_vertices = new List<Vector3>();
        protected List<int> m_triangles = new List<int>();
        protected List<Vector3> m_normals = new List<Vector3>();
        protected List<Vector2> m_uv = new List<Vector2>();
        protected Mesh m_mesh;
        public Mesh mesh { get { return m_mesh; } }

        public static float PointLineDistance(Vector3 vp, Vector3 v)
        {
            return Vector3.Cross(vp, v).magnitude / v.magnitude;
        }

        public static float PointLineProjectionn(Vector3 vp, Vector3 v)
        {
            return Vector3.Dot(vp, v) / v.magnitude;
        }

        protected void CreateTriangle(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            int index = m_vertices.Count;
            Vector3 normal = Vector3.Cross(p1 - p0, p2 - p0).normalized;

            m_vertices.Add(p0);
            m_vertices.Add(p1);
            m_vertices.Add(p2);

            m_normals.Add(normal);
            m_normals.Add(normal);
            m_normals.Add(normal);

            m_triangles.Add(index);
            m_triangles.Add(index + 1);
            m_triangles.Add(index + 2);
        }

        protected void CreateTriangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector2 p0uv, Vector2 p1uv, Vector2 p2uv)
        {
            int index = m_vertices.Count;
            Vector3 normal = Vector3.Cross(p1 - p0, p2 - p0).normalized;

            m_vertices.Add(p0);
            m_vertices.Add(p1);
            m_vertices.Add(p2);

            m_uv.Add(p0uv);
            m_uv.Add(p1uv);
            m_uv.Add(p2uv);

            m_normals.Add(normal);
            m_normals.Add(normal);
            m_normals.Add(normal);

            m_triangles.Add(index);
            m_triangles.Add(index + 1);
            m_triangles.Add(index + 2);
        }

        protected void CreateQuad(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            int index = m_vertices.Count;
            Vector3 normal = Vector3.Cross(p2 - p0, p3 - p1).normalized;

            m_vertices.Add(p0);
            m_vertices.Add(p1);
            m_vertices.Add(p2);
            m_vertices.Add(p3);

            m_normals.Add(normal);
            m_normals.Add(normal);
            m_normals.Add(normal);
            m_normals.Add(normal);

            m_triangles.Add(index);
            m_triangles.Add(index + 1);
            m_triangles.Add(index + 2);
            m_triangles.Add(index + 2);
            m_triangles.Add(index + 3);
            m_triangles.Add(index);
        }

        protected void CreateQuad(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector2 p0uv, Vector2 p1uv, Vector2 p2uv, Vector2 p3uv)
        {
            int index = m_vertices.Count;
            Vector3 normal = Vector3.Cross(p2 - p0, p3 - p1).normalized;

            m_vertices.Add(p0);
            m_vertices.Add(p1);
            m_vertices.Add(p2);
            m_vertices.Add(p3);

            m_uv.Add(p0uv);
            m_uv.Add(p1uv);
            m_uv.Add(p2uv);
            m_uv.Add(p3uv);

            m_normals.Add(normal);
            m_normals.Add(normal);
            m_normals.Add(normal);
            m_normals.Add(normal);

            m_triangles.Add(index);
            m_triangles.Add(index + 1);
            m_triangles.Add(index + 2);
            m_triangles.Add(index + 2);
            m_triangles.Add(index + 3);
            m_triangles.Add(index);
        }

        protected void CreateTriangle(Vector3 center, Vector3 forward, Vector3 right, float width, float length, float offset, int seg, bool generateUV, bool realWorldMapSize, bool flip)
        {
            CreateTriangle(center, forward, right, width, length, offset, seg, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip);
        }

        protected void CreateTriangle(Vector3 center, Vector3 forward, Vector3 right, float width, float length, float offset, int seg, bool generateUV, bool realWorldMapSize, Vector2 uvOffset, Vector2 uvTiling, bool flip)
        {
            Vector3 up = Vector3.Cross(forward, right).normalized;
            float lengthHalf = length * 0.5f;
            float widthHalf = width * 0.5f;
            float c = 0.0f;
            if (offset < -widthHalf) c = (offset + widthHalf) * 0.5f;
            else if (offset > widthHalf) c = (offset - widthHalf) * 0.5f;

            Vector3 p0 = center + right * (-widthHalf - c) - forward * lengthHalf;
            Vector3 p1 = center + right * (offset - c) + forward * lengthHalf;
            Vector3 p2 = center + right * (widthHalf - c) - forward * lengthHalf;

            if (realWorldMapSize) { uvTiling.x *= width; uvTiling.y *= length; }
            if (flip) CreateTriangle(p2, p1, p0, -up, seg, generateUV, uvOffset, uvTiling);
            else CreateTriangle(p0, p1, p2, up, seg, generateUV, uvOffset, uvTiling);
        }

        protected void CreateTriangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 normal, int seg, bool generateUV, Vector2 uvOffset, Vector2 uvTiling)
        {
            int index = m_vertices.Count;
            Vector3 vL = (p1 - p0) / seg;
            Vector3 vW = (p2 - p0) / seg;

            if (generateUV)
            {
                Vector2 vWuv = Vector2.right * uvTiling.x / seg;
                Vector2 vLuv = new Vector2();
                vLuv.x = PointLineProjectionn(vL, vW) / vW.magnitude * uvTiling.x / seg;
                vLuv.y = uvTiling.y / seg;
                for (int i = 0; i <= seg; ++i)
                {
                    for (int j = 0; j <= seg - i; ++j)
                    {
                        m_vertices.Add(p0 + vL * i + vW * j);
                        m_normals.Add(normal);
                        m_uv.Add(uvOffset + vLuv * i + vWuv * j);
                    }
                }
            }
            else
            {
                for (int i = 0; i <= seg; ++i)
                {
                    for (int j = 0; j <= seg - i; ++j)
                    {
                        m_vertices.Add(p0 + vL * i + vW * j);
                        m_normals.Add(normal);
                    }
                }
            }

            int temp = 0;
            for (int i = 0; i < seg; ++i)
            {
                int next = seg + 1 - i;
                for (int j = 0; j < seg - i - 1; ++j)
                {
                    m_triangles.Add(index + temp + j);
                    m_triangles.Add(index + temp + next + j);
                    m_triangles.Add(index + temp + j + 1);
                    m_triangles.Add(index + temp + j + 1);
                    m_triangles.Add(index + temp + next + j);
                    m_triangles.Add(index + temp + next + j + 1);
                }
                m_triangles.Add(index + temp + seg - i - 1);
                m_triangles.Add(index + temp + next + seg - i - 1);
                m_triangles.Add(index + temp + seg - i);
                temp += next;
            }
        }

        protected void CreateTriangle(Vector3 center, Vector3 forward, Vector3 right, float width, float length, float offset, int segW, int segL, bool generateUV, bool realWorldMapSize, bool flip)
        {
            CreateTriangle(center, forward, right, width, length, offset, segW, segL, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip);
        }

        protected void CreateTriangle(Vector3 center, Vector3 forward, Vector3 right, float width, float length, float offset, int segW, int segL, bool generateUV, bool realWorldMapSize, Vector2 uvOffset, Vector2 uvTiling, bool flip)
        {
            Vector3 up = Vector3.Cross(forward, right).normalized;
            float lengthHalf = length * 0.5f;
            float widthHalf = width * 0.5f;
            float c = 0.0f;
            if (offset < -widthHalf) c = (offset + widthHalf) * 0.5f;
            else if (offset > widthHalf) c = (offset - widthHalf) * 0.5f;

            Vector3 p0 = center + right * (-widthHalf - c) - forward * lengthHalf;
            Vector3 p1 = center + right * (offset - c) + forward * lengthHalf;
            Vector3 p2 = center + right * (widthHalf - c) - forward * lengthHalf;

            if (realWorldMapSize) { uvTiling.x *= width; uvTiling.y *= length; }
            if (flip) CreateTriangle(p2, p1, p0, -up, segW, segL, generateUV, uvOffset, uvTiling);
            else CreateTriangle(p0, p1, p2, up, segW, segL, generateUV, uvOffset, uvTiling);
        }

        protected void CreateTriangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 normal, int segW, int segL, bool generateUV, Vector2 uvOffset, Vector2 uvTiling)
        {
            int index = m_vertices.Count;
            Vector3 vW = (p2 - p0) / segW;

            if (generateUV)
            {
                Vector2 vWuv = Vector2.right * uvTiling.x / segW;
                Vector2 vLuv = Vector2.down * uvTiling.y / segL;
                for (int i = 0; i <= segW; ++i)
                {
                    Vector3 vL = p0 + vW * i;
                    for (int j = 0; j <= segL; ++j)
                    {
                        Vector3 p = p1 * (segL - j) / segL + vL * j / segL;
                        m_vertices.Add(p);
                        m_normals.Add(normal);
                        m_uv.Add(uvOffset + vWuv * i + vLuv * j);
                    }
                }
            }
            else
            {
                for (int i = 0; i <= segW; ++i)
                {
                    Vector3 vL = p0 + vW * i;
                    for (int j = 0; j <= segL; ++j)
                    {
                        Vector3 p = p1 * (segL - j) / segL + vL * j / segL;
                        m_vertices.Add(p);
                        m_normals.Add(normal);
                    }
                }
            }

            for (int i = 0; i < segW; ++i)
            {
                m_triangles.Add(index + (segL + 1) * (i + 1) + 1);
                m_triangles.Add(index + (segL + 1) * i + 1);
                m_triangles.Add(index + (segL + 1) * i);
                for (int j = 1; j < segL; ++j)
                {
                    m_triangles.Add(index + (segL + 1) * i + j);
                    m_triangles.Add(index + (segL + 1) * (i + 1) + j);
                    m_triangles.Add(index + (segL + 1) * (i + 1) + (j + 1));
                    m_triangles.Add(index + (segL + 1) * (i + 1) + (j + 1));
                    m_triangles.Add(index + (segL + 1) * i + (j + 1));
                    m_triangles.Add(index + (segL + 1) * i + j);
                }
            }
        }

        protected void CreatePlane(Vector3 center, Vector3 forward, Vector3 right, float width, float length, int segW, int segL, bool generateUV, bool realWorldMapSize, bool flip)
        {
            CreatePlane(center, forward, right, width, length, segW, segL, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip);
        }

        protected void CreatePlane(Vector3 center, Vector3 forward, Vector3 right, float width, float length, int segW, int segL, bool generateUV, bool realWorldMapSize, Vector2 uvOffset, Vector2 uvTiling, bool flip)
        {
            Vector3 up = Vector3.Cross(forward, right).normalized;
            float lengthHalf = length * 0.5f;
            float widthHalf = width * 0.5f;

            Vector3 p0 = center - right * widthHalf - forward * lengthHalf;
            Vector3 p1 = center - right * widthHalf + forward * lengthHalf;
            Vector3 p2 = center + right * widthHalf + forward * lengthHalf;
            Vector3 p3 = center + right * widthHalf - forward * lengthHalf;

            if (realWorldMapSize) { uvTiling.x *= width; uvTiling.y *= length; }
            if (flip) CreatePlane(p3, p2, p1, p0, -up, segW, segL, generateUV, uvOffset, uvTiling);
            else CreatePlane(p0, p1, p2, p3, up, segW, segL, generateUV, uvOffset, uvTiling);
        }

        protected void CreatePlane(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 normal, int segW, int segL, bool generateUV, Vector2 uvOffset, Vector2 uvTiling)
        {
            int index = m_vertices.Count;
            Vector3 vW = (p3 - p0) / segW;
            Vector3 vL = (p1 - p0) / segL;

            if (generateUV)
            {
                Vector2 vWuv = Vector2.right * uvTiling.x / segW;
                Vector2 vLuv = Vector2.up * uvTiling.y / segL;
                for (int i = 0; i <= segW; ++i)
                {
                    for (int j = 0; j <= segL; ++j)
                    {
                        m_vertices.Add(p0 + vW * i + vL * j);
                        m_normals.Add(normal);
                        m_uv.Add(uvOffset + vWuv * i + vLuv * j);
                    }
                }
            }
            else
            {
                for (int i = 0; i <= segW; ++i)
                {
                    for (int j = 0; j <= segL; ++j)
                    {
                        m_vertices.Add(p0 + vW * i + vL * j);
                        m_normals.Add(normal);
                    }
                }
            }

            for (int i = 0; i < segW; ++i)
            {
                for (int j = 0; j < segL; ++j)
                {
                    m_triangles.Add(index + (segL + 1) * i + j);
                    m_triangles.Add(index + (segL + 1) * i + (j + 1));
                    m_triangles.Add(index + (segL + 1) * (i + 1) + (j + 1));
                    m_triangles.Add(index + (segL + 1) * (i + 1) + (j + 1));
                    m_triangles.Add(index + (segL + 1) * (i + 1) + j);
                    m_triangles.Add(index + (segL + 1) * i + j);
                }
            }
        }

        protected void CreateTrapezoid(Vector3 center, Vector3 forward, Vector3 right, float width1, float width2, float length, float offset, int segW, int segL, bool generateUV, bool realWorldMapSize, bool flip)
        {
            CreateTrapezoid(center, forward, right, width1, width2, length, offset, segW, segL, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip);
        }

        protected void CreateTrapezoid(Vector3 center, Vector3 forward, Vector3 right, float width1, float width2, float length, float offset, int segW, int segL, bool generateUV, bool realWorldMapSize, Vector2 uvOffset, Vector2 uvTiling, bool flip)
        {
            Vector3 up = Vector3.Cross(forward, right).normalized;
            float lengthHalf = length * 0.5f;
            float widthHalf1 = width1 * 0.5f;
            float widthHalf2 = width2 * 0.5f;
            float c = 0.0f;
            if (width1 > width2)
            {
                if (offset + widthHalf2 > widthHalf1) c = (offset + widthHalf2 - widthHalf1) * 0.5f;
                else if (offset - widthHalf2 < -widthHalf1) c = (offset - widthHalf2 + widthHalf1) * 0.5f;
            }
            else
            {
                if (offset + widthHalf1 > widthHalf2) c = (offset + widthHalf1 - widthHalf2) * 0.5f;
                else if (offset - widthHalf1 < -widthHalf2) c = (offset - widthHalf1 + widthHalf2) * 0.5f;
            }

            Vector3 p0 = center + right * (-widthHalf1 - c) - forward * lengthHalf;
            Vector3 p1 = center + right * (-widthHalf2 + offset - c) + forward * lengthHalf;
            Vector3 p2 = center + right * (widthHalf2 + offset - c) + forward * lengthHalf;
            Vector3 p3 = center + right * (widthHalf1 - c) - forward * lengthHalf;

            if (realWorldMapSize) { uvTiling.x *= (width1 + width2) * 0.5f; uvTiling.y *= length; }
            if (flip) CreateTrapezoid(p3, p2, p1, p0, -up, segW, segL, generateUV, uvOffset, uvTiling);
            else CreateTrapezoid(p0, p1, p2, p3, up, segW, segL, generateUV, uvOffset, uvTiling);
        }

        protected void CreateTrapezoid(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 normal, int segW, int segL, bool generateUV, Vector2 uvOffset, Vector2 uvTiling)
        {
            int index = m_vertices.Count;
            Vector3 vUp = (p2 - p1) / segW;
            Vector3 vDown = (p3 - p0) / segW;

            if (generateUV)
            {
                Vector2 vWuv = Vector2.right * uvTiling.x / segW;
                Vector2 vLuv = Vector2.down * uvTiling.y / segL;
                for (int i = 0; i <= segW; ++i)
                {
                    Vector3 v1 = p1 + vUp * i;
                    Vector3 v2 = p0 + vDown * i;
                    for (int j = 0; j <= segL; ++j)
                    {
                        Vector3 p = v1 * (segL - j) / segL + v2 * j / segL;
                        m_vertices.Add(p);
                        m_normals.Add(normal);
                        m_uv.Add(uvOffset + vWuv * i + vLuv * j);
                    }
                }
            }
            else
            {
                for (int i = 0; i <= segW; ++i)
                {
                    Vector3 v1 = p1 + vUp * i;
                    Vector3 v2 = p0 + vDown * i;
                    for (int j = 0; j <= segL; ++j)
                    {
                        Vector3 p = v1 * (segL - j) / segL + v2 * j / segL;
                        m_vertices.Add(p);
                        m_normals.Add(normal);
                    }
                }
            }

            for (int i = 0; i < segW; ++i)
            {
                for (int j = 0; j < segL; ++j)
                {
                    m_triangles.Add(index + (segL + 1) * i + j);
                    m_triangles.Add(index + (segL + 1) * (i + 1) + j);
                    m_triangles.Add(index + (segL + 1) * (i + 1) + (j + 1));
                    m_triangles.Add(index + (segL + 1) * (i + 1) + (j + 1));
                    m_triangles.Add(index + (segL + 1) * i + (j + 1));
                    m_triangles.Add(index + (segL + 1) * i + j);
                }
            }
        }

        protected void CreateCircle(Vector3 center, Vector3 forward, Vector3 right, float radius, int sides, int seg, bool generateUV, bool realWorldMapSize, bool flip)
        {
            CreateCircle(center, forward, right, radius, sides, seg, false, 0.0f, 360.0f, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip);
        }

        protected void CreateCircle(Vector3 center, Vector3 forward, Vector3 right, float radius, int sides, int seg, bool sliceOn, float sliceFrom, float sliceTo, bool generateUV, bool realWorldMapSize, bool flip)
        {
            CreateCircle(center, forward, right, radius, sides, seg, sliceOn, sliceFrom, sliceTo, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip);
        }

        protected void CreateCircle(Vector3 center, Vector3 forward, Vector3 right, float radius, int sides, int seg, bool sliceOn, float sliceFrom, float sliceTo, bool generateUV, bool realWorldMapSize, Vector2 uvOffset, Vector2 uvTiling, bool flip)
        {
            Vector3 up = Vector3.Cross(forward, right).normalized;
            Vector2[] sinCos = new Vector2[sides + 1];
            Vector3[] points = new Vector3[sides + 1];

            float start, tRad;
            if (sliceOn)
            {
                start = sliceFrom * deg2rad;
                tRad = (sliceTo - sliceFrom) * deg2rad;
            }
            else
            {
                start = 0.0f;
                tRad = 2 * pi;
            }
            float angle = tRad / sides;

            for (int i = 0; i <= sides; ++i)
            {
                float rad = start + angle * i;
                sinCos[i].x = Mathf.Sin(rad);
                sinCos[i].y = Mathf.Cos(rad);
                points[i] = center + (forward * sinCos[i].y + right * sinCos[i].x) * radius;
            }

            Vector2[] uvs = generateUV ? sinCos : null;
            if (realWorldMapSize) { uvTiling *= radius * 2.0f; }
            if (flip) { up = -up; points = points.Reverse().ToArray(); if (uvs != null) uvs = uvs.Reverse().ToArray(); }
            CreateCircle(points, uvs, center, up, seg, uvOffset, uvTiling);
        }

        protected void CreateHemiCircle(Vector3 center, Vector3 forward, Vector3 right, float radius, int sides, int seg, bool hemiCircle, float cutFrom, float cutTo, bool generateUV, bool realWorldMapSize, bool flip)
        {
            CreateHemiCircle(center, forward, right, radius, sides, seg, hemiCircle, cutFrom, cutTo, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip);
        }

        protected void CreateHemiCircle(Vector3 center, Vector3 forward, Vector3 right, float radius, int sides, int seg, bool hemiCircle, float cutFrom, float cutTo, bool generateUV, bool realWorldMapSize, Vector2 uvOffset, Vector2 uvTiling, bool flip)
        {
            Vector3 up = Vector3.Cross(forward, right).normalized;
            List<Vector2> sinCos = new List<Vector2>();
            List<Vector3> points = new List<Vector3>();
            float angle = pi / sides;
            Vector3 centerOffset = new Vector3();
            float centerUVOffset = 0.0f;

            if (hemiCircle)
            {
                float temp = 0.0f;
                Vector2 tempSCf = new Vector2(Mathf.Sin(cutFrom), -Mathf.Cos(cutFrom));
                Vector2 tempSCt = new Vector2(Mathf.Sin(cutTo), -Mathf.Cos(cutTo));
                if (cutFrom < halfPi)
                {
                    while (temp < cutFrom)
                    {
                        Vector2 tempSC = new Vector2(Mathf.Sin(temp), -Mathf.Cos(temp));
                        sinCos.Add(tempSC * (tempSCf.y / tempSC.y));
                        points.Add(center + (forward * tempSC.y + right * tempSC.x) * (radius * (tempSCf.y / tempSC.y)));
                        temp += angle;
                    }
                    sinCos.Add(tempSCf);
                    points.Add(center + (forward * tempSCf.y + right * tempSCf.x) * radius);
                    while (temp < pi / 2 && temp < cutTo)
                    {
                        Vector2 tempSC = new Vector2(Mathf.Sin(temp), -Mathf.Cos(temp));
                        sinCos.Add(tempSC);
                        points.Add(center + (forward * tempSC.y + right * tempSC.x) * radius);
                        temp += angle;
                    }
                }
                else
                {
                    sinCos.Add(tempSCf);
                    points.Add(center + (forward * tempSCf.y + right * tempSCf.x) * radius);
                    centerUVOffset = tempSCf.y * 0.5f; centerOffset = forward * (tempSCf.y * radius);
                    temp = angle * (Mathf.FloorToInt(cutFrom / angle) + 1);
                }
                if (cutTo > halfPi)
                {
                    while (temp < cutTo)
                    {
                        Vector2 tempSC = new Vector2(Mathf.Sin(temp), -Mathf.Cos(temp));
                        sinCos.Add(tempSC);
                        points.Add(center + (forward * tempSC.y + right * tempSC.x) * radius);
                        temp += angle;
                    }
                    sinCos.Add(tempSCt);
                    points.Add(center + (forward * tempSCt.y + right * tempSCt.x) * radius);
                    while (temp <= pi)
                    {
                        Vector2 tempSC = new Vector2(Mathf.Sin(temp), -Mathf.Cos(temp));
                        sinCos.Add(tempSC * (tempSCt.y / tempSC.y));
                        points.Add(center + (forward * tempSC.y + right * tempSC.x) * (radius * (tempSCt.y / tempSC.y)));
                        temp += angle;
                    }
                }
                else
                {
                    sinCos.Add(tempSCt);
                    points.Add(center + (forward * tempSCt.y + right * tempSCt.x) * radius);
                    centerUVOffset = tempSCt.y * 0.5f; centerOffset = forward * (tempSCt.y * radius);
                }
            }
            else
            {
                for (int i = 0; i <= sides; ++i)
                {
                    Vector2 tempSC = new Vector2(Mathf.Sin(angle * i), -Mathf.Cos(angle * i));
                    sinCos.Add(tempSC);
                    points.Add(center + (forward * tempSC.y + right * tempSC.x) * radius);
                }
            }

            Vector2[] uvs = generateUV ? sinCos.ToArray() : null;
            if (realWorldMapSize) { uvTiling *= radius * 2.0f; }
            if (flip) { up = -up; points.Reverse(); if (uvs != null) uvs = uvs.Reverse().ToArray(); }
            CreateCircle(points.ToArray(), uvs, center + centerOffset, -up, seg, uvOffset, uvTiling, centerUVOffset);
        }

        protected void CreateCircle(Vector3[] points, Vector2[] uvs, Vector3 center, Vector3 normal, int seg, Vector2 uvOffset, Vector2 uvTiling, float centerUVOffset = 0.0f)
        {
            int index = m_vertices.Count;

            if (uvs != null)
            {
                Vector2 cuv = new Vector2(0.5f, 0.5f + centerUVOffset);
                m_vertices.Add(center);
                m_normals.Add(normal);
                m_uv.Add(uvOffset + cuv);
                for (int i = 0; i < points.Length; ++i)
                {
                    Vector3 v = (points[i] - center) / seg;
                    Vector2 vuv = uvs[i] * 0.5f;
                    vuv.y -= centerUVOffset;
                    vuv.x *= uvTiling.x / seg;
                    vuv.y *= uvTiling.y / seg;
                    for (int j = 1; j <= seg; ++j)
                    {
                        m_vertices.Add(center + v * j);
                        m_normals.Add(normal);
                        m_uv.Add(uvOffset + cuv + vuv * j);
                    }
                }
            }
            else
            {
                m_vertices.Add(center);
                m_normals.Add(normal);
                for (int i = 0; i < points.Length; ++i)
                {
                    Vector3 v = (points[i] - center) / seg;
                    for (int j = 1; j <= seg; ++j)
                    {
                        m_vertices.Add(center + v * j);
                        m_normals.Add(normal);
                    }
                }
            }

            for (int i = 0; i < points.Length - 1; ++i)
            {
                m_triangles.Add(index);
                m_triangles.Add(index + seg * i + 1);
                m_triangles.Add(index + seg * (i + 1) + 1);
                for (int j = 1; j < seg; ++j)
                {
                    m_triangles.Add(index + seg * i + j);
                    m_triangles.Add(index + seg * i + j + 1);
                    m_triangles.Add(index + seg * (i + 1) + j);
                    m_triangles.Add(index + seg * (i + 1) + j);
                    m_triangles.Add(index + seg * i + j + 1);
                    m_triangles.Add(index + seg * (i + 1) + j + 1);
                }
            }
        }

        protected void CreateRing(Vector3 center, Vector3 forward, Vector3 right, float radius1, float radius2, int sides, int seg, bool generateUV, bool realWorldMapSize, bool flip)
        {
            CreateRing(center, forward, right, radius1, radius2, sides, seg, false, 0.0f, 360.0f, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip);
        }

        protected void CreateRing(Vector3 center, Vector3 forward, Vector3 right, float radius1, float radius2, int sides, int seg, bool sliceOn, float sliceFrom, float sliceTo, bool generateUV, bool realWorldMapSize, bool flip)
        {
            CreateRing(center, forward, right, radius1, radius2, sides, seg, sliceOn, sliceFrom, sliceTo, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip);
        }

        protected void CreateRing(Vector3 center, Vector3 forward, Vector3 right, float radius1, float radius2, int sides, int seg, bool sliceOn, float sliceFrom, float sliceTo, bool generateUV, bool realWorldMapSize, Vector2 uvOffset, Vector2 uvTiling, bool flip)
        {
            Vector3 up = Vector3.Cross(forward, right).normalized;
            Vector2[] sinCos = new Vector2[sides + 1];
            Vector3[] points = new Vector3[sides + 1];
            
            float start, tRad;
            if (sliceOn)
            {
                start = sliceFrom * deg2rad;
                tRad = (sliceTo - sliceFrom) * deg2rad;
            }
            else
            {
                start = 0.0f;
                tRad = 2 * pi;
            }
            float angle = tRad / sides;

            for (int i = 0; i <= sides; ++i)
            {
                float rad = start + angle * i;
                sinCos[i].x = Mathf.Sin(rad);
                sinCos[i].y = Mathf.Cos(rad);
                points[i] = center + (forward * sinCos[i].y + right * sinCos[i].x) * radius1;
            }

            Vector2[] uvs = generateUV ? sinCos : null;
            if (realWorldMapSize) { uvTiling *= radius1 * 2.0f; }
            if (flip) { up = -up; points = points.Reverse().ToArray(); if (uvs != null) uvs = uvs.Reverse().ToArray(); }
            CreateRing(points, uvs, center, up, radius2 / radius1, seg, uvOffset, uvTiling);
        }

        protected void CreateRing(Vector3[] points, Vector2[] uvs, Vector3 center, Vector3 normal, float ratio, int seg, Vector2 uvOffset, Vector2 uvTiling, float centerUVOffset = 0.0f)
        {
            int index = m_vertices.Count;

            if (uvs != null)
            {
                Vector2 cuv = new Vector2(0.5f, 0.5f + centerUVOffset);
                for (int i = 0; i < points.Length; ++i)
                {
                    Vector3 offset = (points[i] - center) * ratio;
                    Vector3 v = (points[i] - (center + offset)) / seg;
                    Vector2 offsetuv = uvs[i] * ratio * 0.5f;
                    offsetuv.x *= uvTiling.x;
                    offsetuv.y *= uvTiling.y;
                    Vector2 vuv = (uvs[i] - uvs[i] * ratio) * 0.5f;
                    vuv.x *= uvTiling.x / seg;
                    vuv.y *= uvTiling.y / seg;
                    for (int j = 0; j <= seg; ++j)
                    {
                        m_vertices.Add(center + offset + v * j);
                        m_normals.Add(normal);
                        m_uv.Add(cuv + uvOffset + offsetuv + vuv * j);
                    }
                }
            }
            else
            {
                for (int i = 0; i < points.Length; ++i)
                {
                    Vector3 offset = (points[i] - center) * ratio;
                    Vector3 v = (points[i] - (center + offset)) / seg;
                    for (int j = 0; j <= seg; ++j)
                    {
                        m_vertices.Add(center + offset + v * j);
                        m_normals.Add(normal);
                    }
                }
            }

            for (int i = 0; i < points.Length - 1; ++i)
            {
                for (int j = 0; j < seg; ++j)
                {
                    m_triangles.Add(index + (seg + 1) * i + j);
                    m_triangles.Add(index + (seg + 1) * i + j + 1);
                    m_triangles.Add(index + (seg + 1) * (i + 1) + j);
                    m_triangles.Add(index + (seg + 1) * (i + 1) + j);
                    m_triangles.Add(index + (seg + 1) * i + j + 1);
                    m_triangles.Add(index + (seg + 1) * (i + 1) + j + 1);
                }
            }
        }
        
        protected void CreateHemiRing(Vector3 center, Vector3 forward, Vector3 right, float radius1, float radius2, int sides, int seg, bool hemiCircle, float cutFrom, float cutTo, bool generateUV, bool realWorldMapSize, bool flip)
        {
            CreateHemiRing(center, forward, right, radius1, radius2, sides, seg, hemiCircle, cutFrom, cutTo, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip);
        }

        protected void CreateHemiRing(Vector3 center, Vector3 forward, Vector3 right, float radius1, float radius2, int sides, int seg, bool hemiCircle, float cutFrom, float cutTo, bool generateUV, bool realWorldMapSize, Vector2 uvOffset, Vector2 uvTiling, bool flip)
        {
            Vector3 up = Vector3.Cross(forward, right).normalized;
            List<Vector2> sinCos = new List<Vector2>();
            List<Vector3> points = new List<Vector3>();
            List<Vector2> sinCos2 = new List<Vector2>();
            List<Vector3> points2 = new List<Vector3>();
            float angle = pi / sides;
            float ratio = radius2 / radius1;
            Vector3 centerOffset = new Vector3();
            float centerUVOffset = 0.0f;

            if (hemiCircle)
            {
                Vector2 tempSC = new Vector2();
                Vector3 v = new Vector3();
                float r, r2;
                float temp = 0.0f;
                Vector2 tempSCf = new Vector2(Mathf.Sin(cutFrom), -Mathf.Cos(cutFrom));
                Vector2 tempSCt = new Vector2(Mathf.Sin(cutTo), -Mathf.Cos(cutTo));
                float cutFrom2 = Mathf.Acos(Mathf.Clamp(-tempSCf.y / ratio, -1.0f, 1.0f));
                float cutTo2 = Mathf.Acos(Mathf.Clamp(-tempSCt.y / ratio, -1.0f, 1.0f));
                Vector2 tempSCf2 = new Vector2(Mathf.Sin(cutFrom2), -Mathf.Cos(cutFrom2));
                Vector2 tempSCt2 = new Vector2(Mathf.Sin(cutTo2), -Mathf.Cos(cutTo2));
                if (cutFrom < halfPi)
                {
                    if (tempSCf.y > tempSCf2.y)
                    {
                        v = forward * tempSCf2.y + right * tempSCf2.x;
                        temp = angle * Mathf.CeilToInt(cutFrom2 / angle);
                        sinCos.Add(tempSCf2 * ratio);
                        points.Add(center + v * radius2);
                        sinCos2.Add(tempSCf2 * ratio);
                        points2.Add(center + v * radius2);
                    }
                    while (temp < cutFrom)
                    {
                        tempSC = new Vector2(Mathf.Sin(temp), -Mathf.Cos(temp));
                        v = forward * tempSC.y + right * tempSC.x;
                        r = tempSCf.y / tempSC.y;
                        r2 = tempSCt.y / tempSC.y < ratio ? ratio : tempSCt.y / tempSC.y;
                        temp += angle;
                        sinCos.Add(tempSC * r);
                        points.Add(center + v * (radius1 * r));
                        sinCos2.Add(tempSC * r2);
                        points2.Add(center + v * (radius1 * r2));
                    }
                    v = forward * tempSCf.y + right * tempSCf.x;
                    r2 = tempSCt.y / tempSCf.y < ratio ? ratio : tempSCt.y / tempSCf.y;
                    sinCos.Add(tempSCf);
                    points.Add(center + v * radius1);
                    sinCos2.Add(tempSCf * r2);
                    points2.Add(center + v * (radius1 * r2));
                    if (cutTo < halfPi)
                    {
                        if (cutTo2 > cutFrom)
                        {
                            while (temp < cutTo2)
                            {
                                tempSC = new Vector2(Mathf.Sin(temp), -Mathf.Cos(temp));
                                v = forward * tempSC.y + right * tempSC.x;
                                r2 = tempSCt.y / tempSC.y < ratio ? ratio : tempSCt.y / tempSC.y;
                                temp += angle;
                                sinCos.Add(tempSC);
                                points.Add(center + v * radius1);
                                sinCos2.Add(tempSC * r2);
                                points2.Add(center + v * (radius1 * r2));
                            }
                            v = forward * tempSCt2.y + right * tempSCt2.x;
                            r2 = tempSCt.y / tempSCt2.y < ratio ? ratio : tempSCt.y / tempSCt2.y;
                            sinCos.Add(tempSCt2);
                            points.Add(center + v * radius1);
                            sinCos2.Add(tempSCt2 * r2);
                            points2.Add(center + v * (radius1 * r2));
                        }
                        while (temp < cutTo)
                        {
                            tempSC = new Vector2(Mathf.Sin(temp), -Mathf.Cos(temp));
                            v = forward * tempSC.y + right * tempSC.x;
                            r2 = tempSCt.y / tempSC.y < ratio ? ratio : tempSCt.y / tempSC.y;
                            temp += angle;
                            sinCos.Add(tempSC);
                            points.Add(center + v * radius1);
                            sinCos2.Add(tempSC * r2);
                            points2.Add(center + v * (radius1 * r2));
                        }
                        v = forward * tempSCt.y + right * tempSCt.x;
                        sinCos.Add(tempSCt);
                        points.Add(center + v * radius1);
                        sinCos2.Add(tempSCt);
                        points2.Add(center + v * radius1);
                    }
                    else
                    {
                        while (temp < cutTo)
                        {
                            tempSC = new Vector2(Mathf.Sin(temp), -Mathf.Cos(temp));
                            v = forward * tempSC.y + right * tempSC.x;
                            temp += angle;
                            sinCos.Add(tempSC);
                            points.Add(center + v * radius1);
                            sinCos2.Add(tempSC * ratio);
                            points2.Add(center + v * radius2);
                        }
                        v = forward * tempSCt.y + right * tempSCt.x;
                        sinCos.Add(tempSCt);
                        points.Add(center + v * radius1);
                        sinCos2.Add(tempSCt * ratio);
                        points2.Add(center + v * radius2);
                        while (temp < cutTo2)
                        {
                            tempSC = new Vector2(Mathf.Sin(temp), -Mathf.Cos(temp));
                            v = forward * tempSC.y + right * tempSC.x;
                            r = tempSCt.y / tempSC.y;
                            temp += angle;
                            sinCos.Add(tempSC);
                            points.Add(center + v * (radius1 * r));
                            sinCos2.Add(tempSC * ratio);
                            points2.Add(center + v * radius2);
                        }
                        if (tempSCt.y < tempSCt2.y)
                        {
                            v = forward * tempSCt2.y + right * tempSCt2.x;
                            sinCos.Add(tempSCt2 * ratio);
                            points.Add(center + v * radius2);
                            sinCos2.Add(tempSCt2 * ratio);
                            points2.Add(center + v * radius2);
                        }
                    }
                }
                else
                {
                    v = forward * tempSCf.y + right * tempSCf.x;
                    temp = angle * (Mathf.FloorToInt(cutFrom / angle) + 1);
                    sinCos.Add(tempSCf);
                    points.Add(center + v * radius1);
                    sinCos2.Add(tempSCf);
                    points2.Add(center + v * radius1);
                    if (cutTo > cutFrom2)
                    {
                        while (temp < cutFrom2)
                        {
                            tempSC = new Vector2(Mathf.Sin(temp), -Mathf.Cos(temp));
                            v = forward * tempSC.y + right * tempSC.x;
                            r = tempSCt.y > tempSC.y ? 1 : tempSCt.y / tempSC.y;
                            r2 = tempSCf.y / tempSC.y;
                            temp += angle;
                            sinCos.Add(tempSC);
                            points.Add(center + v * (radius1 * r));
                            sinCos2.Add(tempSC * r2);
                            points2.Add(center + v * (radius1 * r2));
                        }
                        v = forward * tempSCf2.y + right * tempSCf2.x;
                        r = tempSCt.y / tempSCf2.y > 1 ? 1 : tempSCt.y / tempSCf2.y;
                        sinCos.Add(tempSCf);
                        points.Add(center + v * (radius1 * r));
                        sinCos2.Add(tempSCf);
                        points2.Add(center + v * radius2);
                    }
                    while (temp < cutTo)
                    {
                        tempSC = new Vector2(Mathf.Sin(temp), -Mathf.Cos(temp));
                        v = forward * tempSC.y + right * tempSC.x;
                        r = tempSCt.y > tempSC.y ? 1 : tempSCt.y / tempSC.y;
                        r2 = tempSCf.y / tempSC.y < ratio ? ratio : tempSCf.y / tempSC.y;
                        temp += angle;
                        sinCos.Add(tempSC);
                        points.Add(center + v * (radius1 * r));
                        sinCos2.Add(tempSC * r2);
                        points2.Add(center + v * (radius1 * r2));
                    }
                    v = forward * tempSCt.y + right * tempSCt.x;
                    r2 = tempSCf.y / tempSCt.y < ratio ? ratio : tempSCf.y / tempSCt.y;
                    sinCos.Add(tempSCt);
                    points.Add(center + v * radius1);
                    sinCos2.Add(tempSCt * ratio);
                    points2.Add(center + v * (radius1 * r2));
                    while (temp < cutTo2)
                    {
                        tempSC = new Vector2(Mathf.Sin(temp), -Mathf.Cos(temp));
                        v = forward * tempSC.y + right * tempSC.x;
                        r = tempSCt.y > tempSC.y ? 1 : tempSCt.y / tempSC.y;
                        r2 = tempSCf.y / tempSC.y < ratio ? ratio : tempSCf.y / tempSC.y;
                        temp += angle;
                        sinCos.Add(tempSC);
                        points.Add(center + v * (radius1 * r));
                        sinCos2.Add(tempSC * r2);
                        points2.Add(center + v * (radius1 * r2));
                    }
                    if (tempSCt.y < tempSCt2.y)
                    {
                        v = forward * tempSCt2.y + right * tempSCt2.x;
                        sinCos.Add(tempSCt2 * ratio);
                        points.Add(center + v * radius2);
                        sinCos2.Add(tempSCt2 * ratio);
                        points2.Add(center + v * radius2);
                    }
                }
            }
            else
            {
                for (int i = 0; i <= sides; ++i)
                {
                    Vector2 tempSC = new Vector2(Mathf.Sin(angle * i), -Mathf.Cos(angle * i));
                    Vector3 v = forward * tempSC.y + right * tempSC.x;
                    sinCos.Add(tempSC);
                    points.Add(center + v * radius1);
                    sinCos2.Add(tempSC * ratio);
                    points2.Add(center + v * radius2);
                }
            }

            Vector2[] uvs = generateUV ? sinCos.ToArray() : null;
            Vector2[] uvs2 = generateUV ? sinCos2.ToArray() : null;
            if (realWorldMapSize) { uvTiling *= radius1 * 2.0f; }
            if (flip) { up = -up; points.Reverse(); points2.Reverse(); if (uvs != null) { uvs = uvs.Reverse().ToArray(); uvs2 = uvs2.Reverse().ToArray(); } }
            CreateRing(points.ToArray(), points2.ToArray(), uvs, uvs2, center + centerOffset, -up, seg, uvOffset, uvTiling, centerUVOffset);
        }

        protected void CreateRing(Vector3[] points, Vector3[] points2, Vector2[] uvs, Vector2[] uvs2, Vector3 center, Vector3 normal, int seg, Vector2 uvOffset, Vector2 uvTiling, float centerUVOffset = 0.0f)
        {
            int index = m_vertices.Count;

            if (uvs != null)
            {
                Vector2 cuv = new Vector2(0.5f, 0.5f + centerUVOffset);
                for (int i = 0; i < points.Length; ++i)
                {
                    Vector3 v = (points[i] - points2[i]) / seg;
                    Vector2 offsetuv = uvs2[i] * 0.5f;
                    offsetuv.y -= centerUVOffset;
                    offsetuv.x *= uvTiling.x;
                    offsetuv.y *= uvTiling.y;
                    Vector2 vuv = (uvs[i] - uvs2[i]) * 0.5f;
                    vuv.y -= centerUVOffset;
                    vuv.x *= uvTiling.x / seg;
                    vuv.y *= uvTiling.y / seg;
                    for (int j = 0; j <= seg; ++j)
                    {
                        m_vertices.Add(points2[i] + v * j);
                        m_normals.Add(normal);
                        m_uv.Add(cuv + uvOffset + offsetuv + vuv * j);
                    }
                }
            }
            else
            {
                for (int i = 0; i < points.Length; ++i)
                {
                    Vector3 v = (points[i] - points2[i]) / seg;
                    for (int j = 0; j <= seg; ++j)
                    {
                        m_vertices.Add(points2[i] + v * j);
                        m_normals.Add(normal);
                    }
                }
            }

            for (int i = 0; i < points.Length - 1; ++i)
            {
                for (int j = 0; j < seg; ++j)
                {
                    m_triangles.Add(index + (seg + 1) * i + j);
                    m_triangles.Add(index + (seg + 1) * i + j + 1);
                    m_triangles.Add(index + (seg + 1) * (i + 1) + j);
                    m_triangles.Add(index + (seg + 1) * (i + 1) + j);
                    m_triangles.Add(index + (seg + 1) * i + j + 1);
                    m_triangles.Add(index + (seg + 1) * (i + 1) + j + 1);
                }
            }
        }

        protected void CreateTorus(Vector3 center, Vector3 forward, Vector3 right, float radius1, float radius2, int sides, int seg, bool sliceOn, float sliceFrom, float sliceTo, bool generateUV, bool realWorldMapSize, bool flip, bool smooth, float sectionSliceFrom = 0.0f, float sectionSliceTo = 360.0f)
        {
            CreateTorus(center, forward, right, radius1, radius2, sides, seg, sliceOn, sliceFrom, sliceTo, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip, smooth, sectionSliceFrom, sectionSliceTo);
        }

        protected void CreateTorus(Vector3 center, Vector3 forward, Vector3 right, float radius1, float radius2, int sides, int seg, bool sliceOn, float sliceFrom, float sliceTo, bool generateUV, bool realWorldMapSize, Vector2 uvOffset, Vector2 uvTiling, bool flip, bool smooth, float sectionSliceFrom, float sectionSliceTo)
        {
            Vector3 up = Vector3.Cross(forward, right).normalized;
            sectionSliceFrom *= deg2rad;
            sectionSliceTo *= deg2rad;
            float angleSection = (sectionSliceTo - sectionSliceFrom) / seg;
            Vector2[] sinCos = new Vector2[seg + 1];
            for (int i = 0; i <= seg; ++i)
            {
                float temp = sectionSliceFrom + angleSection * i - halfPi;
                sinCos[i].x = Mathf.Sin(temp);
                sinCos[i].y = -Mathf.Cos(temp);
            }

            Vector3[] centers = new Vector3[sides + 1];
            Vector3[][] points = new Vector3[sides + 1][];

            float start, tRad;
            if (sliceOn)
            {
                start = sliceFrom * deg2rad;
                tRad = (sliceTo - sliceFrom) * deg2rad;
            }
            else
            {
                start = 0.0f;
                tRad = 2 * pi;
            }
            float angle = tRad / sides;

            for (int i = 0; i <= sides; ++i)
            {
                float rad = start + angle * i;
                float sin = Mathf.Sin(rad);
                float cos = Mathf.Cos(rad);
                centers[i] = center + (forward * cos + right * sin) * radius1;

                points[i] = new Vector3[seg + 1];
                for (int j = 0; j <= seg; ++j)
                {
                    points[i][j] = centers[i] + (up * sinCos[j].y + (centers[i] - center) / radius1 * sinCos[j].x) * radius2;
                }
            }

            if (realWorldMapSize) { uvTiling.x *= tRad * radius1; uvTiling.y *= 2 * pi * radius2; }
            if (flip) { points = points.Reverse().ToArray(); centers = centers.Reverse().ToArray(); }
            CreateTorus(points, centers, seg, generateUV, uvOffset, uvTiling, flip, smooth);
        }

        protected void CreateTorus(Vector3[][] points, Vector3[] centers, int seg, bool generateUV, Vector2 uvOffset, Vector2 uvTiling, bool flip, bool smooth)
        {
            if (smooth)
            {
                int index = m_vertices.Count;
                float f = flip ? -1.0f : 1.0f;
                if (generateUV)
                {
                    Vector2 vWuv = Vector2.right * uvTiling.x / (points.Length - 1);
                    Vector2 vLuv = Vector2.up * uvTiling.y / (points[0].Length - 1);
                    for (int i = 0; i < points.Length; ++i)
                    {
                        for (int j = 0; j < points[i].Length; ++j)
                        {
                            m_vertices.Add(points[i][j]);
                            m_normals.Add((points[i][j] - centers[i]).normalized * f);
                            m_uv.Add(uvOffset + vWuv * i + vLuv * j);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < points.Length; ++i)
                    {
                        for (int j = 0; j < points[i].Length; ++j)
                        {
                            m_vertices.Add(points[i][j]);
                            m_normals.Add((points[i][j] - centers[i]).normalized * f);
                        }
                    }
                }

                for (int i = 0; i < points.Length - 1; ++i)
                {
                    for (int j = 0; j < points[i].Length - 1; ++j)
                    {
                        m_triangles.Add(index + points[i].Length * (i + 1) + j + 1);
                        m_triangles.Add(index + points[i].Length * i + j + 1);
                        m_triangles.Add(index + points[i].Length * (i + 1) + j);
                        m_triangles.Add(index + points[i].Length * (i + 1) + j);
                        m_triangles.Add(index + points[i].Length * i + j + 1);
                        m_triangles.Add(index + points[i].Length * i + j);
                    }
                }
            }
            else
            {
                if (generateUV)
                {
                    Vector2[][] uvs = new Vector2[points.Length][];
                    Vector2 vWuv = Vector2.right * uvTiling.x / (points.Length - 1);
                    Vector2 vLuv = Vector2.up * uvTiling.y / (points[0].Length - 1);
                    for (int i = 0; i < points.Length; ++i)
                    {
                        uvs[i] = new Vector2[points[i].Length];
                        for (int j = 0; j < points[i].Length; ++j)
                        {
                            uvs[i][j] = uvOffset + vWuv * i + vLuv * j;
                        }
                    }

                    for (int i = 0; i < points.Length - 1; ++i)
                    {
                        for (int j = 0; j < points[i].Length - 1; ++j)
                        {
                            CreateQuad(points[i + 1][j], points[i + 1][j + 1], points[i][j + 1], points[i][j], uvs[i + 1][j], uvs[i + 1][j + 1], uvs[i][j + 1], uvs[i][j]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < points.Length - 1; ++i)
                    {
                        for (int j = 0; j < points[i].Length - 1; ++j)
                        {
                            CreateQuad(points[i + 1][j], points[i + 1][j + 1], points[i][j + 1], points[i][j]);
                        }
                    }
                }
            }
        }

        protected void CreateCylinder(Vector3 center, Vector3 forward, Vector3 right, float height, float radius, int sides, int segHeight, bool sliceOn, float sliceFrom, float sliceTo, bool generateUV, bool realWorldMapSize, bool flip, bool smooth)
        {
            CreateCylinder(center, forward, right, height, radius, sides, segHeight, sliceOn, sliceFrom, sliceTo, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip, smooth);
        }

        protected void CreateCylinder(Vector3 center, Vector3 forward, Vector3 right, float height, float radius, int sides, int segHeight, bool sliceOn, float sliceFrom, float sliceTo, bool generateUV, bool realWorldMapSize, Vector2 uvOffset, Vector2 uvTiling, bool flip, bool smooth)
        {
            Vector3 up = Vector3.Cross(forward, right).normalized;
            float heightHalf = height * 0.5f;
            Vector2[] sinCos = new Vector2[sides + 1];
            Vector3[] pointsDown = new Vector3[sides + 1];
            Vector3[] pointsUp = new Vector3[sides + 1];
            Vector3 centerDown = center - up * heightHalf;
            Vector3 centerUp = center + up * heightHalf;

            float start, tRad;
            if (sliceOn)
            {
                start = sliceFrom * deg2rad;
                tRad = (sliceTo - sliceFrom) * deg2rad;
            }
            else
            {
                start = 0.0f;
                tRad = 2 * pi;
            }
            float angle = tRad / sides;

            for (int i = 0; i <= sides; ++i)
            {
                float rad = start + angle * i;
                sinCos[i].x = Mathf.Sin(rad);
                sinCos[i].y = Mathf.Cos(rad);

                Vector3 offset = (forward * sinCos[i].y + right * sinCos[i].x) * radius;
                pointsDown[i] = centerDown + offset;
                pointsUp[i] = centerUp + offset;
            }

            if (realWorldMapSize) { uvTiling.x *= tRad * radius; uvTiling.y *= height; }
            if (flip) { pointsDown = pointsDown.Reverse().ToArray(); pointsUp = pointsUp.Reverse().ToArray(); }
            CreateCylinder(pointsDown, pointsUp, centerDown, centerUp, segHeight, generateUV, uvOffset, uvTiling, flip, smooth);
        }

        protected void CreateCylinder(Vector3[] pDown, Vector3[] pUp, Vector3 centerDown, Vector3 centerUp, int seg, bool generateUV, Vector2 uvOffset, Vector2 uvTiling, bool flip, bool smooth)
        {
            if (smooth)
            {
                int index = m_vertices.Count;
                Vector3 v = (centerUp - centerDown) / seg;
                float f = flip ? -1.0f : 1.0f;

                if (generateUV)
                {
                    Vector2 vWuv = Vector2.right * uvTiling.x / (pDown.Length - 1);
                    Vector2 vLuv = Vector2.up * uvTiling.y / seg;
                    for (int i = 0; i < pDown.Length; ++i)
                    {
                        Vector3 normal = ((pDown[i] - centerDown) + (pUp[i] - centerUp)).normalized * f;
                        for (int j = 0; j <= seg; ++j)
                        {
                            m_vertices.Add(pDown[i] + v * j);
                            m_normals.Add(normal);
                            m_uv.Add(uvOffset + vWuv * i + vLuv * j);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < pDown.Length; ++i)
                    {
                        Vector3 normal = ((pDown[i] - centerDown) + (pUp[i] - centerUp)).normalized * f;
                        if (flip) normal = -normal;
                        for (int j = 0; j <= seg; ++j)
                        {
                            m_vertices.Add(pDown[i] + v * j);
                            m_normals.Add(normal);
                        }
                    }
                }
                
                for (int i = 0; i < pDown.Length - 1; ++i)
                {
                    for (int j = 0; j < seg; ++j)
                    {
                        m_triangles.Add(index + (seg + 1) * (i + 1) + j + 1);
                        m_triangles.Add(index + (seg + 1) * i + j + 1);
                        m_triangles.Add(index + (seg + 1) * (i + 1) + j);
                        m_triangles.Add(index + (seg + 1) * (i + 1) + j);
                        m_triangles.Add(index + (seg + 1) * i + j + 1);
                        m_triangles.Add(index + (seg + 1) * i + j);
                    }
                }
            }
            else
            {
                float offset = uvTiling.x / (pDown.Length - 1);
                float tiling = 1.0f / (pDown.Length - 1);
                for (int i = 0; i < pDown.Length - 1; ++i)
                {
                    Vector3 normal = Vector3.Cross(pUp[i] - pDown[i + 1], pDown[i] - pUp[i + 1]).normalized;
                    CreatePlane(pDown[i + 1], pUp[i + 1], pUp[i], pDown[i], normal, 1, seg, generateUV, new Vector2(uvOffset.x + offset * (i + 1), uvOffset.y), new Vector2(-uvTiling.x * tiling, uvTiling.y));
                }
            }
        }

        protected void CreateCone(Vector3 center, Vector3 forward, Vector3 right, float height, float radius1, float radius2, int sides, int segHeight, bool sliceOn, float sliceFrom, float sliceTo, bool generateUV, bool realWorldMapSize, bool flip, bool smooth)
        {
            Vector3 up = Vector3.Cross(forward, right).normalized;
            Vector3 centerDown = center - up * height * 0.5f;
            Vector3 centerUp = centerDown + up * height;
            CreateCone(centerDown, centerUp, forward, right, height, radius1, radius2, sides, segHeight, sliceOn, sliceFrom, sliceTo, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip, smooth);
        }

        protected void CreateCone(Vector3 center, Vector3 forward, Vector3 right, float height, float radius1, float radius2, int sides, int segHeight, bool sliceOn, float sliceFrom, float sliceTo, bool generateUV, bool realWorldMapSize, Vector2 uvOffset, Vector2 uvTiling, bool flip, bool smooth)
        {
            Vector3 up = Vector3.Cross(forward, right).normalized;
            Vector3 centerDown = center - up * height * 0.5f;
            Vector3 centerUp = centerDown + up * height;
            CreateCone(centerDown, centerUp, forward, right, height, radius1, radius2, sides, segHeight, sliceOn, sliceFrom, sliceTo, generateUV, realWorldMapSize, uvOffset, uvTiling, flip, smooth);
        }

        protected void CreateCone(Vector3 centerDown, Vector3 centerUp, Vector3 forward, Vector3 right, float height, float radius1, float radius2, int sides, int segHeight, bool sliceOn, float sliceFrom, float sliceTo, bool generateUV, bool realWorldMapSize, bool flip, bool smooth)
        {
            CreateCone(centerDown, centerUp, forward, right, height, radius1, radius2, sides, segHeight, sliceOn, sliceFrom, sliceTo, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip, smooth);
        }

        protected void CreateCone(Vector3 centerDown, Vector3 centerUp, Vector3 forward, Vector3 right, float height, float radius1, float radius2, int sides, int segHeight, bool sliceOn, float sliceFrom, float sliceTo, bool generateUV, bool realWorldMapSize, Vector2 uvOffset, Vector2 uvTiling, bool flip, bool smooth)
        {
            Vector3 up = Vector3.Cross(forward, right).normalized;
            float heightHalf = height * 0.5f;
            Vector2[] sinCos = new Vector2[sides + 1];
            Vector3[] pointsDown = new Vector3[sides + 1];
            Vector3[] pointsUp = new Vector3[sides + 1];

            float start, tRad;
            if (sliceOn)
            {
                start = sliceFrom * deg2rad;
                tRad = (sliceTo - sliceFrom) * deg2rad;
            }
            else
            {
                start = 0.0f;
                tRad = 2 * pi;
            }
            float angle = tRad / sides;
            
            for (int i = 0; i <= sides; ++i)
            {
                float rad = start + angle * i;
                sinCos[i].x = Mathf.Sin(rad);
                sinCos[i].y = Mathf.Cos(rad);

                Vector3 offset = forward * sinCos[i].y + right * sinCos[i].x;
                pointsDown[i] = centerDown + offset * radius1;
                pointsUp[i] = centerUp + offset * radius2;
            }

            if (realWorldMapSize) { uvTiling.x *= tRad * (radius1 + radius2) * 0.5f; uvTiling.y *= height; }
            if (flip) { pointsDown = pointsDown.Reverse().ToArray(); pointsUp = pointsUp.Reverse().ToArray(); }
            CreateCone(pointsDown, pointsUp, centerDown, centerUp, up, segHeight, generateUV, uvOffset, uvTiling, flip, smooth);
        }

        protected void CreateCone(Vector3[] pDown, Vector3[] pUp, Vector3 centerDown, Vector3 centerUp, Vector3 up, int seg, bool generateUV, Vector2 uvOffset, Vector2 uvTiling, bool flip, bool smooth)
        {
            if (smooth)
            {
                int index = m_vertices.Count;
                float f = flip ? -1.0f : 1.0f;

                if (generateUV)
                {
                    Vector2 vWuv = Vector2.right * uvTiling.x / (pDown.Length - 1);
                    Vector2 vLuv = Vector2.up * uvTiling.y / seg;
                    for (int i = 0; i < pDown.Length; ++i)
                    {
                        Vector3 v = pUp[i] - pDown[i];
                        Vector3 normal = (pDown[i] - centerDown) + (pUp[i] - centerUp);
                        normal += up * normal.magnitude * PointLineProjectionn(v, -normal) / PointLineProjectionn(v, up);
                        normal.Normalize();
                        //normal = Vector3.Cross(v, Vector3.Cross(normal, v)).normalized * f;
                        v /= seg;
                        for (int j = 0; j <= seg; ++j)
                        {
                            m_vertices.Add(pDown[i] + v * j);
                            m_normals.Add(normal);
                            m_uv.Add(uvOffset + vWuv * i + vLuv * j);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < pDown.Length; ++i)
                    {
                        Vector3 v = pUp[i] - pDown[i];
                        Vector3 normal = (pDown[i] - centerDown) + (pUp[i] - centerUp);
                        normal += up * normal.magnitude * PointLineProjectionn(v, -normal) / PointLineProjectionn(v, up);
                        normal.Normalize();
                        //normal = Vector3.Cross(v, Vector3.Cross(normal, v)).normalized * f;
                        v /= seg;
                        for (int j = 0; j <= seg; ++j)
                        {
                            m_vertices.Add(pDown[i] + v * j);
                            m_normals.Add(normal);
                        }
                    }
                }
                
                for (int i = 0; i < pDown.Length - 1; ++i)
                {
                    for (int j = 0; j < seg; ++j)
                    {
                        m_triangles.Add(index + (seg + 1) * (i + 1) + j + 1);
                        m_triangles.Add(index + (seg + 1) * i + j + 1);
                        m_triangles.Add(index + (seg + 1) * (i + 1) + j);
                        m_triangles.Add(index + (seg + 1) * (i + 1) + j);
                        m_triangles.Add(index + (seg + 1) * i + j + 1);
                        m_triangles.Add(index + (seg + 1) * i + j);
                    }
                }
            }
            else
            {
                float offset = uvTiling.x / (pDown.Length - 1);
                float tiling = 1.0f / (pDown.Length - 1);
                for (int i = 0; i < pDown.Length - 1; ++i)
                {
                    Vector3 normal = Vector3.Cross(pUp[i] - pDown[i + 1], pDown[i] - pUp[i + 1]).normalized;
                    CreateTrapezoid(pDown[i + 1], pUp[i + 1], pUp[i], pDown[i], normal, 1, seg, generateUV, new Vector2(uvOffset.x + offset * (i + 1), uvOffset.y), new Vector2(-uvTiling.x * tiling, uvTiling.y));
                }
            }
        }

        protected void CreateSphere(Vector3 center, Vector3 forward, Vector3 right, float radius, int sides, int seg, bool sliceOn, float sliceFrom, float sliceTo, bool hemiSphere, float cutFrom, float cutTo, bool generateUV, bool realWorldMapSize, bool flip, bool smooth)
        {
            CreateSphere(center, forward, right, radius, sides, seg, sliceOn, sliceFrom, sliceTo, hemiSphere, cutFrom, cutTo, generateUV, realWorldMapSize, Vector2.zero, Vector2.one, flip, smooth);
        }

        protected void CreateSphere(Vector3 center, Vector3 forward, Vector3 right, float radius, int sides, int seg, bool sliceOn, float sliceFrom, float sliceTo, bool hemiSphere, float cutFrom, float cutTo, bool generateUV, bool realWorldMapSize, Vector2 uvOffset, Vector2 uvTiling, bool flip, bool smooth)
        {
            Vector3 up = Vector3.Cross(forward, right).normalized;
            float angleSection = pi / seg;
            List<Vector2> sinCos = new List<Vector2>();
            Vector3[][] points = new Vector3[sides + 1][];

            if (hemiSphere)
            {
                sinCos.Add(new Vector2(Mathf.Sin(cutFrom), -Mathf.Cos(cutFrom)));
                float temp = angleSection * (Mathf.FloorToInt(cutFrom / angleSection) + 1);
                while (temp < cutTo)
                {
                    sinCos.Add(new Vector2(Mathf.Sin(temp), -Mathf.Cos(temp)));
                    temp += angleSection;
                }
                sinCos.Add(new Vector2(Mathf.Sin(cutTo), -Mathf.Cos(cutTo)));
                //uvTiling.y *= (cutTo - cutFrom) / pi;
                //uvOffset.y += cutFrom / pi;
            }
            else
            {
                for (int i = 0; i <= seg; ++i)
                {
                    sinCos.Add(new Vector2(Mathf.Sin(angleSection * i), -Mathf.Cos(angleSection * i)));
                }
            }

            float start, tRad;
            if (sliceOn)
            {
                start = sliceFrom * deg2rad;
                tRad = (sliceTo - sliceFrom) * deg2rad;
            }
            else
            {
                start = 0.0f;
                tRad = pi * 2.0f;
            }
            float angle = tRad / sides;

            for (int i = 0; i <= sides; ++i)
            {
                float rad = start + angle * i;
                Vector3 offset = (right * Mathf.Sin(rad) + forward * Mathf.Cos(rad)) * radius;
                Vector3 offsetUp = up * radius;
                points[i] = new Vector3[sinCos.Count];
                for (int j = 0; j < sinCos.Count; ++j)
                {
                    points[i][j] = center + offset * sinCos[j].x + offsetUp * sinCos[j].y;
                }
            }

            if (realWorldMapSize) { uvTiling.x *= tRad * radius; uvTiling.y *= pi * radius; }
            if (flip) { points = points.Reverse().ToArray(); }
            if (hemiSphere) CreateSphere(points, center, generateUV, uvOffset, uvTiling, flip, smooth, sinCos);
            else CreateSphere(points, center, generateUV, uvOffset, uvTiling, flip, smooth);
        }

        protected void CreateSphere(Vector3[][] points, Vector3 center, bool generateUV, Vector2 uvOffset, Vector2 uvTiling, bool flip, bool smooth, List<Vector2> uvInfo = null)
        {
            if (smooth)
            {
                int index = m_vertices.Count;
                float f = flip ? -1.0f : 1.0f;

                if (generateUV)
                {
                    if (uvInfo == null)
                    {
                        Vector2 vWuv = Vector2.right * uvTiling.x / (points.Length - 1);
                        Vector2 vLuv = Vector2.up * uvTiling.y / (points[0].Length - 1);
                        for (int i = 0; i < points.Length; ++i)
                        {
                            for (int j = 0; j < points[i].Length; ++j)
                            {
                                m_vertices.Add(points[i][j]);
                                m_normals.Add((points[i][j] - center).normalized * f);
                                m_uv.Add(uvOffset + vWuv * i + vLuv * j);
                            }
                        }
                    }
                    else
                    {
                        Vector2 vWuv = Vector2.right * uvTiling.x / (points.Length - 1);
                        for (int i = 0; i < points.Length; ++i)
                        {
                            for (int j = 0; j < points[i].Length; ++j)
                            {
                                m_vertices.Add(points[i][j]);
                                m_normals.Add((points[i][j] - center).normalized * f);
                                m_uv.Add(uvOffset + vWuv * i + new Vector2(0.0f, (uvInfo[j].y + 1.0f) * 0.5f));
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < points.Length; ++i)
                    {
                        for (int j = 0; j < points[i].Length; ++j)
                        {
                            m_vertices.Add(points[i][j]);
                            m_normals.Add((points[i][j] - center).normalized * f);
                        }
                    }
                }

                for (int i = 0; i < points.Length - 1; ++i)
                {
                    for (int j = 0; j < points[i].Length - 1; ++j)
                    {
                        m_triangles.Add(index + points[i].Length * (i + 1) + j + 1);
                        m_triangles.Add(index + points[i].Length * i + j + 1);
                        m_triangles.Add(index + points[i].Length * (i + 1) + j);
                        m_triangles.Add(index + points[i].Length * (i + 1) + j);
                        m_triangles.Add(index + points[i].Length * i + j + 1);
                        m_triangles.Add(index + points[i].Length * i + j);
                    }
                }
            }
            else
            {
                if (generateUV)
                {
                    Vector2[][] uvs = new Vector2[points.Length][];
                    Vector2 vWuv = Vector2.right * uvTiling.x / (points.Length - 1);
                    Vector2 vLuv = Vector2.up * uvTiling.y / (points[0].Length - 1);
                    for (int i = 0; i < points.Length; ++i)
                    {
                        uvs[i] = new Vector2[points[i].Length];
                        for (int j = 0; j < points[i].Length; ++j)
                        {
                            uvs[i][j] = uvOffset + vWuv * i + vLuv * j;
                        }
                    }

                    for (int i = 0; i < points.Length - 1; ++i)
                    {
                        for (int j = 0; j < points[i].Length - 1; ++j)
                        {
                            CreateQuad(points[i + 1][j], points[i + 1][j + 1], points[i][j + 1], points[i][j], uvs[i + 1][j], uvs[i + 1][j + 1], uvs[i][j + 1], uvs[i][j]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < points.Length - 1; ++i)
                    {
                        for (int j = 0; j < points[i].Length - 1; ++j)
                        {
                            CreateQuad(points[i + 1][j], points[i + 1][j + 1], points[i][j + 1], points[i][j]);
                        }
                    }
                }
            }
        }

        protected virtual void CreateMesh()
        {

        }

        public void Apply()
        {
            if (m_mesh == null) return;

            CreateMesh();

            //set mesh
            m_mesh.Clear();
            m_mesh.SetVertices(m_vertices);
            m_mesh.SetNormals(m_normals);
            m_mesh.SetTriangles(m_triangles, 0);
            m_mesh.SetUVs(0, m_uv);
            if (GetComponent<MeshFilter>() != null) GetComponent<MeshFilter>().sharedMesh = m_mesh;

            //clear
            m_vertices.Clear();
            m_triangles.Clear();
            m_normals.Clear();
            m_uv.Clear();
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            DestroyImmediate(m_mesh);
#else
            Destroy(m_mesh);
#endif
        }

        private void Awake()
        {
            m_mesh = new Mesh();
            Apply();
        }

        private void Reset()
        {
            Apply();
        }

        private void OnValidate()
        {
            Apply();
        }

        public GameObject CreateInstance()
        {
            return ProceduralPrimitives.CreateInstance(this);
        }
    }
}