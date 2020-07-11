using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{ 
    public List<NPC> NPCs { get; private set; }

    bool isFirstFrame;

    // Start is called before the first frame update
    void Awake()
    {
        NPCs = new List<NPC>();
        isFirstFrame = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFirstFrame)
        {
            isFirstFrame = false;

            SetRandomInfectedNPC();
        }
    }

    public void RegisterNPC(NPC a_NPC)
    {
        NPCs.Add(a_NPC);
    }

    public void UnregisterNPC(NPC a_NPC)
    {
        NPCs.Remove(a_NPC);
    }

    public bool AreThereAnyInfectedNPCs()
    {
        foreach (NPC npc in NPCs)
        {
            if (npc.isInfected)
                return true;
        }

        return false;
    }

    void SetRandomInfectedNPC()
    {
        int randomNPCIndex = Random.Range(0, NPCs.Count);
        NPCs[randomNPCIndex].OnInfected();
    }
}
