using UnityEngine;

public readonly struct Constants
{
    public readonly static string SaveWorldPath = Application.persistentDataPath + "/worlds/";
    public readonly static string WorldFileExtension = ".world";
    public readonly static string WorldNamePattern = @"[^a-zA-Z\d\s]";

    public readonly struct ErrorMessages
    {
        public readonly static string InvalidName = "World names must not contain special characters.";
        public readonly static string EmptyName = "Worlds must have a name.";
        public readonly static string WorldAlreadyExists = "This world name is already in use.";
    }
    public readonly struct SuccessMessages
    {
        public readonly static string WorldSaved = "World saved successfully.";
    }

    public struct GenerationTypes
    {
        public const int NoType = 0;
        public const int Final = 1;
        public const int PerlinNoise = 2;
        public const int CelularAutomata = 3;
        public const int Templates = 4;
        public const int RandomGeneration = 5;
    }

    public struct WorldSizes
    {
        public static readonly int[] Small = { 500, 500 };
        public static readonly int[] Large = { 1000, 1000 };
        public static readonly int[] Wide = { 2000, 1000 };
    }

    public struct BlockTypes
    {
        public const int Air = 0;
        public const int Dirt = 1;
        public const int Stone = 2;
        public const int Water = 3;
        public const int Wood = 4;
        public const int Leaves = 5;
        public const int Realgar = 6;
        public const int GoldOre = 7;
        public const int Gold = 8;
        public const int IronOre = 9;
        public const int Iron = 10;
        public const int Magma = 11;
        public const int Grass = 12;
        public const int Brick = 13;
        public const int Spikes = 14;
        public const int Snow = 15;
        public const int Ice = 16;
        public const int BlueIce = 17;
        public const int Sand = 18;
        public const int SandStone = 19;
        public const int Obsidian = 20;
        public const int Crystal = 21;
        public const int Jade = 22;
        public const int Ruby = 23;
    }

    public readonly static string[] TemplateTypes = {"topBottomLeftRight",
        "top", "bottom", "left", "right", // 1 exit
        "topBottom", "topLeft", "topRight", "bottomLeft", "bottomRight", "leftRight", //2 exits
        "topBottomLeft", "topBottomRight", "topLeftRight", "bottomLeftRight"}; //3 exits

    public readonly static string[] SpecialTemplateTypes = { "empty", "none", "surface", "sanctuary" };

    public const int TemplateSize = 10;

}
