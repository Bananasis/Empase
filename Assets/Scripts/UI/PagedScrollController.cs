using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PagedScrollController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private ScrollPaged _car;
    private bool _dragged;
    public UnityEvent<int> OnPageChange = new UnityEvent<int>();

    public ScrollPaged car
    {
        get
        {
            if (_car == null)
            {
                _car = GetComponent<ScrollPaged>();
            }

            return _car;
        }
        set => _car = value;
    }

    public int childCount = 1;
    int _currenHeroIndex;

    public int currentPage
    {
        get => _currentPage;
        set
        {
            if (_currentPage == value) return;
            _currentPage = value;
            OnPageChange.Invoke(value);
        }
    }

    private int _currentPage = 0;

    private float scrollbarCircular
    {
        get => car.horizontalScrollbar.value;
        set => car.horizontalScrollbar.value = value > 0 ? (value % 1 + 1) % 1 : 1 + value;
    }

    public int currentHeroIndex
    {
        get => _currenHeroIndex;
        set
        {
            _currenHeroIndex = childCount > 1 ? (value % (childCount - 1) + (childCount - 1)) % (childCount - 1) : 0;
            currentPage = childCount > 1 ? ((_currenHeroIndex - 1 + childCount - 1) % (childCount - 1)) : 0;
        }
    }

    public float swipeThreshold = 0.15f;
    float distance => 1f / (childCount - 1);

    float ItemPosition(int index)
    {
        return index * distance;
    }

    int ItemByPosition(float pos)
    {
        return (int) Math.Round(pos * (childCount - 1) + distance / 2);
    }


    public void GoToItem(int index)
    {
        // scrollbarCircular = ItemPosition(index);
        _currenHeroIndex = index + 1;
        currentPage = index;
    }


    void Update()
    {
        if (!_dragged && childCount > 1)
        {
            var pos = ItemPosition(currentHeroIndex);
            scrollbarCircular = Mathf.Lerp(scrollbarCircular, ClosestCircular(pos), 8f * Time.unscaledDeltaTime);
        }
    }

    float ClosestCircular(float pos)
    {
        var circPosUnder = pos - 1;
        var circPosOver = pos + 1;
        var closestPos = Math.Abs(scrollbarCircular - circPosUnder) < Math.Abs(scrollbarCircular - circPosOver)
            ? (Math.Abs(scrollbarCircular - circPosUnder) < Math.Abs(scrollbarCircular - pos) ? circPosUnder : pos)
            : (Math.Abs(scrollbarCircular - circPosOver) < Math.Abs(scrollbarCircular - pos) ? circPosOver : pos);

        return closestPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragged = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag();
        _dragged = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void EndDrag()
    {
//        Debug.LogError($"enddrag:{Input.touches[0].position.x}");
        if (currentHeroIndex == childCount - 1 || currentHeroIndex == 0)
        {
            if (scrollbarCircular > distance * swipeThreshold && scrollbarCircular < .5f)
            {
                if (scrollbarCircular > distance)
                {
                    currentHeroIndex = ItemByPosition(scrollbarCircular);

                    return;
                }

                currentHeroIndex++;
                return;
            }

            if (scrollbarCircular < 1 - distance * swipeThreshold && scrollbarCircular > .5f)
            {
                if (scrollbarCircular < 1 - distance)
                {
                    currentHeroIndex = ItemByPosition(scrollbarCircular);

                    return;
                }

                currentHeroIndex--;
                return;
            }
        }

        if (scrollbarCircular > ItemPosition(currentHeroIndex) + distance * swipeThreshold)
        {
            if (scrollbarCircular > ItemPosition(currentHeroIndex) + distance)
            {
                currentHeroIndex = ItemByPosition(scrollbarCircular);

                return;
            }

            currentHeroIndex++;
        }
        else if (scrollbarCircular < ItemPosition(currentHeroIndex) - distance * swipeThreshold)
        {
            if (scrollbarCircular < ItemPosition(currentHeroIndex) - distance)
            {
                currentHeroIndex = ItemByPosition(scrollbarCircular);
                return;
            }

            currentHeroIndex--;
        }
    }
}