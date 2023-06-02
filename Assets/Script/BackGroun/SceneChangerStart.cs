using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerStart : MonoBehaviour
{
    public void StartGame()
    {
        SceneFader.instance.LoadScene("Cartoon");
    }
}
