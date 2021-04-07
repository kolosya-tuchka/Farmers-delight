using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public Room GenerateRoom(int width, int height, int PosX, int PosY)
    {
        Room room = new Room();
        Consrtuctions[,] map = new Consrtuctions[height, width];
        for (int i = 0; i < map.GetLength(0); ++i)
        {
            for (int j = 0; j < map.GetLength(1); ++j)
            {
                map[i, j] = new Consrtuctions();
                if (i == 0)
                {
                    if (j == 0) map[i, j].lDC = true;
                    else if (j == width - 1) map[i, j].rDC = true;
                    map[i, j].dW = true;
                }
                else if (i == height - 1)
                {
                    if (j == 0) map[i, j].lUC = true;
                    else if (j == width - 1) map[i, j].rUC = true;
                    map[i, j].uW = true;
                }
                if (j == 0) map[i, j].lW = true;
                else if (j == width - 1) map[i, j].rW = true;

            }
        }
        room.room = map;
        room.PosX = PosX;
        room.PosY = PosY;
        return room;
    }

    //public Consrtuctions[,] GenerateCorridor(Room map1, Room map2, int width, int height)
    //{
    //    if (map1.PosY == map2.PosY ^ map1.PosX == map2.PosX)
    //    {
    //        Consrtuctions[,] map = new Consrtuctions[,];
    //        int cor = map1.room.GetLength(map1.PosY == map2.PosY ? 0:1) / 2;
    //        for (int i = cor - 3; i < cor + 3; i++)
    //        {
    //            if (map1.PosY > map2.PosY)
    //            {
    //                map1.room[0, i].dW = false;
    //                map2.room[map2.room.GetLength(0) - 1, i].uW = false;
    //            }
    //            else if (map1.PosY < map2.PosY)
    //            {
    //                map2.room[0, i].dW = false;
    //                map1.room[map1.room.GetLength(0) - 1, i].uW = false;
    //            }
    //            else if (map1.PosX < map2.PosX)
    //            {
    //                map2.room[i, 0].lW = false;
    //                map1.room[i, map1.room.GetLength(1) - 1].rW = false;
    //            }
    //            else if (map1.PosX > map2.PosX)
    //            {
    //                map1.room[i, 0].lW = false;
    //                map2.room[i, map2.room.GetLength(1) - 1].rW = false;
    //            }
    //        }
    //        return null;
    //    }
    //    else return null;
    //}
}
