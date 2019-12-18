using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CameraEdit
{
    public class MoveForward : MonoBehaviour
    {
        public float MovementSpeed = 1;
        
        
        void Update()
        {
            transform.Translate(Vector3.forward * MovementSpeed / 2);
        }
    }
}
