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

  //Maintain the sound manager and all it's sources through scene changes so the background music is uninterrupted
  void Awake() {
    DontDestroyOnLoad(this.gameObject);
  }

  //Load all audio clips from the resource folder for use on startup
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
  }

  //Each of these public methods are for the game script and piece script to use when certain events occur
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
