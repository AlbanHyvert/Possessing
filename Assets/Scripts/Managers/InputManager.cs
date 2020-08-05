using Engine.Singleton;
using System;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private Vector2 _movingDirection = Vector2.zero;
    private Vector2 _jumpDir = Vector2.zero;
    private Vector3 _mousePosition = Vector3.zero;

    private Vector3 SetMousePosition
    {
        set
        {
            _mousePosition = value;

            if(_onMousePosition != null)
                _onMousePosition(_mousePosition);
        }
    }

    #region Events
    private event Action<Vector3> _onMousePosition = null;
    public event Action<Vector3> OnMousPosition
    {
        add
        {
            _onMousePosition -= value;
            _onMousePosition += value;
        }
        remove
        {
            _onMousePosition -= value;
        }
    }

    private event Action<Vector2> _movingDir = null;
    public event Action<Vector2> MovingDir
    {
        add
        {
            _movingDir -= value;
            _movingDir += value;
        }
        remove
        {
            _movingDir -= value;
        }
    }

    private event Action<Vector2> _jump = null;
    public event Action<Vector2> Jump
    {
        add
        {
            _jump -= value;
            _jump += value;
        }
        remove
        {
            _jump -= value;
        }
    }

    private event Action _interact = null;
    public event Action Interact
    {
        add
        {
            _interact -= value;
            _interact += value;
        }
        remove
        {
            _interact -= value;
        }
    }

    private event Action _possesing = null;
    public event Action Possessing
    {
        add
        {
            _possesing -= value;
            _possesing += value;
        }
        remove
        {
            _possesing -= value;
        }
    }
    #endregion Events

    protected override void Awake()
    {
        base.Awake();

        GameLoopManager.Instance.UpdateManager += OnUpdate;
    }

    private void OnUpdate()
    {
        _movingDirection = Vector3.zero;

        SetMousePosition = Input.mousePosition;
        SetMousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);

        if(Input.GetKey(KeyCode.Z))
        {
            _movingDirection = PlayerManager.Instance.GetPlayer.transform.right;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            _movingDirection = -PlayerManager.Instance.GetPlayer.transform.right;
        }

        if (_interact != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                _interact();
            }
        }

        if(_possesing != null)
        {
            if(Input.GetMouseButtonDown(0))
            {
                _possesing();
            }
        }

        if (_movingDir != null)
            _movingDir(_movingDirection);

        if (_jump != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpDir = PlayerManager.Instance.GetPlayer.transform.up;
                _jump(_jumpDir);
            }

            _jumpDir = Vector2.zero;
        }
    }
}