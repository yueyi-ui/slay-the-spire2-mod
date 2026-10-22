using System;
using System.Collections.Generic;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Saves;

namespace WeijiaMod.Utility
{
    public static class ModAudio
    {
        public static void Play(string folder, string soundName, float volume = 0f, float pitchVariation = 0f, float basePitch = 1f)
        {
            AudioStream stream = GetOrLoadStream(folder, soundName);
            if (stream == null) return;

            var player = new AudioStreamPlayer
            {
                Stream = stream,
                VolumeDb = volume - 3f,
                Bus = "SFX",
                PitchScale = pitchVariation > 0f ? basePitch + (float)GD.RandRange(-pitchVariation, pitchVariation) : basePitch
            };

            var room = NCombatRoom.Instance;
            if (room != null)
            {
                room.AddChild(player, false, Node.InternalMode.Disabled);
                player.Play(0f);
                player.Finished += () => player.QueueFree();
            }
        }

        public static void Play(Creature creature, string folder, string soundName, float volume = 0f)
        {
            Play(folder, soundName, volume, 0f, 1f);
        }

        public static void PlaySfx(string soundName, float volume = 0f, float pitchVariation = 0f)
        {
            Play("", soundName, volume, pitchVariation, 1f);
        }

        public static void PlayGlobalSfx(string soundName, float volume = 0f)
        {
            AudioStream stream = GetOrLoadStream("", soundName);
            if (stream == null) return;

            var player = new AudioStreamPlayer
            {
                Stream = stream,
                VolumeDb = volume - 3f,
                Bus = "SFX"
            };

            var sceneTree = Engine.GetMainLoop() as SceneTree;
            var window = sceneTree?.Root;
            if (window != null)
            {
                window.AddChild(player, false, Node.InternalMode.Disabled);
                player.Play(0f);
                player.Finished += () => player.QueueFree();
            }
        }

        private static AudioStream GetOrLoadStream(string folder, string soundName)
        {
            string key = string.IsNullOrEmpty(folder) ? soundName : $"{folder}/{soundName}";
            if (CachedStreams.TryGetValue(key, out AudioStream cached))
                return cached;

            string path = string.IsNullOrEmpty(folder)
                ? $"res://audio/sfx/{soundName}.ogg"
                : $"res://audio/sfx/{folder}/{soundName}.ogg";

            AudioStream stream = GD.Load < AudioStream > (path);
            if (stream != null)
                CachedStreams[key] = stream;

            return stream;
        }

        public static void PlayMusic(string[] musicOptions, float volumeDbOffset = 0f)
        {
            if (musicOptions == null || musicOptions.Length == 0) return;

            string path = $"res://audio/bgm/{musicOptions[GD.RandRange(0, musicOptions.Length - 1)]}.ogg";
            if (_currentMusicPath == path && _musicPlayer?.Playing == true)
                return;

            StopMusic();
            AudioStream stream = GD.Load < AudioStream > (path);
            if (stream == null) return;

            if (stream is AudioStreamOggVorbis ogg)
                ogg.Loop = true;

            _musicPlayer = new AudioStreamPlayer
            {
                Stream = stream,
                Bus = "Master"
            };
            _currentVolumeOffset = volumeDbOffset;
            _musicPlayer.VolumeDb = Mathf.LinearToDb(Mathf.Pow(SaveManager.Instance.SettingsSave.VolumeBgm, 2f)) + _currentVolumeOffset - 6f;

            var run = NRun.Instance;
            if (run != null)
            {
                run.AddChild(_musicPlayer, false, Node.InternalMode.Disabled);
                _musicPlayer.Play(0f);
                _currentMusicPath = path;
            }
        }

        public static void SetMusicVolume(float volume)
        {
            if (_musicPlayer != null && GodotObject.IsInstanceValid(_musicPlayer))
            {
                _musicPlayer.VolumeDb = Mathf.LinearToDb(Mathf.Pow(volume, 2f)) + _currentVolumeOffset - 6f;
            }
        }

