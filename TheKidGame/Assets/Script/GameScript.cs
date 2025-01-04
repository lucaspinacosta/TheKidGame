using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{

    public List<GameObject> fruitGO, carGo;

    public bool isGameModeFruit, isGameModeCar = false;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameFruit")
        {
            FruitMode();
        }
        else if (SceneManager.GetActiveScene().name == "GameCar")
        {
            CarMode();
        }
    }

    public void FruitMode()
    {
        Debug.Log("Fruit Mode");
        isGameModeCar = false;
        isGameModeFruit = true;

        foreach (GameObject go in fruitGO)
        {
            go.SetActive(true);
        }
    }

    public void CarMode()
    {
        Debug.Log("Car Mode");
        isGameModeFruit = false;
        isGameModeCar = true;

        foreach (GameObject go in carGo)
        {
            go.SetActive(true);
        }
    }

    public void MainMenu()
    {
        Debug.Log("Main Menu");
        SceneManager.LoadScene("Home");
    }

    public void GameMode()
    {
        Debug.Log("Game Mode");
    }
}
