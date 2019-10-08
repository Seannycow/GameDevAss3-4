using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
  private GameScript gameScript;

  private float waitTime = 1.0f;
  private float timer = 0.0f;
  private float xPos = 5.0f;
  private float yPos = 18.0f;
  private int rotNum = 1;
  private float rotAmount = 0.0f;

  // Start is called before the first frame update
  void Start()
  {
    gameScript = GameObject.FindWithTag("Managers").GetComponent<GameScript>();
  }

  // Update is called once per frame
  void Update()
  {
    getInput();
  }

  void getInput() {
    if (Input.GetKeyDown("a") || Input.GetKeyDown(KeyCode.LeftArrow)) {
      xPos -= 1.0f;
      gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
      if (checkPosition()) {

      } else {
        xPos += 1.0f;
        gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
      }
    }
    if (Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.RightArrow)) {
      xPos += 1.0f;
      gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
      if (checkPosition()) {

      } else {
        xPos -= 1.0f;
        gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
      }
    }
    if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) {
      waitTime = 0.2f;
    }
    if (Input.GetKeyDown(KeyCode.Space)) {
      timer = -10000.0f;
    }
    if ((Input.GetKeyDown("r"))) {
      rotAmount = 0.0f;
      switch (gameObject.tag)
      {
        case "I":
        case "S":
        case "Z":
        switch(rotNum)
        {
          case 1:
          rotAmount = 90.0f;
          gameObject.transform.Rotate(0.0f, 0.0f, rotAmount);
          rotNum = 2;
          break;
          case 2:
          rotAmount = 270.0f;
          gameObject.transform.Rotate(0.0f, 0.0f, rotAmount);
          rotNum = 1;
          break;
        }
        break;
        case "J":
        case "L":
        case "T":
        rotAmount = 90.0f;
        gameObject.transform.Rotate(0.0f, 0.0f, rotAmount);
        break;
        case "O":
        //Play sound
        break;
      }

      if (checkPosition()) {

      } else {
        gameObject.transform.Rotate(0,0,-rotAmount);
      }
    }


    timer += Time.deltaTime;

    // Check if we have reached beyond 2 seconds.
    // Subtracting two is more accurate over time than resetting to zero.
    if (timer > waitTime)
    {

      yPos -= 1.0f;

      gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);

      if (checkPosition()) {

      } else {
        yPos += 1.0f;
        gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
        foreach (Transform square in transform) {
          //square.gameObject.transform.position = gameScript.Round(square.position);
          gameScript.setPlacedSquares(square.gameObject);
        }

        gameScript.checkClearLine();
        enabled = false;
        gameScript.instantiateNextPiece();
      }

      // Remove the recorded 2 seconds.
      timer = timer - waitTime;
    }
  }

  bool checkPosition() {
    foreach (Transform square in transform) {
      square.gameObject.transform.position = gameScript.Round(square.position);

      if (gameScript.checkIsInsideGrid (square.gameObject) == false || gameScript.checkSquareCollision(square.gameObject)) {
        return false;
      }
    }
    return true;
  }
}
