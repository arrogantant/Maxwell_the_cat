using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerStart : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Cartoon");
    }
}
