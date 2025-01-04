using UnityEngine;
using UnityEngine.SceneManagement;

public class HomePage : MonoBehaviour
{

    public void LoadGameCar()
    {
        Debug.Log("Load Game");
        SceneManager.LoadScene("GameCar");
    }
    public void LoadGameFruit()
    {
        Debug.Log("Load Game");
        SceneManager.LoadScene("GameFruit");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

}
