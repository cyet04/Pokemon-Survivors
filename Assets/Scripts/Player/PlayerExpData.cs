using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExpData 
{
    public float currentExp;
    public float expRequired;

    public PlayerExpData(float currentExp, float expRequired)
    {
        this.currentExp = currentExp;
        this.expRequired = expRequired;
    }
}

