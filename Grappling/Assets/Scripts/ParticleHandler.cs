using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    public void CreateParticleSystem(ParticleSystem ps, Vector2 pos)
    {
        Instantiate(ps, pos, Quaternion.identity, this.transform);
    }
}
