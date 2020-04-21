using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    [SerializeField] private HexMapCamera hexMapCamera;
    
    [SerializeField] private ScreenTransformGesture twoFingerMoveGesture = default;
    [SerializeField] private ScreenTransformGesture manipulationGesture = default;

    public float PanSpeed = 0.05f;
    public float RotationSpeed = 0.5f;
    public float ZoomSpeed = 0.5f;

    private void OnEnable()
    {
        twoFingerMoveGesture.Transformed += TwoFingerTransformHandler;
        manipulationGesture.Transformed += ManipulationTransformedHandler;
    }

    private void OnDisable()
    {
        twoFingerMoveGesture.Transformed -= TwoFingerTransformHandler;
        manipulationGesture.Transformed -= ManipulationTransformedHandler;
    }

    private void TwoFingerTransformHandler(object sender, System.EventArgs e)
    {
        Vector3 vector = twoFingerMoveGesture.DeltaPosition * PanSpeed;
       
        if (vector.x != 0f || vector.y != 0f)
        {
            hexMapCamera.AdjustPosition(-vector.x, -vector.y);
        }
    }

    private void ManipulationTransformedHandler(object sender, System.EventArgs e)
    {
        float deltaRotation = manipulationGesture.DeltaRotation * RotationSpeed;
        if (deltaRotation != 0f)
        {
            hexMapCamera.AdjustRotation(deltaRotation);
        }

        float deltaZoom = (manipulationGesture.DeltaScale - 1f) * ZoomSpeed;
        if (deltaZoom != 0f)
        {
            hexMapCamera.AdjustZoom(deltaZoom);
        }
    }
}
