using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    [SerializeField] private HexMapCamera hexMapCamera;
    [SerializeField] private ScreenTransformGesture twoFingerMoveGesture;
    public float PanSpeed = 0.01f;
    private void OnEnable()
    {
        twoFingerMoveGesture.Transformed += TwoFingerTransformHandler;
    }

    private void OnDisable()
    {
        twoFingerMoveGesture.Transformed -= TwoFingerTransformHandler;
    }

    private void TwoFingerTransformHandler(object sender, System.EventArgs e)
    {
        Vector3 vector = twoFingerMoveGesture.DeltaPosition * PanSpeed;
        hexMapCamera.AdjustPosition(vector.x, vector.y);
    }
}
