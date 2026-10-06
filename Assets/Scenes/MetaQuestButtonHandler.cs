using UnityEngine;

public class MetaQuestButtonHandler : MonoBehaviour
{
    public GameObject targetGameObject; // Scene SwitcherスクリプトがアタッチされているGameObject

    void Update()
    {
        // Aボタンが押されたかを確認
        if (OVRInput.GetDown(OVRInput.Button.One)) // Aボタン
        {
            if (targetGameObject != null)
            {
                // targetGameObjectが有効でアクティブなら
                SceneSwitcher sceneSwitcher = targetGameObject.GetComponent<SceneSwitcher>();
                if (sceneSwitcher != null)
                {
                    sceneSwitcher.enabled = true; // SceneSwitcherスクリプトを実行可能にする
                }
                else
                {
                    Debug.LogError("SceneSwitcherスクリプトが指定のGameObjectに見つかりませんでした！");
                }
            }
            else
            {
                Debug.LogError("targetGameObjectが指定されていません！");
            }
        }
    }
}
