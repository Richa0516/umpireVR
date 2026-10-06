using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera; // 最初と最後に表示されるメインカメラ
    public Camera camera1;    // カメラ1
    public Camera camera3;    // カメラ2

    private float timer = 0f;          // タイマー
    private bool camerasSwitched = false; // カメラ切り替えが完了したか

    void Start()
    {
        // 全てのカメラを無効化し、メインカメラを有効化
        DisableAllCameras();
        mainCamera.gameObject.SetActive(true);
    }

    void Update()
    {
        // タイマーを更新
        timer += Time.deltaTime;

        if (!camerasSwitched)
        {
            if (timer >= 7f && timer < 8f)
            {
                // 6秒後にカメラ1に切り替え
                SwitchToCamera(camera1);
            }
            else if (timer >= 8f && timer < 9f)
            {
                // 7秒後にカメラ2に切り替え
                SwitchToCamera(camera3);
            }
            else if (timer >= 9.5f)
            {
                // 8秒後にメインカメラに戻す
                SwitchToCamera(mainCamera);
                camerasSwitched = true; // 切り替えを終了
            }
        }
    }

    // 指定されたカメラを有効化する関数
    private void SwitchToCamera(Camera newCamera)
    {
        DisableAllCameras();
        newCamera.gameObject.SetActive(true);
    }

    // 全てのカメラを無効化する関数
    private void DisableAllCameras()
    {
        mainCamera.gameObject.SetActive(false);
        camera1.gameObject.SetActive(false);
        camera3.gameObject.SetActive(false);
    }
}
