using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
  private GameObject currClearLineBlock;
  private GameObject newLevelText;
  public Animator clearLineAnimator;
  public Animator levelUpAnimator;
  public AnimationScript nextLevelTextScript;

    void Start()
    {
      //Initiate all the variables needed to animate the New Level Text
      newLevelText = GameObject.FindGameObjectWithTag("New Level Text");
      levelUpAnimator = newLevelText.GetComponent<Animator>();
      nextLevelTextScript = newLevelText.GetComponent<AnimationScript>();
    }

    //Create the long line clear game object that has it's animation run on start
    public void runClearLine(int line)
    {
      currClearLineBlock = (GameObject)Instantiate(Resources.Load("Clear_Anim_Example", typeof(GameObject)), new Vector3(0.0f, (float)line, -1.0f), Quaternion.identity);
      //clearLineAnimator initialised since the Gamescript will use it to find how long the animation
      clearLineAnimator = currClearLineBlock.GetComponent<Animator>();
    }

}
