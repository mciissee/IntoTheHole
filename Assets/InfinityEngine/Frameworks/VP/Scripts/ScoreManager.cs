/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/

using InfinityEngine.Serialization;
namespace InfinityEngine
{
    /// <summary>
    ///   Score manager class
    /// </summary>
    public static class ScoreManager
    {
        /// <summary>
        /// Gets the value of the pref with the given key
        /// </summary>
        /// <param name="key">Pref key</param>
        /// <param name="defaultValue">default value if the key is missing. </param>
        /// <returns></returns>
        public static int Get(string key, int defaultValue = 0)
        {
            return VP.GetInt(key, defaultValue);
        }

        /// <summary>
        /// Updates the value of this key if and only if the new value is bigger than the old.
        /// </summary>
        /// <param name="key">key of the player pref </param>
        /// <param name="value">key of the player pref </param>
        /// <returns></returns>
        public static bool SetIFBestScore(string key, int value)
        {
            var bestScore = VP.GetInt(key);
            if (value >= bestScore)
            {
                VP.SetInt(key, value);
                VP.Save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the value of the pref with the given key
        /// </summary>
        /// <param name="key">key of the player pref </param>
        /// <param name="value">key of the player pref </param> 
        public static void Set(string key, int value)
        {
            VP.SetInt(key, value);
            VP.Save();
        }

        /// <summary>
        /// Adds "value" in pref with the given key.
        /// </summary>
        /// <param name="key">key of the player pref</param>
        /// <param name="value">value to add</param>
        public static void IncreaseValueOf(string key, int value)
        {
            VP.SetInt(key, VP.GetInt(key) + value);
            VP.Save();
        }
    }
}