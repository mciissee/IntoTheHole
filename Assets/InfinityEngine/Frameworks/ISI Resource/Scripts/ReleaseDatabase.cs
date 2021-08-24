/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                              *
*************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using InfinityEngine.DesignPatterns;
using InfinityEngine.Extensions;
using System.Xml;
using System.Linq;
using InfinityEngine.Utils;

namespace InfinityEngine.ResourceManagement
{

    /// <summary>
    ///  Class used by <a href='http://u3d.as/J4i'>ISI Resource</a> plugin to stores resources usable at runtime.
    /// </summary>
    /// <remarks>
    ///     This class is necessary to the plugin because <c>ScriptableObject</c> are not usable at runtime in the build version.
    ///     So when the plugin updates the resources, a prefab asset will be created with this component.<br/>
    ///     The prefab is loaded at runtime when you request a resource thanks to the function <see cref="ISIResource.Find{T}(ResTypes, string)"/>.
    /// </remarks>
    public class ReleaseDatabase : Singleton<ReleaseDatabase>
    {

        #region Fields

        /// <summary>
        /// Name of the prefab 
        /// </summary>
        public const string PrebaName = "ISIResourcePrefab";

        [HideInInspector] [SerializeField] private List<AnimationClipKV> mAnimationClip;
        [HideInInspector] [SerializeField] private List<AudioClipKV> mAudioClip;
        [HideInInspector] [SerializeField] private List<FontKV> mFont;
        [HideInInspector] [SerializeField] private List<GameObjectKV> mGameObject;
        [HideInInspector] [SerializeField] private List<GUISkinKV> mGUISkin;
        [HideInInspector] [SerializeField] private List<MaterialKV> mMaterial;
        [HideInInspector] [SerializeField] private List<MeshKV> mMesh;
        [HideInInspector] [SerializeField] private List<PhysicMaterialKV> mPhysicMaterial;
        [HideInInspector] [SerializeField] private List<PhysicsMaterial2DKV> mPhysicsMaterial2D;
        [HideInInspector] [SerializeField] private List<ShaderKV> mShader;
        [HideInInspector] [SerializeField] private List<SpriteKV> mSprite;
        [HideInInspector] [SerializeField] private List<TextAssetKV> mTextAsset;
        [HideInInspector] [SerializeField] private List<Texture2DKV> mTexture2D;
        [HideInInspector] [SerializeField] private List<StringKV> mString;
        [HideInInspector] [SerializeField] private List<BoolKV> mBoolean;
        [HideInInspector] [SerializeField] private List<ColorKV> mColor;
        [HideInInspector] [SerializeField] private List<IntKV> mInt32;
        [HideInInspector] [SerializeField] private List<StringKV> mXmlDocument;

        #endregion Fields

        private static bool FindPrefab()
        {
            if (FindObjectOfType<ReleaseDatabase>() == null)
            {
                var prefab = Resources.Load<GameObject>(PrebaName);
                if (prefab == null)
                {
                    Debugger.LogError("ReleaseDatabase prefab is not created !");
                    return false;
                }
                else
                {
                    Instantiate(prefab);
                }
            }
            return true;
        }

        /// <summary>
        /// Creates the prefab which will be used by <see cref="ISIResource"/> at runtime.
        /// </summary>
        public static void CreatePrefab()
        {
            var prefab = new GameObject(PrebaName);
            var component = prefab.AddComponent<ReleaseDatabase>();

            component.mAnimationClip = new List<AnimationClipKV>();
            component.mAudioClip = new List<AudioClipKV>();
            component.mFont = new List<FontKV>();
            component.mGameObject = new List<GameObjectKV>();
            component.mGUISkin = new List<GUISkinKV>();
            component.mMaterial = new List<MaterialKV>();
            component.mMesh = new List<MeshKV>();
            component.mPhysicMaterial = new List<PhysicMaterialKV>();
            component.mPhysicsMaterial2D = new List<PhysicsMaterial2DKV>();
            component.mShader = new List<ShaderKV>();
            component.mSprite = new List<SpriteKV>();
            component.mTextAsset = new List<TextAssetKV>();
            component.mTexture2D = new List<Texture2DKV>();
            component.mString = new List<StringKV>();
            component.mBoolean = new List<BoolKV>();
            component.mColor = new List<ColorKV>();
            component.mInt32 = new List<IntKV>();
            component.mXmlDocument = new List<StringKV>();

            ISIResource.Resources.ForEach((res, listResource) =>
            {
                listResource.ForEach(resource =>
                {
                    Add(res, resource.Key, resource.Obj);
                });
            });
        }

