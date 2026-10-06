using UnityEngine;

public class BallMovement : MonoBehaviour
{
    // 始点を設定する範囲の四角形
    Vector3 topLeft = new Vector3(-50, 0, -58);
    Vector3 topRight = new Vector3(44, 0, -53);
    Vector3 bottomLeft = new Vector3(-26, 0, -34);
    Vector3 bottomRight = new Vector3(20, 0, -30);

    // 初期位置（ランダム設定される）と終点
    Vector3 startPos;
    Vector3 endPos = new Vector3(-19, 0, -19);

    // 移動を開始するフラグ
    private bool canMove = false;

    // 等速運動の速度（例: 1ユニット/秒）
    public float speed = 1.0f;

    // ボールが最初にいる位置（地面下）
    private Vector3 hiddenPos = new Vector3(0, -10, 0); // 地面下に設定

    // ボールの到達時間を記録
    public static float ballReachTime = -1f; // -1で初期化（未到達）

    void Start()
    {
        // 四角形の範囲内でランダムな位置を計算
        float randomX = Random.Range(Mathf.Min(topLeft.x, bottomLeft.x), Mathf.Max(topRight.x, bottomRight.x));
        float randomZ = Random.Range(Mathf.Min(topLeft.z, topRight.z), Mathf.Max(bottomLeft.z, bottomRight.z));
        startPos = new Vector3(randomX, 0, randomZ);

        // 最初は地面下にボールをセット
        transform.position = hiddenPos;

        // 8秒後に移動を開始する
        Invoke(nameof(StartMoving), 7.5f);
    }

    void Update()
    {
        // 移動可能かを確認
        if (canMove)
        {
            // 経過時間を計算
            float t = Time.timeSinceLevelLoad - 7.5f; // 7.5秒後からの時間を計算
            t = Mathf.Clamp01(t * speed); // 正規化（0～1）

            // ボールを移動させる
            transform.position = Vector3.Lerp(startPos, endPos, t);

            // ボールが目標地点に到達した場合
            if (Vector3.Distance(transform.position, endPos) < 0.1f && ballReachTime == -1f)
            {
                ballReachTime = Time.timeSinceLevelLoad; // 到達時間を記録
                Debug.Log("Ball reached the target at: " + ballReachTime);
            }
        }
    }

    void StartMoving()
    {
        // 移動を許可
        canMove = true;

        // ボールの位置を画面上に戻す
        transform.position = startPos;
    }
}
