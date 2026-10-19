using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class VFXSpawner : MonoBehaviour
{
    private  ObjectPool<ParticleSystem> vfxPool;

    public void SpawnVFX(ParticleSystem prefab, Vector2 position)
    {
        if (vfxPool == null)
        {
            vfxPool = CreatePool(prefab);
        }
        ParticleSystem vfx = vfxPool.Get();     
        vfx.transform.position = position;
        vfx.Play();
        StartCoroutine(DespawnTime(vfx, 1f));
    }
    IEnumerator DespawnTime(ParticleSystem vfx, float time)
    {
        yield return new WaitForSeconds(time);
        vfxPool.Release(vfx);        
    }
    private ObjectPool<ParticleSystem> CreatePool(ParticleSystem prefab)
    {
        ObjectPool<ParticleSystem> pool = null;
        pool = new ObjectPool<ParticleSystem>
        (
            () =>
            {
                ParticleSystem vfx = Instantiate(prefab, transform);
                vfx.gameObject.SetActive(true); 
                return vfx;
            },
            vfx => { },
            vfx => { },
            vfx => Destroy(vfx.gameObject),
            true, 
            50, 
            2000
        );
        return pool;
    }
}