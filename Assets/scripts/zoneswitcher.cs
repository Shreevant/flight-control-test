using UnityEngine;
using System.Collections.Generic;
using TMPro;   // Important

public class GameManager : MonoBehaviour
{
    public List<GameObject> objectsList = new List<GameObject>();

    private GameObject currentActiveObject;

    public TextMeshProUGUI switchCountText;  // Assign in Inspector
    private int count = 0;

    void Start()
    {
        foreach (GameObject obj in objectsList)
        {
            obj.SetActive(false);
        }

        ActivateRandomObject();
        UpdateUI();
    }

    public void SwitchObject()
    {
        if (objectsList.Count <= 1) return;

        if (currentActiveObject != null)
            currentActiveObject.SetActive(false);

        GameObject newObject;

        do
        {
            int randomIndex = Random.Range(0, objectsList.Count);
            newObject = objectsList[randomIndex];
        }
        while (newObject == currentActiveObject);

        currentActiveObject = newObject;
        currentActiveObject.SetActive(true);

        // Increase counter
        count++;

        // Update UI
        UpdateUI();
    }

    void ActivateRandomObject()
    {
        int randomIndex = Random.Range(0, objectsList.Count);
        currentActiveObject = objectsList[randomIndex];
        currentActiveObject.SetActive(true);
    }

    void UpdateUI()
    {
        switchCountText.text = "SUCCESFULL DROPS: " + count;
    }
}