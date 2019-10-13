using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
  private GameScript gameScript;
  // game object of the phantom piece that indicates where the current piece will fall
  private GameObject ghost;
  private float timer = 0.0f;
  private float xPos = 0.0f;
  private float yPos = 0.0f;
  // Number to keep track of where the piece is in the rotation
  private int rotNum = 1;
  private float rotAmount = 0.0f;

  void Start()
  {
    gameScript = GameObject.FindWithTag("Managers").GetComponent<GameScript>();
    xPos = transform.position.x;
    yPos = transform.position.y;
    // Finding the ghost game object with tags
    ghost = GameObject.FindGameObjectWithTag("Ghost " + gameObject.tag);
    configureGhost();
  }

  // Check for key inputs every frame
  void Update()
  {
    getInput();
    dropdown();
  }

  // Deals with any actions from inputs from the user
  void getInput() {
    if (Input.GetKeyDown("a") || Input.GetKeyDown(KeyCode.LeftArrow)) {
      movePiece(-1.0f);
    }

    if (Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.RightArrow)) {
      movePiece(1.0f);
    }

    if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) {
      gameScript.waitTime = 0.02f;
    }

    // Rotates each piece correctly, has many switch cases as rotation is different for many of the pieces
    if ((Input.GetKeyDown("r"))) {
      rotAmount = 0.0f;
      switch (gameObject.tag)
      {
        // I, S and Z all rotate twice
        case "I":
        case "S":
        case "Z":
        switch(rotNum)
        {
          case 1:
          rotAmount = 90.0f;
          rotatePiece(rotAmount);
          rotNum = 2;
          break;
          case 2:
          rotAmount = 270.0f;
          rotatePiece(rotAmount);
          rotNum = 1;
          break;
        }
        break;
        // J, L and T all rotate 4 times to it just increments their angles by 90 degrees each time
        case "J":
        case "L":
        case "T":
        rotAmount = 90.0f;
        rotatePiece(rotAmount);
        break;
        case "O":
        break;
      }

      /** Checks to make sure the rotation wouldn't put any of the piece's squares out of bounds
      and if it does then reverting the rotation all in the same frame so the user will not see
      the change**/
      if (checkPosition(transform)) {
        // Every time the piece is changed by the user, the ghost needs to check if it needs to move aswell
        configureGhost();
        // Play rotation sound all the way from the sound manager
        gameScript.playSoundFromPiece("pieceRotation");
      } else {
        // Reverting rotation
        rotatePiece(-rotAmount);
        gameScript.playSoundFromPiece("actionFailure");
      }
    }
  }

  /** Moves piece depending on the direction given with a positive or negative 1.0f.
  Again moves the piece, checks if it was a legal move and moves it back if it was illegal**/
  private void movePiece(float direction){
    xPos += direction;
    gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
    if (checkPosition(transform)) {
      configureGhost();
      gameScript.playSoundFromPiece("move");
    } else {
      xPos -= direction;
      gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
      gameScript.playSoundFromPiece("actionFailure");
    }
  }

  /** Rotates main piece game objects and then rotates the squares that make up a
  piece in the opposite dirrection so the sprites are the correct rotation.
  This is done for both the current piece and ghost piece**/
  private void rotatePiece(float rotAmount) {
    gameObject.transform.Rotate(0.0f, 0.0f, rotAmount);
    ghost.transform.Rotate(0.0f, 0.0f, rotAmount);
    foreach (Transform square in transform) {
      square.Rotate(0.0f, 0.0f, -rotAmount);
    }
    foreach (Transform square in ghost.transform) {
      square.Rotate(0.0f, 0.0f, -rotAmount);
    }
  }

  // Drops the piece one unit if possible
  private void dropdown() {
    // Tallies up all the time between frames acting as a game clock. This helps with frame independent movement
    timer += Time.deltaTime;
    /** Once the games current wait time (varies depending on level) has elapsed,
    the dropdown will occur **/
    if (timer > gameScript.waitTime)
    {

      yPos -= 1.0f;

      gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);

      if (!checkPosition(transform)) {
        yPos += 1.0f;
        gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
        /** If piece has collided with another square due to the dropdown then
        the piece is placed and will be added to the game script multidimensional
        array housing the position of each placed piece **/
        foreach (Transform square in transform) {
          // Precautionary rounding of the positions just in case
          square.gameObject.transform.position = gameScript.Round(square.position);
          gameScript.setPlacedSquares(square.gameObject);
        }
        gameScript.playSoundFromPiece("piecePlacement");

        // Initiate clear line process including logic and an animation
        gameScript.checkClearLine();
        // Disable placed piece
        enabled = false;
        // Ghost is no longer needed
        Destroy(ghost);
        // Make a new piece which will have this script in it to restart the process
        StartCoroutine(gameScript.coroutineInstantiateNextPiece());
      }

      // Remove the recorded time.
      timer = timer - gameScript.waitTime;
    }
  }

  /** Checks if position of all squares in the piece are in the boundaries of the game
  or if they are inside a placed piece **/
  private bool checkPosition(Transform checkTransform) {
    foreach (Transform square in checkTransform) {
      square.gameObject.transform.position = gameScript.Round(square.position);
      if (gameScript.checkIsInsideGrid (square.gameObject) == false || gameScript.checkSquareCollision(square.gameObject)) {
        // If position illegal return false
        return false;
      }
    }
    // If there is no problem return true
    return true;
  }

  /** Move ghost piece down one unit and checking if there are any collisions, if
  not moving down another unit and checking again until it hits the border or a
  placed piece, indicating where the piece would land if it were to just drop all
  the way down**/
  private void configureGhost() {
    ghost.transform.position = new Vector3(xPos, yPos, 0.0f);
    for (float y = ghost.transform.position.y; y > 0; y--) {
      ghost.transform.position = new Vector3(xPos, ghost.transform.position.y-1, 0.0f);
      if (!checkPosition(ghost.transform)) {
        ghost.transform.position = new Vector3(xPos, ghost.transform.position.y+1, 0.0f);
        break;
      }
    }
  }
}
