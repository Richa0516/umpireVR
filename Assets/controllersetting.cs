using UnityEngine;
using UnityEngine.UI;

public class MetaQuestButtonMapper : MonoBehaviour
{
    public Button selectRunnerButton; // Select_Runner ボタン
    public Button selectBallButton;   // Select_Ball ボタン
    public Button resetButton;        // Reset ボタン (新規追加)

    void Update()
    {
        // Aボタンで Select_Runner をクリック
        if (OVRInput.GetDown(OVRInput.Button.One)) // Aボタン
        {
            selectRunnerButton.onClick.Invoke();
        }

        // Bボタンで Select_Ball をクリック
        if (OVRInput.GetDown(OVRInput.Button.Two)) // Bボタン
        {
            selectBallButton.onClick.Invoke();
        }

        // Xボタンで reset をクリック
        if (OVRInput.GetDown(OVRInput.Button.Three)) // Xボタン
        {
            resetButton.onClick.Invoke();
        }
    }
}
