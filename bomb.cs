using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        switch (collider.tag)
        {
            case "ball":
                // Detonate ahead of position

                Debug.Log("BOOM");

                Vector3 objLocation = transform.position;
                objLocation.z += 10.0f;
                objLocation.x = Constants.CenterXPos;

                GameObject bombFx = Instantiate((GameObject)Resources.Load("BombExplosion"), objLocation, transform.rotation);

                Collider[] colliders = Physics.OverlapSphere(objLocation, Constants.BombBlastRadius);

                foreach (Collider nearbyObject in colliders)
                {
                    Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                    if (rb != null && 
                        nearbyObject.tag != "ball" && 
                        nearbyObject.tag != "genGround" &&
                        nearbyObject.tag != "interactive")
                    {
                        //rb.AddExplosionForce(Constants.BombExplosionForce, objLocation, Constants.BombBlastRadius);

                        Destroy(nearbyObject.gameObject);
                    }
                }

                Destroy(gameObject);
                Destroy(bombFx, 4); //Destroy after 5 seconds.
                break;
        }
    }
}
