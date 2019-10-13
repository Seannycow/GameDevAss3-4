using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
  private GameScript gameScript;
  private GameObject ghost;
  private float timer = 0.0f;
  private float xPos = 0.0f;
  private float yPos = 0.0f;
  private int rotNum = 1;
  private float rotAmount = 0.0f;

  // Start is called before the first frame update
  void Start()
  {
    gameScript = GameObject.FindWithTag("Managers").GetComponent<GameScript>();
    xPos = transform.position.x;
    yPos = transform.position.y;
    ghost = GameObject.FindGameObjectWithTag("Ghost " + gameObject.tag);
    configureGhost();
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
      if (checkPosition(transform)) {
          configureGhost();
      } else {
        xPos += 1.0f;
        gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);

      }
    }
    if (Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.RightArrow)) {
      xPos += 1.0f;
      gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
      if (checkPosition(transform)) {
        configureGhost();
      } else {
        xPos -= 1.0f;
        gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);

      }
    }
    if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) {
      gameScript.waitTime = 0.02f;
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
          ghost.transform.Rotate(0.0f, 0.0f, rotAmount);
          foreach (Transform square in transform) {
            square.Rotate(0.0f, 0.0f, -rotAmount);
          }
          foreach (Transform square in ghost.transform) {
            square.Rotate(0.0f, 0.0f, -rotAmount);
          }
          rotNum = 2;
          break;
          case 2:
          rotAmount = 270.0f;
          gameObject.transform.Rotate(0.0f, 0.0f, rotAmount);
          ghost.transform.Rotate(0.0f, 0.0f, rotAmount);
          foreach (Transform square in transform) {
            square.Rotate(0.0f, 0.0f, -rotAmount);
          }
          foreach (Transform square in ghost.transform) {
            square.Rotate(0.0f, 0.0f, -rotAmount);
          }
          rotNum = 1;
          break;
        }
        break;
        case "J":
        case "L":
        case "T":
        rotAmount = 90.0f;
        gameObject.transform.Rotate(0.0f, 0.0f, rotAmount);
        ghost.transform.Rotate(0.0f, 0.0f, rotAmount);
        foreach (Transform square in transform) {
          square.Rotate(0.0f, 0.0f, -rotAmount);
        }
        foreach (Transform square in ghost.transform) {
          square.Rotate(0.0f, 0.0f, -rotAmount);
        }
        break;
        case "O":
        //Play sound
        break;
      }

      if (checkPosition(transform)) {
        configureGhost();
      } else {
        gameObject.transform.Rotate(0,0,-rotAmount);
        ghost.transform.Rotate(0.0f, 0.0f, rotAmount);
        foreach (Transform square in transform) {
          square.Rotate(0.0f, 0.0f, rotAmount);
        }
        foreach (Transform square in ghost.transform) {
          square.Rotate(0.0f, 0.0f, rotAmount);
        }
      }
    }


    timer += Time.deltaTime;

    // Check if we have reached beyond 1 seconds.
    // Subtracting two is more accurate over time than resetting to zero.
    if (timer > gameScript.waitTime)
    {

      yPos -= 1.0f;

      gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);

      if (checkPosition(transform)) {

      } else {
        yPos += 1.0f;
        gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
        foreach (Transform square in transform) {
          //square.gameObject.transform.position = gameScript.Round(square.position);
          gameScript.setPlacedSquares(square.gameObject);
        }
        gameScript.checkClearLine();
        enabled = false;
        Destroy(ghost);
        gameScript.instantiateNextPiece();
      }

      // Remove the recorded time.
      timer = timer - gameScript.waitTime;
    }
  }

  private bool checkPosition(Transform checkTransform) {
    foreach (Transform square in checkTransform) {
      square.gameObject.transform.position = gameScript.Round(square.position);

      if (gameScript.checkIsInsideGrid (square.gameObject) == false || gameScript.checkSquareCollision(square.gameObject)) {
        return false;
      }
    }
    return true;
  }

  private void configureGhost() {
    ghost.transform.position = new Vector3(xPos, yPos, 0.0f);
    for (float y = ghost.transform.position.y; y > 0; y--) {
      ghost.transform.position = new Vector3(xPos, ghost.transform.position.y-1, 0.0f);
      Debug.Log("Outer: " + ghost.transform.position);
      if (!checkPosition(ghost.transform)) {
        Debug.Log("Collision at: " + ghost.transform.position);
        ghost.transform.position = new Vector3(xPos, ghost.transform.position.y+1, 0.0f);
        break;
      }
    }
  }
}
