using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationScript : MonoBehaviour
{
  private Animator animator;
  private Text newLevelText;
    // Start is called before the first frame update
    void Start()
    {
      animator = gameObject.GetComponent<Animator>();
      if (gameObject.tag == "Clear Line Anim")
      {
        animator.Play("Clear_Anim_Example");
        StartCoroutine(EndClearAnim());
      }
      else if (gameObject.tag == "New Level Text")
      {
        animator.Play("Level_Up");
        newLevelText = gameObject.GetComponent<Text>();
        newLevelText.text = "Level 0";
        Debug.Log("Destroy Game Object 1");
        StartCoroutine(EndLevelUpAnim());
      }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void LevelUp(int level) {
      newLevelText.text = "Level " + level;
      animator.speed = 1;
      StartCoroutine(EndLevelUpAnim());
    }

    IEnumerator EndClearAnim()
    {
      yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
      Destroy(gameObject);
    }

    IEnumerator EndLevelUpAnim()
    {
      Debug.Log("Destroy Game Object 2");
      yield return new WaitForSeconds(1.5f);
      Debug.Log("Destroy Game Object 3");
      animator.speed = 0;
    }



}
