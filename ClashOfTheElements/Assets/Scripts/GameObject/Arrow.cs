using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

    //Properties
	public static int damage = 1;
	public static float speed = 10;

	//The targeted monster
	private Transform target;
    
	    void Start() {
        //disable it after 5 seconds maximum
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
        //this might get called from another object
        CancelInvoke();
        //inactive due to pooler
        this.gameObject.SetActive(false);
    }

	public void setTarget(Transform t) {
		target = t;
	}
}
