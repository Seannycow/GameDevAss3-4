using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{

    public static int gridWidth = 10;
    public static int gridHieght = 20;
    private int nextPieceNum = 0;
    // Start is called before the first frame update
    void Start()
    {
      instantiateNextPiece();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool checkIsInsideGrid (Vector2 pos) {
      return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    public Vector2 Round (Vector2 pos) {
      return new Vector2 (Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    public void instantiateNextPiece () {

      nextPieceNum = Random.Range(1, 8);
      Debug.Log(nextPieceNum);
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
      GameObject nextPiece = (GameObject)Instantiate(Resources.Load(nextPieceName, typeof(GameObject)), new Vector2(5.0f, 20.0f), Quaternion.identity);
      Piece script = nextPiece.AddComponent(typeof(Piece)) as Piece;
    }
}
