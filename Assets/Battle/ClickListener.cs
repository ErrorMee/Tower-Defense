using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public static ClickListener Get(GameObject obj)
    {
        ClickListener listener = obj.GetComponent<ClickListener>();
        if (listener == null)
        {
            listener = obj.AddComponent<ClickListener>();
        }
        return listener;
    }

    System.Action<GameObject> mClickedHandler = null;
    System.Action<GameObject,float,float> mPointerUpHandler = null;
    System.Action<GameObject, float, float> mPointerDownHandler = null;
    System.Action<GameObject> mLongPressHandler = null;

    private bool isPointerDown;
    private float timePressStarted;
    public float durationThreshold = 1.0f;
    public float timeScale = 0f; // 需要越按越快使用 
    private bool longPressTriggered = false;
    private Vector2 timePressStartedPos;
    private string mClickSound;
    public bool enableClickSound = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        mClickedHandler?.Invoke(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        longPressTriggered = false;
        timePressStarted = Time.time;

        timePressStartedPos = eventData.position;
        if (mPointerDownHandler != null)
        {
            float timePress = Time.time - timePressStarted;
            float magnitude = (timePressStartedPos - eventData.position).magnitude;
            mPointerDownHandler.Invoke(gameObject, timePress, magnitude);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        longPressTriggered = true;
        if (mPointerUpHandler != null)
        {
            float timePress = Time.time - timePressStarted;
            float magnitude = (timePressStartedPos - eventData.position).magnitude;
            mPointerUpHandler.Invoke(gameObject, timePress, magnitude);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerDown = false;
    }

    public void SetLongPressHandler(System.Action<GameObject> handler, string clickSound = "006_commonClick")
    {
        mLongPressHandler = handler;
        mClickSound = clickSound;
    }

    public void SetClickHandler(System.Action<GameObject> handler, string clickSound = "006_commonClick")
    {
        mClickedHandler = handler;
        mClickSound = clickSound;
    }

    public void SetPointerUpHandler(System.Action<GameObject, float, float> handler, string clickSound = "006_commonClick")
    {
        mPointerUpHandler = handler;
        mClickSound = clickSound;
    }

    public void SetPointerDownHandler(System.Action<GameObject, float, float> handler, string clickSound = "006_commonClick")
    {
        mPointerDownHandler = handler;
        mClickSound = clickSound;
    }

    private void Update()
    {
        if (isPointerDown && !longPressTriggered)
        {
            if (Time.time - timePressStarted > durationThreshold)
            {
                durationThreshold -= timeScale;
                if (durationThreshold <= 0.08f)
                {
                    durationThreshold = 0.08f;
                }
                timePressStarted = Time.time;
                mLongPressHandler?.Invoke(gameObject);
            }
            return;
        }
    }

}
