using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Runs;

namespace WeijiaMod.Character
{
	public static class WeijiaVisualBuilder
	{
		public static NCreatureVisuals Build()
		{
			NCreatureVisuals ncreatureVisuals = new NCreatureVisuals();
			Control control = new Control();
			control.Name = "Bounds";
			control.UniqueNameInOwner = true;
			control.Position = new Vector2(-130f, -260f);
			control.Size = new Vector2(260f, 260f);
			ncreatureVisuals.AddChild(control, false, Node.InternalMode.Disabled);
			control.Owner = ncreatureVisuals;
			Marker2D marker2D = new Marker2D();
			marker2D.Name = "CenterPos";
			marker2D.UniqueNameInOwner = true;
			marker2D.Position = new Vector2(0f, -120f);
			ncreatureVisuals.AddChild(marker2D, false, Node.InternalMode.Disabled);
			marker2D.Owner = ncreatureVisuals;
			Marker2D marker2D2 = new Marker2D();
			marker2D2.Name = "OrbPos";
			marker2D2.UniqueNameInOwner = true;
			marker2D2.Position = new Vector2(-130f, -120f);
			ncreatureVisuals.AddChild(marker2D2, false, Node.InternalMode.Disabled);
			marker2D2.Owner = ncreatureVisuals;
			Marker2D marker2D3 = new Marker2D();
			marker2D3.Name = "TalkPos";
			marker2D3.UniqueNameInOwner = true;
			marker2D3.Position = new Vector2(60f, -310f);
			ncreatureVisuals.AddChild(marker2D3, false, Node.InternalMode.Disabled);
			marker2D3.Owner = ncreatureVisuals;
			Marker2D marker2D4 = new Marker2D();
			marker2D4.Name = "IntentPos";
			marker2D4.UniqueNameInOwner = true;
			marker2D4.Position = new Vector2(0f, -270f);
			ncreatureVisuals.AddChild(marker2D4, false, Node.InternalMode.Disabled);
			marker2D4.Owner = ncreatureVisuals;
			Node2D node2D = new Node2D();
			node2D.Name = "Visuals";
			node2D.UniqueNameInOwner = true;
			node2D.Scale = new Vector2(1.3f, 1.3f);
			ncreatureVisuals.AddChild(node2D, false, Node.InternalMode.Disabled);
			node2D.Owner = ncreatureVisuals;
			Sprite2D sprite2D = WeijiaVisualBuilder.CreateSprite("Shadow", "res://images/char/shadow.png", new Vector2(0f, -5f));
			node2D.AddChild(sprite2D, false, Node.InternalMode.Disabled);
			sprite2D.Owner = ncreatureVisuals;
			Node2D node2D2 = new Node2D();
			node2D2.Name = "Waist";
			node2D2.Position = new Vector2(2f, -48f);
			node2D.AddChild(node2D2, false, Node.InternalMode.Disabled);
			node2D2.Owner = ncreatureVisuals;
			Sprite2D sprite2D2 = WeijiaVisualBuilder.CreateSprite("RightLeg", "res://images/char/leg_right.png", new Vector2(18f, 43f));
			node2D2.AddChild(sprite2D2, false, Node.InternalMode.Disabled);
			sprite2D2.Owner = ncreatureVisuals;
			Sprite2D sprite2D3 = WeijiaVisualBuilder.CreateSprite("LeftLeg", "res://images/char/leg_left.png", new Vector2(-20f, 45f));
			node2D2.AddChild(sprite2D3, false, Node.InternalMode.Disabled);
			sprite2D3.Owner = ncreatureVisuals;
			Sprite2D sprite2D4 = WeijiaVisualBuilder.CreateSprite("Body", "res://images/char/body.png", new Vector2(0f, -5f));
			sprite2D4.Offset = new Vector2(0f, -50f);
			node2D2.AddChild(sprite2D4, false, Node.InternalMode.Disabled);
			sprite2D4.Owner = ncreatureVisuals;
			Sprite2D sprite2D5 = WeijiaVisualBuilder.CreateSprite("RightArm", "res://images/char/right_hand.png", new Vector2(52f, -69f));
			sprite2D5.Rotation = 0.2f;
			node2D2.AddChild(sprite2D5, false, Node.InternalMode.Disabled);
			sprite2D5.Owner = ncreatureVisuals;
			Node2D node2D3 = new Node2D();
			node2D3.Name = "LeftArm";
			node2D3.Position = new Vector2(-32f, -60f);
			node2D3.Rotation = -0.15f;
			node2D2.AddChild(node2D3, false, Node.InternalMode.Disabled);
			node2D3.Owner = ncreatureVisuals;
			Sprite2D sprite2D6 = WeijiaVisualBuilder.CreateSprite("HandGun", "res://images/char/hand_gun.png", new Vector2(-5f, 5f));
			node2D3.AddChild(sprite2D6, false, Node.InternalMode.Disabled);
			sprite2D6.Owner = ncreatureVisuals;
			Sprite2D sprite2D7 = WeijiaVisualBuilder.CreateSprite("Gun", "res://images/char/gun.png", new Vector2(-12f, -12f));
			sprite2D7.Rotation = 0.3f;
			sprite2D7.Scale = new Vector2(0.85f, 0.85f);
			node2D3.AddChild(sprite2D7, false, Node.InternalMode.Disabled);
			sprite2D7.Owner = ncreatureVisuals;
			Node2D node2D4 = new Node2D();
			node2D4.Name = "Head";
			node2D4.Position = new Vector2(-2f, -105f);
			node2D2.AddChild(node2D4, false, Node.InternalMode.Disabled);
			node2D4.Owner = ncreatureVisuals;
			Sprite2D sprite2D8 = WeijiaVisualBuilder.CreateSprite("Hat", "res://images/char/hat.png", new Vector2(20f, -45f));
			node2D4.AddChild(sprite2D8, false, Node.InternalMode.Disabled);
			sprite2D8.Owner = ncreatureVisuals;
			Sprite2D sprite2D9 = WeijiaVisualBuilder.CreateSprite("Eye", "res://images/char/eye.png", new Vector2(18f, -12f));
			node2D4.AddChild(sprite2D9, false, Node.InternalMode.Disabled);
			sprite2D9.Owner = ncreatureVisuals;
			Sprite2D sprite2D10 = WeijiaVisualBuilder.CreateSprite("Corpse", "res://images/char/corpse.png", new Vector2(0f, -20f));
			sprite2D10.Visible = false;
			sprite2D10.Offset = new Vector2(0f, -30f);
			node2D.AddChild(sprite2D10, false, Node.InternalMode.Disabled);
			sprite2D10.Owner = ncreatureVisuals;
			AnimationPlayer animPlayer = WeijiaVisualBuilder.CreateAnimationPlayer(node2D2, node2D4, node2D3, sprite2D7, sprite2D5, node2D, sprite2D10);
			ncreatureVisuals.AddChild(animPlayer, false, Node.InternalMode.Disabled);
			animPlayer.Owner = ncreatureVisuals;
			animPlayer.AnimationFinished += delegate(StringName animName)
			{
				bool flag = animName != "idle" && animName != "die";
				if (flag)
				{
					animPlayer.Play("idle", -1.0, 1f, false);
				}
			};
			animPlayer.CallDeferred("play", new Variant[]
			{
				"idle"
			});
			return ncreatureVisuals;
		}

