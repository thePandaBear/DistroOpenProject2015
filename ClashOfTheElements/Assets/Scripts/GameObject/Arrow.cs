using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

    //Properties
	public static int damage = 1;
	public static float speed = 10;

	//The targeted monster
	private Transform target;
    
	    void Start() {
        //disable it after 5 seconds, whatever happens
        Invoke("Disable", 5f);
    }

	void FixedUpdate() {
		// Is the target still there?
		if (target) {
			// Fly towards the target 
			Vector2 dir = target.position - transform.position;
			GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
		} else {
			// Otherwise destroy self
			Disable();
		}
	}

    public void Disable() {
        //if we are called from another gameobject,
        //cancel the timed invoke
        CancelInvoke();
        //since we're pooling it, make it inactive instead of destroying it
        this.gameObject.SetActive(false);
    }

	public void setTarget(Transform t) {
		target = t;
	}
}
