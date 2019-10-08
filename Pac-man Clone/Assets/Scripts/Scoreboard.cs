using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
  private Text scoreText;
  private int score = 0;
    // Start is called before the first frame update
    void Start()
    {
      scoreText = this.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
      scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void incrementScore (int level, int lines) {
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
      scoreText.text = score.ToString();
    }
}
