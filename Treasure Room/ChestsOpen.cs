using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cainos.PixelArtPlatformer_VillageProps;
using Unity.VisualScripting;
using UnityEngine;

public class ChestsOpen : MonoBehaviour
{
    [SerializeField] GameObject[] chests;
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] gold;
    private Animator animator;
    private void Awake()
    {
        foreach (GameObject chest in chests)
        {
            animator = chest.GetComponent<Animator>();
            animator.SetBool("IsOpened", false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            foreach (GameObject chest in chests)
            {
                animator = chest.GetComponent<Animator>();
                animator.SetBool("IsOpened", true);
            }
            foreach(GameObject gold in gold)
            {
                gold.SetActive(true);
            }
            player.GetComponent<PlayerMovement_1>().CantMove();
            player.GetComponent<Animator>().SetBool("Win", true);
            StartCoroutine(EnableMovementAfterAnimation());
        }
    }

    private IEnumerator EnableMovementAfterAnimation()
    {
        // Wait for the animation to complete
        yield return new WaitForSeconds(1f);
        player.GetComponent<Animator>().SetBool("Win", false);
        // Re-enable movement
        player.GetComponent<PlayerMovement_1>().CanMove();
        gameObject.SetActive(false);
    }
}
