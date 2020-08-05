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
}
