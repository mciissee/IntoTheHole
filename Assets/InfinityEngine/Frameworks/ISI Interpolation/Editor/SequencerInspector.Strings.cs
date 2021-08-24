using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityEditor
{
    public partial class SequencerInspector
    {
        public static class Strings
        {

            public const string New = @"New";
            public const string FILE = @"File";
            public const string Name = @"Name";
            public const string Confirm = @"Confirm";
            public const string Cancel = @"Cancel";
            public const string Save = @"Save";
            public const string Duration = @"Duration";
            public const string Delay = @"Delay";
            public const string Ease = @"Ease";
            public const string RepeatCount = @"Repeat";
            public const string RepeatInterval = @"Interval";
            public const string RepeatType = @"Repeat Type";
            public const string From = @"From";
            public const string To = @"To";
            public const string DragAndDrop = @"Drag and drop your file here!";

            public const string Options = @"Options";

            public const string SelectedAnim = "Selected Anim";
            public const string SequencerHelp = @"
            Click on the button '+' at the bottom to add new animation sequence.

            An animation sequence is a mix of different animations (Translation, Rotation..)
            When you add new sequence, you must give it a name. After that, you can add animations into the sequence
            thanks to the button '+' in the area 'Timeline'.

            Each animation is linked to a Transform object (the transform of this GameObject by default).
            The transform allow you to target the components of a GameObjects which can be animated. You can change the default transform
            to target any GameObject.

            There is 3 ways to play a sequence :

            - Use the function 'PlaySequenceWithName' by script with the name of the sequence.
            - Use the event 'Onclick' of the component 'Button' of any GameObject
            - Click on the button play aside of the name of the sequence.
            ";
            public const string AnimMissingTransform = "You must specify a valid Transform for the animation.";
            public const string AnimMissingFadable = "The transform of the animation must have a component of type [Text, CanvasGroup, SpriteRenderer or Image]";
            public const string MissingColorable = "The transform of the animation must have a component of type [Text, SpriteRenderer, Image, or MeshRenderer (with a material with color property)]";
            public const string ComponentToAnimate = "Component To Animate";
            public const string AnimatedComponentHelp = "The component to animate in the GameObject of the transform";
            public const string NameWarning = "A name is required for the sequence";
            public const string DisableOnHide = "Disable On Hide";
            public const string DisableOnHideHelp = "Disable the animations of the sequencer when this GameObject is not visible in the scene";
            public const string DisableOnPause = "Disable On Pause";
            public const string DisableOnPauseHelp = "Disable the animations of the sequencer when the game is on pause";
            public const string SetAsOther = "Set As Other";
            public const string SetAsOtherHelp = "Copy the values of another one Sequencer component.";
            public const string SequenceHeadingHelp = @"
            - Name -> The name of the sequence (used to play it)
            - Duration => The 'minimal' duration of the sequence. (minimal because each single animation of the sequence can be played in loop) 
            ";
            public const string DurationWarning = "You must specify a duration greater than 0 so that the sequence can be played!";
            public const string PlayOnStart = "Play On Start";
            public const string SequenceToPlay = "Sequence To Play";
            public const string SequenceToPlayHelp = " The name of the sequence to play at the starts of the game";
            public const string Timeline = "Timeline";
            public const string Time = "Time";
            public const string TimeHelp = " The interval of time where the selected animation will be played.";
            public const string AnimValueHelp = @"
            - From => Starts (position, rotation, scale, fade...)
            - To => Ends (position, rotation, scale, fade...)
            - Current => Use the current (position, rotation, scale or fade) of the component to animate.
            ";
            public const string Current = "Current";
            public const string AnimEaseHelp = "- Ease => This parameter controls the timing of the animation (Choose the option 'Custom' to makes your own ease function).";
            public const string AnimLoopHelp = @"
            - Repeat  => Animation reapetition count. (-1 for infinite).
            - Interval  => Wait time between each reapetition.
            - Repeat Type  : Animation loop type,  'Restart' =  Restarts the animation from the starts value after each completion. 'Inverse' = invert the starts and ends value after each completion.
            ";
            public const string RotationMode = "Rotation Mode";
            public const string RotationModeHelp = @"
            - Fast => Take the shortest path.
            - Around360 => Make a rotation around 360°.
            ";
            public const string DisableGameObjectAtEnd = "Disable GameObject At End";
            public const string DisableGameObjectAtEndHelp = "Disable the GameObject of the sequencer at the end of this sequence";
            public const string OnStartCallback = "On Start Callback";
            public const string OnUpdateCallback = "On Update Callback";
            public const string OnCompleteCallback = "On Complete Callback";
            public const string OnTerminateCallback = "On Terminate Callback";
            public const string CallbackHelp = "Callback actions to do at the different steps of the animation (starts of the animation, updates of the animation, completion of the animation (called after each loop) and ends of the animation)  ";
            public const string OnStartSound = "On Start Sound";
            public const string OnCompleteSound = "On Complete Sound";
            public const string SoundHelp = "Sound to play at starts and/or ends.";
            public const string MeshRendererMissingMaterial = "The MeshRenderer component of the GameObject linked to the animation must have a material with '_Color' property.";
            public const string FadeWarning = "This animation is linkked to a GameObject which  has {0} components whose alpha can be changed, make sure you choose the right one.";
            public const string ColorWarning = "This animation is linkked to a GameObject which has {0} components whose color can be changed, make sure you choose the right one.";

        }
    }
}
