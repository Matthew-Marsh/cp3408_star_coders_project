using System.Collections.Generic;
using UnityEngine;

// Script to use if works by collison on the floor tag with Grass, Water or Stone
public class FootstepManager : MonoBehaviour
{
    public List<AudioClip> grassSteps = new List<AudioClip>();
    public List<AudioClip> waterSteps = new List<AudioClip>();
    public List<AudioClip> stoneSteps = new List<AudioClip>();

    private enum Surface { grass, water, stone };
    private Surface surface;

    private List<AudioClip> currentList;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayStep()
    {
        if (currentList == null)
            return;

        AudioClip clip = currentList[Random.Range(0, currentList.Count)];
        source.PlayOneShot(clip);
    }

    private void SelectStepList()
    {
        switch (surface)
        {
            case Surface.grass:
                currentList = grassSteps;
                break;
            case Surface.water:
                currentList = waterSteps;
                break;
            case Surface.stone:
                currentList = stoneSteps;
                break;
            default:
                currentList = null;
                break;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Grass")
        {
            surface = Surface.grass;
        }

        if (hit.transform.tag == "Water")
        {
            surface = Surface.water;
        }

        if (hit.transform.tag == "Stone")
        {
            surface = Surface.stone;
        }

        SelectStepList();

    }

}