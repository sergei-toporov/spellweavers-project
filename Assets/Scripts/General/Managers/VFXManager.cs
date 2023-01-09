using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellweavers
{
    public class VFXManager : MonoBehaviour
    {
        protected static VFXManager manager;
        public static VFXManager Manager { get => manager; }

        [SerializeField] protected ParticleVFXCollectionSO particleVFXCollection;
        public ParticleVFXCollectionSO ParticleVFXCollection { get => particleVFXCollection; }

        protected void Awake()
        {
            if (manager != this && manager != null)
            {
                Destroy(this);
            }
            else
            {
                manager = this;
            }
        }
    }
}

