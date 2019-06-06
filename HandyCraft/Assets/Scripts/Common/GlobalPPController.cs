using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GlobalPPController : MonoBehaviour
{
    private PostProcessVolume volume;
    private Vignette vignette;

    private int playerMaxHP;

    public float maxVignetteIntensity;

    private void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out vignette);
        vignette.enabled.value = true;
    }

    public void BindPlayer(CharacterInfo info)
    {
        playerMaxHP = info.GetMaxHp();
        info.OnHpChange += OnPlayerHPChange;
        info.OnDie += OnPlayerDie;
    }

    private void OnPlayerHPChange(int hp)
    {
        vignette.intensity.value = (1f - (float)hp / playerMaxHP) * maxVignetteIntensity;
    }

    private void OnPlayerDie()
    {
        //do something
    }
}
