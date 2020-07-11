using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class NPCManager : MonoBehaviour
{ 
    public List<NPC> NPCs { get; private set; }

    bool isFirstFrame;

	public UnityEvent OnLastInfectedDied;

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

		if (!AreThereAnyInfectedNPCs()) {
			OnLastInfectedDied?.Invoke();
		}
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

    void SetRandomInfectedNPC(int a_NumNPCsToInfect = 1)
    {
        for (int i = 0; i < a_NumNPCsToInfect; i++)
        {
            int randomNPCIndex = Random.Range(0, NPCs.Count);

            if (NPCs[randomNPCIndex].isInfected)
                i--;
            else
                NPCs[randomNPCIndex].OnInfected();
        }
    }
}
