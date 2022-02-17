using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{

    public float diss;

    internal bool Rotate(GameObject obj, Quaternion end, float speed)
    {
        float distance;
        if (obj.transform.parent != null) 
        {
            
            float sped = speed * Time.deltaTime;
            obj.transform.localRotation = Quaternion.Lerp(obj.transform.localRotation, end, sped);
            distance = Quaternion.Angle(obj.transform.localRotation, end);
        }
    else
        {
            
            float sped = speed * Time.deltaTime;
            obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, end, sped);
            distance = Quaternion.Angle(obj.transform.rotation, end);
        }
       
        if (distance < 1)
            return true;
        else
            return false;
    }
    internal void RotateAround(GameObject objToRotateAround, float Speed)
    {
        float sped = Speed * Time.deltaTime;
        objToRotateAround.transform.Rotate(new Vector3(0,sped, 0));
    }

    internal bool FollowOtherObject(GameObject followerObj, GameObject followedObj, float sped)
    {
        followerObj.transform.parent = followedObj.transform;
        Vector3 targetPos = new Vector3(0, 3, 0);
        float speed = sped * Time.deltaTime;
        followerObj.transform.localPosition = Vector3.Lerp(followerObj.transform.localPosition, targetPos, speed);

        float dis =  followerObj.transform.localPosition.y - targetPos.y;
        diss = dis;
        if (dis < 1)
            return true;
        else
            return false;
    }
    
}
