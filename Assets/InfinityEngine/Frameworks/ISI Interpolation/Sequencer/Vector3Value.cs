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
    internal struct Vector3Value
    {
        public Vector3 starts;
        public Vector3 ends;

        public Vector3Value(Vector3 starts, Vector3 ends)
        {
            this.starts = starts;
            this.ends = ends;
        }

        public Vector3Value(Vector3Value copy)
        {
            this.starts = copy.starts;
            this.ends = copy.ends;
        }

        public XmlNode ToXml(XmlNode root)
        {
            var doc = root.OwnerDocument;
            var fromNode = doc.CreateElement("from");
            fromNode.AppendChild(doc.CreateTextNode(string.Format("{0},{1},{2}", starts[0], starts[1], starts[2])));
            root.InsertBefore(fromNode, root.FirstChild);
            var toNode = doc.CreateElement("to");
            toNode.AppendChild(doc.CreateTextNode(string.Format("{0},{1},{2}", ends[0], ends[1], ends[2])));
            root.AppendChild(toNode);
            root.InsertAfter(toNode, root.FirstChild);
            return root;
        }

        public static Vector3Value FromXml(XmlNode root)
        {

            var separator = new string[] { "," };
            string[] xyz;
            var newStarts = Vector3.zero;
            var newEnds = Vector3.zero;
            XmlNode startsValNode;
            XmlNode endsValNode;

            try
            {
                startsValNode = root["from"];
                xyz = startsValNode.FirstChild.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < 3; i++)
                {
                    newStarts[i] = float.Parse(xyz[i]);
                }

                endsValNode = root["to"];
                xyz = endsValNode.FirstChild.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < 3; i++)
                {
                    newEnds[i] = float.Parse(xyz[i]);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to the parse the Vector3 value of the isiseq file : \n" + ex.StackTrace);
                return new Vector3Value();
            }

            return new Vector3Value(newStarts, newEnds);
        }
    }

}