using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Monster : MonoBehaviour {

    public int health;
    int nextWaypointIndex = 0;
    public float Speed;

	// Use this for initialization
	void Start () {
 
	}
	
	// Update is called once per frame
	void Update () {

        if (Vector2.Distance(transform.position,
            GameManager.Instance.waypoints[nextWaypointIndex].transform.position) < 0.01f) {

            //is this waypoint the last one?
            if (nextWaypointIndex == GameManager.Instance.waypoints.Length - 1) {
                RemoveAndDestroy();
                GameManager.Instance.doDamage();
            } else {
                //next waypoint
                nextWaypointIndex++;
                //turn to waypoint
                transform.LookAt(GameManager.Instance.waypoints[nextWaypointIndex].transform.position,
                    -Vector3.forward);
                //only in the z axis
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
            }
        }

        //move
        transform.position = Vector2.MoveTowards(transform.position,
            GameManager.Instance.waypoints[nextWaypointIndex].transform.position,
            Time.deltaTime * Speed);

    }

    void RemoveAndDestroy() {
        //remove it from the enemy list
        GameManager.Instance.monsterList.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Arrow") {//if we're hit by an arrow
            Debug.Log("I got hit! D:");
            if (health > 0) {
                //decrease enemy health
                health -= Arrow.damage;
                if (health <= 0) {
                    RemoveAndDestroy();
                    Debug.Log("I got killed! :'-(");
                }
            }
            col.gameObject.GetComponent<Arrow>().Disable(); //disable the arrow
        }
    }
}
