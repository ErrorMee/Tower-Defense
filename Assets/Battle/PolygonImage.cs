#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif
using UnityEngine;
using UnityEngine.UI;

public class PolygonImage : Image
{
    [Range(3, 32)]
    [SerializeField]
    private int m_Count = 3;
    public int Count {
        get { return m_Count; }
        set { m_Count = Mathf.Clamp(value,3,32); SetVerticesDirty(); }
    }

    [SerializeField]
    private Vector2 m_Center = Vector2.zero;
    public Vector2 Center
    {
        get { return m_Center; }
        set {
            m_Center.x = Mathf.Clamp(value.x, -1, 1);
            m_Center.y = Mathf.Clamp(value.y, -1, 1);
            SetVerticesDirty();
        }
    }

    [SerializeField]
    private float m_Radius = 100;
    public float Radius
    {
        get { return m_Radius; }
        set {
            m_Radius = Mathf.Clamp(value, 0, rectTransform.sizeDelta.x);
            SetVerticesDirty();
        }
    }

    [Range(0, 120)]
    [SerializeField]
    private float m_Angle = 0;
    public float Angle
    {
        get { return m_Angle; }
        set
        {
            m_Angle = Mathf.Clamp(value, 0, 120);
            SetVerticesDirty();
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        Vector3 centerPostion = new Vector3(rectTransform.sizeDelta.x * m_Center.x,
            rectTransform.sizeDelta.y * m_Center.y, 0);
        Vector2 centerUV = m_Center + Vector2.one / 2;

        UIVertex center = UIVertex.simpleVert;
        center.position = centerPostion;
        center.uv0 = centerUV;
        center.color = this.color;

        vh.AddVert(center);

        float radian = m_Angle / 180 * Mathf.PI;
        float angle;
        Vector3 cornerPostion;
        Vector2 cornerUV = Vector2.zero;

        for (int i = 0; i < m_Count; i++)
        {
            angle = Mathf.PI * 2 / m_Count * i + radian;

            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);

            cornerPostion.x = sin * m_Radius;
            cornerPostion.y = cos * m_Radius;
            cornerPostion.z = 0;

            cornerUV.x = sin * m_Radius / rectTransform.sizeDelta.x;
            cornerUV.y = cos * m_Radius / rectTransform.sizeDelta.y;

            UIVertex cornerVertex = UIVertex.simpleVert;
            cornerVertex.position = cornerPostion + centerPostion;
            cornerVertex.uv0 = cornerUV + centerUV;
            cornerVertex.color = this.color;

            vh.AddVert(cornerVertex);
        }

        for (int i = 1; i < m_Count; i++)
        {
            vh.AddTriangle(0, i, i + 1);
        }
        vh.AddTriangle(0, m_Count, 1);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(PolygonImage), true)]
public class PolygonImageEditor : GraphicEditor
{
    SerializedProperty m_Sprite;
    GUIContent m_SpriteContent;

    SerializedProperty m_Count;
    SerializedProperty m_Center;
    SerializedProperty m_Radius;
    SerializedProperty m_Angle;

    protected override void OnEnable()
    {
        base.OnEnable();

        m_SpriteContent = new GUIContent("Source Image");
        m_Sprite = serializedObject.FindProperty("m_Sprite");

        m_Count = serializedObject.FindProperty("m_Count");
        m_Center = serializedObject.FindProperty("m_Center");
        m_Radius = serializedObject.FindProperty("m_Radius");
        m_Angle = serializedObject.FindProperty("m_Angle");

        SetShowNativeSize(true);
    }

    protected override void OnDisable() {}

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SpriteGUI();
        AppearanceControlsGUI();
        RaycastControlsGUI();
        SetShowNativeSize(false);
        NativeSizeButtonGUI();
        MaskGUI();

        serializedObject.ApplyModifiedProperties();
    }

    void SetShowNativeSize(bool instant)
    {
        Image.Type type = Image.Type.Filled;
        bool showNativeSize = (type == Image.Type.Simple || type == Image.Type.Filled) && m_Sprite.objectReferenceValue != null;
        base.SetShowNativeSize(showNativeSize, instant);
    }

    protected void SpriteGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_Sprite, m_SpriteContent);
        EditorGUI.EndChangeCheck();
    }

    protected void MaskGUI()
    {
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(m_Count);
        EditorGUILayout.PropertyField(m_Center);
        EditorGUILayout.PropertyField(m_Radius);
        EditorGUILayout.PropertyField(m_Angle);

        EditorGUI.EndChangeCheck();
    }

    [MenuItem("GameObject/UI/PolygonImage")]
    public static void CreatePolygonImage()
    {
        var goRoot = Selection.activeGameObject;
        if (goRoot == null)
            return;
        var polygon = new GameObject("PolygonImage");
        polygon.AddComponent<PolygonImage>();
        polygon.transform.SetParent(goRoot.transform, false);
        polygon.transform.SetAsLastSibling();
        Undo.RegisterCreatedObjectUndo(polygon, "Created " + polygon.name);
    }
}
#endif