using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public enum TouchState
{
    Edit,
    Unit
}

public class TouchController : MonoBehaviour
{
    [SerializeField] private HexMapCamera hexMapCamera = default;
    [SerializeField] private HexMapEditor hexMapEditor = default;
    [SerializeField] private HexGameUI hexGameUI = default;

    [SerializeField] private ScreenTransformGesture twoFingerMoveGesture = default;
    [SerializeField] private ScreenTransformGesture manipulationGesture = default;
    [SerializeField] private TapGesture dblclickGesture = default;
    [SerializeField] private LongPressGesture longPressGesture = default;

    public float PanSpeed = 0.02f;
    public float RotationSpeed = 0.5f;
    public float ZoomSpeed = 0.5f;

    private bool EditMode = true;

    private TouchState touchState = TouchState.Edit;

    private void OnEnable()
    {
        twoFingerMoveGesture.Transformed += FingerTransformHandler;
        twoFingerMoveGesture.StateChanged += FingerChangedHandler;
        manipulationGesture.Transformed += ManipulationTransformedHandler;
        dblclickGesture.Tapped += DBLTappedHandler;
        longPressGesture.StateChanged += LongPressedHandler;
    }

    private void OnDisable()
    {
        twoFingerMoveGesture.Transformed -= FingerTransformHandler;
        twoFingerMoveGesture.StateChanged -= FingerChangedHandler;
        manipulationGesture.Transformed -= ManipulationTransformedHandler;
        dblclickGesture.Tapped -= DBLTappedHandler;
        longPressGesture.StateChanged -= LongPressedHandler;
    }

    private void FingerTransformHandler(object sender, System.EventArgs e)
    {
        if (hexMapEditor.validEdit)
        {
            return;
        }

        Vector3 vector = twoFingerMoveGesture.DeltaPosition * PanSpeed;

        if (touchState == TouchState.Unit)
        {
            
        } else
        {
            if (vector.x != 0f || vector.y != 0f)
            {
                hexMapCamera.AdjustPosition(-vector.x, -vector.y);
            }
        }
    }

    private void FingerChangedHandler(object sender, GestureStateChangeEventArgs e)
    {
        //Debug.Log(e.State);
        if (!EditMode)
        {
            switch (e.State)
            {
                case Gesture.GestureState.Began:
                    if (hexGameUI.DoSelection() != null)
                    {
                        touchState = TouchState.Unit;
                    }
                    else
                    {
                        touchState = TouchState.Edit;
                    }
                    break;
                case Gesture.GestureState.Recognized:
                    if (touchState == TouchState.Unit)
                    {
                        hexGameUI.DoMove();
                        touchState = TouchState.Edit;
                    }
                    break;
            }
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

        Vector3 vector = manipulationGesture.DeltaPosition * PanSpeed;

        if (vector.x != 0f || vector.y != 0f)
        {
            hexMapCamera.AdjustPosition(-vector.x, -vector.y);
        }
    }

    private void DBLTappedHandler(object sender, System.EventArgs e)
    {
        hexMapEditor.CreateUnit();
    }

    private void LongPressedHandler(object sender, GestureStateChangeEventArgs e)
    {
        if (e.State == Gesture.GestureState.Recognized)
        {
            hexMapEditor.DestroyUnit();
        }
    }

    public void SetEditMode(bool toggle)
    {
        EditMode = toggle;
    }
}
