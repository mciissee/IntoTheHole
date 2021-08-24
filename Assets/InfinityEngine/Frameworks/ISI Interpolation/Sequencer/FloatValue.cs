/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                                      *
*************************************************************************************************************************************/

using System;
using System.Xml;
using UnityEngine;

namespace InfinityEngine.Interpolations
{
    [Serializable]
    internal struct FloatValue
    {
        [Range(0, 1)]
        public float starts;

        [Range(0, 1)]
        public float ends;

        public FloatValue(float starts, float ends)
        {
            this.starts = starts;
            this.ends = ends;
        }

        public FloatValue(FloatValue copy)
        {
            this.starts = copy.starts;
            this.ends = copy.ends;
        }

        public XmlNode ToXml(XmlNode root)
        {
            var doc = root.OwnerDocument;
            var fromNode = doc.CreateElement("from");
            fromNode.AppendChild(doc.CreateTextNode(starts.ToString()));
            root.InsertBefore(fromNode, root.FirstChild);

            var toNode = doc.CreateElement("to");
            toNode.AppendChild(doc.CreateTextNode(ends.ToString()));
            root.AppendChild(toNode);
            root.InsertAfter(toNode, root.FirstChild);
            return root;
        }

        public static FloatValue FromXml(XmlNode root)
        {
            XmlNode fromNode;
            XmlNode toNode;

            float newStarts;
            float newEnds;

            try
            {
                fromNode = root["from"];
                newStarts = float.Parse(fromNode.FirstChild.Value);

                toNode = root["to"];
                newEnds = float.Parse(toNode.FirstChild.Value);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to the load the animation : \n" + ex.StackTrace);
                return new FloatValue();
            }

            return new FloatValue(newStarts, newEnds);

        }

    }
}