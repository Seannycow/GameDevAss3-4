using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
  private Text scoreText;
  private Text levelText;
  public Sprite levelTextSprite;
  private int score = 0;
  public int level = 0;

  void Awake(){
    // So level is set before the GameScript starts.
    level = PlayerPrefs.GetInt("Level");
  }

  void Start()
  {
    scoreText = this.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
    scoreText.text = score.ToString();
    levelText = this.gameObject.transform.GetChild(2).gameObject.GetComponent<Text>();
    levelText.text = level.ToString();
  }

  // Increments score on a line clear based on the level and number of lines cleared
  public void incrementScore (int lines) {
    int linesScore = 0;
    switch(lines) {
      case 1:
      linesScore = 40;
      break;
      case 2:
      linesScore = 100;
      break;
      case 3:
      linesScore = 300;
      break;
      case 4:
      linesScore = 1200;
      break;
    }
    score += linesScore * (level+1);
    // Display new score on scoreboard
    scoreText.text = score.ToString();
  }

  // Increments level on scoreboard
  public void levelUp() {
    level++;
    levelText.text = level.ToString();
  }
}
