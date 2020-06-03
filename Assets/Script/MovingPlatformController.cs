using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MovingPlatformController : MonoBehaviour
{

    GameObject[] goingtoPointArray;
    bool takeTheDistanceOnce = true;
    int distanceCount = 0;
    bool comeBack = true;
    
    Vector3 distancePoint;
    // Start is called before the first frame update
    void Start()
    {
       goingtoPointArray = new GameObject[transform.childCount];

        for(int i=0; i<goingtoPointArray.Length; i++)
        {
            goingtoPointArray[i] = transform.GetChild(0).gameObject;
            goingtoPointArray[i].transform.SetParent(transform.parent);
        }
    }
    void FixedUpdate()
    {
        goToPoint();
    }

    void goToPoint()
    {
        if (takeTheDistanceOnce)
        {
            distancePoint = (goingtoPointArray[distanceCount].transform.position - transform.position).normalized;
            takeTheDistanceOnce = false;
        }
        float distance = Vector3.Distance(goingtoPointArray[distanceCount].transform.position,transform.position);
        transform.position += distancePoint * Time.deltaTime * 8;

        if (distance < 0.5f)
        {
            takeTheDistanceOnce = true;

            if(distanceCount == goingtoPointArray.Length - 1)
            {
                comeBack = false;
            }
            else if(distanceCount == 0)
            {
                comeBack = true;
            }

            if (comeBack)
            {
                distanceCount++;
            }
            else
            {
                distanceCount--;
            }
        }
        
        
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount ; i++) 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);
        }
        for (int i = 0; i < transform.childCount-1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i+1).position);
        }
    }
#endif


}
#if UNITY_EDITOR
    [CustomEditor(typeof(MovingPlatformController))]
    [System.Serializable]
    
    class movidPlatformEditor : Editor
    {
        public override void OnInspectorGUI() //unity inspector tarafinda editoryel kismi acar.
        {
            MovingPlatformController script = (MovingPlatformController)target;

        if(GUILayout.Button("Add Point"))
        {
            GameObject newObject = new GameObject();
            newObject.transform.parent = script.transform; // platform nesnenim altinda nesneleri turetiyor.
            newObject.transform.position = script.transform.position;
            newObject.name = (script.transform.childCount).ToString(); //child kadar sayisi nesneme isim atar.
        }
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("platformSpeed"));

    }
    }


#endif