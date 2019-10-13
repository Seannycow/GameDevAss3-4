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
  // Houses the each square in multidimensional array of game space
  private GameObject[,] placedSquares = new GameObject[10,20];
  public static int gridWidth = 10;
  public static int gridHieght = 20;
  // Used to determine which type of piece will come next
  private int nextPieceNum = 0;
  // Amount of lines cleared this level
  private int linesCleared = 0;

  // Wait times for determining how fast the piece's dropdown is
  private float baseWaitTime = 1.0f;
  private float levelWaitTime = 1.0f;
  public float waitTime = 1.0f;
  // Used to wait for animations to play
  private float newPieceWaitTime = 0.0f;

  private Vector2 nextPieceWindowPos = new Vector2(-4.0f, 11.5f);
  private bool firstGameObjectFlag = true;

  // Start is called before the first frame update
  void Start()
  {
    soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    soundManager.playGameStart();
    animationManager = gameObject.GetComponent<AnimationManager>();
    // Saves length of the next level animation
    newPieceWaitTime = animationManager.levelUpAnimator.GetCurrentAnimatorStateInfo(0).length;
    // Instantiate first piece
    nextPieceWindowPiece = (GameObject)Instantiate(Resources.Load(assignPiece(), typeof(GameObject)), nextPieceWindowPos, Quaternion.identity);
    windowPieceBringToFront(nextPieceWindowPiece, "UI");
    // Determines the wait time of each level (important in start in case not starting at lvl 0)
    levelWaitTime = baseWaitTime * 1/(scoreboard.level+1);
    StartCoroutine(coroutineInstantiateNextPiece());
  }

  // Rounding positions to whole numbers
  public Vector2 Round (Vector2 pos) {
    return new Vector2 (Mathf.Round(pos.x), Mathf.Round(pos.y));
  }
/*****************************************************************************************************************************************************************************************************************************************************************************************/
  // Takes a game object (square) and inserts it in the multidimensional array where it is in the game
  public void setPlacedSquares (GameObject obj) {
    placedSquares[Convert.ToInt32(obj.transform.position.x), Convert.ToInt32(obj.transform.position.y)] = obj;
  }

  // Returns true if inside border, returns false if not
  public bool checkIsInsideGrid (GameObject obj) {
    return (Convert.ToInt32(obj.transform.position.x) >= 0 && Convert.ToInt32(obj.transform.position.x) < gridWidth && Convert.ToInt32(obj.transform.position.y) >= 0 && Convert.ToInt32(obj.transform.position.y) <= 19);
  }

  // Checks if square collides with any placed squares in the game
  public bool checkSquareCollision (GameObject obj) {
    if(checkIsInsideGrid(obj) == true){
      return placedSquares[Convert.ToInt32(obj.transform.position.x), Convert.ToInt32(obj.transform.position.y)];
    }
    return false;
  }

  // Scans the top row to check if the player has failed to call game over
  private void checkGameOver() {
    for (int i=0; i<10; i++) {
      if (placedSquares[i, 19]) {
        gameOver();
      }
    }
  }

  // Check there are any rows with all 10 squares are filled, is called every time a piece is placed
  public void checkClearLine ()  {
    checkGameOver();

    int numOfLines = 0;
    int startLine = 0;
    int placedSquareCount = 0;
    bool startLineIsSet = false;

    /** Loops through the multidimensional array and counts the amount of squares are in the rows
    from right to left, there there is ever a hole then it moves on to the next line (since it is
    impossible there will be a full line).**/
    for (int j=0; j<20; j++) {
      for (int i=0; i<10; i++) {
        if (placedSquares[i, j]) {
          placedSquareCount++;
        } else {
          break;
        }
      }
      /** If there is a full row, add to counter of number of lines cleared, save the start line,
      this will overwrite if there is another full row further up**/
      if (placedSquareCount == 10) {
        numOfLines++;
        startLine = j;
        startLineIsSet = true;
      }
      // Reset counter
      placedSquareCount = 0;
    }
    // Sets startLine to the lowest cleared line
    startLine = startLine - (numOfLines-1);
    if (startLineIsSet){
      clearLine(startLine, numOfLines);
    }
  }

  // Initiate animation and check if player made it to the next level
  private void clearLine (int startLine, int numOfLines) {
    StartCoroutine(WaitForAnim(startLine, numOfLines));
    soundManager.playLineClear();
    // Update the score on the scoreboard
    scoreboard.incrementScore(numOfLines);
    linesCleared += numOfLines;

    if (linesCleared >= scoreboard.level*10+10) {
      linesCleared -= scoreboard.level*10+10;
      levelUp();
    }
  }
