using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    public delegate void TowerPlaced();
    public event TowerPlaced towerPlaced;
    public bool placing { get; private set; }
    public bool canPlace = true;
    public bool stoppedClicking = false;
    private void Start(){
        placing = true;
    }
    private void Update()
    {
        if(!Input.GetMouseButton(0)){
            stoppedClicking = true;
        }
        if (placing)
        {
            Vector2 position = new Vector3(Mathf.Floor(InputManager.Instance.mousePosition.x + 0.5f), Mathf.Floor(InputManager.Instance.mousePosition.y + 0.5f), 0);
            transform.position = position;
            bool collide = false;

            List<Vector2> pathList = new List<Vector2>();
            foreach (Vector3 point3d in Path.Instance.path)
            {
                pathList.Add(new Vector3(point3d.x, point3d.y, 0));
            }
            Vector2[] path = pathList.ToArray();

            for (int i = 0; i < path.Length - 1; i++)
            {
                bool xCollide = false;
                if (path[i].x < path[i + 1].x)
                {
                    if (path[i].x <= position.x && path[i + 1].x >= position.x)
                    {
                        xCollide = true;
                    }
                }
                else
                {
                    if (path[i + 1].x <= position.x && path[i].x >= position.x)
                    {
                        xCollide = true;
                    }
                }

                bool yCollide = false;
                if (path[i].y < path[i + 1].y)
                {
                    if (path[i].y <= position.y && path[i + 1].y >= position.y)
                    {
                        yCollide = true;
                    }
                }
                else
                {
                    if (path[i + 1].y <= position.y && path[i].y >= position.y)
                    {
                        yCollide = true;
                    }
                }
                if (xCollide && yCollide)
                    collide = true;
            }
            canPlace = !collide;
            foreach(GameObject x in GameObject.FindGameObjectsWithTag("tower")){
                if(x.transform.position == transform.position && x != gameObject){
                    canPlace = false;
                }
            }
            if (Input.GetMouseButtonDown(0) && canPlace && stoppedClicking)
            {
                placing = false;
                towerPlaced?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(gameObject);
            }

            

        }

    }

    


}
