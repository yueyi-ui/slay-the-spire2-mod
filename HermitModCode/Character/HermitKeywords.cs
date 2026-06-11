using System;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace HermitMod.Character
{
    public static class HermitKeywords
    {
        [CustomEnum(null)]
        [KeywordProperties(AutoKeywordPosition.None)]
        public static CardKeyword DeadOn;

        [CustomEnum(null)]
        [KeywordProperties(AutoKeywordPosition.None)]
        public static CardKeyword Concentrate;

        [CustomEnum(null)]
        [KeywordProperties(AutoKeywordPosition.None)]
        public static CardKeyword Bruise;

        [CustomEnum(null)]
        [KeywordProperties(AutoKeywordPosition.None)]
        public static CardKeyword Rugged;

        [CustomEnum(null)]
        [KeywordProperties(AutoKeywordPosition.None)]
        public static CardKeyword Bounty;

        [CustomEnum(null)]
        [KeywordProperties(AutoKeywordPosition.None)]
        public static CardKeyword Stun;
        
        // 新增：比特关键词
        [CustomEnum(null)]
        [KeywordProperties(AutoKeywordPosition.None)]
        public static CardKeyword Bit;
    }
}