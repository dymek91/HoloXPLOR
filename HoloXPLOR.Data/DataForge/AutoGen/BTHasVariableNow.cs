using System;
using System.Xml.Serialization;

namespace HoloXPLOR.Data.DataForge
{
    [XmlRoot(ElementName = "BTHasVariableNow")]
    public partial class BTHasVariableNow : BTNode
    {
        [XmlArray(ElementName = "Name")]
        [XmlArrayItem(Type = typeof(BTInputString))]
        [XmlArrayItem(Type = typeof(BTInputStringValue))]
        [XmlArrayItem(Type = typeof(BTInputStringVar))]
        [XmlArrayItem(Type = typeof(BTInputStringBB))]
        public BTInputString[] Name { get; set; }

    }
}
