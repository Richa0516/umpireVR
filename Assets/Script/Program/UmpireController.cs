using UnityEngine;

public class UmpireController : MonoBehaviour
{
    public Transform player; // バッターランナー
    public BallController ballController;

    private bool isJudged = false;

    void Update()
    {
        if (isJudged) return;

        float playerDistance = Vector3.Distance(transform.position, player.position);
        float ballDistance = Vector3.Distance(transform.position, ballController.transform.position);

        Debug.Log("審判の現在位置: " + transform.position);
        Debug.Log("バッターとの距離: " + playerDistance);
        Debug.Log("ボールとの距離: " + ballDistance);

        if (playerDistance < 0.3f)
        {
            if (ballController.isAtFirstBase)
            {
                Debug.Log("アウト！");
            }
            else
            {
                Debug.Log("セーフ！");
            }
            isJudged = true;
        }
    }
}