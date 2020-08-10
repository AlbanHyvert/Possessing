using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HumanoidBody : MonoBehaviour
{
    [SerializeField] private E_BodyStatus _status = E_BodyStatus.FREE;
    [SerializeField] private float _movingSpeed = 5;
    [SerializeField] private float _checkGroundDist = 0.2f;
    [SerializeField] private float _jumpForce = 10;
    [SerializeField] private int _throwForce = 100;
    [SerializeField] private LayerMask _jumpableSurfaces = 0;
    [SerializeField] private Transform _hand = null;
    [SerializeField] private ParticleSystem _particle = null;
    [Space]
    [SerializeField] private Animator _animator = null;

    #region Privates
    private Dictionary<E_HumanoidStates, IHumanoidState> _states = null;
    private E_HumanoidStates _currentState = E_HumanoidStates.IDLE;
    private Vector2 _direction = Vector2.zero;
    private Vector2 _jumpDir = Vector2.zero;
    private bool _isHolding = false;
    private bool _isSprinting = false;
    private bool _isGrounded = false;
    private PlayerController _player = null;
    private Rigidbody2D _rb = null;
    #endregion Privates

    #region Properties
    #region Get
    public E_BodyStatus GetStatus { get { return _status; } }
    public Animator GetAnimator { get { return _animator; } }
    public float MovingSpeed { get { return _movingSpeed; } }
    public float JumpForce { get { return _jumpForce; } }
    public bool IsHolding { get { return _isHolding; } }
    public Vector2 GetDirection { get { return _direction; } }
    public bool GetIsSprinting { get { return _isSprinting; } }
    public bool GetIsGrounded { get { return _isGrounded; } }
    public Rigidbody2D GetRigidbody { get { return _rb; } }
    #endregion Get

    #region Set
    public E_BodyStatus SetStatus
    { 
        set 
        {
            _status = value;

            if (_updateStatus != null)
                _updateStatus(_status);   
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
    public Vector2 SetJumpDir
    {
        set
        {
            _jumpDir = value;

            if(_jumping != null)
                _jumping(_jumpDir);
        }
    }
    #endregion Set
    #endregion Properties

    #region Events
    private event Action<E_BodyStatus> _updateStatus = null;
    public event Action<E_BodyStatus> UpdateStatus
    {
        add
        {
            _updateStatus -= value;
            _updateStatus += value;
        }
        remove
        {
            _updateStatus -= value;
        }
    }

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

    private void Init()
    {
        _states = new Dictionary<E_HumanoidStates, IHumanoidState>();

        _states.Add(E_HumanoidStates.IDLE, new HumanIdleState());
        _states.Add(E_HumanoidStates.WALK, new HumanWalkState());
        _states.Add(E_HumanoidStates.SPRINT, new HumanSprintState());
        _states.Add(E_HumanoidStates.INAIR, new HumanInAirState());

        _states[E_HumanoidStates.IDLE].Init(this);
        _states[E_HumanoidStates.WALK].Init(this);
        _states[E_HumanoidStates.SPRINT].Init(this);
        _states[E_HumanoidStates.INAIR].Init(this);

        ChangeState(_currentState);
    }

    public void SetPlayer(PlayerController playerController)
    {
        if(_player != null)
        {
            _player.SelfUpdate -= Tick;
            if (_player.GetPortableObj != null)
            {
                _player.GetPortableObj.transform.SetParent(null);
            }
        }   

        if (playerController != null)
        {
            ParticleSystem.MainModule main = _particle.main;

            SetStatus = E_BodyStatus.POSSESSED;
            main.startColor = Color.blue;
            GameLoopManager.Instance.UpdateBody -= Tick;
            playerController.SelfUpdate += Tick;
            _player = playerController;
        }
        else
        {
            ParticleSystem.MainModule main = _particle.main;

            SetStatus = E_BodyStatus.FREE;
            main.startColor = Color.yellow;

            FixedJoint2D joint = this.GetComponent<FixedJoint2D>();

            if (joint != null)
                joint.connectedBody = null;
            if(_player != null)
                _player.SetPortableObj = null;
            _player = null;

            GameLoopManager.Instance.UpdateBody += Tick;
            Direction(Vector2.zero);
        }
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        GameLoopManager.Instance.GamePaused += IsPaused;

        Init();

        ParticleSystem.MainModule main = _particle.main;

        switch (_status)
        {
            case E_BodyStatus.POSSESSED:
                main.startColor = Color.blue;
                break;
            case E_BodyStatus.FREE:
                main.startColor = Color.yellow;
                break;
            default:
                break;
        }

        GameLoopManager.Instance.UpdateBody += Tick;
        //InputManager.Instance.Interact += PickUp;
    }

    private void IsPaused(bool value)
    {
        if(value == false)
        {
            if(_player != null)
                SetPlayer(_player);
        }
        else
        {
            GameLoopManager.Instance.UpdateBody -= Tick;
            if(_player != null)
                _player.SelfUpdate -= Tick;
            InputManager.Instance.Interact -= PickUp;
            Direction(Vector2.zero);
            ChangeState(E_HumanoidStates.IDLE);
        }
    }

    public void Direction(Vector2 dir)
    {
        _direction = dir;
    }

    private void PickUp()
    {
        if (_player.GetPortableObj != null)
        {
            FixedJoint2D joint = this.GetComponent<FixedJoint2D>();

            joint.connectedBody = _player.GetPortableObj.GetComponent<Rigidbody2D>();
            _player.GetPortableObj.transform.SetParent(_hand);

            _isHolding = true;

            InputManager.Instance.Interact += Throw;
            InputManager.Instance.Interact -= PickUp;
        }

    }

    private void Interact()
    {

    }

    private void Throw()
    {
        FixedJoint2D joint = this.GetComponent<FixedJoint2D>();
        Rigidbody2D portableRb = _player.GetPortableObj.GetComponent<Rigidbody2D>();

        joint.connectedBody = null;
        _player.GetPortableObj.transform.SetParent(null);
        portableRb.AddForce(this.transform.right * _throwForce);
        _isHolding = false;
        _player.SetPortableObj = null;
        InputManager.Instance.Interact += PickUp;
        InputManager.Instance.Interact -= Throw;
    }

    private void Tick()
    {
        bool isGrounded = Physics2D.Raycast(this.transform.position, -this.transform.up, _checkGroundDist, _jumpableSurfaces);

        if (isGrounded != _isGrounded)
        {
            SetIsGrounded = isGrounded;

            if(_isGrounded == true)
            {
                if(_animator != null)
                    _animator.SetBool("isJumping", false);
            }
            else
            {
                if (_animator != null)
                    _animator.SetBool("isJumping", true);
            }
        }

        _states[_currentState].Tick();
    }

    public void ChangeState(E_HumanoidStates nextState)
    {
        _states[_currentState].Exit();

        GameLoopManager.Instance.UpdateBody -= Tick;

        if(_player != null)
            _player.SelfUpdate -= Tick;

        _currentState = nextState;

        _states[nextState].Enter();

        if(_player != null)
            _player.SelfUpdate += Tick;
        else
            GameLoopManager.Instance.UpdateBody += Tick;
    }
}