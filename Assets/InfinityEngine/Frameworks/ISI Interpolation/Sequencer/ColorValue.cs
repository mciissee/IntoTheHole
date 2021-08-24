/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                                      *
************************************************************************************************************************************/

using System;
using System.Xml;
using UnityEngine;

namespace InfinityEngine.Interpolations
{

    [Serializable]
    internal struct ColorValue
    {
        public Color starts;
        public Color ends;

        public ColorValue(Color starts, Color ends)
        {
            this.starts = starts;
            this.ends = ends;
        }

        public ColorValue(ColorValue copy)
        {
            this.starts = copy.starts;
            this.ends = copy.ends;
        }

        public XmlNode ToXml(XmlNode root)
        {
            var doc = root.OwnerDocument;
            var fromNode = doc.CreateElement("from");
            fromNode.AppendChild(doc.CreateTextNode(string.Format("{0},{1},{2},{3}", starts[0], starts[1], starts[2], starts[3])));
            root.InsertBefore(fromNode, root.FirstChild);
            var toNode = doc.CreateElement("to");
            toNode.AppendChild(doc.CreateTextNode(string.Format("{0},{1},{2},{3}", ends[0], ends[1], ends[2], ends[3])));
            root.AppendChild(toNode);
            root.InsertAfter(toNode, root.FirstChild);
            return root;
        }

        public static ColorValue FromXml(XmlNode root)
        {

            var separator = new string[] { "," };
            string[] xyz;
            var newStarts = Color.black;
            var newEnds = Color.black;
            XmlNode startsValNode;
            XmlNode endsValNode;

            try
            {
                startsValNode = root["from"];
                xyz = startsValNode.FirstChild.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < 4; i++)
                {
                    newStarts[i] = float.Parse(xyz[i]);
                }

                endsValNode = root["to"];
                xyz = endsValNode.FirstChild.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < 4; i++)
                {
                    newEnds[i] = float.Parse(xyz[i]);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to the parse the color value of the isiseq file : \n" + ex.StackTrace);
                return new ColorValue();
            }

            return new ColorValue(newStarts, newEnds);
        }
    }

}