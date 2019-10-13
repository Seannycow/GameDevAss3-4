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
  private int startLevel = 0;

    // Start is called before the first frame update
    void Start () {
		playGameBtn = GameObject.FindGameObjectWithTag("Play Game Button").GetComponent<Button>();
		playGameBtn.onClick.AddListener(PlayGame);
    lvlSelectBtn = GameObject.FindGameObjectWithTag("Level Select Button").GetComponent<Button>();
    lvlSelectBtn.onClick.AddListener(LevelIncrease);
    lvlSelectImage = GameObject.FindGameObjectWithTag("Level Select Button").GetComponent<Image>();
    Debug.Log(playGameBtn);
    Debug.Log(lvlSelectBtn);
    Debug.Log(lvlSelectImage);
	}

	void PlayGame(){
    PlayerPrefs.SetInt("Level", startLevel);
    SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}

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

    // Update is called once per frame
    void Update()
    {

    }

}
