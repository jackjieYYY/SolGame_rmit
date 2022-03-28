using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceBModelController : MonoBehaviour
{
    public GameObject AwakeExplosion;

    private void Awake()
    {
        var awakeExplosion = Instantiate(AwakeExplosion, transform.position, transform.rotation);
        Destroy(awakeExplosion, 2f);
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
