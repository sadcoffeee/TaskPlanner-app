using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiscManager : MonoBehaviour
{
    public GameObject funOptions;
    public GameObject funObjects;

    public GameObject coinFlip;
    SkyboxSelector skyboxSelector;
    void Start()
    {
        skyboxSelector = transform.GetComponent<SkyboxSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startCoinflip()
    {
        funOptions.SetActive(false);
        coinFlip.SetActive(true);
    }
    public void showSkyboxMenu()
    {
        skyboxSelector.ToggleOptionsMenu(true);
    }

    public void backToMenu()
    {
        foreach (Transform child in funObjects.transform)
        {
            child.gameObject.SetActive(false);
        }

        Camera.main.transform.position = new Vector3(0, 0, -10);
        Camera.main.transform.rotation = Quaternion.identity;

        funOptions.SetActive(true);
    }

    public void goToFunScene()
    {
        SceneManager.LoadScene("FunScene");
    }
    public void goToTaskScene()
    {
        SceneManager.LoadScene("TaskScene");
    }

}
