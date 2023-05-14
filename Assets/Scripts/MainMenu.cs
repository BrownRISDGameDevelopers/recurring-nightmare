using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string newGameScene;

    public void NewGame()
    {
        SceneManager.LoadScene((int) GameManager.SceneIndexTable.Game);
    }

    public void InstructionPage()
    {
        SceneManager.LoadScene((int) GameManager.SceneIndexTable.Instructions);
        //placeholder for instruction page
    }

    public void CreditsPage()
    {
        SceneManager.LoadScene((int) GameManager.SceneIndexTable.Credits);
    }

    public void TitlePage()
    {
        SceneManager.LoadScene((int) GameManager.SceneIndexTable.Title);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
