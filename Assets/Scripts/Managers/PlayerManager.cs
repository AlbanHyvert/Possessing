using Engine.Singleton;
using System;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerController _playerPrefab = null;

    private PlayerController _player = null;

    public PlayerController GetPlayer { get { return _player; } }

    public PlayerController SetPlayer 
    { 
        set 
        {
            _player = value;
            if(_findPlayer != null)
                _findPlayer(_player);

            if (_player != null)
                GameLoopManager.Instance.UpdateManager += OnUpdate;
            else
            {
                GameLoopManager.Instance.UpdateManager -= OnUpdate;
            }
        }
    }

    private event Action<PlayerController> _findPlayer = null;
    public event Action<PlayerController> FindPlayer
    {
        add
        {
            _findPlayer -= value;
            _findPlayer += value;
        }
        remove
        {
            _findPlayer -= value;
        }
    }

    private event Action<Vector2> _onPlayerPos = null;
    public event Action<Vector2> OnPlayerPos
    {
        add
        {
            _onPlayerPos -= value;
            _onPlayerPos += value;
        }
        remove
        {
            _onPlayerPos -= value;
        }
    }

    private void OnUpdate()
    {
        if (_onPlayerPos != null)
            _onPlayerPos(_player.transform.position);
    }
}
