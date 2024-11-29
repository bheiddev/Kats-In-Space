using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("MainMenu Screen");
    }
}