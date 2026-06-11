using System;
using System.Collections.Generic;
using BaseLib.Abstracts;
using Godot;
using HermitMod.Cards;
using HermitMod.Extensions;
using HermitMod.Relics;
using HermitMod.Utility;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;

namespace HermitMod.Character
{
    public class Hermit : PlaceholderCharacterModel
    {
        public override Color NameColor => Color;
        public override Color MapDrawingColor => Color;
        public override Color RemoteTargetingLineColor => new Color("2ecc71ff"); // 绿色
        public override Color RemoteTargetingLineOutline => new Color("1e8449ff"); // 深绿

        public override CharacterGender Gender => CharacterGender.Masculine;

        public override int StartingHp => 75;
        public override int StartingGold => 99;

        public override IEnumerable<CardModel> StartingDeck
        {
            get
            {
                return new CardModel[]
                {
                    ModelDb.Card<Strike_Hermit>(),
                    ModelDb.Card<Strike_Hermit>(),
                    ModelDb.Card<Strike_Hermit>(),
                    ModelDb.Card<Strike_Hermit>(),
                    ModelDb.Card<Defend_Hermit>(),
                    ModelDb.Card<Defend_Hermit>(),
                    ModelDb.Card<Defend_Hermit>(),
                    ModelDb.Card<Defend_Hermit>()
                };
            }
        }

        public override IReadOnlyList<RelicModel> StartingRelics
        {
            get
            {
                return new RelicModel[] { ModelDb.Relic < OldLocket > () };
            }
        }

        public override CardPoolModel CardPool => ModelDb.CardPool<HermitCardPool>();
        public override RelicPoolModel RelicPool => ModelDb.RelicPool<HermitRelicPool>();
        public override PotionPoolModel PotionPool => ModelDb.PotionPool<HermitPotionPool>();

        public override string CustomCharacterSelectBg => "res://HermitMod/scenes/hermit_select_bg.tscn";
        public override string CustomCharacterSelectIconPath => "HermitButton.png".CharacterUiPath();
        public override string CustomCharacterSelectLockedIconPath => "HermitButton.png".CharacterUiPath();

        public override Control CustomIcon
        {
            get
            {
                var texture = ResourceLoader.Load < Texture2D > ("res://HermitMod/images/charui/character_icon_hermit.png", null, ResourceLoader.CacheMode.Reuse);
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

        public override string CustomVisualPath => "res://HermitMod/scenes/creature_visuals/hermit.tscn";
        public override string CustomEnergyCounterPath => "res://HermitMod/scenes/combat/energy_counters/hermit_energy_counter.tscn";
        public override string CustomMerchantAnimPath => "res://HermitMod/scenes/merchant/characters/hermit_merchant.tscn";
        public override string CustomRestSiteAnimPath => "res://HermitMod/scenes/rest_site/characters/hermit_rest_site.tscn";

        public const string CharacterId = "HermitMod";
        public static readonly Color Color = new Color("2ecc71");      // 绿色
        public static readonly Color CardBackColor = new Color("27ae60"); // 翠绿
    }
}