        internal static void Add(ResTypes res, string name, object value)
        {
            switch (res)
            {
                case ResTypes.String:
                    Instance.mString.Add(new StringKV(name, (string)value));
                    break;
                case ResTypes.XmlDocument:
                    Instance.mXmlDocument.Add(new StringKV(name, (string)value));
                    break;
                case ResTypes.Boolean:
                    Instance.mBoolean.Add(new BoolKV(name, (bool)value));
                    break;
                case ResTypes.Int32:
                    Instance.mInt32.Add(new IntKV(name, (int)value));
                    break;
                case ResTypes.Color:
                    Instance.mColor.Add(new ColorKV(name, (Color)value));
                    break;

                case ResTypes.AnimationClip:
                    Instance.mAnimationClip.Add(new AnimationClipKV(name, (AnimationClip)value));
                    break;
                case ResTypes.AudioClip:
                    Instance.mAudioClip.Add(new AudioClipKV(name, (AudioClip)value));
                    break;
                case ResTypes.Font:
                    Instance.mFont.Add(new FontKV(name, (Font)value));
                    break;
                case ResTypes.GameObject:
                    Instance.mGameObject.Add(new GameObjectKV(name, (GameObject)value));
                    break;
                case ResTypes.GUISkin:
                    Instance.mGUISkin.Add(new GUISkinKV(name, (GUISkin)value));
                    break;
                case ResTypes.Material:
                    Instance.mMaterial.Add(new MaterialKV(name, (Material)value));
                    break;
                case ResTypes.Mesh:
                    Instance.mMesh.Add(new MeshKV(name, (Mesh)value));
                    break;
                case ResTypes.PhysicMaterial:
                    Instance.mPhysicMaterial.Add(new PhysicMaterialKV(name, (PhysicMaterial)value));
                    break;
                case ResTypes.PhysicsMaterial2D:
                    Instance.mPhysicsMaterial2D.Add(new PhysicsMaterial2DKV(name, (PhysicsMaterial2D)value));
                    break;
                case ResTypes.Shader:
                    Instance.mShader.Add(new ShaderKV(name, (Shader)value));
                    break;
                case ResTypes.Sprite:
                    Instance.mSprite.Add(new SpriteKV(name, (Sprite)value));
                    break;
                case ResTypes.TextAsset:
                    Instance.mTextAsset.Add(new TextAssetKV(name, (TextAsset)value));
                    break;
                case ResTypes.Texture2D:
                    Instance.mTexture2D.Add(new Texture2DKV(name, (Texture2D)value));
                    break;
            }
        }

