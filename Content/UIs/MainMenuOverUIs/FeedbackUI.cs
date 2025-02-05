﻿using InnoVault.UIHandles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityOverhaul.Content.UIs.MainMenuOverUIs
{
    internal class FeedbackUI : UIHandle, ICWRLoader
    {
        private float _sengs;
        internal bool _active;
        internal static FeedbackUI Instance { get; private set; }

        private static Asset<Texture2D> githubOAC;
        private static Asset<Texture2D> steamOAC;

        private Vector2 githubPos1 => new Vector2(Main.screenWidth - 60, 60);

        private Vector2 githubPos2 => new Vector2(Main.screenWidth - 140, 60);

        private Vector2 githubPos => Vector2.Lerp(githubPos1, githubPos2, _sengs);

        private Vector2 githubCenter => githubPos + new Vector2(githubOAC.Width(), githubOAC.Height()) / 2 * githubSiz;

        public override LayersModeEnum LayersMode => LayersModeEnum.Mod_MenuLoad;

        private int Time;

        private float githubSiz1 => 0.001f;

        private float githubSiz2 => 0.05f;

        private bool old_onGithub;
        private bool old_onSteam;

        private bool onGithub => MousePosition.Distance(githubCenter) < githubOAC.Width() * githubSiz2 / 2f;

        private bool onSteam => MousePosition.Distance(steamCenter) < steamOAC.Width() * githubSiz2 / 2f;

        private float githubSiz => float.Lerp(githubSiz1, githubSiz2, _sengs);

        private Vector2 steamPos1 => new Vector2(Main.screenWidth - 80, 60);

        private Vector2 steamPos2 => new Vector2(Main.screenWidth - 200, 60);

        private Vector2 steamPos => Vector2.Lerp(steamPos1, steamPos2, _sengs);

        private Vector2 steamCenter => steamPos + new Vector2(steamOAC.Width(), steamOAC.Height()) / 2 * githubSiz;
        public override bool Active => CWRLoad.OnLoadContentBool;
        public bool OnActive() => _active || _sengs > 0;
        void ICWRLoader.LoadAsset() {
            githubOAC = CWRUtils.GetT2DAsset(CWRConstant.UI + "GithubOAC");
            steamOAC = CWRUtils.GetT2DAsset(CWRConstant.UI + "SteamOAC");
        }
        public override void Load() {
            Instance = this;
            _sengs = 0;
        }
        public override void UnLoad() {
            Instance = null;
            _sengs = 0;
            steamOAC = null;
            githubOAC = null;
        }

        public void Initialize() {
            if (_active) {
                if (_sengs < 1) {
                    _sengs += 0.04f;
                }
            }
            else {
                if (_sengs > 0) {
                    _sengs -= 0.04f;
                }
            }
        }

        public override void Update() {
            Initialize();

            if (_sengs >= 1 && githubOAC != null && steamOAC != null) {
                if (!old_onSteam && onSteam || !old_onGithub && onGithub) {
                    SoundEngine.PlaySound(SoundID.MenuTick with { Pitch = 0.6f, Volume = 0.6f });
                }

                old_onSteam = onSteam;
                old_onGithub = onGithub;

                if (keyLeftPressState == KeyPressState.Pressed) {
                    if (onGithub) {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        (CWRConstant.githubUrl + "/issues/new").WebRedirection(true);
                    }
                    else if (onSteam) {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        CWRConstant.steamFeedback.WebRedirection(true);
                    }
                    else {
                        SoundEngine.PlaySound(SoundID.MenuClose);
                        _active = false;
                    }
                }
            }

            if (OnActive()) {
                KeyboardState currentKeyState = Main.keyState;
                KeyboardState previousKeyState = Main.oldKeyState;
                if (currentKeyState.IsKeyDown(Keys.Escape) && !previousKeyState.IsKeyDown(Keys.Escape)) {
                    SoundEngine.PlaySound(SoundID.MenuClose);
                    _active = false;
                }
            }

            Time++;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (!OnActive()) {
                return;
            }

            Color color = VaultUtils.MultiStepColorLerp(Math.Abs(MathF.Sin(Time * 0.035f)), Color.Gold, Color.Green);

            spriteBatch.Draw(CWRUtils.GetT2DAsset(CWRConstant.Placeholder2).Value, Vector2.Zero
                , new Rectangle(0, 0, Main.screenWidth, Main.screenHeight)
                , Color.Black * _sengs * 0.85f, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);

            spriteBatch.Draw(githubOAC.Value, githubPos, null
                , (onGithub ? color : Color.White) * _sengs, 0f, Vector2.Zero, githubSiz, SpriteEffects.None, 0);
            spriteBatch.Draw(steamOAC.Value, steamPos, null
                , (onSteam ? color : Color.White) * _sengs, 0f, Vector2.Zero, githubSiz, SpriteEffects.None, 0);
        }
    }
}
