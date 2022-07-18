using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchDetector : MonoBehaviour
{
    [SerializeField] private List<GameObject> CubesInTrigger = new List<GameObject>();

    [SerializeField] private GameObject ParentCube;

    private enum TriggersType{
        PositiveX, NegativeX, PositiveY, NegativeY
    }

    [SerializeField] private TriggersType TriggerType;

    private void Start() => CubesInTrigger.Add(ParentCube);

    private bool HitFlag = false;

    private float distanceDetection = 0.1f;
    
    [SerializeField] private bool Test = false;

    private void FixedUpdate(){
        if(!CubesInTrigger.Contains(ParentCube.gameObject)){
            CubesInTrigger.Add(ParentCube.gameObject);
        }

        switch(TriggerType){
            case TriggersType.PositiveX:
            RaycastDetect(transform.position, transform.right * distanceDetection);
            break;

            case TriggersType.NegativeX:
            RaycastDetect(transform.position, -transform.right * distanceDetection);
            break;

            case TriggersType.PositiveY:
            RaycastDetect(transform.position, transform.up * distanceDetection);
            break;

            case TriggersType.NegativeY:
            RaycastDetect(transform.position, -transform.up * distanceDetection);
            break;
        }

        if(CubesInTrigger.Count == 3){
            for (int i = 0; i < CubesInTrigger.Count; i++)
            {
                if(CubesInTrigger[i].gameObject != null){
                    Destroy(CubesInTrigger[i].gameObject, 2f);
                }
            }
            CubesInTrigger.Capacity = 0;
        }
    }

    protected void RaycastDetect(Vector3 origin, Vector3 direction){
        if(GetComponentInParent<Cube>().CubeCanBeDetected){
            Debug.DrawRay(origin, direction * 1, Color.blue, 1.3f);
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, 1.3f);

            for (int i = 0; i < hits.Length; i++)
            {
                if(hits[i].collider.tag != "Bottom" && hits[i].collider.GetComponent<Cube>().CubeCanBeDetected){ // Sorry for this bullshit, i didn't have any time
                        if(hits[i]){
                        HitFlag = true;
                        if(hits[i].collider.tag == ParentCube.tag && !CubesInTrigger.Contains(hits[i].collider.gameObject)){
                            if(hits[i].collider.gameObject != ParentCube){ 
                                CubesInTrigger.Add(hits[i].collider.gameObject);
                            }
                        }
                    }   
                    else if(!hits[i] && HitFlag){
                        HitFlag = false;
                        if(!HitFlag){
                            if(CubesInTrigger.Contains(hits[i].collider.gameObject)){
                                CubesInTrigger.Capacity--;
                            }
                        }
                    }
                }
            }
        }
    }  
}

