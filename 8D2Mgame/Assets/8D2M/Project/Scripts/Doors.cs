using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{

    [Header("Keybinds")]
    public KeyCode Interact = KeyCode.E;

    public Animator door;
    public GameObject openText;
    public AudioSource doorSound;
    public bool inReach;

    // Start is called before the first frame update
    void Start()
    {
        inReach = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // If we look at the object, text will apear 
        // Tag reach applys to the "Reach" object that is conected to the player cam, substituting arrays
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
            openText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If we look away from the object, text will disapear
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            openText.SetActive(false);
        }
    }
  
    // Update is called once per frame
    void Update()
    {
        // If the door is closed, the open animation plays, if not the closed animation plays
        if (inReach && Input.GetKeyDown(Interact))
        {
            DoorOpens();
        }
        else
        {
            DoorCloses();
        }
    }
   
    // If ran, it runs the "open" animation, and ignorse the closed animation 
    void DoorOpens()
    {
        Debug.Log("It Opens");
        door.SetBool("Open", true);
        door.SetBool("Closed", false);
        doorSound.Play();

    }
    // If ran, it runs the "closed" animation, and ignorse the open animation 
    void DoorCloses()
    {
        //Debug.Log("It Closes");
        door.SetBool("Open", false);
        door.SetBool("Closed", true);
    }
}
