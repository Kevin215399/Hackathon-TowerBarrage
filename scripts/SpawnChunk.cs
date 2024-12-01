using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SpawnChunk
{
    public int enemyIndex = 0;
    public int enemyAmount = 0;
    public float delay = 1;
    public SpawnChunk(int enemyIndex, int enemyAmount, int delay){
        this.enemyIndex = enemyIndex;
        this.enemyAmount = enemyAmount;
        this.delay = delay;
    }
}
