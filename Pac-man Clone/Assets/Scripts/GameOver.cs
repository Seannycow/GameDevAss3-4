using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
  private Button againBtn;
  private Button exitBtn;

    // Start is called before the first frame update
    void Start () {
    againBtn = GameObject.FindGameObjectWithTag("Try Again Button").GetComponent<Button>();
    againBtn.onClick.AddListener(PlayGame);
    exitBtn = GameObject.FindGameObjectWithTag("Exit Button").GetComponent<Button>();
    exitBtn.onClick.AddListener(Exit);
  }

  void PlayGame(){
    SceneManager.LoadScene("Game", LoadSceneMode.Single);
  }

  void Exit() {
    SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
  }

    // Update is called once per frame
    void Update()
    {

    }
}
