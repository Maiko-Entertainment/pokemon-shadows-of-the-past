using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Save",
    "Set Safe Spawn to Current",
    "Set players safe spawn in save file."
)]
public class FungusSetSafeSpawnToCurrent : Command
{

    public override void OnEnter()
    {
        WorldMap map = WorldMapMaster.GetInstance().GetCurrentMap();
        SaveMaster.Instance.activeSaveFile.lastSafeZoneMapId = map.mapId;
        SaveMaster.Instance.activeSaveFile.lastSafeZoneIndex = map.defaultSafePlacePosIndex;
        Continue();
        base.OnEnter();
    }
}
