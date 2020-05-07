using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public enum GameState {LevelState, PauseState,LearnState} // list that contains all the states of our game
    public GameState currentGameState; // used to show the correct gamestate
    private bool isPaused; // checks to see if the game is paused
    private bool isLearning; // checks to see if players have loaded up the instructions

    public GameObject Level; // reference to the game state
    public GameObject Pause; // reference to the pause screen
    public GameObject Learn; // reference to the learn screen

    // Start is called before the first frame update
    void Start()
    {
        currentGameState = GameState.LevelState; // the game starts on the level
        ShowScreen(Level);
    }

    // Update is called once per frame
    void Update()
    {
        // checks to see if either player has paused the game
       if(Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Joystick2Button7))
    	{
    		if(isPaused)
    		{
    			isPaused = false; // the game isn't paused
				currentGameState = GameState.LevelState; // the game is active
				ShowScreen(Level);
    		} 

			else
    		{
    			isPaused = true; // the game is paused
				currentGameState = GameState.PauseState; // the pause screen is displayed
				ShowScreen (Pause);
    		}
    	}
        // checks to see if either player has paused the game
        if (Input.GetKeyDown(KeyCode.Joystick1Button6) || Input.GetKeyDown(KeyCode.Joystick2Button6))
        {
            if (isLearning)
            {
                isLearning = false; // the game isn't paused
                currentGameState = GameState.LevelState; // the game is active
                ShowScreen(Level);
            }

            else
            {
                isLearning = true; // the game is paused
                currentGameState = GameState.LearnState; // the pause screen is displayed
                ShowScreen(Learn);
            }
        }
    }

    // fucntion that restarts the level
    public void RestartMatch()
    {
        SceneManager.LoadScene("GameScene");
    }

    // this function shows the instructions screen
    public void HowToPlay()
    {
        currentGameState = GameState.LearnState;
        ShowScreen(Learn);
    }

    // this brings players back to the learn screen
    public void BackToLearn()
    {
        currentGameState = GameState.PauseState;
        ShowScreen(Pause);
    }

    // function that quits the match
    public void QuitMatch()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // void that indicates which screen should be shown
    private void ShowScreen (GameObject gameObjectToShow) 
	{
    	Level.SetActive(false); // all of these are set to false
    	Pause.SetActive(false);
        Learn.SetActive(false);

    	gameObjectToShow.SetActive(true); // this function ensures that only the correct screen is shown
    }
}