        public static void FadeIn(string[] musicOptions, float duration = 1f, float volumeDbOffset = 0f)
        {
            if (musicOptions == null || musicOptions.Length == 0) return;

            string path = $"res://audio/bgm/{musicOptions[GD.RandRange(0, musicOptions.Length - 1)]}.ogg";
            if (_currentMusicPath == path && _musicPlayer?.Playing == true)
                return;

            if (_musicPlayer != null && GodotObject.IsInstanceValid(_musicPlayer))
            {
                _outgoingFadeTween?.Kill();
                _outgoingPlayer?.QueueFree();
                _outgoingPlayer = _musicPlayer;
                _outgoingFadeTween = _outgoingPlayer.CreateTween();
                _outgoingFadeTween.TweenProperty(_outgoingPlayer, "volume_db", -80f, duration)
                    .SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
                _outgoingFadeTween.TweenCallback(Callable.From(() =>
                {
                    _outgoingPlayer?.QueueFree();
                    _outgoingPlayer = null;
                }));
            }

            _fadeTween?.Kill();
            _musicPlayer = null;
            _currentMusicPath = null;

            AudioStream stream = GD.Load < AudioStream > (path);
            if (stream == null) return;

            if (stream is AudioStreamOggVorbis ogg)
                ogg.Loop = true;

            _musicPlayer = new AudioStreamPlayer
            {
                Stream = stream,
                Bus = "Master",
                VolumeDb = -80f
            };
            _currentVolumeOffset = volumeDbOffset;

            var run = NRun.Instance;
            if (run != null)
            {
                run.AddChild(_musicPlayer, false, Node.InternalMode.Disabled);
                _musicPlayer.Play(0f);
                _currentMusicPath = path;
                float from = Mathf.LinearToDb(Mathf.Pow(SaveManager.Instance.SettingsSave.VolumeBgm, 2f)) + _currentVolumeOffset - 6f;
                _fadeTween = _musicPlayer.CreateTween();
                _fadeTween.TweenProperty(_musicPlayer, "volume_db", from, duration)
                    .SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
            }
        }

        public static void FadeOut(float duration = 1f)
        {
            if (_musicPlayer == null || !GodotObject.IsInstanceValid(_musicPlayer))
                return;

            _fadeTween?.Kill();
            _fadeTween = _musicPlayer.CreateTween();
            _fadeTween.TweenProperty(_musicPlayer, "volume_db", -80f, duration)
                .SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
            _fadeTween.TweenCallback(Callable.From(StopMusicImmediate));
        }

        private static void StopMusicImmediate()
        {
            _fadeTween?.Kill();
            _fadeTween = null;
            _outgoingFadeTween?.Kill();
            _outgoingFadeTween = null;

            if (_musicPlayer != null && GodotObject.IsInstanceValid(_musicPlayer))
            {
                _musicPlayer.Stop();
                _musicPlayer.QueueFree();
            }
            _musicPlayer = null;
            _currentMusicPath = null;

            if (_outgoingPlayer != null && GodotObject.IsInstanceValid(_outgoingPlayer))
            {
                _outgoingPlayer.Stop();
                _outgoingPlayer.QueueFree();
            }
            _outgoingPlayer = null;
        }

        public static void StopMusic()
        {
            StopMusicImmediate();
        }

        public static bool IsPlayingLegacyMusic()
        {
            return _musicPlayer?.Playing == true;
        }

