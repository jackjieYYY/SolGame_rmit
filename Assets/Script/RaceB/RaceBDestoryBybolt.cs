using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceBDestoryBybolt : MonoBehaviour
{

    public GameObject Explosion;
    int BoltMaxCount = 2;
    int BoltCount = 0;

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        BoltCount++;
        if (this.BoltCount > this.BoltMaxCount)
        {
            var temp = Instantiate(Explosion, transform.position, transform.rotation); //Spawn in the broken version
            Destroy(temp, 2f);
            Destroy(gameObject);

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
