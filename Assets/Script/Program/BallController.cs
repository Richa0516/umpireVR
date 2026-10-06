using UnityEngine;
using System.Collections.Generic;


public class BallController : MonoBehaviour
{    //オブジェクトの認識
    public GameObject Ball,First_L_Hand, Pitcher_R_Hand, SS_R_Hand, First_Glove, Pitcher_Glove, BBPB_SS;




    public Vector3 moundPosition = new Vector3(0f, 1.5f, -19f);
    public Vector3 homePosition = new Vector3(0f, 1.5f, 0f);
    public Vector3 shortStopPosition;
    public Vector3 firstBasePosition = new Vector3(-18.7f, 1.2f, -19.2f);

    private float hitSpeed = 0.0f;
    public float throwSpeed = 15.0f;
    public float detectionRadius = 1.0f; // shortStopPositionの付近を検出する範囲
    private enum BallState { PrePitch, Pitch, HitToShortStop, RollToFirstBase, Stop }
    private BallState currentState = BallState.PrePitch;

    public bool isAtFirstBase = false; // 判定用フラグ
    private float pitchSpeed;          // ピッチ速度
    private float pitchTime = 0.5f;    // ホームベース到達時間 (短縮)
    private float prePitchWaitTime = 5.04f; // ピッチ前の待機時間
    private float pitchStartTime = -1f;   // ピッチ開始時間
    private float SS_catchTime;//SSがボールをキャッチした瞬間の時間；
    private Rigidbody rb;              // Rigidbodyコンポーネント
    private bool gravityEnabled = false; // 重力が有効か
    private bool SS_catchTime_Check = false;

    // ボールの到達時間を記録
    public static float ballReachTime = -1f; // -1fで未到達を表す

    public static float firstBaseReachTime = -1f; // ファーストベース到達時刻を記録



 

    void Start()
    {
        SS_catchTime_Check = false;//初期化
        transform.position = moundPosition;
        shortStopPosition = SS_R_Hand.transform.position;

        // ピッチ速度を計算
        pitchSpeed = Vector3.Distance(moundPosition, homePosition) / pitchTime;

        // Rigidbodyの設定
        rb = GetComponent<Rigidbody>();
        if (rb == null){
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;               // 初期状態で重力無効
        rb.isKinematic = true;               // 初期状態で物理演算無効
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // 高速移動用

        //初めはPitcherの左手の子オブジェクトに設定
        Ball.transform.SetParent(Pitcher_R_Hand.transform);
        Ball.transform.position = Pitcher_R_Hand.transform.position;
        Ball.transform.rotation = Pitcher_R_Hand.transform.rotation;

    }

    void Update()
    {
        switch (currentState)
        {
            case BallState.PrePitch:
                HandlePrePitch();
                break;

            case BallState.Pitch:
                Ball.transform.SetParent(null);
                MoveBall(homePosition, pitchSpeed, BallState.HitToShortStop);
                break;

            case BallState.HitToShortStop:
                PerformGravityBounce(shortStopPosition);
                CheckNearPosition(shortStopPosition, detectionRadius, BallState.RollToFirstBase);
                break;

            case BallState.RollToFirstBase:
                firstBasePosition = First_Glove.transform.position;
                RollToFirstBase();
                break;

            case BallState.Stop:
                if (!isAtFirstBase)
                {
                    isAtFirstBase = true;

                    if (ballReachTime == -1f)
                    {
                        ballReachTime = Time.timeSinceLevelLoad;
                        Ball.transform.SetParent(First_L_Hand.transform);
                        Ball.transform.position = First_Glove.transform.position;
                        Ball.transform.rotation = First_Glove.transform.rotation;


                        //Debug.Log($"ボールが一塁に到達しました！到達時間: {ballReachTime}");
                    }
                }
                break;
        }
    }

    private void HandlePrePitch()
    {
        // ピッチ開始時刻が未設定なら、現在の時刻を設定
        if (pitchStartTime == -1f)
        {
            pitchStartTime = Time.timeSinceLevelLoad;
        }

        // 待機時間が経過したらピッチを開始
        if (Time.timeSinceLevelLoad >= pitchStartTime + prePitchWaitTime)
        {
            currentState = BallState.Pitch;
            Debug.Log("ピッチが開始されました！");
        }
    }

    private void MoveBall(Vector3 target, float speed, BallState nextState)
    {
        float step = speed * Time.deltaTime;
        Ball.transform.SetParent(null);

        transform.position = Vector3.MoveTowards(transform.position, target, step);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentState = nextState;
        }
    }

    private void PerformGravityBounce(Vector3 target)
    {
        if (!gravityEnabled)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            gravityEnabled = true;

            List<float> values = new List<float>();
            for (float i = 12.9f; i < 13.4f; i += 0.01f)
            {
                values.Add((float)System.Math.Round(i, 2)); // 小数点以下2桁で丸める
            }

            // ランダムに値を取得
            int index = Random.Range(0, values.Count); // UnityのRandom.Rangeを使用
            hitSpeed = values[index];

            Debug.Log("ランダムに選ばれた値: " + hitSpeed);


            Vector3 direction = (target - transform.position).normalized;
            Vector3 initialVelocity = new Vector3(direction.x, 0.3f, direction.z) * hitSpeed;
            rb.linearVelocity = initialVelocity;
        }
    }

    private void RollToFirstBase()
    {
        rb.isKinematic = true;
        rb.useGravity = false;

        MoveBall(firstBasePosition, throwSpeed, BallState.Stop);

        // 到達時刻を記録
        if (Vector3.Distance(transform.position, firstBasePosition) < 0.1f && firstBaseReachTime == -1f)
        {
            firstBaseReachTime = Time.timeSinceLevelLoad; // 現在の時間を記録
            Debug.Log($"ボールがファーストベースに到達しました！到達時間: {firstBaseReachTime}");
        }
    }

    private void CheckNearPosition(Vector3 targetPosition, float radius, BallState nextState)
    {
        if (Vector3.Distance(transform.position, targetPosition) <= radius)
        {
            Debug.Log($"Target付近（{targetPosition}）に到達しました。次の状態に遷移します。");
            if (!SS_catchTime_Check) {

                SS_catchTime = Time.timeSinceLevelLoad;
                SS_catchTime_Check = true;
            }

            Ball.transform.SetParent(SS_R_Hand.transform);
            Ball.transform.position = SS_R_Hand.transform.position;
            Ball.transform.rotation = SS_R_Hand.transform.rotation;

            if (Time.timeSinceLevelLoad - SS_catchTime >= 0.5f){
                Debug.Log("Time.timeSinceLevelLoad - SS_catchTime >= 1.0fを通りました");  
                Ball.transform.SetParent(null);
                rb.isKinematic = true;
                rb.useGravity = false;
                rb.linearVelocity = Vector3.zero;
                gravityEnabled = false;
                currentState = nextState;
            }
        }
    }

    public void HitBall()
    {
        currentState = BallState.HitToShortStop;
        Debug.Log("ボールが打たれました！");
    }
}
