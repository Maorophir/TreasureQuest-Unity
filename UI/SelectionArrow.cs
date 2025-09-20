using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound; //Arrow movement sound
    [SerializeField] private AudioClip interactSound; // Selection sound
    private RectTransform rect;
    private int currentPosition;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    private void Update()
    {   
        // Changes arrow's position
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1);   
        
        // Interact with options
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
             Interact();
    }

    private void ChangePosition(int _change)
    {   
        currentPosition += _change;
        if (_change != 0)
            SoundManager.instance.PlaySound(changeSound);
        

        if (currentPosition < 0)
            currentPosition = options.Length -1;
        else if (currentPosition > options.Length -1)
            currentPosition = 0;

        // Assign the Y position of the current option to the arrow
        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
    }

    private void Interact()
    {
        SoundManager.instance.PlaySound(interactSound);

        // Access the button component on each option and call it's function
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
