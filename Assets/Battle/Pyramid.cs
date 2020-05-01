using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class Pyramid : MonoBehaviour
{
    [Range(3, 8)]
    [SerializeField] private int m_Count = 3;

    [Range(0, 8)]
    [SerializeField] private float m_Height = 3;

    private Mesh m_Mesh;
    private Vector3[] m_Vertices;

    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = m_Mesh = new Mesh();
        m_Mesh.name = "Pyramid";

        m_Vertices = new Vector3[m_Count + 1];
        Vector2[] uv = new Vector2[m_Vertices.Length];
        Vector4[] tangents = new Vector4[m_Vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        m_Vertices[0] = new Vector3(0, m_Height, 0);
        uv[0] = new Vector2(0.5f, 0.5f);
        tangents[0] = tangent;

        for (int i = 1; i <= m_Count; i++)
        {
            float angle = Mathf.PI * 2 / m_Count * i;

            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);

            m_Vertices[i] = new Vector3(sin, 0, cos);
            uv[i] = new Vector2(0.5f, 0.5f);
            tangents[i] = tangent;
        }

        m_Mesh.vertices = m_Vertices;
        m_Mesh.uv = uv;
        m_Mesh.tangents = tangents;


        int[] triangles = new int[(m_Count + 1) * 3];

        for (int i = 0; i < m_Count; i++)
        {
            int index = i * 3;

            triangles[index + 2] = i + 1;
            triangles[index + 1] = 0;

            if (i == m_Count - 1)
            {
                triangles[index] = 1;
            }
            else
            {
                triangles[index] = i + 2;
            }
        }

        triangles[m_Count * 3 + 2] = 1;
        triangles[m_Count * 3 + 1] = 2;
        triangles[m_Count * 3] = 3;

        m_Mesh.triangles = triangles;
        m_Mesh.RecalculateNormals();
    }
}
