using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
  private GameObject currClearLineBlock;
  public Animator clearLineAnimator;
    // Start is called before the first frame update
    void Start()
    {
      clearLineAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void runClearLine(int line)
    {
      currClearLineBlock = (GameObject)Instantiate(Resources.Load(
      "Clear_Anim_Example", typeof(GameObject)), new Vector3(0.0f, (float)line, -1.0f), Quaternion.identity);
    }
}
