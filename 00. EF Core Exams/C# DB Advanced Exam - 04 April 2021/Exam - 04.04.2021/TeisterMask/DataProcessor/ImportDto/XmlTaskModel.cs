using System;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class XmlTaskModel
    {
        public string Name { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime DueTime { get; set; }
        public ExecutionType ExecutionType { get; set; }
        public LabelType LabelType { get; set; }
    }
}
