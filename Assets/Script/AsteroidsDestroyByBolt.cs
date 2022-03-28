using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsDestroyByBolt : MonoBehaviour
{

    [Tooltip("\"Fractured\" is the object that this will break into")]
    public GameObject Explosion;

    public int BoltMaxCount;
    private int BoltCount = 0;

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        BoltCount++;
        GameObject temp = Instantiate(Explosion, transform.position, transform.rotation); //Spawn in the broken version
        temp.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        Destroy(temp, 1f);
        if (this.BoltCount > this.BoltMaxCount){

            Destroy(gameObject,0.5f);
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
    private void OnDestroy()
    {

    }
}
