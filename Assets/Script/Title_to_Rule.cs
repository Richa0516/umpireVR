using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    void Start()
    {
        // Additiveモードで複数のシーンをロード
        SceneManager.LoadScene("Title", LoadSceneMode.Additive);
        SceneManager.LoadScene("Rule", LoadSceneMode.Additive);
        SceneManager.LoadScene("BaseBall", LoadSceneMode.Additive);

        // Scene1をアクティブに設定
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Title"));
    }
}




public class SceneSwitcher1 : MonoBehaviour
{
    // ���̊֐����{�^���� OnClick() �ɐݒ肷��
    public void SwitchToRuleScene()
    {
        SceneManager.LoadScene("Rule");
    }
}