/*****************************************************************************************************************************************************************************************************************************************************************************************/
  /** Dealing with everything that comes with a clear line, the animation and moving
  around the game objects in the game world and array**/
  IEnumerator WaitForAnim(int startLine, int numOfLines)
  {
    // Initilised here because it is used outside this loop
    int k = 0;
    for(k = 0; k<numOfLines; k++) {
      // Run clear line animation for all lines that where full
      animationManager.runClearLine(startLine+k);
    }
    // Wait for time of clear line animation
    yield return new WaitForSeconds(animationManager.clearLineAnimator.GetCurrentAnimatorStateInfo(0).length);

    // Loop through each column in game
    for(int i = 0; i<10; i++){
      // Looping through each line that was cleared and clear the multidimensional array
      for(k = 0; k<numOfLines; k++) {
        Destroy(placedSquares[i,startLine+k]);
        // Very dumb algorithm because Array.Clear is weird
        Array.Clear(placedSquares, (i*20)+startLine+k, 1);
      }
      /** For each column, move all the squares above the cleared line down to
      replace the squares that are being cleared for both the game world and
      multidimensional array **/
      for (int j=startLine+k; j<21; j++) {
        if (j < 20 && placedSquares[i,j]) {
          placedSquares[i,j].transform.position = new Vector2(
          placedSquares[i,j].transform.position.x, placedSquares[i,j].transform.position.y-numOfLines);
          placedSquares[i,j-numOfLines] = placedSquares[i,j];
          Array.Clear(placedSquares, (i*20)+j, 1);
        }
      }
    }
  }

  // When enough lines have been cleared, play new level animation and clear board
  private void levelUp() {
    // There is no lvl 10 so it goes to the game over screen if you make it past lvl 9
    if (scoreboard.level > 9) {
      gameOver();
    }
    //Increment level on scoreboard
    scoreboard.levelUp();
    // Run next level splash screen animations
    animationManager.nextLevelTextScript.LevelUp(scoreboard.level);
    soundManager.playGameStart();
    // Length of mentioned animation
    newPieceWaitTime = 1.5f;
    // Set new game speed
    levelWaitTime = baseWaitTime * 1/(scoreboard.level+1);
    // Reset array
    placedSquares = new GameObject[10,20];
    //Destroy only the game pieces
    string[] tagsToDestroy = {"J", "I", "L", "O", "S", "T", "Z"};
    foreach (string tag in tagsToDestroy) {
      GameObject[] gameObjects = GameObject.FindGameObjectsWithTag (tag);
      foreach (GameObject gameObj in gameObjects)
      {
        Destroy(gameObj);
      }
    }

    // Start the process by spawning another piece
    StartCoroutine(coroutineInstantiateNextPiece());
  }

  // Moves the next window piece to the current piece and initialise everything it needs
  public IEnumerator coroutineInstantiateNextPiece()
  {
    // Wait for animation such as clear line if there is one to wait for
    yield return new WaitForSeconds(newPieceWaitTime);

    // If this is the start of a new level, created the next window piece first so it can become the current
    if(!firstGameObjectFlag && newPieceWaitTime == 1.5f)
    {
      nextPieceWindowPiece = (GameObject)Instantiate(Resources.Load(assignPiece(), typeof(GameObject)), nextPieceWindowPos, Quaternion.identity);
      // Change all the sprites to the UI layer so they can be seen on the UI
      windowPieceBringToFront(nextPieceWindowPiece, "UI");
    }
    firstGameObjectFlag = false;

    nextPieceWindowPiece.transform.position = findStartPos(nextPieceWindowPiece.tag);
    currentPiece = nextPieceWindowPiece;
    windowPieceBringToFront(currentPiece, "Default");
    // Create ghost piece
    currentPieceGhost = (GameObject)Instantiate(Resources.Load("Ghost_Tetromino_" + currentPiece.tag, typeof(GameObject)), currentPiece.transform.position, Quaternion.identity);

    // Reset next window piece
    Piece script = currentPiece.AddComponent(typeof(Piece)) as Piece;
    nextPieceWindowPiece = (GameObject)Instantiate(Resources.Load(assignPiece(), typeof(GameObject)), nextPieceWindowPos, Quaternion.identity);
    windowPieceBringToFront(nextPieceWindowPiece, "UI");
    // Set level dropdown time
    waitTime = levelWaitTime;
    // Resets value as animations have played out
    newPieceWaitTime = 0.0f;
  }

  // Assign a random piece and their starting positions depending on piece
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

  // Sets sprites UI or Default layers
  private void windowPieceBringToFront(GameObject piece, String layerName) {
    foreach (Transform square in piece.transform) {
      SpriteRenderer sprite = square.gameObject.GetComponent<SpriteRenderer>();
      sprite.sortingLayerName = layerName;
    }
  }

  // Start position is different for some of the piece cause of their shape
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

  // Load game over scene
  private void gameOver() {
    // Keep scoreboard in case they try again
    PlayerPrefs.SetInt("Level", scoreboard.level);
    soundManager.playGameOver();
    SceneManager.LoadScene("Game Over State", LoadSceneMode.Single);
  }

  // Supply sounds to the piece script from the sound manager
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
