using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 8:
                HumanoidBody body = collision.GetComponent<HumanoidBody>();

                if(body.GetStatus == E_BodyStatus.POSSESSED)
                {
                    CPsManager.Instance.SetLastHitCP = this;
                    gameObject.SetActive(false);
                }
                break;
            case 9:
                BallBody ball = collision.GetComponent<BallBody>();

                if(ball.GetStatus == E_BodyStatus.POSSESSED)
                {
                    CPsManager.Instance.SetLastHitCP = this;
                    gameObject.SetActive(false);
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