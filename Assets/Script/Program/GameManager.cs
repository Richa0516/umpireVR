using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int first = 0; // first=0:同時、first=1:ランナーが先、first=2:ボールが先
    public static bool arrive = false;

    void Update()
    {
        if (BallController.firstBaseReachTime != -1f && BaseballPlayerController.firstBaseReachTime != -1f)
        {
            float timeDifference = Mathf.Abs(BallController.firstBaseReachTime - BaseballPlayerController.firstBaseReachTime);
            const float threshold = 0.00f; // 同時と判定する時間差の閾値

            if (timeDifference <= threshold)
            {
                Debug.Log("ボールとランナーが同時に到達しました！");
                first = 0;
                arrive = true;
            }
            else if (BallController.firstBaseReachTime < BaseballPlayerController.firstBaseReachTime)
            {
                Debug.Log("ボールが先に到達しました！");
                first = 2;
                arrive = true;
            }
            else
            {
                Debug.Log("ランナーが先に到達しました！");
                first = 1;
                arrive = true;
            }
        }
    }

}
