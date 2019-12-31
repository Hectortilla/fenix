using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour {
    Camera _camera;
    public Text indicator;
    private float distance = 4f;
    void Start(){
    	_camera = GetComponentInChildren<Camera>();
    	indicator.enabled = false;
    	InvokeRepeating("CheckForInteractableInFront", 0, 0.1f);
    }

    void Update(){
		if (Input.GetKeyDown(KeyCode.E)){
			InteractInFront();
		}
    }

    void CheckForInteractableInFront(){
        RaycastHit hit;
        // Debug.DrawRay(_camera.transform.position, _camera.transform.forward * distance);
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, distance)) {
            Interactable interactable = hit.transform.gameObject.GetComponentInParent<Interactable>();
            if (interactable != null){
    			indicator.enabled = true;
            } else {
            	indicator.enabled = false;
            }
        } else {
            indicator.enabled = false;
        }
    }

    void InteractInFront(){
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, distance)) {
            Interactable interactable = hit.transform.gameObject.GetComponentInParent<Interactable>();
            if (interactable != null){
    			interactable.Interact();
            }
        }
    }

}
