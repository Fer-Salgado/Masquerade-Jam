using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;


public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundry;
    [SerializeField] Direction direction;
    [SerializeField] Transform teleportTargetPosition;
    CinemachineConfiner2D confiner2D;

    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Teleport
    }

    private void Awake()
    {
        confiner2D = FindObjectOfType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.CompareTag("Player"))
        {
           confiner2D.BoundingShape2D = mapBoundry;
           UpdatePlayerPosition(collision.gameObject);
        }
        

    }

    void UpdatePlayerPosition(GameObject player)
    {
        if(direction == Direction.Teleport)
        {
            player.transform.position = teleportTargetPosition.position;
            return;
        }

        Vector3 additivePos = player.transform.position;
        switch (direction)
        {
            case Direction.Up:
                additivePos.y += 2;
                break;
            case Direction.Down:
                additivePos.y -= 1;
                break;
            case Direction.Left:
                additivePos.x -= -2;
                break;
            case Direction.Right:
                additivePos.x += 2;
                break;

        }
        player.transform.position = additivePos;
    }
}
