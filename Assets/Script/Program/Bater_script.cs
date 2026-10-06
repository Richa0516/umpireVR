using UnityEngine;

public class BaseballPlayerController : MonoBehaviour
{
    private Animation animation; // Animationコンポーネント
    public GameObject dustEffectPrefab; // 砂埃エフェクトのPrefab
    private GameObject activeDustEffect; // インスタンス化された砂埃エフェクト
    private enum PlayerState { Kamae, Hit, Run, Slide } // プレイヤーの状態
    private PlayerState currentState = PlayerState.Kamae;

    public float runSpeed = 4.0f; // 走る速度
    public float slideSpeed = 3.0f; // スライディング速度
    private Vector3 startPosition = new Vector3(-1.14f, 0f, -0.1f); // 初期位置
    private Vector3 slideStartPosition = new Vector3(-15.01f, 0f, -14.77f); // スライディング開始位置
    private Vector3 slideEndPosition = new Vector3(-18.5f, 0, -18.5f); // スライディング終了位置 
    public float rotationSpeed = 5.0f; // 回転速度

    private bool isSliding = false; // スライディング中かどうかのフラグ

    // バッターの到達時間を記録
    public static float playerReachTime = -1f; // -1で初期化（未到達）
    public static float firstBaseReachTime = -1f;

    public GameObject bat; // バットのGameObjectをInspectorで設定する

    private float kamaeStartTime = -1f; // kamaeアニメーションの開始時間を記録

    void Start()
    {
        transform.position = startPosition; // 初期位置に設定
        animation = GetComponent<Animation>(); // Animationコンポーネントを取得
        animation.Play("kamae"); // 最初に構えアニメーションを再生
        kamaeStartTime = Time.timeSinceLevelLoad; // `kamae` アニメーション開始時間を記録
    }

    void Update()
    {
        Debug.Log("Now state is " + currentState);
        switch (currentState)
        {
            case PlayerState.Kamae:
                PerformKamae(); // 構える状態を処理
                break;
            case PlayerState.Hit:
                PerformHit(); // バットを振る状態を処理
                break;
            case PlayerState.Run:
                PerformRun(); // 走る状態を処理
                break;
            case PlayerState.Slide:
                PerformSlide(); // スライディング状態を処理
                break;
        }

        // バッターが目的地に到達した場合
        if (Vector3.Distance(transform.position, slideEndPosition) < 0.1f && playerReachTime == -1f)
        {
            playerReachTime = Time.timeSinceLevelLoad; // 到達時間を記録
            Debug.Log("Player reached the target at: " + playerReachTime);
        }
    }

    private void PerformKamae()
    {
        // `kamae` アニメーションの再生中は状態を維持
        if (Time.timeSinceLevelLoad - kamaeStartTime >= 4.7f)
        {
            // 5秒経過後に次の状態へ移行
            currentState = PlayerState.Hit;
            animation.Play("Hit1"); // Hit1アニメーションを再生
        }
    }

    private void PerformHit()
    {
        // "Hit1"アニメーション中は位置を固定
        if (animation.clip.name == "Hit1" && animation.isPlaying)
        {
            transform.position = startPosition; // 位置を固定
            return; // アニメーション再生中は位置を更新しない
        }

        // "Hit1"アニメーションが終了したら次の状態へ
        if (!animation.isPlaying)
        {
            currentState = PlayerState.Run; // 次は走る状態へ
            animation.Play("Run"); // 走るアニメーションを再生

            // バットを非表示にする
            if (bat != null)
            {
                bat.SetActive(false);
            }
        }
    }

    private void PerformRun()
    {
        // 走るアニメーションが再生されていない場合、再生
        if (!animation.isPlaying || animation.clip.name != "Run")
        {
            animation.Play("Run");
        }

        // 走る方向に回転する処理
        Vector3 direction = slideStartPosition - transform.position; // 進行方向を計算
        if (direction.magnitude > 0.1f) // 進行方向がまだ十分にある場合
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction); // 進行方向に回転
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // スムーズに回転
        }

        // 指定位置に向かって移動
        float step = runSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, slideStartPosition, step);

        // スライディング開始位置に到達したらスライディングを開始
        if (Vector3.Distance(transform.position, slideStartPosition) < 0.1f)
        {
            StartSliding();
        }
    }

    private void StartSliding()
    {
        isSliding = true;
        currentState = PlayerState.Slide; // スライディング状態に遷移
        animation.Stop(); // 現在のアニメーションを停止
        animation.Play("Slide"); // スライディングアニメーションを再生

        // 砂埃エフェクトを生成
        if (dustEffectPrefab != null)
        {
            activeDustEffect = Instantiate(dustEffectPrefab, transform.position, Quaternion.identity);
            activeDustEffect.transform.parent = transform; // プレイヤーの位置に追従させる
        }
    }

    private void PerformSlide()
    {
        // 砂埃をプレイヤーの位置に追従させる
        if (activeDustEffect != null)
        {
            activeDustEffect.transform.position = transform.position;
        }

        // スライディング移動
        float step = slideSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, slideEndPosition, step);

        // ファーストベース到達時刻を記録
        if (Vector3.Distance(transform.position, slideEndPosition) < 0.1f && firstBaseReachTime == -1f)
        {
            firstBaseReachTime = Time.timeSinceLevelLoad; // 現在の時間を記録
            Debug.Log($"ランナーがファーストベースに到達しました！到達時間: {firstBaseReachTime}");
        }

        // スライディング終了時にエフェクトを破棄
        if (Vector3.Distance(transform.position, slideEndPosition) < 0.1f)
        {
            animation.Stop(); // アニメーションを停止
            if (activeDustEffect != null)
            {
                Destroy(activeDustEffect); // 砂埃エフェクトを破棄
            }
        }
    }
}
