using UnityEngine;
using UnityEngine.InputSystem;  // 新しい入力システムを使用
using System.Collections.Generic; // リスト管理

public class RenderTextureCapture : MonoBehaviour
{
    public RenderTexture renderTexture; // インスペクタに表示される
    public string saveFolder = "Assets/RecordedFrames"; // 保存先のフォルダパス
    private int frameCount = 0;
    private bool isRecording = false;
    public Transform batterTransform; // バッターのTransformをインスペクタで設定
    public Vector3 targetPosition = new Vector3(-15.01f, 0f, -14.77f); // 目標の座標

    // 録画フレームを保持するリスト
    public List<Texture2D> capturedFrames = new List<Texture2D>();

    private bool hasStartedRecording = false;  // 録画が開始されたかどうかのフラグ

    void Start()
    {
        // フォルダの確認と作成
        if (!System.IO.Directory.Exists(saveFolder))
        {
            System.IO.Directory.CreateDirectory(saveFolder);
            Debug.Log("Folder created: " + saveFolder);
        }
        else
        {
            Debug.Log("Folder already exists: " + saveFolder);
        }
    }

    void Update()
    {
        // バッターが目標の位置に到達したときに録画開始
        if (Vector3.Distance(batterTransform.position, targetPosition) < 0.1f && !hasStartedRecording)
        {
            StartRecording();
            hasStartedRecording = true;  // 録画開始フラグを立てる
            Invoke("StopRecording", 3f); // 3秒後に録画を停止
        }
    }

    // 録画開始
    void StartRecording()
    {
        frameCount = 0;
        isRecording = true;
        InvokeRepeating("CaptureFrame", 0, 0.010f); // 約30FPSでキャプチャ
    }

    // 録画停止
    void StopRecording()
    {
        isRecording = false;
        CancelInvoke("CaptureFrame");
        Debug.Log("Recording stopped.");
    }

    // フレームごとに画像をキャプチャ
    void CaptureFrame()
    {
        if (!isRecording) return;

        // 解像度を1.5倍に変更（画質向上のため）
        int width = Mathf.RoundToInt(renderTexture.width * 1.5f);
        int height = Mathf.RoundToInt(renderTexture.height * 1.5f);
        
        // RenderTextureを変更した解像度に合わせて作成
        RenderTexture tempTexture = new RenderTexture(width, height, 24);
        Graphics.Blit(renderTexture, tempTexture);

        // RenderTextureの内容をTexture2Dにコピー
        RenderTexture.active = tempTexture;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        // リストにフレームを追加
        capturedFrames.Add(screenShot);

        frameCount++;
    }
}

