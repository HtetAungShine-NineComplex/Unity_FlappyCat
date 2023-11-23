using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //[SerializeField] private Transform start_Pos;
    [SerializeField] private Transform end_Pos;
    [SerializeField] private float speed;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, end_Pos.position, speed * Time.deltaTime);
    }
}
