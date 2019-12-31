using UnityEngine;
using System.Collections;

public class Item : Interactable {
    private Transform parentTransform;
    private Transform parentCameraTransform;

    [SerializeField]
    public Sprite icon;
    [SerializeField]
    public int price = 1;

    public string description = "Whatever";
    // Use this for initialization
    void Start () {
	
	}
    public void Pick(GameObject _parent) {
        parentTransform = _parent.transform;
        parentCameraTransform = _parent.GetComponentInChildren<Camera>().transform;

        _parent.GetComponent<Inventory>().AddItem(this);

        gameObject.SetActive(false);
    }

    public void Equip() {
        gameObject.SetActive(true);

        if (gameObject.GetComponent<Rigidbody>()) { 
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        gameObject.transform.parent = parentCameraTransform;
        gameObject.transform.position = parentCameraTransform.position + parentCameraTransform.forward + parentCameraTransform.right / 2 - parentCameraTransform.up / 3;
        gameObject.transform.rotation = parentCameraTransform.rotation;
        gameObject.SetActive(true);
    }

    public void Drop() {
        gameObject.transform.parent = null;
        transform.position = parentTransform.position + parentTransform.forward * 3;
        parentTransform = null;
        if (gameObject.GetComponent<Rigidbody>())
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        gameObject.SetActive(true);
    }
    public override void Interact() {
        Pick(GameObject.FindGameObjectsWithTag("Player")[0]);
    }
    public void Use() {
        parentTransform.GetComponent<Inventory>().DropItem(this);
    }

    void OnCollisionEnter (Collision col)
    {
        if(col.gameObject.tag == "vehicle")
        {
            DestroyItem();
        }
    }

    public void DestroyItem() {
        Destroy(gameObject, 0.5f);
    }
}
