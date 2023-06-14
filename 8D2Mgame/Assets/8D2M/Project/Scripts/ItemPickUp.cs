using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{

    [Header("Keybinds")]
    public KeyCode Grab = KeyCode.E;
    public KeyCode Drop = KeyCode.Q;

    [Header("Objects")]
    public GameObject Item;
    public Transform ItemHolder;

    public GameObject openText;
    public AudioSource pickUpSound;
    public bool inReach;

    // Start is called before the first frame update
    void Start()
    {
        // Stops item from having gravity.
        Item.GetComponent<Rigidbody>().isKinematic = false;
        inReach = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(Drop))
        {
            DropItem();
        }
    }

    private void DropItem()
    {
        //Drops item
        ItemHolder.DetachChildren();
        // Gives item new possition when droppped 
        Item.transform.eulerAngles = new Vector3(Item.transform.position.x, Item.transform.position.z, Item.transform.position.y);
        // re-enables gravity to allow item fall drop
        Item.GetComponent<Rigidbody>().isKinematic = false;
        Item.GetComponent<MeshCollider>().enabled = true;
    }

    private void GrabItem()
    {
        Item.GetComponent<Rigidbody>().isKinematic = true;

        Item.transform.position = ItemHolder.transform.position;
        Item.transform.rotation = ItemHolder.transform.rotation;
        // Disables collider on item to prevent collision with player
        Item.GetComponent<MeshCollider>().enabled = false;

        Item.transform.SetParent(ItemHolder);
        pickUpSound.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            if (inReach && Input.GetKey(Grab))
            {
                GrabItem();
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
