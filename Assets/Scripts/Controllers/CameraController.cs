using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController _player = null;
    private GameObject _newBody = null;

    private void Start()
    {
        _player = PlayerManager.Instance.GetPlayer;

        if(_player != null)
        {
            PlayerManager.Instance.OnPlayerPos += FollowPlayer;
        }

        InputManager.Instance.OnMousPosition += MousePosition;
        InputManager.Instance.Possessing += PossessBody;
        PlayerManager.Instance.FindPlayer += GetPlayer;

        GameLoopManager.Instance.GamePaused += IsPaused;
    }

    private void IsPaused(bool value)
    {
        if(value == false)
        {
            if(_player != null)
                PlayerManager.Instance.OnPlayerPos += FollowPlayer;

            InputManager.Instance.OnMousPosition += MousePosition;
            InputManager.Instance.Possessing += PossessBody;
            PlayerManager.Instance.FindPlayer += GetPlayer;
        }
        else
        {
            PlayerManager.Instance.OnPlayerPos -= FollowPlayer;
            InputManager.Instance.OnMousPosition -= MousePosition;
            InputManager.Instance.Possessing -= PossessBody;
            PlayerManager.Instance.FindPlayer -= GetPlayer;
        }
    }

    private void GetPlayer(PlayerController player)
    {
        _player = player;
        PlayerManager.Instance.OnPlayerPos += FollowPlayer;
    }

    private void MousePosition(Vector3 pos)
    {
        Vector2 raycastPos = pos;
        RaycastHit2D hit = Physics2D.Raycast(raycastPos, Vector2.zero);

        if (hit.collider != null)
        {
            _newBody = hit.collider.gameObject;

            switch (_newBody.layer)
            {
                case 9:
                    _player.SetPortableObj = _newBody;
                    break;
                default:
                    break;
            }
        }
        else
        {
            if(_player.GetPortableObj != null)
                _player.GetPortableObj.transform.SetParent(null);

            _player.SetPortableObj = null;
            _newBody = null;
        }

        HumanoidBody body = _player.GetHumanoid;

        if(body != null && body.IsHolding == true)
        {
            if(_player.GetPortableObj != null)
            {
                float dist = Vector2.Distance(_player.transform.localPosition, _player.GetPortableObj.transform.localPosition);
                _player.GetPortableObj.transform.position = (Vector2)pos;
            } 
        }
    }

    private void PossessBody()
    {
        if(_newBody != null)
        {
            switch (_newBody.layer)
            {
                case 8:
                    _player.FindBodyType(_newBody.transform);
                    break;
                case 9:
                    _player.FindBodyType(_newBody.transform);
                    break;
                case 10:
                    _player.FindBodyType(_newBody.transform);
                    break;
                case 11:
                    _player.FindBodyType(_newBody.transform);
                    break;
                case 12:
                    _player.FindBodyType(_newBody.transform);
                    break;
                case 13:
                    _player.FindBodyType(_newBody.transform);
                    break;
                case 14:
                    _player.FindBodyType(_newBody.transform);
                    break;
                default:
                    break;
            }
        }
    }

    private void FollowPlayer(Vector2 playerPos)
    {
        if(CanSeePlayer() == false)
        {
            Vector2 selfPos = this.transform.position;
            selfPos.x = playerPos.x + 3;
            selfPos.y = playerPos.y + 1;
            this.transform.position = selfPos;
        }
    }

    private bool CanSeePlayer()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        if (GeometryUtility.TestPlanesAABB(planes, _player.GetBodyCol.bounds))
        {
            return true;
        }
        else
            return false;
    }
}