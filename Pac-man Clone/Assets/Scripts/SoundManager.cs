using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  private AudioSource[] audioSources;
  private AudioClip gameStart;
  private AudioClip gameOver;
  private AudioClip pieceRotation;
  private AudioClip move;
  private AudioClip actionFailure;
  private AudioClip lineClear;
  private AudioClip piecePlacement;

    // Start is called before the first frame update
    void Start()
    {
      audioSources = GetComponents<AudioSource>();
      gameStart = (AudioClip) Resources.Load("Sounds/Game_Start");
      gameOver = (AudioClip) Resources.Load("Sounds/Game_Over");
      pieceRotation = (AudioClip) Resources.Load("Sounds/Rotate");
      move = (AudioClip) Resources.Load("Sounds/Move");
      actionFailure = (AudioClip) Resources.Load("Sounds/Action_Failure");
      lineClear = (AudioClip) Resources.Load("Sounds/Line_Clear");
      piecePlacement = (AudioClip) Resources.Load("Sounds/Piece_Placement");
      audioSources[1].clip = gameStart;
      audioSources[1].Play();
    }

    public void playGameStart(){
      audioSources[1].clip = gameStart;
      audioSources[1].Play();
    }

    public void playGameOver(){
      audioSources[1].clip = gameOver;
      audioSources[1].Play();
    }

    public void playPieceRotation(){
      audioSources[1].clip = pieceRotation;
      audioSources[1].Play();
    }

    public void playMove(){
      audioSources[1].clip = move;
      audioSources[1].Play();
    }

    public void playActionFailure(){
      audioSources[1].clip = actionFailure;
      audioSources[1].Play();
    }

    public void playLineClear(){
      audioSources[1].clip = lineClear;
      audioSources[1].Play();
    }

    public void playPiecePlacement(){
      audioSources[1].clip = piecePlacement;
      audioSources[1].Play();
    }
}
