using System;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using WeijiaMod.Cards;
using WeijiaMod.Extensions;

namespace WeijiaMod.Character
{
    public class WeijiaCardPool : CustomCardPoolModel
    {
        public override string Title => "WeijiaMod";
        public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
        public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
        public override float H => 0.085f;
        public override float S => 0.22f;
        public override float V => 0.93f;
        public override Color DeckEntryCardColor => Weijia.CardBackColor;
        public override bool IsColorless => false;

        protected override CardModel[] GenerateAllCards()
        {
            return new CardModel[]
            {
                // Basic (starter)
                ModelDb.Card<Strike_Weijia>(),
                ModelDb.Card<Defend_Weijia>(),
                ModelDb.Card<HelloWorld>(),

                // Individual cards
                ModelDb.Card<WeijiaKickCombo>(),
                ModelDb.Card<WeijiaVersionBump>(),
                ModelDb.Card<WeijiaFocusCard>(),
                ModelDb.Card<WeijiaBrawl>(),
                ModelDb.Card<WeijiaShortFuse>(),
                ModelDb.Card<WeijiaDetermination>(),
                ModelDb.Card<WeijiaBlackWind>(),
                ModelDb.Card<WeijiaManifest>(),

                // 娱乐卡牌
                ModelDb.Card<WeijiaFamilyCard>(),

                // Batch1 - Common
                ModelDb.Card<WeijiaQuickDraw>(),
                ModelDb.Card<WeijiaHeadshot>(),
                ModelDb.Card<WeijiaCalledShot>(),
                ModelDb.Card<WeijiaDeadeye>(),
                ModelDb.Card<WeijiaGlare>(),
                ModelDb.Card<WeijiaPistolWhip>(),
                ModelDb.Card<WeijiaCheat>(),
                ModelDb.Card<WeijiaCovet>(),
                ModelDb.Card<WeijiaSnipe>(),
                ModelDb.Card<WeijiaTakeAim>(),
                ModelDb.Card<WeijiaFeint>(),
                ModelDb.Card<WeijiaMisfire>(),
                ModelDb.Card<WeijiaSprayPray>(),
                ModelDb.Card<WeijiaTrackingShot>(),
                ModelDb.Card<WeijiaVantage>(),
                ModelDb.Card<WeijiaRoughhouse>(),
                ModelDb.Card<WeijiaHoleUp>(),
                ModelDb.Card<WeijiaTakeCover>(),
                ModelDb.Card<WeijiaLowProfile>(),
                ModelDb.Card<WeijiaBodyArmor>(),

                // Batch2 - Uncommon
                ModelDb.Card<WeijiaAdapt>(),
                ModelDb.Card<WeijiaBigShot>(),
                ModelDb.Card<WeijiaCombo>(),
                ModelDb.Card<WeijiaDesperado>(),
                ModelDb.Card<WeijiaDissolve>(),
                ModelDb.Card<WeijiaDive>(),
                ModelDb.Card<WeijiaEnervate>(),
                ModelDb.Card<WeijiaFinalCanter>(),
                ModelDb.Card<WeijiaFlashPowder>(),
                ModelDb.Card<WeijiaFromBeyond>(),
                ModelDb.Card<WeijiaGambit>(),
                ModelDb.Card<WeijiaGoldenBullet>(),
                ModelDb.Card<WeijiaGrudge>(),
                ModelDb.Card<WeijiaHighCaliber>(),
                ModelDb.Card<WeijiaHighNoon>(),
                ModelDb.Card<WeijiaMagnum>(),
                ModelDb.Card<WeijiaMaintenance>(),
                ModelDb.Card<WeijiaMalice>(),
                ModelDb.Card<WeijiaMidnight>(),
                ModelDb.Card<WeijiaPurgatory>(),
                ModelDb.Card<WeijiaReprieve>(),
                ModelDb.Card<WeijiaShadowCloak>(),
                ModelDb.Card<WeijiaShowdown>(),
                ModelDb.Card<WeijiaSnapshot>(),
                ModelDb.Card<WeijiaSteadyStrike>(),
                ModelDb.Card<WeijiaVirtue>(),
                ModelDb.Card<WeijiaWideOpen>(),
                ModelDb.Card<WeijiaCoalescence>(),
                ModelDb.Card<WeijiaCursedWeapon>(),
                ModelDb.Card<WeijiaDeadMansHand>(),

                // Batch3 - Rare
                ModelDb.Card<WeijiaDeadOrAlive>(),
                ModelDb.Card<WeijiaEternalForm>(),
                ModelDb.Card<WeijiaEyeOfTheStorm>(),
                ModelDb.Card<WeijiaFatalDesire>(),
                ModelDb.Card<WeijiaFullyLoaded>(),
                ModelDb.Card<WeijiaGestalt>(),
                ModelDb.Card<WeijiaGhostlyPresence>(),
                ModelDb.Card<WeijiaHeroicBravado>(),
                ModelDb.Card<WeijiaHorror>(),
                ModelDb.Card<WeijiaImpendingDoom>(),
                ModelDb.Card<WeijiaItchyTrigger>(),
                ModelDb.Card<WeijiaLoneWolf>(),
                ModelDb.Card<WeijiaLuckOfTheDraw>(),
                ModelDb.Card<WeijiaMementoCard>(),
                ModelDb.Card<WeijiaNoHoldsBarred>(),
                ModelDb.Card<WeijiaOverwhelmingPower>(),
                ModelDb.Card<WeijiaRoundhouseKick>(),
                ModelDb.Card<WeijiaDivineFusion>(),

                // Extra cards - themed: taekwondo, debate, CS major, Nuwa (Honor of Kings)
                ModelDb.Card<WeijiaTaekwondoKick>(),
                ModelDb.Card<WeijiaDebateVictory>(),
                ModelDb.Card<WeijiaNuwaGuard>(),

                // NEW - Cycle core
                ModelDb.Card<WeijiaBitSurge>(),
                ModelDb.Card<WeijiaBruiseHarvest>(),

                // NEW v2 - Energy, draw, shrink synergy
                ModelDb.Card<WeijiaEnergySurge>(),
                ModelDb.Card<WeijiaFocusedMind>(),
                ModelDb.Card<WeijiaTinyBullet>(),
                ModelDb.Card<WeijiaLastStand>(),
                ModelDb.Card<WeijiaQuickReload>(),
                ModelDb.Card<WeijiaDoubleTap>(),
            };
        }
    }
}

