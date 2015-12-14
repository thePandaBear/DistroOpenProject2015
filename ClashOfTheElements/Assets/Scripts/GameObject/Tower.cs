using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Linq;

public class Tower : MonoBehaviour {
	
	public GameObject arrowPrefab;
	public float fireRate = 2f;
	private float lastFire = 0f;
	GameObject target;
    private TowerState State;
    public float range;

    void Start () {
	
		State = TowerState.Searching;
	}

	void Update () {

        /*//if we're in the last round and we've killed all monsters, do nothing
        if (GameManager.Instance.finalRoundFinished &&
            GameManager.Instance.monsters.Where(x => x != null).Count() == 0)
            State = TowerState.Inactive;*/

        //searching for an enemy
        if (State == TowerState.Searching) {

            //no enemies left?
            if (GameManager.Instance.monsters.Where(x => x != null).Count() == 0) return;

            //find the closest enemy
            //aggregate method proposed here
            //http://unitygems.com/linq-1-time-linq/
            target = GameManager.Instance.monsters.Where(x => x != null)
           .Aggregate((current, next) => Vector2.Distance(current.transform.position, transform.position)
               < Vector2.Distance(next.transform.position, transform.position)
              ? current : next);

            //if there is an enemy and is close to us, target it
            if (target != null && target.activeSelf
                && Vector3.Distance(transform.position, target.transform.position)
                < range) {
                State = TowerState.Targeting;
            }

        } else if (State == TowerState.Targeting) {
            //if the targeted enemy is still close to us, look at it and shoot!
            if (target != null
                && Vector3.Distance(transform.position, target.transform.position)
                    < range) {
                Shoot();
            } else //enemy has left our shooting range, so look for another one
              {
                State = TowerState.Searching;
            }
        }
    }

    private void Shoot() {

        Vector2 direction = target.transform.position - transform.position;
        //if the enemy is still close to us
        if (Time.time - lastFire > fireRate) {

            if (target != null && target.activeSelf
            && Vector3.Distance(transform.position, target.transform.position)
                    < range) {
                //create a new arrow
                GameObject go = ObjectPoolerManager.Instance.ArrowPooler.GetPooledObject();
                go.transform.position = transform.position;
                go.transform.rotation = transform.rotation;

				//Aim and shoot
				go.GetComponent<Arrow>().setTarget(target.transform);
                go.SetActive(true);

            } else//find another enemy
              {
                State = TowerState.Searching;
            }

            lastFire = Time.time;
        }
    }
}
