using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
  private Animator clearLineAnimator;
    // Start is called before the first frame update
    void Start()
    {
      clearLineAnimator = gameObject.GetComponent<Animator>();
      clearLineAnimator.Play("Clear_Anim_Example");
      StartCoroutine(WaitForAnim());
    }
    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator WaitForAnim()
    {
      yield return new WaitForSeconds(clearLineAnimator.GetCurrentAnimatorStateInfo(0).length);
      Destroy(this.gameObject);
    }

}
