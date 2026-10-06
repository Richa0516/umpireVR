using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher2 : MonoBehaviour
{
    // ���̊□���{�^���� OnClick() �ɐݒ肷��
    public void SwitchToBaseBallScene()
    {
        SceneManager.LoadScene("BaseBall");

    }
}