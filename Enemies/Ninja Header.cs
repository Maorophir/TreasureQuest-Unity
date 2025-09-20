using UnityEngine;

public class NinjaHeader : MonoBehaviour
{
    [SerializeField] private Transform textTransform; // Assign your text transform here

    private void Update()
    {
        // Check if the enemy is flipped (scale.x is -1)
        if (transform.localScale.x < 0)
        {
            // If the enemy is flipped, make sure the text is not
            textTransform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // Otherwise, ensure the text matches the normal scale
            textTransform.localScale = new Vector3(1, 1, 1);
        }
    }
}
