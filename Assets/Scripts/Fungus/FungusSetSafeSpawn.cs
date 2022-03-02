using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Save",
    "Set Safe Spawn",
    "Set players safe spawn in save file."
)]
public class FungusSetSafeSpawn : Command
{
    public int mapId = 0;
    public int safeZoneMapIndex = 0;

    public override void OnEnter()
    {
        SaveMaster.Instance.activeSaveFile.lastSafeZoneMapId = mapId;
        SaveMaster.Instance.activeSaveFile.lastSafeZoneIndex = safeZoneMapIndex;
        base.OnEnter();
    }
}