        internal static T Find<T>(ResTypes res, string name)
        {
            if (res == ResTypes.None)
                return default(T);

            if (FindPrefab())
            {
                IKeyValue f = null;
                switch (res)
                {
                    case ResTypes.String:
                        f = Instance.mString.Find(e => e.Key == name);
                        break;
                    case ResTypes.Boolean:
                        f = Instance.mBoolean.Find(e => e.Key == name);
                        break;
                    case ResTypes.Int32:
                        f = Instance.mInt32.Find(e => e.Key == name);
                        break;
                    case ResTypes.Color:
                        f = Instance.mColor.Find(e => e.Key == name);
                        break;
                    case ResTypes.XmlDocument:
                        f = Instance.mXmlDocument.Find(e => e.Key == name);
                        break;
                    case ResTypes.AnimationClip:
                        f = Instance.mAnimationClip.Find(e => e.Key == name);
                        break;
                    case ResTypes.AudioClip:
                        f = Instance.mAudioClip.Find(e => e.Key == name);
                        break;
                    case ResTypes.GameObject:
                        f = Instance.mGameObject.Find(e => e.Key == name);
                        break;
                    case ResTypes.GUISkin:
                        f = Instance.mGUISkin.Find(e => e.Key == name);
                        break;
                    case ResTypes.Mesh:
                        f = Instance.mMesh.Find(e => e.Key == name);
                        break;
                    case ResTypes.PhysicMaterial:
                        f = Instance.mPhysicMaterial.Find(e => e.Key == name);
                        break;
                    case ResTypes.PhysicsMaterial2D:
                        f = Instance.mPhysicsMaterial2D.Find(e => e.Key == name);
                        break;
                    case ResTypes.Shader:
                        f = Instance.mShader.Find(e => e.Key == name);
                        break;
                    case ResTypes.Sprite:
                        f = Instance.mSprite.Find(e => e.Key == name);
                        break;
                    case ResTypes.TextAsset:
                        f = Instance.mTextAsset.Find(e => e.Key == name);
                        break;
                    case ResTypes.Texture2D:
                        f = Instance.mTexture2D.Find(e => e.Key == name);
                        break;
                }

                if (res == ResTypes.XmlDocument && f != null)
                {
                    var str = (string)f.Obj;
                    var xml = new XmlDocument();
                    xml.LoadXml(str);
                    return (T)(object)xml;

                }
                return f?.Obj == null ? default(T) : (T)f.Obj;
            }
            return default(T);
        }

        internal static List<T> GetResources<T>(ResTypes type) where T : IKeyValue
        {
            if (type == ResTypes.None)
                return null;

            switch (type)
            {
                case ResTypes.String:
                    return Instance.mString.Cast<T>().ToList();

                case ResTypes.Boolean:
                    return Instance.mBoolean.Cast<T>().ToList();

                case ResTypes.Int32:
                    return Instance.mInt32.Cast<T>().ToList();

                case ResTypes.Color:
                    return Instance.mColor.Cast<T>().ToList();

                case ResTypes.XmlDocument:
                    return Instance.mXmlDocument.Cast<T>().ToList();

                case ResTypes.AnimationClip:
                    return Instance.mAnimationClip.Cast<T>().ToList();

                case ResTypes.AudioClip:
                    return Instance.mAudioClip.Cast<T>().ToList();

                case ResTypes.Font:
                    return Instance.mFont.Cast<T>().ToList();

                case ResTypes.GameObject:
                    return Instance.mGameObject.Cast<T>().ToList();

                case ResTypes.GUISkin:
                    return Instance.mGUISkin.Cast<T>().ToList();

                case ResTypes.Material:
                    return Instance.mMaterial.Cast<T>().ToList();

                case ResTypes.Mesh:
                    return Instance.mMesh.Cast<T>().ToList();

                case ResTypes.PhysicMaterial:
                    return Instance.mPhysicMaterial.Cast<T>().ToList();

                case ResTypes.PhysicsMaterial2D:
                    return Instance.mPhysicsMaterial2D.Cast<T>().ToList();

                case ResTypes.Shader:
                    return Instance.mShader.Cast<T>().ToList();

                case ResTypes.Sprite:
                    return Instance.mSprite.Cast<T>().ToList();

                case ResTypes.TextAsset:
                    return Instance.mTextAsset.Cast<T>().ToList();

                case ResTypes.Texture2D:
                    return Instance.mTexture2D.Cast<T>().ToList();

                default:
                    return null;
            }
        }

    }
}