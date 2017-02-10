using Nop.Core;

namespace Nop.Plugin.Misc.AddFootwear.Domain
{
    public class Size : BaseEntity
    {
        public string SizeText { get; set; }
        public bool IsAvailable { get; set; }
    }
}