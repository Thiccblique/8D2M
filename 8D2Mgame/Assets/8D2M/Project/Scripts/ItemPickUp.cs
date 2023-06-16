using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{

    [Header("Keybinds")]
    public KeyCode grabKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;

    [Header("Objects")]
    public GameObject Item;
    public Transform ItemHolder;

    public GameObject openText;
    public AudioSource pickUpSound;
    public bool inReach;

    // Start is called before the first frame update
    void Start()
    {
        
        inReach = false;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void DropItem()
    {
        //Removes Item as a child
        ItemHolder.DetachChildren();
        // Gives item new possition when droppped 
        Item.transform.eulerAngles = new Vector3(Item.transform.position.x, Item.transform.position.z, Item.transform.position.y);
        // re-enables gravity and colider to allow item fall drop
        Item.GetComponent<Rigidbody>().isKinematic = false;
        Item.GetComponent<MeshCollider>().enabled = true;
    }

    private void GrabItem()
    {
        // Disables gravity 
        Item.GetComponent<Rigidbody>().isKinematic = true;
        // Sets item position and rotation to be the same as ItemHolder
        Item.transform.position = ItemHolder.transform.position;
        Item.transform.rotation = ItemHolder.transform.rotation;

        // Disables collider on item to prevent collision with player
        Item.GetComponent<MeshCollider>().enabled = false;

        // Item transforms position abd becones a child to the ItemHolder
        Item.transform.SetParent(ItemHolder);
        pickUpSound.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        // If reach took is in reach of item and you press Grab key, you run Grab function 
        if (other.gameObject.tag == "Reach")
        {
            if (inReach && Input.GetKey(grabKey))
            {
                GrabItem();
            }
        }
        if (other.gameObject.tag == "Dropper")
        {
            if (Input.GetKey(dropKey))
            {
                DropItem();
            }
        }
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
}
