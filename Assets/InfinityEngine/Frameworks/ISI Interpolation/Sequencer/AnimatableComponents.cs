/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                                      *
*************************************************************************************************************************************/

namespace InfinityEngine.Interpolations
{
    /// <summary>
    /// The components which can be animated by <see cref="Sequencer"/> class 
    /// </summary>
    public enum AnimatableComponents
    {
        /// <summary>
        /// Default
        /// </summary>
        None,

        /// <summary>
        ///  <a href="https://docs.unity3d.com/ScriptReference/Transform.html"> Transform </a> component
        /// </summary>
        Transform,

        /// <summary>
        ///  <a href="https://docs.unity3d.com/ScriptReference/RectTransform.html"> RectTransform </a> component
        /// </summary>
        RectTransform,

        /// <summary>
        ///  <a href="https://docs.unity3d.com/ScriptReference/CanvasGroup.html"> CanvasGroup </a> component
        /// </summary>
        CanvasGroup,

        /// <summary>
        ///  <a href="https://docs.unity3d.com/ScriptReference/Camera.html"> Camera </a> component
        /// </summary>
        Camera,

        /// <summary>
        ///  <a href="https://docs.unity3d.com/ScriptReference/UI.Text.html"> UI.Text </a> component
        /// </summary>
        Text,

        /// <summary>
        ///  <a href="https://docs.unity3d.com/ScriptReference/UI.Image.html"> UI.Image </a> component
        /// </summary>
        Image,
        
        /// <summary>
        ///  <a href="https://docs.unity3d.com/ScriptReference/SpriteRenderer.html"> SpriteRenderer </a> component
        /// </summary>
        SpriteRenderer,

        /// <summary>
        ///  <a href="https://docs.unity3d.com/ScriptReference/MeshRenderer.html"> MeshRenderer </a> component
        /// </summary>
        MeshRenderer
       
    }

}