using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _startingBody = null;
    [SerializeField] private float _maxDistFromPlayer = 100;
    private E_Bodies _currentBody = E_Bodies.PLAYER;
    private GameObject _portableObj = null;

    public GameObject GetPortableObj { get { return _portableObj; } }
    private E_Bodies GetCurrentBody { get { return _currentBody; } }
    private E_Bodies SetCurrentBody { set { _currentBody = value; } }
    public GameObject SetPortableObj { set { _portableObj = value; } }
    public HumanoidBody GetHumanoid { get { return _humanoid; } }
    public float MaxDistFromPlayer { get { return _maxDistFromPlayer; } }

    #region BodyRefType
    private HumanoidBody _humanoid = null;
    private BallBody _ball = null;
    #endregion BodyRefType

    private event Action _selfUpdate = null;
    public event Action SelfUpdate
    {
        add
        {
            _selfUpdate -= value;
            _selfUpdate += value;
        }
        remove
        {
            _selfUpdate -= value;
        }
    }

    private void Start()
    {
        PlayerManager.Instance.SetPlayer = this;

        FindBodyType(_startingBody.transform);

        if(_currentBody == E_Bodies.PLAYER)
        {
            InputManager.Instance.MovingDir += SelfMovement;
            //InputManager.Instance.Jump += SelfJump;
        }
        else
        {
            InputManager.Instance.MovingDir += PossessedMovement;
            InputManager.Instance.Jump += PossessedJump;
        }
    }

    public void FindBodyType(Transform body)
    {
        GameLoopManager.Instance.UpdatePlayer -= OnUpdate;

        if(_humanoid != null)
        {
            _humanoid.SetPlayer(null);
            _humanoid = null;
            SetCurrentBody = E_Bodies.PLAYER;
        }
        else if(_ball != null)
        {
            _ball.SetPlayer(null);
            _ball = null;
            SetCurrentBody = E_Bodies.PLAYER;
        }

        switch (body.gameObject.layer)
        {
            case 8:
                _humanoid = body.GetComponent<HumanoidBody>();
                this.transform.position = body.transform.position;
                this.transform.SetParent(body);
                _humanoid.SetPlayer(this);
                SetCurrentBody = E_Bodies.HUMANOID;
                GameLoopManager.Instance.UpdatePlayer += OnUpdate;
                break;
            case 9:
                _ball = body.GetComponent<BallBody>();
                this.transform.position = body.transform.position;
                this.transform.SetParent(body);
                _ball.SetPlayer(this);
                SetCurrentBody = E_Bodies.BALL;
                GameLoopManager.Instance.UpdatePlayer += OnUpdate;
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
            case 13:
                break;
            case 14:
                break;
            default:
                break;
        }
    }

    private void SelfMovement(Vector2 dir)
    {
        transform.Translate(dir);
    }

    private void PossessedJump(Vector2 dir)
    {
        if (_humanoid != null)
            _humanoid.SetJumpDir = dir;
        else if (_ball != null)
            _ball.SetJumpDir = dir;
    }

    private void PossessedMovement(Vector2 dir)
    {
        if (_humanoid != null)
            _humanoid.Direction(dir);
        else if (_ball != null)
            _ball.Direction(dir);
    }

    private void OnUpdate()
    {
        if (_selfUpdate != null)
            _selfUpdate();
    }
}