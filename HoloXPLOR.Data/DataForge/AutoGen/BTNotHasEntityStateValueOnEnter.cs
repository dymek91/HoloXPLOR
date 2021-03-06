using System;
using System.Xml.Serialization;

namespace HoloXPLOR.Data.DataForge
{
    [XmlRoot(ElementName = "BTNotHasEntityStateValueOnEnter")]
    public partial class BTNotHasEntityStateValueOnEnter : BTDecorator
    {
        [XmlArray(ElementName = "Path")]
        [XmlArrayItem(Type = typeof(BTInputString))]
        [XmlArrayItem(Type = typeof(BTInputStringValue))]
        [XmlArrayItem(Type = typeof(BTInputStringVar))]
        [XmlArrayItem(Type = typeof(BTInputStringBB))]
        public BTInputString[] Path { get; set; }

    }
}
