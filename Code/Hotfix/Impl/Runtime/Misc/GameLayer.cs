using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameLayer
{
    public static string Layer_Str_Default = "Default";
    public static string Layer_Str_NGUI = "NGUI";
    public static string Layer_Str_SpecialText = "SpecialText";
    public static string Layer_Str_Scene = "Scene";
    public static string Layer_Str_SpecialItem = "SpecialItem";
    public static string Layer_Str_Monster = "Monster";
    public static string Layer_Str_LocalPlayer = "LocalPlayer";
    public static string Layer_Str_Player = "Player";
    public static string Layer_Str_Npc = "Npc";
    public static string Layer_Str_Pet = "Pet";
    public static string Layer_Str_Guide = "Guide";
    public static string Layer_Str_TopName = "TopName";
    public static string Layer_Str_Map = "Map";
    public static string Layer_Str_3dPreView = "3dPreView";
    public static string Layer_Str_BlockExceptPet = "BlockExceptPet";
    public static string Layer_Str_CG = "CG";
    public static string Layer_Str_CG_UI = "CG_UI";
    public static string Layer_Str_System = "System";
    public static string Layer_Str_CG_Top = "CG_Top";
    public static string Layer_Str_BlockExTeamA = "BlockExTeamA";
    public static string Layer_Str_BlockExTeamB = "BlockExTeamA";
    public static string Layer_Str_BlockExTeamC = "BlockExTeamA";
    public static string Layer_Str_BlockOnlyMonster = "BlockExTeamA";

    public static int Layer_Default = LayerMask.NameToLayer(Layer_Str_Default);
    public static int Layer_NGUI = LayerMask.NameToLayer(Layer_Str_NGUI);
    public static int Layer_SpecialText = LayerMask.NameToLayer(Layer_Str_SpecialText);
    public static int Layer_Scene = LayerMask.NameToLayer(Layer_Str_Scene);
    public static int Layer_SpecialItem = LayerMask.NameToLayer(Layer_Str_SpecialItem);
    public static int Layer_Monster = LayerMask.NameToLayer(Layer_Str_Monster);
    public static int Layer_LocalPlayer = LayerMask.NameToLayer(Layer_Str_LocalPlayer);
    public static int Layer_Player = LayerMask.NameToLayer(Layer_Str_Player);
    public static int Layer_Npc = LayerMask.NameToLayer(Layer_Str_Npc);
    public static int Layer_Pet = LayerMask.NameToLayer(Layer_Str_Pet);
    public static int Layer_Guide = LayerMask.NameToLayer(Layer_Str_Guide);
    public static int Layer_TopName = LayerMask.NameToLayer(Layer_Str_TopName);
    public static int Layer_Map = LayerMask.NameToLayer(Layer_Str_Map);
    public static int Layer_3dPreView = LayerMask.NameToLayer(Layer_Str_3dPreView);
    public static int Layer_BlockExceptPet = LayerMask.NameToLayer(Layer_Str_BlockExceptPet);
    public static int Layer_CG = LayerMask.NameToLayer(Layer_Str_CG);
    public static int Layer_CG_UI = LayerMask.NameToLayer(Layer_Str_CG_UI);
    public static int Layer_System = LayerMask.NameToLayer(Layer_Str_System);
    public static int Layer_CG_Top = LayerMask.NameToLayer(Layer_Str_CG_Top);
    public static int Layer_BlockExTeamA = LayerMask.NameToLayer(Layer_Str_BlockExTeamA);
    public static int Layer_BlockExTeamB = LayerMask.NameToLayer(Layer_Str_BlockExTeamB);
    public static int Layer_BlockExTeamC = LayerMask.NameToLayer(Layer_Str_BlockExTeamC);
    public static int Layer_BlockOnlyMonster = LayerMask.NameToLayer(Layer_Str_BlockOnlyMonster);

    public static int Mask_Nothing = 0;
    public static int Mask_Default = GameLayer.GetMask(Layer_Default);
    public static int Mask_NGUI = GameLayer.GetMask(Layer_NGUI);
    public static int Mask_SpecialText = GameLayer.GetMask(Layer_SpecialText);
    public static int Mask_Scene = GameLayer.GetMask(Layer_Scene);
    public static int Mask_SpecialItem = GameLayer.GetMask(Layer_SpecialItem);
    public static int Mask_Monster = GameLayer.GetMask(Layer_Monster);
    public static int Mask_LocalPlayer = GameLayer.GetMask(Layer_LocalPlayer);
    public static int Mask_Player = GameLayer.GetMask(Layer_Player);
    public static int Mask_Npc = GameLayer.GetMask(Layer_Npc);
    public static int Mask_Pet = GameLayer.GetMask(Layer_Pet);
    public static int Mask_Guide = GameLayer.GetMask(Layer_Guide);
    public static int Mask_TopName = GameLayer.GetMask(Layer_TopName);
    public static int Mask_Map = GameLayer.GetMask(Layer_Map);
    public static int Mask_3dPreView = GameLayer.GetMask(Layer_3dPreView);
    public static int Mask_BlockExceptPet = GameLayer.GetMask(Layer_BlockExceptPet);
    public static int Mask_CG = GameLayer.GetMask(Layer_CG);
    public static int Mask_CG_UI = GameLayer.GetMask(Layer_CG_UI);
    public static int Mask_System = GameLayer.GetMask(Layer_System);
    public static int Mask_CG_Top = GetMask(Layer_CG_Top);
    public static int Mask_BlockExTeamA = GetMask(Layer_BlockExTeamA);
    public static int Mask_BlockExTeamB = GetMask(Layer_BlockExTeamB);
    public static int Mask_BlockExTeamC = GetMask(Layer_BlockExTeamC);
    public static int Mask_BlockOnlyMonster = GetMask(Layer_BlockOnlyMonster);
    public static int Mask_UI = Mask_NGUI | Mask_Guide | Mask_TopName | Mask_3dPreView | Mask_CG_UI;

    public static int GetMask(int layer)
    {
        return 1 << layer;
    }
    
    public static int GetCombineMask(string[] layers)
    {
        string text = string.Empty;
        int num = 0;
        for (int i = 0; i < layers.Length; i++)
        {
            text = layers[i];
            int num2 = LayerMask.NameToLayer(text);
            num |= 1 << num2;
        }
        return num;
    }
}

