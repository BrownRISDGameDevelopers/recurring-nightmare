using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Scene Indices
 * 0: Title Screen
 * 1: Instructions
 * 2: Credits
 * 3: Game!!
 */

public class MainMenu : MonoBehaviour
{
    public string newGameScene;

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void InstructionPage()
    {
        SceneManager.LoadScene(1);
        //placeholder for instruction page
    }

    public void CreditsPage()
    {
        //placeholder for credits page
    }

    public void TitlePage()
    {
        SceneManager.LoadScene(0);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
