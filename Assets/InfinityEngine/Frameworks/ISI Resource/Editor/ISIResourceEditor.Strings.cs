﻿#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/


namespace InfinityEditor
{
    public partial class ISIResourceEditor
    {
        internal static class Strings
        {
            public const string HelpURL = "http://www.mceinc-engine.com.s3-website.eu-west-2.amazonaws.com/Manual-ISI%20Resource.html";

            public const string FolderMustBeRelativeToAssets = "You must choose a folder relative to Assets folder";
            public const string UpdateRequired = "ISI Resource plugin is corrupted, it will be updated !";
            public const string AutoGeneratedComment = "This class is generated automaticaly by InfinityEngine, it contains constants used by many scripts.  DO NOT EDIT IT !";
            public const string Clear = "Clear";
            public const string Settings = "Settings";
            public const string Database = "Database";
            public const string Resources = "Resources";
            public const string Update = "Update";
            public const string Missings = "Missings";
            public const string LetterRequiredInAssetName = "The name of the asset '{0}' must contains at least one letter";
            public const string FolderDuplication = "This path is already taken into account!";
            public const string Updated = "The database is updated !";
            public const string HelpUpdate = "Search all resources and generate some resources that provides access to the resources in script like the class 'R'.";
            public const string DropFolderHere = "DROP FOLDERS HERE";
            public const string ChooseFolder = "OR CHOOSE A FOLDER";
            public const string ClearAll = "CLEAR ALL";

            public const string InclusionArea = "Inclusion Area";
            public const string HelpInclusion = @"
            The folders in which to search for resources.
            Note that each subfolder of these folders will also be taken into account unless the subfolder is placed
            in the 'Exclusion Area'.
            Drag and drop your folder to include it or remove it. If there is no folder, the folder ""Assets"" will be included by default.
            ";

            public const string ExclusionArea = "Exclusion Area";
            public const string HelpExclusion = @"
            The folders in which do not search for resources.
            Note that if you exclude a folder, all of its subfolders will also be excluded.
            Drag and drop your folder to include it or remove it.
            You cannot remove the default folders. 
            ";

            public const string ResourceDuplication = @"There is already a resource of type {0} with the name '{1}' in the database, please change the name of the resource!";
            public const string DefaultPath = "Impossible, the folder {0} is excluded by default";
        }
    }
}