using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MaceControl : MonoBehaviour
{
    GameObject[] pointsToGo;
    bool takeTheDistanceOnce = true;
    bool ifForward = true;
    int distanceBetweenCount = 0;
    Vector3 betweenDistance;
    GameObject player;
    RaycastHit2D ray;
    public LayerMask layerMask;
    int speed = 5;
    public Sprite frontSide;
    public Sprite backSide;
    SpriteRenderer spriteRenderer;

    float fireTime = 0;
    public GameObject bullet;

    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        pointsToGo = new GameObject[transform.childCount];
        spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < pointsToGo.Length; i++)
        {
            pointsToGo[i] = transform.GetChild(0).gameObject;
            pointsToGo[i].transform.SetParent(transform.parent);

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DidSeeCharacter();
        if(ray.collider.tag == "Player")
        {
            speed = 8;
            spriteRenderer.sprite = frontSide;
            MaceFire();
        }
        else
        {
            speed = 4;
            spriteRenderer.sprite = backSide;
        }

        SawGoToPoints();
        
    }

    void MaceFire() {

        fireTime += Time.deltaTime;
        if(fireTime > Random.Range(0.2f, 1))
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            fireTime = 0;
        }
    }

    void DidSeeCharacter()
    {
        Vector3 rayYonum = player.transform.position - transform.position;
        ray = Physics2D.Raycast(transform.position, rayYonum, 1000,layerMask);
        Debug.DrawLine(transform.position,ray.point,Color.magenta);
    }


    void SawGoToPoints()
    {
        if (takeTheDistanceOnce)
        {
            betweenDistance = (pointsToGo[distanceBetweenCount].transform.position - transform.position).normalized;
            takeTheDistanceOnce = false;
        }
        float distance = Vector3.Distance(transform.position, pointsToGo[distanceBetweenCount].transform.position);
        transform.position += betweenDistance * Time.deltaTime * speed;
        if (distance < 0.5f)
        {

            takeTheDistanceOnce = true;

            if (ifForward)
            {
                distanceBetweenCount++;
            }
            else
            {
                distanceBetweenCount--;
            }
            if (distanceBetweenCount == pointsToGo.Length - 1)
            {
                ifForward = false;
            }
            else if (distanceBetweenCount == 0)
            {
                ifForward = true;
            }
        }

    }
    public Vector2 GetDirection()
    {
        return (player.transform.position - transform.position).normalized;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);
        }
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);
        }
    }
#endif


#if UNITY_EDITOR
    [CustomEditor(typeof(MaceControl))]
    [System.Serializable]

    class MaceControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MaceControl script = (MaceControl)target;

            if (GUILayout.Button("produce"))
            {
                GameObject newObject = new GameObject();
                newObject.transform.parent = script.transform;
                newObject.transform.position = script.transform.position;
                newObject.name = script.transform.childCount.ToString();
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("layerMask"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("frontSide"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("backSide"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("bullet"));

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }
#endif
}
