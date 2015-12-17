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
    public GameManager gameManager;

    void Start () {
        gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
        State = TowerState.Searching;
	}

	void Update () {

        // check the state of the tower
        if (State == TowerState.Searching) {
            // tower is searching for an enemy
            
            // check if there are enemies on the map
            if (GameManager.Instance.monsterList.Where(x => x != null).Count() == 0) return;

            // set target to closest enemy
            target = GameManager.Instance.monsterList.Where(x => x != null).Aggregate((current, next) => Vector2.Distance(current.transform.position, transform.position) < Vector2.Distance(next.transform.position, transform.position) ? current : next);

            // check if the target is in range
            if (target != null && target.activeSelf && Vector3.Distance(transform.position, target.transform.position) < range + gameManager.rangeAdd) {
                // target is in range, set state to targeting
                State = TowerState.Targeting;
            }
        } else if (State == TowerState.Targeting) {
            // tower is targeting enemy

            // check if target is still in range
            if (target != null && Vector3.Distance(transform.position, target.transform.position) < range + gameManager.rangeAdd) {

                // enemy is in shooting range, execute attack
                attack();
            } else {
                // enemy is no more in shooting range, change state to searching again
                State = TowerState.Searching;
            }
        }
    }

    private void attack() {
        
        // check if the tower has reloaded already
        if (Time.time - lastFire > fireRate) {
            // tower is ready to shoot
            // check if target is still in range
            if (target != null && target.activeSelf && Vector3.Distance(transform.position, target.transform.position) < range + gameManager.rangeAdd) {
                // target is still in range
                // create a new arrow to shoot
                GameObject go = ObjectPoolerManager.Instance.ArrowPooler.GetPooledObject();
                go.transform.position = transform.position;
                go.transform.rotation = transform.rotation;

				// set direction of arrow towards target
				go.GetComponent<Arrow>().setTarget(target.transform);

                // activate arrow
                go.SetActive(true);
            } else {
                // target is no more in range, change state to searching again
                State = TowerState.Searching;
            }
            // update the last fired time
            lastFire = Time.time;
        }
    }
}
