using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
  private Button playGameBtn;
  private Button lvlSelectBtn;
  private Image lvlSelectImage;
  //Variable to set what level the game will be on when it starts
  private int startLevel = 0;

    // Get components and add listeners to the buttons, waiting for the user to click on them
    void Start () {
		playGameBtn = GameObject.FindGameObjectWithTag("Play Game Button").GetComponent<Button>();
		playGameBtn.onClick.AddListener(PlayGame);
    lvlSelectBtn = GameObject.FindGameObjectWithTag("Level Select Button").GetComponent<Button>();
    lvlSelectBtn.onClick.AddListener(LevelIncrease);
    lvlSelectImage = GameObject.FindGameObjectWithTag("Level Select Button").GetComponent<Image>();
	}

  // On click for the play game button will save whatever level was selected with the other button and save it so it can be used through a scene change
	void PlayGame(){
    PlayerPrefs.SetInt("Level", startLevel);
    SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}

  // Adds to the startLevel counter on click of level select button. Since there are only 10 levels, the button will cycle back to 0 after 9
  void LevelIncrease() {
    if (startLevel < 9) {
      startLevel++;
    }
    else
    {
      startLevel = 0;
    }
    lvlSelectImage.sprite = Resources.Load<Sprite>("Sprites/Text/Level Select "+ startLevel);
  }
}
