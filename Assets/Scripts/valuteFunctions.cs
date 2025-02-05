using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class valuteFunctions : MonoBehaviour
{
    public List<Animator> animator;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("coin"))
        {
            StartCoroutine(PlayAnimationAndWait(animator[0], "coinEffect", collision.gameObject));

        }
    }
    IEnumerator PlayAnimationAndWait(Animator animator, string animationName, GameObject coin)
    {
        animator.Play(animationName);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length - 0.5f);
        Destroy(coin);
    }

}
