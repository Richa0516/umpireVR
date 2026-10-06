using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;


public class SceneSwitcher : MonoBehaviour
{
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SceneManager.LoadScene("BaseBall");
        }
    }
}
