using UnityEngine;
using System.Collections;
namespace CS7056_AIToolKit
{
    /// <summary>
    /// Simple Attribute pair to hold attributes in
    /// </summary>
    public class AttributePair : object
    {
        public AttributePair(string label_, string value_)
        {
            label = label_;
            value = value_;
        }

        public string label = "";
        public string value = "";

    }
}