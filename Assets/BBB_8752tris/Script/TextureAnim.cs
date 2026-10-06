using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextureAnim : MonoBehaviour
{
    [SerializeField]
    RenderTextureCapture renderTextureCapture; // RenderTextureCaptureをインスペクタで設定

    Texture2D tex;
    RawImage animImage;

    [SerializeField]
    float frameInterval = 0.1f;

    private bool isPlaying = false;  // アニメーションが再生中かどうかのフラグ

    void Start()
{
    // RawImageコンポーネントの取得
    animImage = GetComponent<RawImage>();

    // 初期状態でRawImageを非アクティブにしておく
    animImage.gameObject.SetActive(false);

    // テクスチャオブジェクトを作成
    tex = new Texture2D(256, 256, TextureFormat.RGB24, false);
    animImage.texture = Texture2D.blackTexture;

    // 最初はアニメーションを再生しない
    isPlaying = false;
}

public void StartReplay()
{
    if (renderTextureCapture != null && renderTextureCapture.capturedFrames.Count > 0)
    {
        isPlaying = true;
        StartCoroutine(PlayAnimation());
    }
    else
    {
        Debug.LogWarning("No frames to replay or RenderTextureCapture not set.");
    }
}


    // リプレイを停止
    void StopReplay()
{
    isPlaying = false;
    animImage.texture = Texture2D.blackTexture;  // 停止後は画面を黒に戻す
    animImage.gameObject.SetActive(false);  // リプレイが終了したらRawImageを非アクティブ化
}


    // アニメーション再生コルーチン
    IEnumerator PlayAnimation()
    {
        int frameIndex = 0;
        while (frameIndex < renderTextureCapture.capturedFrames.Count && isPlaying)
        {
            // メモリ上の画像を読み込んで表示
            tex = renderTextureCapture.capturedFrames[frameIndex];
            animImage.texture = tex;

            frameIndex++;
            yield return new WaitForSeconds(frameInterval);
        }

        // アニメーションが終了したら停止
        StopReplay();
    }
}
