using System;
using System.Collections.Generic;
using BaseLib.Abstracts;
using Godot;
using HermitMod.Cards;
using HermitMod.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace HermitMod.Character
{
    public class HermitCardPool : CustomCardPoolModel
    {
        public override string Title => "HermitMod";
        public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
        public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
        public override float H => 0.085f;
        public override float S => 0.22f;
        public override float V => 0.93f;
        public override Color DeckEntryCardColor => Hermit.CardBackColor;
        public override bool IsColorless => false;

        protected override CardModel[] GenerateAllCards()
        {
            return new CardModel[]
            {
                ModelDb.Card<Strike_Hermit>(),
                ModelDb.Card<Defend_Hermit>(),
                ModelDb.Card<HelloWorld>()
            };
        }
    }
}