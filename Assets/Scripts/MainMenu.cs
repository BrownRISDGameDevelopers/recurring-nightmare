using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string newGameScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void instructionPage()
    {
        //placeholder for instruction page
    }

    public void creditsPage()
    {
        //placeholder for credits page
    }

    public void endGame()
    {
        Application.Quit();
    }
}
