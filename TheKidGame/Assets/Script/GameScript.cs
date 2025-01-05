using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{

    [Header("Game Objects")]
    public List<GameObject> fruitGO;
    public List<GameObject> carGo;
    public List<GameObject> boxObjects;

    [Header("Audio Mixer")]
    public List<AudioClip> matchAudioClipsList;

    [Header("Handle Pairs")]
    private GameObject firstClickedObject = null; // Store the first clicked object
    private HashSet<string> matchedObjects = new HashSet<string>(); // Keep track of matched object pairs
    public AudioSource audioSource; // Audio source for playing audio clips


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
        foreach (GameObject go in carGo)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in fruitGO)
        {
            go.SetActive(true);
        }
        RandomizeGameObjectPositions(boxObjects);
    }

    public void CarMode()
    {
        Debug.Log("Car Mode");
        foreach (GameObject go in fruitGO)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in carGo)
        {
            go.SetActive(true);
        }

        RandomizeGameObjectPositions(boxObjects);
    }

    public void MainMenu()
    {
        Debug.Log("Main Menu");
        SceneManager.LoadScene("Home");
    }

    public void GameMode(string mode, bool boolean)
    {
        Debug.Log("Game Mode");
        if (mode == "Fruit")
        {
            FruitMode();
        }
        else if (mode == "Car")
        {
            CarMode();
        }
    }

    public void StartStopAnimator(GameObject go)
    {
        Debug.Log("Start Stop Animator");
        go.GetComponent<Animator>().enabled = !go.GetComponent<Animator>().enabled;
    }

    public void RandomizeGameObjectPositions(List<GameObject> gameObjects)
    {
        if (gameObjects == null || gameObjects.Count < 2)
        {
            Debug.LogWarning("Not enough game objects to shuffle positions!");
            return;
        }

        Debug.Log("Randomizing game object positions...");

        List<Vector3> positions = gameObjects.Select(go => go.transform.position).ToList();

        for (int i = 0; i < positions.Count; i++)
        {
            int randomIndex = Random.Range(0, positions.Count);
            Vector3 temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i] != null)
            {
                gameObjects[i].transform.position = positions[i];
            }
            else
            {
                Debug.LogWarning($"GameObject at index {i} is null. Skipping...");
            }
        }

        Debug.Log("Game object positions randomized.");
    }

    public void PlayAudio(AudioClip audioClip)
    {
        if (firstClickedObject != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void AskForMatchObject(GameObject clickedObject)
    {
        string objectName = clickedObject.name;

        // Check if the clicked object is already matched
        if (matchedObjects.Contains(objectName))
        {
            Debug.Log($"{objectName} is already matched.");
            return;
        }

        // First click: store the clicked object
        if (firstClickedObject == null)
        {
            firstClickedObject = clickedObject;
            PlayNextAudio(clickedObject); // Play the next audio for the current pair
            Debug.Log($"First object clicked: {objectName}");
            return;
        }

        // Second click: check if it matches the first clicked object
        if (IsMatchingObject(firstClickedObject.name, objectName))
        {
            Debug.Log($"Match found: {firstClickedObject.name} and {objectName}");
            firstClickedObject.GetComponent<Button>().interactable = false;
            clickedObject.GetComponent<Button>().interactable = false;
            matchedObjects.Add(firstClickedObject.name);
            matchedObjects.Add(objectName);
        }
        else
        {
            
            StartStopAnimator(GetActiveChild(firstClickedObject));
            StartStopAnimator(GetActiveChild(clickedObject));
            Debug.Log($"No match: {firstClickedObject.name} and {objectName}");
        }

        // Reset for next pair
        firstClickedObject = null;

        if (matchedObjects.Count == boxObjects.Count)
        {
            Debug.Log("All pairs matched!");
            foreach (GameObject go in boxObjects)
            {
                go.GetComponent<Button>().interactable = true;
                StartStopAnimator(GetActiveChild(go));
            }
        }
    }
    GameObject GetActiveChild(GameObject parent)
    {
        // Iterate through all child transforms
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.activeSelf) // Check if the child GameObject is active
            {
                return child.gameObject; // Return the active child GameObject
            }
        }

        // Return null if no active child is found
        return null;
    }

    private bool IsMatchingObject(string name1, string name2)
    {
        string baseName1 = name1.Split(' ')[0]; // Extract the base name
        string baseName2 = name2.Split(' ')[0];
        return baseName1 == baseName2;
    }

    private void PlayNextAudio(GameObject clickedObject)
    {
        // Determine the index of the current pair
        int index = boxObjects.IndexOf(clickedObject) / 2; // Each pair has two objects
        if (index < matchAudioClipsList.Count)
        {
            audioSource.PlayOneShot(matchAudioClipsList[index]); // Play the corresponding audio clip
        }
        else
        {
            Debug.LogWarning("No audio clip found for this pair.");
        }
    }

}
