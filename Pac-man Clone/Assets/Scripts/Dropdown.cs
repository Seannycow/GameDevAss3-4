using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropdown : MonoBehaviour
{
  private GameObject nextPiece;
  // private GameObject rightFrameGO;
  // private GameObject leftFrameGO;
  // private EdgeCollider2D topFrameEC;
  // private EdgeCollider2D rightFrameEC;
  // private EdgeCollider2D leftFrameEC;
  private bool canMoveRight = true;
  private bool canMoveLeft = true;
  private bool planted = false;
  private float waitTime = 1.0f;
  private float timer = 0.0f;
  private float xPos = -1.0f;
  private float yPos = 4.5f;
  private int rotNum = 1;
  private int nextPieceNum = 0;
    // private GameObject piece = gameObject;
    // Start is called before the first frame update
    void Start()
    {

      // if (topFrameGO == null)
      // {
      //     topFrameGO = GameObject.FindWithTag("Top Frame");
      //     topFrameEC = topFrameGO.GetComponent<EdgeCollider>();
      // }
      // if (rightFrameGO == null)
      // {
      //     rightFrameGO = GameObject.FindWithTag("Right Frame");
      //     rightFrameEC = topFrameGO.GetComponent<EdgeCollider>();
      // }
      // if (leftFrameGO == null)
      // {
      //     leftFrameGO = GameObject.FindWithTag("Left Frame");
      //     leftFrameEC = topFrameGO.GetComponent<EdgeCollider>();
      // }
    }

    // Update is called once per frame
    void Update()
    {
      waitTime = 0.15f;
      gameObject.GetComponent<Collider2D>().offset = new Vector2(0.0f, 0.0f);
      if (planted) {
        nextPieceNum = Random.Range(1, 8);
        Debug.Log(nextPieceNum);
        switch (nextPieceNum)
        {
          case 1:
            nextPiece = GameObject.FindWithTag("I");
            break;
          case 2:
            nextPiece = GameObject.FindWithTag("J");
            break;
          case 3:
            nextPiece = GameObject.FindWithTag("L");
            break;
          case 4:
            nextPiece = GameObject.FindWithTag("O");
            break;
          case 5:
            nextPiece = GameObject.FindWithTag("S");
            break;
          case 6:
            nextPiece = GameObject.FindWithTag("T");
            break;
          case 7:
            nextPiece = GameObject.FindWithTag("Z");
            break;
        }
        Destroy(gameObject.GetComponent<Dropdown>());
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        nextPiece = Instantiate(nextPiece, new Vector3(-1.0f, 4.5f, 0.0f), Quaternion.identity);
        Dropdown script = nextPiece.AddComponent(typeof(Dropdown)) as Dropdown;
        gameObject.tag = "Planted";
      } else {
        if (canMoveLeft && (Input.GetKeyDown("a") || Input.GetKeyDown(KeyCode.LeftArrow))) {
          xPos -= 0.5f;
          gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
        }
        if (canMoveRight && (Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.RightArrow))) {
          xPos += 0.5f;
          gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) {
          waitTime = 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
          timer = -10000.0f;
        }
        if ((Input.GetKeyDown("r"))) {
          switch (gameObject.tag)
          {
              case "I":
              case "S":
              case "Z":
                  switch(rotNum)
                  {
                    case 1:
                      gameObject.transform.Rotate(0.0f, 0.0f, 90.0f);
                      xPos += 0.5f;
                      yPos -= 1.0f;
                      rotNum = 2;
                      break;
                    case 2:
                      gameObject.transform.Rotate(0.0f, 0.0f, 270.0f);
                      xPos -= 0.5f;
                      yPos += 1.0f;
                      rotNum = 1;
                      break;
                  }
                  gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
                  break;
              case "J":
              case "L":
              case "T":
              gameObject.transform.Rotate(0.0f, 0.0f, 90.0f);
                switch(rotNum)
                {
                  case 1:
                    yPos -= 1.5f;
                    rotNum = 2;
                    break;
                  case 2:
                    xPos += 1.5f;
                    rotNum = 3;
                    break;
                  case 3:
                    yPos += 1.5f;
                    rotNum = 4;
                    break;
                  case 4:
                    xPos -= 1.5f;
                    rotNum = 1;
                    break;
                }
                gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
                break;
              case "O":
                  //Play sound
                  break;
              default:
                  //Console.WriteLine("Default case");
                  break;
          }
        }


        timer += Time.deltaTime;

          // Check if we have reached beyond 2 seconds.
          // Subtracting two is more accurate over time than resetting to zero.
          if (timer > waitTime)
          {

            yPos -= 0.5f;

            gameObject.transform.position = new Vector3(xPos, yPos, 0.0f);
            switch(rotNum)
            {
              case 1:
                gameObject.GetComponent<Collider2D>().offset = new Vector2(0.0f, -0.25f);
                break;
              case 2:
                gameObject.GetComponent<Collider2D>().offset = new Vector2(-0.25f, 0.0f);
                break;
              case 3:
                gameObject.GetComponent<Collider2D>().offset = new Vector2(0.0f, 0.25f);
                break;
              case 4:
                gameObject.GetComponent<Collider2D>().offset = new Vector2(0.25f, 0.0f);
                break;
            }

            // Remove the recorded 2 seconds.
            timer = timer - waitTime;
          }
      }


    }

    void OnCollisionEnter2D(Collision2D coll) {
      Debug.Log(coll.gameObject.tag);
        if (coll.gameObject.tag == "Left Frame")
            canMoveLeft = false;
        if (coll.gameObject.tag == "Right Frame")
          canMoveRight = false;
        if (coll.gameObject.tag == "Bottom Frame" || coll.gameObject.tag == "Planted")
        {
          //gameObject.transform.position = new Vector3(xPos, yPos + 0.5f, 0.0f);
          planted = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll) {

      if (coll.gameObject.tag == "Left Frame")
          canMoveLeft = true;
      if (coll.gameObject.tag == "Right Frame")
        canMoveRight = true;
    }
}
