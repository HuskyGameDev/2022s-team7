using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLogic : MonoBehaviour {
    
    public Transform target;
    
private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
 
    private void Update() {
        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(target.position);
        targetScreenPos.z = 0;
        Vector3 targetToMouseDir = Input.mousePosition - targetScreenPos;
 
        Vector3 targetToMe = transform.position - target.position;
        targetToMe.z = 0;
 
        Vector3 newTargetToMe = Vector3.RotateTowards(targetToMe, targetToMouseDir, 3, 0f);
 
        transform.position = target.position + 3 *newTargetToMe.normalized;
    }
}