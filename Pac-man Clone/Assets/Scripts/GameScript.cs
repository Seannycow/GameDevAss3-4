using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameScript : MonoBehaviour
{
  private AnimationManager animationManager;
  public Scoreboard scoreboard;
  public static int gridWidth = 10;
  public static int gridHieght = 20;
  private int nextPieceNum = 0;
  private GameObject[,] placedSquares = new GameObject[10,20];
  private int[,] placedSquaresTest = new int[10,20];
  // Start is called before the first frame update
  void Start()
  {
    animationManager = gameObject.GetComponent<AnimationManager>();
    instantiateNextPiece();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public bool checkIsInsideGrid (GameObject obj) {
    return (Convert.ToInt32(obj.transform.position.x) >= 0 && Convert.ToInt32(obj.transform.position.x) < gridWidth && Convert.ToInt32(obj.transform.position.y) >= 0);

  }

  public bool checkSquareCollision (GameObject obj) {
    if(checkIsInsideGrid(obj) == true){
      return placedSquares[Convert.ToInt32(obj.transform.position.x), Convert.ToInt32(obj.transform.position.y)];
    }
    return false;
  }

  public void setPlacedSquares (GameObject obj) {
    placedSquares[Convert.ToInt32(obj.transform.position.x), Convert.ToInt32(obj.transform.position.y)] = obj;
  }

  public void checkClearLine ()  {
    int numOfLines = 0;
    int startLine = 0;
    int placedSquareCount = 0;
    bool startLineIsSet = false;

    for (int j=0; j<20; j++) {
      for (int i=0; i<10; i++) {
        //Debug.Log("Line " + j.ToString() + "\n Column " + i.ToString() + placedSquares[i,j]);
        if (placedSquares[i, j]) {
          placedSquareCount++;
        } else {
          break;
        }
      }
      if (placedSquareCount == 10) {
        numOfLines++;
        startLine = j;
        startLineIsSet = true;
        Debug.Log("+numLines");
      }
      placedSquareCount = 0;
    }
    Debug.Log("Before " + startLine);
    startLine = startLine - (numOfLines-1);
    Debug.Log("Line " + startLine + "\n" + "numOfLines " + numOfLines);
    if (startLineIsSet){
      clearLine(startLine, numOfLines);
    }
  }

  // public void debugPlacedSquaresTest() {
  //
  //   for (int i = 0; i < 10; i++)
  //   {
  //
  //     for (int j = 0; j < 20; j++)
  //     {
  //       placedSquaresTest[i,j] = 1;
  //     }
  //   }
  //
  //   string s = "Test: ";
  //   for (int j = 19; j > -1; j--)
  //   {
  //
  //     for (int i = 0; i < 10; i++)
  //     {
  //       if (placedSquaresTest[i,j] == 1) {
  //         s += "1";
  //       } else {
  //         s+= "0";
  //       }
  //     }
  //     s += "\n";
  //   }
  //     Debug.Log(s);
  //
  //     for (int j = 0; j < 3; j++){
  //       int i =1;
  //       Array.Clear(placedSquaresTest, (j*20)+i, 1);
  //       Debug.Log((j*20)+i);
  //     }
  //     s = "Test 2: ";
  //     for (int j = 19; j > -1; j--)
  //     {
  //
  //       for (int i = 0; i < 10; i++)
  //       {
  //         if (placedSquaresTest[i,j] == 1) {
  //           s += "1";
  //         } else {
  //           s+= "0";
  //         }
  //       }
  //       s += "\n";
  //     }
  //     Debug.Log(s);
  // }

  public void debugPlacedSquares() {
    string s = "";
    for (int j = 19; j > -1; j--)
    {

      for (int i = 0; i < 10; i++)
      {
        s += exists(i,j);
      }
      s += "\n";
    }
    Debug.Log(s);
  }

  public string exists(int i, int j) {
    if (placedSquares[i,j] != null) {
      return "1";
    } else {
      return "0";
    }
  }

  private void clearLine (int startLine, int numOfLines) {
    StartCoroutine(WaitForAnim(startLine, numOfLines));
    scoreboard.incrementScore(0, numOfLines);
  }

  IEnumerator WaitForAnim(int startLine, int numOfLines)
  {
    int k = 0;
    for(k = 0; k<numOfLines; k++) {
      animationManager.runClearLine(startLine+k);
    }
    yield return new WaitForSeconds(animationManager.clearLineAnimator.GetCurrentAnimatorStateInfo(0).length);

    for(int i = 0; i<10; i++){
      for(k = 0; k<numOfLines; k++) {
        Destroy(placedSquares[i,startLine+k]);
        Array.Clear(placedSquares, (i*20)+startLine+k, 1);
        debugPlacedSquares();
      }
      for (int j=startLine+k; j<21; j++) {
        if (j < 20 && exists(i,j) == "1") {
          placedSquares[i,j].transform.position = new Vector2(
          placedSquares[i,j].transform.position.x, placedSquares[i,j].transform.position.y-numOfLines);
          placedSquares[i,j-numOfLines] = placedSquares[i,j];
          Array.Clear(placedSquares, (i*20)+j, 1);
        }
      }
    }
  }

  public Vector2 Round (Vector2 pos) {
    return new Vector2 (Mathf.Round(pos.x), Mathf.Round(pos.y));
  }

  public void instantiateNextPiece () {

    nextPieceNum = UnityEngine.Random.Range(1, 8);
    string nextPieceName = "";
    switch (nextPieceNum)
    {
      case 1:
      nextPieceName = "Tetromino_I";
      break;
      case 2:
      nextPieceName = "Tetromino_J";
      break;
      case 3:
      nextPieceName = "Tetromino_L";
      break;
      case 4:
      nextPieceName = "Tetromino_O";
      break;
      case 5:
      nextPieceName = "Tetromino_S";
      break;
      case 6:
      nextPieceName = "Tetromino_T";
      break;
      case 7:
      nextPieceName = "Tetromino_Z";
      break;
    }
    GameObject nextPiece = (GameObject)Instantiate(Resources.Load(nextPieceName, typeof(GameObject)), new Vector2(5.0f, 18.0f), Quaternion.identity);
    Piece script = nextPiece.AddComponent(typeof(Piece)) as Piece;
  }
}
