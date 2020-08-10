using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 8:
                HumanoidBody body = collision.GetComponent<HumanoidBody>();

                if (body.GetStatus == E_BodyStatus.POSSESSED)
                {
                    Debug.Log("you win");
                    GameLoopManager.Instance.SetIsPaused = true;
                }
                break;
            case 9:
                BallBody ball = collision.GetComponent<BallBody>();

                if (ball.GetStatus == E_BodyStatus.POSSESSED)
                {
                    GameLoopManager.Instance.SetIsPaused = true;
                    Debug.Log("you win");
                }
                break;
            case 10:
                //set
                break;
            case 11:
                //set
                break;
            case 12:
                //set
                break;
            case 13:
                //set
                break;
            case 14:
                //set
                break;
            default:
                break;
        }
    }
}
