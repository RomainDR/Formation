using UnityEngine;

public abstract class Collectible : GPEBase
{
    protected Vector3 Position { get; set; }
    protected Quaternion Rotation { get; set; }

    public void Reset()
    {
        
    }
}