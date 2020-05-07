using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public enum GameState {TitleState,LearnState};
    public GameState currentGameState;

    public GameObject Title;
    public GameObject Learn;

    // Start is called before the first frame update
    void Start()
    {
        // shows the title screen when the game starts
        currentGameState = GameState.TitleState;
        ShowScreen(Title);
    }

    // loads up the instructions when the player presses start on the title screen
    public void HowToPlay()
    {
        currentGameState = GameState.LearnState;
        ShowScreen(Learn);
    }

    // loads the level when players hit play on the instructions screen
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // function used to control which screen shows up during gameplay
    private void ShowScreen (GameObject gameObjectToShow)
    {
        Title.SetActive(false);
        Learn.SetActive(false);

        gameObjectToShow.SetActive(true);
    }
}
