using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int width = 10, height = 10;
    public Consrtuctions CellPrefab;
    public GameObject spawner;
    Room map;
    Level lvl;
    private void Start()
    {
        lvl = GetComponent<Level>();
        SpawnRoom();
    }
    void SpawnRoom()
    {
        Generator generator = gameObject.GetComponent<Generator>();

        map = generator.GenerateRoom(width, height, 0, 0);

        for (int i = 0; i < map.room.GetLength(0); ++i)
            for (int j = 0; j < map.room.GetLength(1); ++j)
            {
                Consrtuctions cell = Instantiate(CellPrefab, new Vector3(j + map.PosX * lvl.roomSpaceW, i + map.PosY * lvl.roomSpaceH, 0), Quaternion.identity);
                cell.LWall.SetActive(map.room[i, j].lW);
                cell.RWall.SetActive(map.room[i, j].rW);
                cell.UWall.SetActive(map.room[i, j].uW);
                cell.DWall.SetActive(map.room[i, j].dW);
                cell.LUCorner.SetActive(map.room[i, j].lUC);
                cell.RUCorner.SetActive(map.room[i, j].rUC);
                cell.LDCorner.SetActive(map.room[i, j].lDC);
                cell.RDCorner.SetActive(map.room[i, j].rDC);

                cell.transform.parent = GameObject.Find("Room").transform;

                if ((i == 0 || i == height - 1) && (j == 0 || j == width - 1))
                {
                    GameObject sp =  Instantiate(spawner, new Vector3(j + 1, i - 1, 0), Quaternion.identity);

                    sp.transform.parent = GameObject.Find("Room").transform;
                }
            }
    }

}
