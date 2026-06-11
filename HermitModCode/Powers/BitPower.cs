using System;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace HermitMod.Powers
{
    public sealed class BitPower : HermitPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
    }
}