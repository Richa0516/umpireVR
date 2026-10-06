using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;  // IEnumerator を使うために必要


public class GameController : MonoBehaviour
{
    public Button Button_B; // 正解ボタン
    public Button Button_R; // 不正解ボタン
    public TextMeshProUGUI resultText; // 結果を表示するテキスト
    public Button resetButton; // リセットボタン
    public Transform batterTransform; // バッターのTransform
    public Vector3 targetPosition = new Vector3(-18.5f, 0, -18.5f); // バッターの目標位置
    public TextureAnim textureAnim; // TextureAnimをインスペクタで設定

    // カメラの参照
    public Camera centerEyeCamera; // 通常画面のカメラ
    public Camera replayTVCamera; // リプレイ画面のカメラ

    private bool buttonsDisplayed = false; // ボタンが表示されたかどうか

    void Awake()
    {
        // 静的変数をリセット
        GameManager.arrive = false;
        GameManager.first = Random.Range(1, 3); // 1か2をランダムに設定
    }

    void Start()
    {
        // 初期化
        resultText.text = ""; // 結果テキストを空にする
        resetButton.gameObject.SetActive(false); // リセットボタンを非表示
        Button_B.gameObject.SetActive(false); // 二択ボタンを非表示
        Button_R.gameObject.SetActive(false); // 二択ボタンを非表示

        // ボタンにイベントを登録
        Button_B.onClick.AddListener(() => OnButtonClick(2)); // 正解ボタン
        Button_R.onClick.AddListener(() => OnButtonClick(1)); // 不正解ボタン
        resetButton.onClick.AddListener(OnResetButtonClick); // リセットボタン

        // 初期状態では通常カメラを有効にし、リプレイカメラを無効にする
        centerEyeCamera.enabled = true;
        replayTVCamera.enabled = false;
    }

    void Update()
    {
        // バッターが目標位置に到達したら二択ボタンを表示
        if (!buttonsDisplayed && IsBatterAtTargetPosition())
        {
            Debug.Log("Batter reached target position. Displaying buttons.");
            GameManager.arrive = true; // バッターが到達したことを示す
            Button_B.gameObject.SetActive(true);
            Button_R.gameObject.SetActive(true);
            buttonsDisplayed = true; // 一度だけボタンを表示する
        }
    }

    // バッターが目標位置に到達したか確認
    private bool IsBatterAtTargetPosition()
    {
        return Vector3.Distance(batterTransform.position, targetPosition) < 0.1f;
    }

    public void OnButtonClick(int choice)
{
    Judgement(choice);

    // リプレイ用カメラに切り替え
    SwitchToReplayCamera();

    // リプレイ開始
    textureAnim.gameObject.SetActive(true);
    textureAnim.StartReplay();

    // リプレイが終わった後に通常カメラに戻す
    StartCoroutine(SwitchBackToCenterEyeCamera());
}

// 遅れてカメラを切り替えるためのコルーチン
private IEnumerator SwitchBackToCenterEyeCamera()
{
    yield return new WaitForSeconds(5); // リプレイ時間に応じて待機時間を設定
    SwitchToCenterEyeCamera();
}

    // リセットボタンを表示
    public void ShowResetButton()
    {
        resetButton.gameObject.SetActive(true); // リセットボタンを表示
        Button_B.interactable = false; // 二択ボタンを無効化
        Button_R.interactable = false;
        Debug.Log("RESET.");
    }

    // リセットボタンが押されたときの処理
    public void OnResetButtonClick()
    {
        // シーンを再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 判定処理
    void Judgement(int choice)
    {
        Debug.Log($"Judgement called. GameManager.arrive: {GameManager.arrive}, GameManager.first: {GameManager.first}, choice: {choice}");
        if (GameManager.arrive)
        {
            if (choice == GameManager.first)
            {
                resultText.text = "Correct!!";
            }
            else
            {
                resultText.text = "Incorrect!!";
            }
        }
        else
        {
            resultText.text = "Don't judge before they arrive!!";
        }

        

        ShowResetButton();
    }

    // リプレイ用カメラに切り替える
    private void SwitchToReplayCamera()
    {
        centerEyeCamera.enabled = false; // 通常カメラを無効化
        replayTVCamera.enabled = true; // リプレイカメラを有効化
        Debug.Log("Switched to Replay TV Camera.");
    }

    // 通常カメラに戻す
    public void SwitchToCenterEyeCamera()
    {
        replayTVCamera.enabled = false; // リプレイカメラを無効化
        centerEyeCamera.enabled = true; // 通常カメラを有効化
        Debug.Log("Switched to Center Eye Camera.");
    }
}