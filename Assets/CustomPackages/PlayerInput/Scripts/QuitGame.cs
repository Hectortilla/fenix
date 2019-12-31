using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour {
    void Update() {
        if (Input.GetKey("p")) {
            Application.Quit();
        }

		if(Input.GetKeyDown(KeyCode.Escape)){

			Cursor.visible = !Cursor.visible;
			if(!Cursor.visible){
				Cursor.lockState = CursorLockMode.Locked;
			} else {
				Cursor.lockState = CursorLockMode.None;
			}
		}
    }
}
