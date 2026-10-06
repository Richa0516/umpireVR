using UnityEngine;

public class Ruuning : MonoBehaviour
{
    private Animation animation; // Animationコンポーネント
    private enum PlayerState { Stance1, Batting, Run, Slide } // プレイヤーの状態
    private PlayerState currentState = PlayerState.Stance1;

    public float runSpeed = 5.4f; // 走る速度
    public float slideSpeed = 6.0f; // スライディング速度
    private Vector3 startPosition = new Vector3(-1.14f, 0f, -0.1f); // 初期位置
    private Vector3 slideStartPosition = new Vector3(-15.0f, 0f, -15.0f); // スライディング開始位置
    private Vector3 slideEndPosition = new Vector3(-18.0f, 0f, -18.0f); // スライディング終了位置
    public float rotationSpeed = 5.0f; // 回転速度

    

    void Start()
    {
        transform.position = startPosition; // 初期位置に設定
        animation = GetComponent<Animation>(); // Animationコンポーネントを取得
        animation.Play("kamae"); // 最初に構えアニメーションを再生
    }

    void Update()
    {
        switch (currentState)
        {
            case PlayerState.Stance1:
                PerformKamae(); // 構える状態を処理
                break;
            case PlayerState.Batting:
                PerformHit(); // バットを振る状態を処理
                break;
            case PlayerState.Run:
                PerformRun(); // 走る状態を処理
                break;
            case PlayerState.Slide:
                PerformSlide(); // スライディング状態を処理
                break;
        }
    }

    private void PerformKamae()
    {
        if (!animation.isPlaying || animation.clip.name == "kamae")
        {
            // 構えアニメーションが終了したら次に進む
            if (!animation.isPlaying)
            {
                currentState = PlayerState.Batting; // 次はバットを振る状態へ
                animation.Play("Hit1"); // Hit1アニメーションを再生
            }
        }
    }

    // BallControllerスクリプトを呼び出すための参照
    public BallController ballController;

    private bool hasHitBall = false; // ボールを打ったかのフラグ

    public GameObject bat; // Inspector でバットの GameObject を設定

    private void PerformHit()
    {
        if (animation.clip.name == "Hit1" && animation.isPlaying)
        {
            transform.position = startPosition;

            // ボールを打つ瞬間にバットを非表示にする処理を追加
            if (!hasHitBall && animation["Hit1"].time >= 0.23f)
            {
                if (ballController != null)
                {
                    ballController.HitBall();
                    hasHitBall = true;
                }
            }
            return;
        }

        // アニメーションが終了したら次の状態へ
        if (!animation.isPlaying)
        {
            currentState = PlayerState.Run; // 次は走る状態へ
            animation.Play("Run");

            // バットを非表示にする
            if (bat != null)
            {
                bat.SetActive(false); // バットを非アクティブにする
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
        Debug.Log("バッターの現在位置: " + transform.position + ", 目的地: " + slideStartPosition);

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
    private bool isSliding = false; // スライディング中かどうかのフラグ

    private void StartSliding()
    {
        isSliding = true;
        currentState = PlayerState.Slide; // スライディング状態に遷移
        animation.Stop(); // 現在のアニメーションを停止
        animation.Play("Slide"); // スライディングアニメーションを再生
    }

    private void PerformSlide()
    {
        if (!isSliding)
        {
            Debug.Log("スライディング開始");
        }

        isSliding = true; // スライディング中であることを示す

        float step = slideSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, slideEndPosition, step);

        if (Vector3.Distance(transform.position, slideEndPosition) < 0.1f)
        {
            isSliding = false; // スライディング終了
            animation.Stop();
        }
    }

}
