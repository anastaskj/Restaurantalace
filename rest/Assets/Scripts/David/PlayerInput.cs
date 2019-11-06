using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Vector2 _downPossition = Vector2.zero;
    private Vector2 _upPossition;
    private Vector2 _currentPossiton;
    private float _minSwipeDistance = 100;
    private float _swipeSpeed = 2;
    private float _dragTimer = 2;
    private float _dragTimerReset = 2;
    private bool _swiping = false;
    
    public delegate void InputHandler(Vector2 position);
    public static event InputHandler OnTouch;
    public static event InputHandler OnDrag;

    public delegate void SwipeHandler(float distance);
    public static event SwipeHandler OnSwipe;
    public static event SwipeHandler OnMove;

    void Update()
    {
        if (Input.touches.Length < 0)
            return;

        if (Input.touches.Length > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            this._downPossition = Input.GetTouch(0).position;
        }

        if (this._downPossition == Vector2.zero)
            return;
        else if (this._dragTimer > 0)
            this._dragTimer -= Time.deltaTime;

        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            this._currentPossiton = Input.GetTouch(0).position;
            float distance = this.GetSwipeDistance(this._downPossition, this._currentPossiton);

            if (distance != 0 && this._dragTimer > 0)
            {
                this._swiping = true;
                OnMove?.Invoke(distance);
            }
            else if (this._dragTimer <= 0 && !this._swiping)
                OnDrag?.Invoke(this._currentPossiton);
        }

        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            this._upPossition = Input.GetTouch(0).position;

            int direction = this.GetSwipeDirection(this._downPossition, this._upPossition);

            if (direction != 0)
            {
                this._swiping = false;
                OnSwipe?.Invoke(direction);
            }
            else if (!this._swiping)
                OnTouch?.Invoke(this._downPossition);

            this._dragTimer = this._dragTimerReset;
            this._downPossition = Vector2.zero;
        }
    }
    
    private float GetSwipeDistance(Vector2 downPossition, Vector2 currentPossiton)
    {
        if (Math.Abs(downPossition.x - currentPossiton.x) > this._minSwipeDistance)
        {
            return (downPossition.x - currentPossiton.x) * this._swipeSpeed / 4000;
        }

        return 0;
    }

    private int GetSwipeDirection(Vector2 downPossition, Vector2 upPossiton)
    {
        if (Math.Abs(downPossition.x - upPossiton.x) > _minSwipeDistance)
        {
            if (downPossition.x > upPossiton.x)
                return 1;
            else
                return -1;
        }

        return 0;
    }
}
