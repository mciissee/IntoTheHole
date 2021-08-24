#pragma warning disable IDE1006 // Naming Styles

using UnityEngine;
using InfinityEngine;

namespace InfinityEngine.Serialization
{
    ///<summary>
    /// This class is generated automaticaly by InfinityEngine, it contains constants used by many scripts.  DO NOT EDIT IT ! 
    ///</summary>
    public static class R2
    {
        public class PrefGetSet<T>
        {
            ///<summary>
            /// The key of the encapsulated preference 
            ///</summary>
            public string Key { get; private set; }

            ///<summary>
            /// The value of the encapsulated preference, Call VP.Save() after you modify this value. 
            ///</summary>
            public T Value { get => VP.Get<T>(Key); set => VP.Set(Key, value); }

            public PrefGetSet(string key) => Key = key;

            public static implicit operator T(PrefGetSet<T> self) => self.Value;
        }

        public static class integers
        {
            public const string Names = "Score,BestScore,TotalScore";
            public static PrefGetSet<int> Score = new PrefGetSet<int>("Score");
            public static PrefGetSet<int> BestScore = new PrefGetSet<int>("BestScore");
            public static PrefGetSet<int> TotalScore = new PrefGetSet<int>("TotalScore");
        }
        public static class floats
        {
            public const string Names = "";
        }
        public static class doubles
        {
            public const string Names = "";
        }
        public static class longs
        {
            public const string Names = "";
        }
        public static class strings
        {
            public const string Names = "";
        }
        public static class bools
        {
            public const string Names = "IsShowTutorial,EnableAccelerometer,EnableTouchMode";
            public static PrefGetSet<bool> IsShowTutorial = new PrefGetSet<bool>("IsShowTutorial");
            public static PrefGetSet<bool> EnableAccelerometer = new PrefGetSet<bool>("EnableAccelerometer");
            public static PrefGetSet<bool> EnableTouchMode = new PrefGetSet<bool>("EnableTouchMode");
        }
        public static class vector2s
        {
            public const string Names = "";
        }
        public static class vector3s
        {
            public const string Names = "";
        }
        public static class vector4s
        {
            public const string Names = "";
        }
        public static class quaternions
        {
            public const string Names = "";
        }
        public static class colors
        {
            public const string Names = "";
        }
    }
}