        public static void PlayAmbience(string ambienceName, float volumeDbOffset = 0f)
        {
            string path = $"res://audio/bgm/{ambienceName}.ogg";
            if (_currentAmbiencePath == path && _ambiencePlayer?.Playing == true)
                return;

            StopAmbience();
            AudioStream stream = GD.Load < AudioStream > (path);
            if (stream == null) return;

            if (stream is AudioStreamOggVorbis ogg)
                ogg.Loop = true;

            _ambiencePlayer = new AudioStreamPlayer
            {
                Stream = stream,
                Bus = "Master"
            };
            _ambiencePlayer.VolumeDb = Mathf.LinearToDb(Mathf.Pow(SaveManager.Instance.SettingsSave.VolumeAmbience, 2f)) + volumeDbOffset - 6f;

            var run = NRun.Instance;
            if (run != null)
            {
                run.AddChild(_ambiencePlayer, false, Node.InternalMode.Disabled);
                _ambiencePlayer.Play(0f);
                _currentAmbiencePath = path;
            }
        }

        public static void FadeInAmbience(string ambienceName, float duration = 1f, float volumeDbOffset = 0f)
        {
            string path = $"res://audio/bgm/{ambienceName}.ogg";
            if (_currentAmbiencePath == path && _ambiencePlayer?.Playing == true)
                return;

            StopAmbience();
            AudioStream stream = GD.Load < AudioStream > (path);
            if (stream == null) return;

            if (stream is AudioStreamOggVorbis ogg)
                ogg.Loop = true;

            _ambiencePlayer = new AudioStreamPlayer
            {
                Stream = stream,
                Bus = "Master",
                VolumeDb = -80f
            };

            var run = NRun.Instance;
            if (run != null)
            {
                run.AddChild(_ambiencePlayer, false, Node.InternalMode.Disabled);
                _ambiencePlayer.Play(0f);
                _currentAmbiencePath = path;
                float from = Mathf.LinearToDb(Mathf.Pow(SaveManager.Instance.SettingsSave.VolumeAmbience, 2f)) + volumeDbOffset - 6f;
                _ambienceFadeTween = _ambiencePlayer.CreateTween();
                _ambienceFadeTween.TweenProperty(_ambiencePlayer, "volume_db", from, duration)
                    .SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
            }
        }

        public static void FadeOutAmbience(float duration = 1f)
        {
            if (_ambiencePlayer == null || !GodotObject.IsInstanceValid(_ambiencePlayer))
                return;

            _ambienceFadeTween?.Kill();
            _ambienceFadeTween = _ambiencePlayer.CreateTween();
            _ambienceFadeTween.TweenProperty(_ambiencePlayer, "volume_db", -80f, duration)
                .SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
            _ambienceFadeTween.TweenCallback(Callable.From(StopAmbience));
        }

        public static void StopAmbience()
        {
            _ambienceFadeTween?.Kill();
            _ambienceFadeTween = null;

            if (_ambiencePlayer != null && GodotObject.IsInstanceValid(_ambiencePlayer))
            {
                _ambiencePlayer.Stop();
                _ambiencePlayer.QueueFree();
            }
            _ambiencePlayer = null;
            _currentAmbiencePath = null;
        }

        public static void SetAmbienceVolume(float volume)
        {
            if (_ambiencePlayer != null && GodotObject.IsInstanceValid(_ambiencePlayer))
            {
                _ambiencePlayer.VolumeDb = Mathf.LinearToDb(Mathf.Pow(volume, 2f)) - 6f;
            }
        }

        private static readonly Dictionary<string, AudioStream> CachedStreams = new Dictionary<string, AudioStream>();
        private static AudioStreamPlayer _musicPlayer;
        private static string _currentMusicPath;
        private static float _currentVolumeOffset;
        private static Tween _fadeTween;
        private static AudioStreamPlayer _outgoingPlayer;
        private static Tween _outgoingFadeTween;
        private static AudioStreamPlayer _ambiencePlayer;
        private static string _currentAmbiencePath;
        private static Tween _ambienceFadeTween;

        private const float MusicVolumeOffset = -6f;
        private const float AmbienceVolumeOffset = -6f;
        private const float SfxVolumeOffset = -3f;
        private const string ModPath = "res://WeijiaMod";
    }
}




