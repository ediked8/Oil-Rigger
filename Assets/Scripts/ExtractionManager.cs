using UnityEngine;

public class ExtractionManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ParticleSystemRenderer particleSystemRenderer;
    MaterialPropertyBlock propblock;
    public ParticleSystem smokeVFX;
    public ParticleSystem fireVFX;

    private void Start()
    {
        propblock = new MaterialPropertyBlock();
        fireVFX = particleSystemRenderer.GetComponent<ParticleSystem>();
    }
    public void PlayEffect(int index)
    {
        particleSystemRenderer.GetPropertyBlock(propblock);
        switch(index)
        {
            case 0:
                smokeVFX.Stop();
                propblock.SetColor("_TintColor", new Color(0, 0, 0));
                fireVFX.Play();
                break;
            case 1:
                smokeVFX.Stop();
                fireVFX.Play();
                propblock.SetColor("_TintColor", new Color(1, 1, 1));
               
                break;
            case 2:
                smokeVFX.Stop();
                fireVFX.Play();
                propblock.SetColor("_TintColor", new Color(0, 1, 0));
                break;
            case 3:
                smokeVFX.Stop();
                fireVFX.Play();
                propblock.SetColor("_TintColor", new Color(1, 0, 0));
                break;

            case 4:
                fireVFX.Stop();
                smokeVFX.Play();
                break;


        }
        particleSystemRenderer.SetPropertyBlock(propblock);
        


    }



}
