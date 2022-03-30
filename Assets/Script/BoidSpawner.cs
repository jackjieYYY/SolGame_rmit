using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{

    //BoidSpawner �ĵ���ģʽ��ֻ�������BoidSpawner��һ��ʵ�������Դ���ھ�̬����S��
    static public BoidSpawner S;

    //���ò���������Boid�������Ϊ
    public int numBoids = 100;                  //boid �ĸ���
    public GameObject boidPrefab;               //boid ��unity�е�Ԥ����
    public GameObject master;
    public float spawnRadius = 100f;            //ʵ���� boid ��λ�÷�Χ
    public float spawnVelcoty = 1f;            //boid ���ٶ�
    public float minVelocity = 0f;
    public float maxVelocity = 2f;
    public float nearDist = 1f;                //�ж�Ϊ������ boid ����С��Χֵ
    public float collisionDist = 0.5f;            //�ж�Ϊ����� boid ����С��Χֵ(������ײ����)
    public float velocityMatchingAmt = 0.01f;   //�� ������boid ��ƽ���ٶ� ����(Ӱ�����ٶ�)
    public float flockCenteringAmt = 0.15f;     //�� ������boid ��ƽ����ά��� ����(Ӱ�����ٶ�)
    public float collisionAvoidanceAmt = -0.5f; //�� �����boid ��ƽ����ά��� ����(Ӱ�����ٶ�)
    public float mouseAtrractionAmt = 0.01f;    //�� �������� ����ʱ��������� ����(Ӱ�����ٶ�)
    public float mouseAvoidanceAmt = 0.75f;     //�� �������� ��Сʱ��������� ����(Ӱ�����ٶ�)
    public float mouseAvoiddanceDsit = 15f;
    public float velocityLerpAmt = 0.25f;       //���Բ�ֵ���������ٶȵ� ����
    public bool ______________;

    public Vector3 mousePos;        //�����λ��


    // Start is called before the first frame update
    void Start()
    {
        //���õ�������SΪBoidSpawner�ĵ�ǰʵ��
        S = this;

        //��ʼ��NumBoids(��ǰΪ100)��Boids
        for (int i = 0; i < numBoids; i++)
            Instantiate(boidPrefab);
    }

    private void LateUpdate()
    {
        //��ȡ�����λ��
        Vector3 mousePos2d = new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.transform.position.y);

        //������ռ䵽��Ļ�ռ�任λ��
        mousePos = this.GetComponent<Camera>().ScreenToWorldPoint(mousePos2d);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
