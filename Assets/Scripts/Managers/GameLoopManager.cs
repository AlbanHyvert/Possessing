using Engine.Singleton;
using System;
using UnityEngine;

public class GameLoopManager : Singleton<GameLoopManager>
{
    private bool _ispaused = false;

    #region Properties

    #region Set
    public bool SetIsPaused
    {
        set
        {
            _ispaused = value;
            if(_gamePaused != null)
                _gamePaused(_ispaused);
        }
    }
    #endregion Set
    #endregion Properties

    #region Events
    private event Action _updateBody = null;
    public event Action UpdateBody
    {
        add
        {
            _updateBody -= value;
            _updateBody += value;
        }
        remove
        {
            _updateBody -= value;
        }
    }

    private event Action _updatePlayer = null;
    public event Action UpdatePlayer
    {
        add
        {
            _updatePlayer -= value;
            _updatePlayer += value;
        }
        remove
        {
            _updatePlayer -= value;
        }
    }

    private event Action _updateCamera = null;
    public event Action UpdateCamera
    {
        add
        {
            _updateCamera -= value;
            _updateCamera += value;
        }
        remove
        {
            _updateCamera -= value;
        }
    }

    private event Action _updateManager = null;
    public event Action UpdateManager
    {
        add
        {
            _updateManager -= value;
            _updateManager += value;
        }
        remove
        {
            _updateManager -= value;
        }
    }

    private event Action<bool> _gamePaused = null;
    public event Action<bool> GamePaused
    {
        add
        {
            _gamePaused -= value;
            _gamePaused += value;
        }
        remove
        {
            _gamePaused -= value;
        }
    }
    #endregion Events

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            SetIsPaused = !_ispaused;
        }

        if (_updateBody != null)
            _updateBody();

        if (_updatePlayer != null)
            _updatePlayer();

        if (_updateCamera != null)
            _updateCamera();

        if (_updateManager != null)
            _updateManager();
    }
}