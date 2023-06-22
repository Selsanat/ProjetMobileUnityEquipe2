using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class savingData
{
    public int lifeArbo;
    public int lifePretre;
    public int waveCount;
    public int levelArbo;
    public int levelPretre;
    public int expArbo;
    public int expPretre;
    public bool isPretrePlayed;
    public bool isArboPlayed;
    public bool needToResetMap;

    public savingData(int lifeArbo, int lifePretre, int waveCount, int levelArbo, int levelPretre, int expArbo, int expPretre, bool isPretrePlayed, bool isArboPlayed, bool needToResetMap)
    {
        Debug.Log(lifeArbo + " " + lifePretre + " " + waveCount + " " + levelArbo + " " + levelPretre + " " + expArbo + " " + expPretre + " " + isPretrePlayed + " " + isArboPlayed);
        this.lifeArbo = lifeArbo;
        this.lifePretre = lifePretre;
        this.waveCount = waveCount;
        this.levelArbo = levelArbo;
        this.levelPretre = levelPretre;
        this.expArbo = expArbo;
        this.expPretre = expPretre;
        this.isPretrePlayed = isPretrePlayed;
        this.isArboPlayed = isArboPlayed;
        this.needToResetMap = needToResetMap;
    }
}
