using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
  private Button againBtn;
  private Button exitBtn;

    // Setting listeners on buttons
    void Start () {
    againBtn = GameObject.FindGameObjectWithTag("Try Again Button").GetComponent<Button>();
    againBtn.onClick.AddListener(PlayGame);
    exitBtn = GameObject.FindGameObjectWithTag("Exit Button").GetComponent<Button>();
    exitBtn.onClick.AddListener(Exit);
  }

  // Returns player to game at the level they failed on if try again is clickd
  void PlayGame(){
    SceneManager.LoadScene("Game", LoadSceneMode.Single);
  }

  // Returns to main menu if exit is clicked
  void Exit() {
    SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
  }
}
