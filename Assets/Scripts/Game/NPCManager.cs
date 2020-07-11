using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class NPCManager : MonoBehaviour
{ 
    public List<NPC> NPCs { get; private set; }
	
	public UnityEvent onLastInfectedDied;

    // Start is called before the first frame update
	private void Awake() {
		var levelManager = FindObjectOfType<LevelManager>();
	}

    public void RegisterNPC(NPC a_NPC)
    {
        NPCs.Add(a_NPC);
    }

    public void UnregisterNPC(NPC a_NPC)
    {
        NPCs.Remove(a_NPC);

		if (!AreThereAnyInfectedNPCs()) {
			onLastInfectedDied?.Invoke();
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
	
	public void PreLevelLoad() {
		NPCs = new List<NPC>();
    }

    public void PostLevelLoad() {
		SetRandomInfectedNPC();
	}

    private void SetRandomInfectedNPC()
    {
        int randomNPCIndex = Random.Range(0, NPCs.Count);
        NPCs[randomNPCIndex].OnInfected();
    }
}
