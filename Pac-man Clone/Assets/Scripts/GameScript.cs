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
  private int linesCleared = 0;
  private float baseWaitTime = 1.0f;
  private float levelWaitTime = 1.0f;
  public float waitTime = 1.0f;
  private float newPieceWaitTime = 0.0f;
  //private Text nextLevelText;

  // Start is called before the first frame update
  void Start()
  {
    animationManager = gameObject.GetComponent<AnimationManager>();
    newPieceWaitTime = animationManager.levelUpAnimator.GetCurrentAnimatorStateInfo(0).length;
    //nextLevelText = GameObject.Find("Next Level Text");
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
      }
      placedSquareCount = 0;
    }
    startLine = startLine - (numOfLines-1);
    if (startLineIsSet){
      clearLine(startLine, numOfLines);
    }
  }

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
    scoreboard.incrementScore(numOfLines);
    linesCleared += numOfLines;

    if (linesCleared >= scoreboard.level*10+10) {
      linesCleared -= scoreboard.level*10+10;
      levelUp();
    }
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

  private void levelUp() {
    scoreboard.levelUp();
    animationManager.nextLevelTextScript.LevelUp(scoreboard.level);
    newPieceWaitTime = 1.5f;
    levelWaitTime = baseWaitTime * 1/(scoreboard.level+1);
    placedSquares = new GameObject[10,20];
    string[] tagsToDestroy = {"J", "I", "L", "O", "S", "T", "Z"};
    foreach (string tag in tagsToDestroy) {
      GameObject[] gameObjects = GameObject.FindGameObjectsWithTag (tag);
      foreach (GameObject gameObj in gameObjects)
      {
        Destroy(gameObj);
      }
    }
  }

  public Vector2 Round (Vector2 pos) {
    return new Vector2 (Mathf.Round(pos.x), Mathf.Round(pos.y));
  }

  public void instantiateNextPiece () {
    StartCoroutine(coroutineInstantiateNextPiece());
  }

  IEnumerator coroutineInstantiateNextPiece()
  {
    yield return new WaitForSeconds(newPieceWaitTime);
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
    waitTime = levelWaitTime;
    newPieceWaitTime = 0.0f;
  }
}