		private static Sprite2D CreateSprite(string name, string texturePath, Vector2 position)
		{
			return new Sprite2D
			{
				Name = name,
				Position = position,
				Texture = GD.Load<Texture2D>(texturePath)
			};
		}

		private static AnimationPlayer CreateAnimationPlayer(Node2D waist, Node2D head, Node2D leftArm, Node2D gun, Sprite2D rightArm, Node2D visuals, Sprite2D corpse)
		{
			AnimationPlayer animationPlayer = new AnimationPlayer();
			animationPlayer.Name = "AnimPlayer";
			AnimationLibrary animationLibrary = new AnimationLibrary();
			animationLibrary.AddAnimation("idle", WeijiaVisualBuilder.CreateIdleAnimation());
			animationLibrary.AddAnimation("hurt", WeijiaVisualBuilder.CreateHitAnimation());
			animationLibrary.AddAnimation("attack", WeijiaVisualBuilder.CreateAttackAnimation());
			animationLibrary.AddAnimation("cast", WeijiaVisualBuilder.CreateCastAnimation());
			animationLibrary.AddAnimation("die", WeijiaVisualBuilder.CreateDeadAnimation());
			animationPlayer.AddAnimationLibrary("", animationLibrary);
			return animationPlayer;
		}

		private static Animation CreateIdleAnimation()
		{
			Animation animation = new Animation();
			animation.Length = 1.6666f;
			animation.LoopMode = Animation.LoopModeEnum.Linear;
			int trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist:position");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, new Vector2(2f, -48f), 1f);
			animation.TrackInsertKey(trackIdx, 0.833299994468689, new Vector2(6.84f, -48f), 1f);
			animation.TrackInsertKey(trackIdx, 1.666599988937378, new Vector2(2f, -48f), 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/Head:position");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, new Vector2(-2f, -105f), 1f);
			animation.TrackInsertKey(trackIdx, 0.833299994468689, new Vector2(-2f, -103.2f), 1f);
			animation.TrackInsertKey(trackIdx, 1.666599988937378, new Vector2(-2f, -105f), 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/Head:rotation");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, 0f, 1f);
			animation.TrackInsertKey(trackIdx, 0.833299994468689, 0.111f, 1f);
			animation.TrackInsertKey(trackIdx, 1.666599988937378, 0f, 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/LeftArm:position");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, new Vector2(-32f, -60f), 1f);
			animation.TrackInsertKey(trackIdx, 0.833299994468689, new Vector2(-36f, -66f), 1f);
			animation.TrackInsertKey(trackIdx, 1.666599988937378, new Vector2(-32f, -60f), 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/LeftArm:rotation");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, -0.15f, 1f);
			animation.TrackInsertKey(trackIdx, 0.833299994468689, -0.35f, 1f);
			animation.TrackInsertKey(trackIdx, 1.666599988937378, -0.15f, 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/RightArm:rotation");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, 0.2f, 1f);
			animation.TrackInsertKey(trackIdx, 0.833299994468689, 0.45f, 1f);
			animation.TrackInsertKey(trackIdx, 1.666599988937378, 0.2f, 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/LeftArm/Gun:rotation");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, 0.4f, 1f);
			animation.TrackInsertKey(trackIdx, 1.166700005531311, 0.154f, 1f);
			animation.TrackInsertKey(trackIdx, 1.666700005531311, 0.4f, 1f);
			return animation;
		}

		private static Animation CreateHitAnimation()
		{
			Animation animation = new Animation();
			animation.Length = 0.5f;
			animation.LoopMode = Animation.LoopModeEnum.None;
			int trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist:position");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, new Vector2(2f, -48f), 1f);
			animation.TrackInsertKey(trackIdx, 0.08330000191926956, new Vector2(-8.7f, -46.67f), 1f);
			animation.TrackInsertKey(trackIdx, 0.5, new Vector2(2f, -48f), 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist:rotation");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, 0f, 1f);
			animation.TrackInsertKey(trackIdx, 0.08330000191926956, -0.32f, 1f);
			animation.TrackInsertKey(trackIdx, 0.5, 0f, 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/Head:position");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, new Vector2(-2f, -105f), 1f);
			animation.TrackInsertKey(trackIdx, 0.08330000191926956, new Vector2(9.55f, -110.88f), 1f);
			animation.TrackInsertKey(trackIdx, 0.5, new Vector2(-2f, -105f), 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/Head:rotation");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, 0f, 1f);
			animation.TrackInsertKey(trackIdx, 0.08330000191926956, 0.446f, 1f);
			animation.TrackInsertKey(trackIdx, 0.5, 0f, 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/RightArm:rotation");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, 0.2f, 1f);
			animation.TrackInsertKey(trackIdx, 0.08330000191926956, -0.28f, 1f);
			animation.TrackInsertKey(trackIdx, 0.5, 0.2f, 1f);
			return animation;
		}

		private static Animation CreateAttackAnimation()
		{
			Animation animation = new Animation();
			animation.Length = 0.5f;
			animation.LoopMode = Animation.LoopModeEnum.None;
			int trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist:position");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, new Vector2(2f, -48f), 1f);
			animation.TrackInsertKey(trackIdx, 0.10000000149011612, new Vector2(30f, -48f), 1f);
			animation.TrackInsertKey(trackIdx, 0.15000000596046448, new Vector2(40f, -48f), 1f);
			animation.TrackInsertKey(trackIdx, 0.5, new Vector2(2f, -48f), 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/LeftArm:rotation");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, -0.15f, 1f);
			animation.TrackInsertKey(trackIdx, 0.10000000149011612, 0.3f, 1f);
			animation.TrackInsertKey(trackIdx, 0.15000000596046448, 0.4f, 1f);
			animation.TrackInsertKey(trackIdx, 0.5, -0.15f, 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/LeftArm/Gun:rotation");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, 0.4f, 1f);
			animation.TrackInsertKey(trackIdx, 0.11999999731779099, 0.1f, 1f);
			animation.TrackInsertKey(trackIdx, 0.18000000715255737, 0.8f, 1f);
			animation.TrackInsertKey(trackIdx, 0.5, 0.4f, 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist:rotation");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, 0f, 1f);
			animation.TrackInsertKey(trackIdx, 0.10000000149011612, 0.15f, 1f);
			animation.TrackInsertKey(trackIdx, 0.5, 0f, 1f);
			return animation;
		}

		private static Animation CreateCastAnimation()
		{
            Animation animation = new Animation();
			animation.Length = 0.6f;
			animation.LoopMode = Animation.LoopModeEnum.None;
			int trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist:position");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, new Vector2(2f, -48f), 1f);
			animation.TrackInsertKey(trackIdx, 0.15000000596046448, new Vector2(-5f, -53f), 1f);
			animation.TrackInsertKey(trackIdx, 0.6000000238418579, new Vector2(2f, -48f), 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/LeftArm:rotation");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, -0.15f, 1f);
			animation.TrackInsertKey(trackIdx, 0.15000000596046448, -0.6f, 1f);
			animation.TrackInsertKey(trackIdx, 0.6000000238418579, -0.15f, 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/LeftArm:position");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, new Vector2(-32f, -60f), 1f);
			animation.TrackInsertKey(trackIdx, 0.15000000596046448, new Vector2(-32f, -76f), 1f);
			animation.TrackInsertKey(trackIdx, 0.6000000238418579, new Vector2(-32f, -60f), 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/RightArm:rotation");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, 0.2f, 1f);
			animation.TrackInsertKey(trackIdx, 0.15000000596046448, -0.35f, 1f);
			animation.TrackInsertKey(trackIdx, 0.6000000238418579, 0.2f, 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist/Head:rotation");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, 0f, 1f);
			animation.TrackInsertKey(trackIdx, 0.15000000596046448, -0.15f, 1f);
			animation.TrackInsertKey(trackIdx, 0.6000000238418579, 0f, 1f);
			return animation;
		}

		private static Animation CreateDeadAnimation()
		{
			Animation animation = new Animation();
			animation.Length = 0.8f;
			animation.LoopMode = Animation.LoopModeEnum.None;
			int trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist:position");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, new Vector2(2f, -48f), 1f);
			animation.TrackInsertKey(trackIdx, 0.4000000059604645, new Vector2(-10f, 7f), 1f);
			animation.TrackInsertKey(trackIdx, 0.800000011920929, new Vector2(-10f, 7f), 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist:rotation");
			animation.TrackSetInterpolationType(trackIdx, Animation.InterpolationType.Cubic);
			animation.TrackInsertKey(trackIdx, 0.0, 0f, 1f);
			animation.TrackInsertKey(trackIdx, 0.4000000059604645, -0.5f, 1f);
			animation.TrackInsertKey(trackIdx, 0.800000011920929, -0.5f, 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Waist:modulate");
			animation.TrackInsertKey(trackIdx, 0.0, new Color(1f, 1f, 1f, 1f), 1f);
			animation.TrackInsertKey(trackIdx, 0.6000000238418579, new Color(1f, 1f, 1f, 0.3f), 1f);
			animation.TrackInsertKey(trackIdx, 0.800000011920929, new Color(1f, 1f, 1f, 0f), 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Corpse:visible");
			animation.TrackInsertKey(trackIdx, 0.0, false, 1f);
			animation.TrackInsertKey(trackIdx, 0.5, true, 1f);
			trackIdx = animation.AddTrack(Animation.TrackType.Value, -1);
			animation.TrackSetPath(trackIdx, "Visuals/Corpse:modulate");
			animation.TrackInsertKey(trackIdx, 0.0, new Color(1f, 1f, 1f, 0f), 1f);
			animation.TrackInsertKey(trackIdx, 0.5, new Color(1f, 1f, 1f, 0f), 1f);
			animation.TrackInsertKey(trackIdx, 0.800000011920929, new Color(1f, 1f, 1f, 1f), 1f);
			return animation;
		}

		private const string CharDir = "res://images/char/";

		private const float WaistY = -48f;
	}
}





