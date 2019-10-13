using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
  private AnimationManager animationManager;
  public Scoreboard scoreboard;
  private SoundManager soundManager;
  private GameObject nextPieceWindowPiece;
  private GameObject currentPiece;
  private GameObject currentPieceGhost;
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
  private Vector2 nextPieceWindowPos = new Vector2(-4.0f, 11.5f);
  private bool firstGameObjectFlag = true;

  // Start is called before the first frame update
  void Start()
  {
    soundManager = gameObject.GetComponent<SoundManager>();
    animationManager = gameObject.GetComponent<AnimationManager>();
    newPieceWaitTime = animationManager.levelUpAnimator.GetCurrentAnimatorStateInfo(0).length;
    nextPieceWindowPiece = (GameObject)Instantiate(Resources.Load(assignPiece(), typeof(GameObject)), nextPieceWindowPos, Quaternion.identity);
    windowPieceBringToFront(nextPieceWindowPiece, "UI");
    levelWaitTime = baseWaitTime * 1/(scoreboard.level+1);
    instantiateNextPiece();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public bool checkIsInsideGrid (GameObject obj) {
    return (Convert.ToInt32(obj.transform.position.x) >= 0 && Convert.ToInt32(obj.transform.position.x) < gridWidth && Convert.ToInt32(obj.transform.position.y) >= 0 && Convert.ToInt32(obj.transform.position.y) <= 19);

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

  private void checkGameOver() {
      for (int i=0; i<10; i++) {
        if (placedSquares[i, 19]) {
          gameOver();
        }
      }
  }

  public void checkClearLine ()  {
    checkGameOver();

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

  private void gameOver() {
    PlayerPrefs.SetInt("Level", scoreboard.level);
    soundManager.playGameOver();
    SceneManager.LoadScene("Game Over State", LoadSceneMode.Single);
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
    soundManager.playLineClear();
    scoreboard.incrementScore(numOfLines);
    linesCleared += numOfLines;

    if (linesCleared >= scoreboard.level*1+1) {
      //linesCleared -= scoreboard.level*10+10;
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
    soundManager.playGameStart();
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

  private string assignPiece() {
    nextPieceNum = UnityEngine.Random.Range(1, 8);
    string nextPieceName = "";
    switch (nextPieceNum)
    {
      case 1:
      nextPieceName = "Tetromino_I";
      nextPieceWindowPos = new Vector2(-4.0f, 11.5f);
      break;
      case 2:
      nextPieceName = "Tetromino_J";
      nextPieceWindowPos = new Vector2(-3.5f, 11.0f);
      break;
      case 3:
      nextPieceName = "Tetromino_L";
      nextPieceWindowPos = new Vector2(-3.5f, 11.0f);
      break;
      case 4:
      nextPieceName = "Tetromino_O";
      nextPieceWindowPos = new Vector2(-4.0f, 11.0f);
      break;
      case 5:
      nextPieceName = "Tetromino_S";
      nextPieceWindowPos = new Vector2(-3.5f, 12.0f);
      break;
      case 6:
      nextPieceName = "Tetromino_T";
      nextPieceWindowPos = new Vector2(-3.5f, 11.0f);
      break;
      case 7:
      nextPieceName = "Tetromino_Z";
      nextPieceWindowPos = new Vector2(-3.5f, 12.0f);
      break;
    }
    return nextPieceName;
  }

  private void windowPieceBringToFront(GameObject piece, String layerName) {
    foreach (Transform square in piece.transform) {
      SpriteRenderer sprite = square.gameObject.GetComponent<SpriteRenderer>();
      sprite.sortingLayerName = layerName;
    }
  }

  private Vector2 findStartPos(String tag) {
    Vector2 startPos = new Vector2(4.0f, 18.0f);
    switch (tag)
    {
      case "I":
      case "S":
      case "Z":
      startPos = new Vector2(4.0f, 19.0f);
      break;
    }
    return startPos;
  }

  IEnumerator coroutineInstantiateNextPiece()
  {
    yield return new WaitForSeconds(newPieceWaitTime);

    if(!firstGameObjectFlag && newPieceWaitTime == 1.5f)
    {
      nextPieceWindowPiece = (GameObject)Instantiate(Resources.Load(assignPiece(), typeof(GameObject)), nextPieceWindowPos, Quaternion.identity);
      windowPieceBringToFront(nextPieceWindowPiece, "UI");
    }
    firstGameObjectFlag = false;

    nextPieceWindowPiece.transform.position = findStartPos(nextPieceWindowPiece.tag);
    currentPiece = nextPieceWindowPiece;
    windowPieceBringToFront(currentPiece, "Default");
    currentPieceGhost = (GameObject)Instantiate(Resources.Load("Ghost_Tetromino_" + currentPiece.tag, typeof(GameObject)), currentPiece.transform.position, Quaternion.identity);


    Piece script = currentPiece.AddComponent(typeof(Piece)) as Piece;
    nextPieceWindowPiece = (GameObject)Instantiate(Resources.Load(assignPiece(), typeof(GameObject)), nextPieceWindowPos, Quaternion.identity);
    windowPieceBringToFront(nextPieceWindowPiece, "UI");
    waitTime = levelWaitTime;
    newPieceWaitTime = 0.0f;
  }

  public void playSoundFromPiece (string sound) {
    switch(sound) {
      case "pieceRotation":
        soundManager.playPieceRotation();
        break;
      case "move":
        soundManager.playMove();
        break;
      case "actionFailure":
        soundManager.playActionFailure();
        break;
      case "piecePlacement":
        soundManager.playPiecePlacement();
        break;
    }
  }
}
