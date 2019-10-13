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
    // Start is called before the first frame update
    void Start()
    {
      newLevelText = GameObject.FindGameObjectWithTag("New Level Text");
      levelUpAnimator = newLevelText.GetComponent<Animator>();
      nextLevelTextScript = newLevelText.GetComponent<AnimationScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void runClearLine(int line)
    {
      currClearLineBlock = (GameObject)Instantiate(Resources.Load(
      "Clear_Anim_Example", typeof(GameObject)), new Vector3(0.0f, (float)line, -1.0f), Quaternion.identity);
      clearLineAnimator = currClearLineBlock.GetComponent<Animator>();
    }

    public void levelUp()
    {

    }
}
