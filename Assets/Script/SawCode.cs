using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SawCode : MonoBehaviour
{
    GameObject[] pointsToGo;
    bool takeTheDistanceOnce = true;
    bool ifForward = true;
    int distanceBetweenCount = 0;
    Vector3 betweenDistance;
    void Start()
    {
        pointsToGo = new GameObject[transform.childCount];

        for(int i = 0; i < pointsToGo.Length ; i++)
        {
            pointsToGo[i] = transform.GetChild(0).gameObject;
            pointsToGo[i].transform.SetParent(transform.parent);

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 0, 5);
        SawGoToPoints();
    }

    void SawGoToPoints()
    {
        if(takeTheDistanceOnce)
        {
            betweenDistance = (pointsToGo[distanceBetweenCount].transform.position - transform.position).normalized;
            takeTheDistanceOnce = false;
        }
        float distance = Vector3.Distance(transform.position, pointsToGo[distanceBetweenCount].transform.position);
        transform.position += betweenDistance * Time.deltaTime * 8;
        if(distance < 0.5f)
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
           if(distanceBetweenCount == pointsToGo.Length - 1)
            {
                ifForward = false;
            }
            else if(distanceBetweenCount == 0)
            {
                ifForward = true;
            }
        }
       
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);
        }
        for(int i = 0; i < transform.childCount-1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);
        }
    }
#endif


#if UNITY_EDITOR
    [CustomEditor(typeof(SawCode))]
    [System.Serializable]

    class SawEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SawCode script = (SawCode)target;

            if (GUILayout.Button("produce"))
            {
                GameObject newObject = new GameObject();
                newObject.transform.parent = script.transform;
                newObject.transform.position = script.transform.position;
                newObject.name = script.transform.childCount.ToString();
            }
        }
    }
#endif
}
