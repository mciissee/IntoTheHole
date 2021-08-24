#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;
using System.Xml;
namespace InfinityEngine.ResourceManagement {
	/// <summary>This class is generated automaticaly by InfinityEngine, it contains constants used by many scripts.  DO NOT EDIT IT !</summary>
	public static class R {
		public static class animationclip {
			public const string Names = "";
		}
		public static class audioclip {
			public const string Names = "background-music,sound-fail,sound-power-up";
			public static AudioClip BackgroundMusic => ISIResource.Find<AudioClip>(ResTypes.AudioClip, "background-music");
			public static AudioClip SoundFail => ISIResource.Find<AudioClip>(ResTypes.AudioClip, "sound-fail");
			public static AudioClip SoundPowerUp => ISIResource.Find<AudioClip>(ResTypes.AudioClip, "sound-power-up");
		}
		public static class font {
			public const string Names = "";
		}
		public static class gameobject {
			public const string Names = "Bonus_Item_01,Large_Obstacle_01,Large_Obstacle_02,Pipe,Ship_1,Ship_2";
			public static GameObject BonusItem01 => ISIResource.Find<GameObject>(ResTypes.GameObject, "Bonus_Item_01");
			public static GameObject LargeObstacle01 => ISIResource.Find<GameObject>(ResTypes.GameObject, "Large_Obstacle_01");
			public static GameObject LargeObstacle02 => ISIResource.Find<GameObject>(ResTypes.GameObject, "Large_Obstacle_02");
			public static GameObject Pipe => ISIResource.Find<GameObject>(ResTypes.GameObject, "Pipe");
			public static GameObject Ship1 => ISIResource.Find<GameObject>(ResTypes.GameObject, "Ship_1");
			public static GameObject Ship2 => ISIResource.Find<GameObject>(ResTypes.GameObject, "Ship_2");
		}
		public static class guiskin {
			public const string Names = "";
		}
		public static class material {
			public const string Names = "";
		}
		public static class mesh {
			public const string Names = "";
		}
		public static class physicmaterial {
			public const string Names = "";
		}
		public static class physicsmaterial2d {
			public const string Names = "";
		}
		public static class shader {
			public const string Names = "";
		}
		public static class sprite {
			public const string Names = "";
		}
		public static class textasset {
			public const string Names = "";
		}
		public static class texture2d {
			public const string Names = "";
		}
		public static class xmls {
			public const string Names = "";
		}
		public static class strings {
			public const string Names = "";
		}
		public static class int32 {
			public const string Names = "";
		}
		public static class boolean {
			public const string Names = "";
		}
		public static class color {
			public const string Names = "";
		}
		public static class tags {
			public const string Names = "Untagged,Respawn,Finish,EditorOnly,MainCamera,Player,GameController,Item";
			public const string Untagged = "Untagged";
			public const string Respawn = "Respawn";
			public const string Finish = "Finish";
			public const string EditorOnly = "EditorOnly";
			public const string MainCamera = "MainCamera";
			public const string Player = "Player";
			public const string GameController = "GameController";
			public const string Item = "Item";
		}
		public static class layers {
			public const string Names = "Default,TransparentFX,IgnoreRaycast,Water,UI";
			public const int Default = 1;
			public const int TransparentFX = 2;
			public const int IgnoreRaycast = 4;
			public const int Water = 16;
			public const int UI = 32;
		}
	}
}
