using Engine.Singleton;
using UnityEngine;

public class CPsManager : Singleton<CPsManager>
{
    private CheckPoint _lastHitCP = null;

    public CheckPoint SetLastHitCP { set { _lastHitCP = value; } }

    public void RespawnPlayer()
    {
        Transform player = PlayerManager.Instance.GetPlayer.GetBody;

        player.position = (Vector2)_lastHitCP.transform.position;
    }
}