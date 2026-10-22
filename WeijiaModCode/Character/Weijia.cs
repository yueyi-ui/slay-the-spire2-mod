using System;
using System.Collections.Generic;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using WeijiaMod.Cards;
using WeijiaMod.Extensions;
using WeijiaMod.Relics;
using WeijiaMod.Utility;

namespace WeijiaMod.Character
{
    public class Weijia : PlaceholderCharacterModel
    {
        public override Color NameColor => new Color("2196F3");
        public override Color MapDrawingColor => new Color("2196F3");
        public override Color RemoteTargetingLineColor => new Color("2196F3FF");
        public override Color RemoteTargetingLineOutline => new Color("1565C0FF");

        public override CharacterGender Gender => CharacterGender.Masculine;

        public override int StartingHp => 72;
        public override int StartingGold => 99;

        public override IEnumerable<CardModel> StartingDeck
        {
            get
            {
                return new CardModel[]
                {
                    ModelDb.Card<Strike_Weijia>(),
                    ModelDb.Card<Strike_Weijia>(),
                    ModelDb.Card<Strike_Weijia>(),
                    ModelDb.Card<Strike_Weijia>(),
                    ModelDb.Card<Defend_Weijia>(),
                    ModelDb.Card<Defend_Weijia>(),
                    ModelDb.Card<Defend_Weijia>(),
                    ModelDb.Card<Defend_Weijia>(),
                    ModelDb.Card<HelloWorld>(),
                    ModelDb.Card<WeijiaFocusCard>()
                };
            }
        }

        public override IReadOnlyList<RelicModel> StartingRelics
        {
            get
            {
                return new RelicModel[] { ModelDb.Relic < WeijiaStudentID > () };
            }
        }

        public override CardPoolModel CardPool => ModelDb.CardPool<WeijiaCardPool>();
        public override RelicPoolModel RelicPool => ModelDb.RelicPool<WeijiaRelicPool>();
        public override PotionPoolModel PotionPool => ModelDb.PotionPool<WeijiaPotionPool>();

        public override string CustomCharacterSelectBg => "res://scenes/hermit_select_bg.tscn";
        public override string CustomCharacterSelectIconPath => "HermitButton.png".CharacterUiPath();
        public override string CustomCharacterSelectLockedIconPath => "HermitButton.png".CharacterUiPath();

        public override Control CustomIcon
        {
            get
            {
                var texture = ResourceLoader.Load < Texture2D > ("res://images/charui/character_icon_hermit.png", null, ResourceLoader.CacheMode.Reuse);
                if (texture == null) return null;

                var control = new Control();
                control.SetAnchorsPreset(Control.LayoutPreset.FullRect, false);

                var texRect = new TextureRect
                {
                    Texture = texture,
                    ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                    StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered
                };
                texRect.SetAnchorsPreset(Control.LayoutPreset.FullRect, false);
                texRect.GrowHorizontal = Control.GrowDirection.Both;
                texRect.GrowVertical = Control.GrowDirection.Both;

                control.AddChild(texRect, false, Node.InternalMode.Disabled);
                return control;
            }
        }

        public override string CustomIconTexturePath => "character_icon_hermit.png".CharacterUiPath();
        public override string CustomMapMarkerPath => "map_marker_hermit.png".CharacterUiPath();
        public override string CustomArmPointingTexturePath => "res://" + "multiplayer_hand_hermit_point.png".CharacterUiPath();
        public override string CustomArmRockTexturePath => "res://" + "multiplayer_hand_hermit_rock.png".CharacterUiPath();
        public override string CustomArmPaperTexturePath => "res://" + "multiplayer_hand_hermit_paper.png".CharacterUiPath();
        public override string CustomArmScissorsTexturePath => "res://" + "multiplayer_hand_hermit_scissors.png".CharacterUiPath();

        public override string CharacterSelectSfx
        {
            get
            {
                ModAudio.PlayGlobalSfx("hermit_select", 5f);
                return "";
            }
        }

        public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";
        public override string CustomAttackSfx => "event:/sfx/enemy/enemy_attacks/crossbow_ruby_raider/crossbow_ruby_raider_reload";
        public override string CustomCastSfx => null;

        public override string CustomVisualPath => "res://scenes/creature_visuals/weijia.tscn";
        public override string CustomEnergyCounterPath => "res://scenes/combat/energy_counters/hermit_energy_counter.tscn";
        public override string CustomMerchantAnimPath => null;
        public override string CustomRestSiteAnimPath => null;

        public const string CharacterId = "WeijiaMod";
        public static readonly Color Color = new Color("2196F3");
        public static readonly Color CardBackColor = new Color("1565C0");
    }
}





