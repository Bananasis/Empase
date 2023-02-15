using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Zenject;

public partial class MouseInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEndDragHandler, IDragHandler
{
    [Inject] private ILevelManager _levelManager;
    private bool mousePressed;
    private bool mousePressedLastState;
    private bool shiftPressed;
    private Vector2 deltaScroll;
    private Camera cam;
    private float pressDuration;




    private void Start()
    {
        cam = Camera.main;
#if UNITY_EDITOR
        //       OnScrollNoShift.AddListener((d) => Debug.Log($"shift {d}"));
        // OnScrollShift.AddListener((d) => Debug.Log($"noshift {d}"));
        // mousePress.AddListener((p,d,b) => Debug.Log($"press {d} {p} {b}"));
        // mouseClick.AddListener((d) => Debug.Log($"click {d}"));
#endif
    }

    private void Update()
    {
        if (_levelManager.paused == PauseState.Pause) return;
        shiftPressed = Input.GetKey(KeyCode.LeftShift);
        deltaScroll += Input.mouseScrollDelta;
    }

    private void FixedUpdate()
    {
        if (deltaScroll != Vector2.zero)
        {
            if (shiftPressed) OnScrollShift.Invoke(deltaScroll);
            else OnScrollNoShift.Invoke(deltaScroll);
            deltaScroll = Vector2.zero;
        }

        if (!(mousePressed || mousePressedLastState)) return;
        Vector2 worldPosition = cam.ScreenToWorldPoint(Input.mousePosition -
                                                       cam.transform.position.z * Vector3.forward);
        if (mousePressed)
        {
            OnMousePress.Invoke((worldPosition,pressDuration, true));
            pressDuration += Time.fixedDeltaTime;
            if (!mousePressedLastState)
                OnMouseClick.Invoke(worldPosition);
        }
        else
        {
            if (mousePressedLastState)
            {
                OnMousePress.Invoke((worldPosition, pressDuration, false));
                pressDuration = 0;
            }
        }

        mousePressedLastState = mousePressed;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        mousePressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mousePressed = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        mousePressed = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        mousePressed = true;
    }
}