using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceDroppables : MonoBehaviour
{
    [SerializeField] private GameObject HealthPot;
    [SerializeField] private GameObject ShieldPot;
    [SerializeField] private GameObject ShootSpeedUpgrade;
    [SerializeField] private GameObject MovementSpeedUpgrade;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This could drop nothing, or it could drop (health pots, shield pots, currency, etc...)
    public void dropDroppable(Vector3 position) {
        int randValue = Random.Range(1, 101);
        if (randValue <= 10) { 
            // 10% chance to get a Health pot
            Instantiate(HealthPot, position, Quaternion.identity);
        } else if (randValue <= 25) { 
            // 15% chance to get a Shield pot
            Instantiate(ShieldPot, position, Quaternion.identity);
        } else if (randValue <= 32) {
            // 7% chance to get a Shoot Speed Upgrade
            Instantiate(ShootSpeedUpgrade, position, Quaternion.identity);
        } else if (randValue <= 39) {
            // 7% chance to get a Movement Speed Upgrade
            Instantiate(MovementSpeedUpgrade, position, Quaternion.identity);
        }
    }
}
