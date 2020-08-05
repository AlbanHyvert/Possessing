using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallBody : MonoBehaviour
{
    [SerializeField] private E_BodyStatus _status = E_BodyStatus.FREE;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _accelerationSpeed = 2;
    [SerializeField] private float _desallerationSpeed = 0;
    [SerializeField] private int _jumpForce = 100;
    [SerializeField] private LayerMask _jumpableSurfaces = 0;
    [SerializeField] private ParticleSystem _particle = null;

    #region Privates
    private Dictionary<E_BallState, IBallStates> _states = null;
    private E_BallState _currentState = E_BallState.IDLE;
    private PlayerController _player = null;
    private Rigidbody2D _rb = null;
    private Vector2 _jumpDir = Vector2.zero;
    private Vector2 _direction = Vector2.zero;
    private bool _isGrounded = false;
    #endregion Privates

    #region Properties
    #region GET
    public PlayerController GetPlayer { get { return _player; } }
    public Rigidbody2D GetRigidbody { get { return _rb; } }
    public bool GetIsGrounded { get { return _isGrounded; } }
    public float GetSpeed { get { return _speed; } }
    public float GetAccSpeed { get { return _accelerationSpeed; } }
    public float GetDesaccSpeed { get { return _desallerationSpeed; } }
    public int GetJumpForce { get { return _jumpForce; } }
    public Vector2 GetDirection { get { return _direction; } }
    #endregion GET

    #region SET
    public Vector2 SetJumpDir
    {
        set
        {
            _jumpDir = value;

            if(_jumping != null)
                _jumping(_jumpDir);
        }
    }
    private bool SetIsGrounded
    {
        set
        {
            _isGrounded = value;
            if(_onIsGrounded != null)
                _onIsGrounded(_isGrounded);
        }
    }
    #endregion SET
    #endregion Properties

    #region Events
    private event Action<bool> _onIsGrounded = null;
    public event Action<bool> OnIsGrounded
    {
        add
        {
            _onIsGrounded -= value;
            _onIsGrounded += value;
        }
        remove
        {
            _onIsGrounded -= value;
        }
    }

    private event Action<Vector2> _jumping = null;
    public event Action<Vector2> Jumping
    {
        add
        {
            _jumping -= value;
            _jumping += value;
        }
        remove
        {
            _jumping -= value;
        }
    }
    #endregion Events

    public void Direction(Vector2 dir)
    {
        _direction = dir;
    }

    public void SetPlayer(PlayerController player)
    {
        ParticleSystem.MainModule main = _particle.main;

        if (_player != null)
            _player.SelfUpdate -= Tick;

        if (player != null)
        {
            _status = E_BodyStatus.POSSESSED;
            main.startColor = Color.blue;

            GameLoopManager.Instance.UpdateBody -= Tick;
            player.SelfUpdate += Tick;
            _player = player;
        }
        else
        {
            _status = E_BodyStatus.FREE;

            main.startColor = Color.yellow;

            _player = null;
            GameLoopManager.Instance.UpdateBody += Tick;
            Direction(Vector2.zero);
        }
    }

    private void FindPlayer(PlayerController player)
    {
        _player = player;
    }

    private void Init()
    {
        _states = new Dictionary<E_BallState, IBallStates>();

        _states.Add(E_BallState.IDLE, new BallIdleState());
        _states.Add(E_BallState.ROLL, new BallRollState());
        _states.Add(E_BallState.INAIR, new BallInAirState());

        _states[E_BallState.IDLE].Init(this);
        _states[E_BallState.ROLL].Init(this);
        _states[E_BallState.INAIR].Init(this);

        ChangeState(_currentState);
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _player = PlayerManager.Instance.GetPlayer;

        PlayerManager.Instance.FindPlayer += FindPlayer;

        Init();

        GameLoopManager.Instance.UpdateBody += Tick;
    }

    private void Tick()
    {
        bool isGrounded = Physics2D.Raycast(this.transform.position, -this.transform.up, 0.8f, _jumpableSurfaces);

        if (isGrounded != _isGrounded)
        {
            SetIsGrounded = isGrounded;
        }

        _states[_currentState].Tick();
    }

    public void ChangeState(E_BallState nextState)
    {
        _states[_currentState].Exit();

        GameLoopManager.Instance.UpdateBody -= Tick;

        if (_player != null)
            _player.SelfUpdate -= Tick;

        _currentState = nextState;

        _states[nextState].Enter();

        if (_player != null)
            _player.SelfUpdate += Tick;
        else
            GameLoopManager.Instance.UpdateBody += Tick;
    }
}