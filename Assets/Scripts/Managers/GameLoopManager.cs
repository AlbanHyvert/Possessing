using Engine.Singleton;
using System;

public class GameLoopManager : Singleton<GameLoopManager>
{
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

    private void Update()
    {
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
