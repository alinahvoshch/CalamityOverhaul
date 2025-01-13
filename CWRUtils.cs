﻿using CalamityMod;
using CalamityMod.Events;
using CalamityMod.NPCs.NormalNPCs;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content;
using CalamityOverhaul.Content.Events.TungstenRiotEvent;
using CalamityOverhaul.Content.Items;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static CalamityMod.CalamityUtils;

namespace CalamityOverhaul
{
    public static class CWRUtils
    {
        #region System
        public static LocalizedText SafeGetItemName<T>() where T : ModItem {
            Type type = typeof(T);
            return type.BaseType == typeof(EctypeItem)
                ? Language.GetText($"Mods.CalamityOverhaul.Items.{(Activator.CreateInstance(type) as EctypeItem)?.Name}.DisplayName")
                : GetItemName<T>();
        }

        public static LocalizedText SafeGetItemName(int id) {
            ModItem item = ItemLoader.GetItem(id);
            if (item == null) {
                return CWRLocText.GetText("None");
            }
            return item.GetLocalization("DisplayName");
        }

        /// <summary>
        /// 一个额外的跳字方法，向游戏内打印对象的ToString内容
        /// </summary>
        /// <param name="obj"></param>
        public static void Domp(this object obj, Color color = default) {
            if (color == default) {
                color = Color.White;
            }
            if (obj == null) {
                Text("ERROR Is Null", Color.Red);
                return;
            }
            Text(obj.ToString(), color);
        }

        /// <summary>
        /// 一个额外的跳字方法，向控制台面板打印对象的ToString内容，并自带换行
        /// </summary>
        /// <param name="obj"></param>
        public static void DompInConsole(this object obj, bool outputLogger = true) {
            if (obj == null) {
                Console.WriteLine("ERROR Is Null");
                return;
            }
            string value = obj.ToString();
            Console.WriteLine(value);
            if (outputLogger) {
                CWRMod.Instance.Logger.Info(value);
            }
        }

        /// <summary>
        /// 将 Item 数组的信息写入指定路径的文件中
        /// </summary>
        /// <param name="items">要导出的 Item 数组</param>
        /// <param name="path">写入文件的路径，默认为 "D:\\模组资源\\AAModPrivate\\input.cs"</param>
        //public static void ExportItemTypesToFile(Item[] items, string path = "D:\\Mod_Resource\\input.cs") {
        //    try {
        //        int columnIndex = 0;
        //        using StreamWriter sw = new(path);
        //        sw.Write("string[] fullItems = new string[] {");
        //        foreach (Item item in items) {
        //            columnIndex++;
        //            // 根据是否有 ModItem 决定写入的内容
        //            string itemInfo = item.ModItem == null ? $"\"{item.type}\"" : $"\"{item.ModItem.FullName}\"";
        //            sw.Write(itemInfo);
        //            sw.Write(", ");
        //            // 每行最多写入9个元素，然后换行
        //            if (columnIndex >= 9) {
        //                sw.WriteLine();
        //                columnIndex = 0;
        //            }
        //        }
        //        sw.Write("};");
        //    } catch (UnauthorizedAccessException) {
        //        CWRMod.Instance.Logger.Info($"UnauthorizedAccessException: 无法访问文件路径 '{path}'. 权限不足");
        //    } catch (DirectoryNotFoundException) {
        //        CWRMod.Instance.Logger.Info($"DirectoryNotFoundException: 文件路径 '{path}' 中的目录不存在");
        //    } catch (PathTooLongException) {
        //        CWRMod.Instance.Logger.Info($"PathTooLongException: 文件路径 '{path}' 太长");
        //    } catch (IOException) {
        //        CWRMod.Instance.Logger.Info($"IOException: 无法打开文件 '{path}' 进行写入");
        //    } catch (Exception e) {
        //        CWRMod.Instance.Logger.Info($"An error occurred: {e.Message}");
        //    }
        //}

        public static int GetTileDorp(Tile tile) {
            int stye = TileObjectData.GetTileStyle(tile);
            if (stye == -1) {
                stye = 0;
            }

            return TileLoader.GetItemDropFromTypeAndStyle(tile.TileType, stye);
        }

        public static Player InPosFindPlayer(Vector2 position, int maxRange = 3000) {
            foreach (Player player in Main.player) {
                if (!player.Alives()) {
                    continue;
                }
                if (maxRange == -1) {
                    return player;
                }
                int distance = (int)player.position.To(position).Length();
                if (distance < maxRange) {
                    return player;
                }
            }
            return null;
        }

        public static Player TileFindPlayer(int i, int j) {
            return InPosFindPlayer(new Vector2(i, j) * 16, 9999);
        }

        public static Chest FindNearestChest(int x, int y) {
            int distance = 99999;
            Chest nearestChest = null;

            for (int c = 0; c < Main.chest.Length; c++) {
                Chest currentChest = Main.chest[c];
                if (currentChest != null) {
                    int length = (int)Math.Sqrt(Math.Pow(x - currentChest.x, 2) + Math.Pow(y - currentChest.y, 2));
                    if (length < distance) {
                        nearestChest = currentChest;
                        distance = length;
                    }
                }
            }
            return nearestChest;
        }

        public static void AddItem(this Chest chest, Item item) {
            Item infoItem = item.Clone();
            for (int i = 0; i < chest.item.Length; i++) {
                if (chest.item[i] == null) {
                    chest.item[i] = new Item();
                }
                if (chest.item[i].type == ItemID.None) {
                    chest.item[i] = infoItem;
                    return;
                }
                if (chest.item[i].type == item.type) {
                    chest.item[i].stack += infoItem.stack;
                    return;
                }
            }
        }

        public static Color[] GetColorDate(Texture2D tex) {
            Color[] colors = new Color[tex.Width * tex.Height];
            tex.GetData(colors);
            List<Color> nonTransparentColors = [];
            foreach (Color color in colors) {
                if ((color.A > 0 || color.R > 0 || color.G > 0 || color.B > 0) && color != Color.White && color != Color.Black) {
                    nonTransparentColors.Add(color);
                }
            }
            return nonTransparentColors.ToArray();
        }

        #endregion

        #region AIUtils

        #region 工具部分

        public const float atoR = MathHelper.Pi / 180;

        public static float AtoR(this float num) => num * atoR;

        public static float RtoA(this float num) => num / atoR;

        public static void SetArrowRot(int proj) => Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation() + MathHelper.PiOver2;
        public static void SetArrowRot(this Projectile proj) => proj.rotation = proj.velocity.ToRotation() + MathHelper.PiOver2;

        public static void UpdateOldPosCache(this Projectile projectile, bool useCenter = true, bool addVelocity = true) {
            for (int i = 0; i < projectile.oldPos.Length - 1; i++)
                projectile.oldPos[i] = projectile.oldPos[i + 1];
            projectile.oldPos[^1] = (useCenter ? projectile.Center : projectile.position) + (addVelocity ? projectile.velocity : Vector2.Zero);
        }

        public static void InitOldPosCache(this Projectile projectile, int trailCount, bool useCenter = true) {
            projectile.oldPos = new Vector2[trailCount];

            for (int i = 0; i < trailCount; i++) {
                if (useCenter)
                    projectile.oldPos[i] = projectile.Center;
                else
                    projectile.oldPos[i] = projectile.position;
            }
        }

        /// <summary>
        /// 如果对象是一个蠕虫体节，那么按机会分母的倒数返回布尔值，如果输入5，那么会有4/5的概率返回<see langword="true"/>
        /// </summary>
        /// <param name="targetNPCType"></param>
        /// <param name="randomCount"></param>
        /// <returns></returns>
        public static bool FromWormBodysRandomSet(int targetNPCType, int randomCount) {
            return CWRLoad.WormBodys.Contains(targetNPCType) && !Main.rand.NextBool(randomCount);
        }
        /// <summary>
        /// 如果对象是一个蠕虫体节，那么按机会分母的倒数返回布尔值，如果输入5，那么会有4/5的概率返回<see langword="true"/>
        /// </summary>
        /// <param name="targetNPCType"></param>
        /// <param name="randomCount"></param>
        /// <returns></returns>
        public static bool FromWormBodysRandomSet(this NPC npc, int randomCount) => FromWormBodysRandomSet(npc.type, randomCount);

        /// <summary>
        /// 这个NPC是否属于一个蠕虫体节
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        public static bool IsWormBody(this NPC npc) => CWRLoad.WormBodys.Contains(npc.type);

        /// <summary>
        /// 世界实体坐标转物块坐标
        /// </summary>
        /// <param name="wePos"></param>
        /// <returns></returns>
        public static Vector2 WEPosToTilePos(Vector2 wePos) {
            int tilePosX = (int)(wePos.X / 16f);
            int tilePosY = (int)(wePos.Y / 16f);
            Vector2 tilePos = new(tilePosX, tilePosY);
            tilePos = PTransgressionTile(tilePos);
            return tilePos;
        }

        /// <summary>
        /// 物块坐标转世界实体坐标
        /// </summary>
        /// <param name="tilePos"></param>
        /// <returns></returns>
        public static Vector2 TilePosToWEPos(Vector2 tilePos) {
            float wePosX = (float)(tilePos.X * 16f);
            float wePosY = (float)(tilePos.Y * 16f);

            return new Vector2(wePosX, wePosY);
        }

        /// <summary>
        /// 计算一个渐进速度值
        /// </summary>
        /// <param name="thisCenter">本体位置</param>
        /// <param name="targetCenter">目标位置</param>
        /// <param name="speed">速度</param>
        /// <param name="shutdownDistance">停摆范围</param>
        /// <returns></returns>
        public static float AsymptoticVelocity(Vector2 thisCenter, Vector2 targetCenter, float speed, float shutdownDistance) {
            Vector2 toMou = targetCenter - thisCenter;
            float thisSpeed = toMou.LengthSquared() > shutdownDistance * shutdownDistance ? speed : MathHelper.Min(speed, toMou.Length());
            return thisSpeed;
        }

        /// <summary>
        /// 根据索引返回在player域中的player实例，同时考虑合法性校验
        /// </summary>
        /// <returns>当获取值非法时将返回 <see cref="null"/> </returns>
        public static Player GetPlayerInstance(int playerIndex) {
            if (playerIndex.ValidateIndex(Main.player)) {
                Player player = Main.player[playerIndex];

                return player.Alives() ? player : null;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// 根据索引返回在npc域中的npc实例，同时考虑合法性校验
        /// </summary>
        /// <returns>当获取值非法时将返回 <see cref="null"/> </returns>
        public static NPC GetNPCInstance(int npcIndex) {
            if (npcIndex.ValidateIndex(Main.npc)) {
                NPC npc = Main.npc[npcIndex];

                return npc.Alives() ? npc : null;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// 根据索引返回在projectile域中的Projectile实例，同时考虑合法性校验
        /// </summary>
        /// <returns>当获取值非法时将返回 <see cref="null"/> </returns>
        public static Projectile GetProjectileInstance(int projectileIndex) {
            if (projectileIndex.ValidateIndex(Main.projectile)) {
                Projectile proj = Main.projectile[projectileIndex];
                return proj.Alives() ? proj : null;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// 获取鞭类弹幕的路径点集
        /// </summary>
        public static List<Vector2> GetWhipControlPoints(this Projectile projectile) {
            List<Vector2> list = [];
            Projectile.FillWhipControlPoints(projectile, list);
            return list;
        }

        #endregion

        #region 行为部分

        public static void WulfrumAmplifierAI(NPC npc, float maxrg = 495f, int maxchargeTime = 600) {
            List<int> SuperchargableEnemies = [
                ModContent.NPCType<WulfrumDrone>(),
                ModContent.NPCType<WulfrumGyrator>(),
                ModContent.NPCType<WulfrumHovercraft>(),
                ModContent.NPCType<WulfrumRover>()
            ];

            npc.ai[1] = (int)MathHelper.Lerp(npc.ai[1], maxrg, 0.1f);

            if (Main.rand.NextBool(4)) {
                float dustCount = MathHelper.TwoPi * npc.ai[1] / 8f;
                for (int i = 0; i < dustCount; i++) {
                    float angle = MathHelper.TwoPi * i / dustCount;
                    Dust dust = Dust.NewDustPerfect(npc.Center, 229);
                    dust.position = npc.Center + angle.ToRotationVector2() * npc.ai[1];
                    dust.scale = 0.7f;
                    dust.noGravity = true;
                    dust.velocity = npc.velocity;
                }
            }

            for (int i = 0; i < Main.maxNPCs; i++) {
                NPC npcAtIndex = Main.npc[i];
                if (!npcAtIndex.active)
                    continue;
                if (!SuperchargableEnemies.Contains(npcAtIndex.type) && npcAtIndex.type != ModContent.NPCType<WulfrumRover>())
                    continue;
                if (npcAtIndex.ai[3] > 0f)
                    continue;
                if (npc.Distance(npcAtIndex.Center) > npc.ai[1])
                    continue;

                npcAtIndex.ai[3] = maxchargeTime;
                npcAtIndex.netUpdate = true;

                if (Main.dedServ)
                    continue;

                for (int j = 0; j < 10; j++) {
                    Dust.NewDust(npcAtIndex.position, npcAtIndex.width, npcAtIndex.height, DustID.Electric);
                }
            }
        }

        public static void SpawnGunDust(Projectile projectile, Vector2 pos, Vector2 velocity, int splNum = 1) {
            if (Main.myPlayer != projectile.owner) return;

            pos += velocity.SafeNormalize(Vector2.Zero) * projectile.width * projectile.scale * 0.71f;
            for (int i = 0; i < 30 * splNum; i++) {
                int dustID;
                switch (Main.rand.Next(6)) {
                    case 0:
                        dustID = 262;
                        break;
                    case 1:
                    case 2:
                        dustID = 54;
                        break;
                    default:
                        dustID = 53;
                        break;
                }
                float num = Main.rand.NextFloat(3f, 13f) * splNum;
                float angleRandom = 0.06f;
                Vector2 dustVel = new Vector2(num, 0f).RotatedBy((double)velocity.ToRotation(), default);
                dustVel = dustVel.RotatedBy(0f - angleRandom);
                dustVel = dustVel.RotatedByRandom(2f * angleRandom);
                if (Main.rand.NextBool(4)) {
                    dustVel = Vector2.Lerp(dustVel, -Vector2.UnitY * dustVel.Length(), Main.rand.NextFloat(0.6f, 0.85f)) * 0.9f;
                }
                float scale = Main.rand.NextFloat(0.5f, 1.5f);
                int idx = Dust.NewDust(pos, 1, 1, dustID, dustVel.X, dustVel.Y, 0, default, scale);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].position = pos;
            }
        }

        /// <summary>
        /// 让弹幕进行爆炸效果的操作
        /// </summary>
        /// <param name="projectile">要爆炸的投射物</param>
        /// <param name="blastRadius">爆炸效果的半径（默认为 120 单位）</param>
        /// <param name="explosionSound">爆炸声音的样式（默认为默认的爆炸声音）</param>
        public static void Explode(this Projectile projectile, int blastRadius = 120, SoundStyle explosionSound = default, bool spanSound = true) {
            Vector2 originalPosition = projectile.position;
            int originalWidth = projectile.width;
            int originalHeight = projectile.height;

            if (spanSound) {
                _ = SoundEngine.PlaySound(explosionSound == default ? SoundID.Item14 : explosionSound, projectile.Center);
            }

            projectile.position = projectile.Center;
            projectile.width = projectile.height = blastRadius * 2;
            projectile.position.X -= projectile.width / 2;
            projectile.position.Y -= projectile.height / 2;

            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;

            projectile.Damage();

            projectile.position = originalPosition;
            projectile.width = originalWidth;
            projectile.height = originalHeight;
        }

        /// <summary>
        /// 普通的追逐行为
        /// </summary>
        /// <param name="entity">需要操纵的实体</param>
        /// <param name="TargetCenter">目标地点</param>
        /// <param name="Speed">速度</param>
        /// <param name="ShutdownDistance">停摆距离</param>
        /// <returns></returns>
        public static Vector2 ChasingBehavior(this Entity entity, Vector2 TargetCenter, float Speed, float ShutdownDistance = 16) {
            if (entity == null) {
                return Vector2.Zero;
            }

            Vector2 ToTarget = TargetCenter - entity.Center;
            Vector2 ToTargetNormalize = ToTarget.SafeNormalize(Vector2.Zero);
            Vector2 speed = ToTargetNormalize * AsymptoticVelocity(entity.Center, TargetCenter, Speed, ShutdownDistance);
            entity.velocity = speed;
            return speed;
        }

        /// <summary>
        /// 更加缓和的追逐行为
        /// </summary>
        /// <param name="entity">需要操纵的实体</param>
        /// <param name="TargetCenter">目标地点</param>
        /// <param name="SpeedUpdates">速度的更新系数</param>
        /// <param name="HomingStrenght">追击力度</param>
        /// <returns></returns>
        public static Vector2 SmoothHomingBehavior(this Entity entity, Vector2 TargetCenter, float SpeedUpdates = 1, float HomingStrenght = 0.1f) {
            float targetAngle = entity.AngleTo(TargetCenter);
            float f = entity.velocity.ToRotation().RotTowards(targetAngle, HomingStrenght);
            Vector2 speed = f.ToRotationVector2() * entity.velocity.Length() * SpeedUpdates;
            entity.velocity = speed;
            return speed;
        }

        public static void EntityToRot(this NPC entity, float ToRot, float rotSpeed) {
            //entity.rotation = MathHelper.SmoothStep(entity.rotation, ToRot, rotSpeed);

            // 将角度限制在 -π 到 π 的范围内
            entity.rotation = MathHelper.WrapAngle(entity.rotation);

            // 计算差异角度
            float diff = MathHelper.WrapAngle(ToRot - entity.rotation);

            // 选择修改幅度小的方向进行旋转
            if (Math.Abs(diff) < MathHelper.Pi) {
                entity.rotation += diff * rotSpeed;
            }
            else {
                entity.rotation -= MathHelper.WrapAngle(-diff) * rotSpeed;
            }
        }

        /// <summary>
        /// 处理实体的旋转行为
        /// </summary>
        public static void EntityToRot(this Projectile entity, float ToRot, float rotSpeed) {
            //entity.rotation = MathHelper.SmoothStep(entity.rotation, ToRot, rotSpeed);

            // 将角度限制在 -π 到 π 的范围内
            entity.rotation = MathHelper.WrapAngle(entity.rotation);

            // 计算差异角度
            float diff = MathHelper.WrapAngle(ToRot - entity.rotation);

            // 选择修改幅度小的方向进行旋转
            if (Math.Abs(diff) < MathHelper.Pi) {
                entity.rotation += diff * rotSpeed;
            }
            else {
                entity.rotation -= MathHelper.WrapAngle(-diff) * rotSpeed;
            }
        }

        #endregion

        #endregion

        #region GameUtils
        /// <summary>
        /// 是否处于入侵期间
        /// </summary>
        public static bool Invasion => Main.invasionType > 0 || Main.pumpkinMoon
                || Main.snowMoon || DD2Event.Ongoing || AcidRainEvent.AcidRainEventIsOngoing
                || TungstenRiot.Instance.TungstenRiotIsOngoing;

        public static bool ZoneHell(this Player player) => player.position.Y > Main.maxTilesY * 16f / 15f * 14f;

        public static bool IsTool(this Item item) => item.pick > 0 || item.axe > 0 || item.hammer > 0;

        public static void GiveMeleeType(this Item item, bool isGiveTrueMelee = false) => item.DamageType = GiveMeleeType(isGiveTrueMelee);

        public static DamageClass GiveMeleeType(bool isGiveTrueMelee = false) => isGiveTrueMelee ? ModContent.GetInstance<TrueMeleeDamageClass>() : DamageClass.Melee;

        /// <summary>
        /// 目标弹药是否应该判定为一个木箭
        /// </summary>
        /// <param name="player"></param>
        /// <param name="ammoType"></param>
        /// <returns></returns>
        public static bool IsWoodenAmmo(this Player player, int ammoType) {
            if (player.hasMoltenQuiver && ammoType == ProjectileID.FireArrow) {
                return true;
            }
            if (ammoType == ProjectileID.WoodenArrowFriendly) {
                return true;
            }

            return false;
        }

        public static void SetItemLegendContentTops(ref List<TooltipLine> tooltips, string itemKey) {
            TooltipLine legendtops = tooltips.FirstOrDefault((TooltipLine x) => x.Text.Contains("[legend]") && x.Mod == "Terraria");
            if (legendtops != null) {
                KeyboardState state = Keyboard.GetState();
                if ((state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift))) {
                    legendtops.Text = Language.GetTextValue($"Mods.CalamityOverhaul.Items.{itemKey}.Legend");
                    legendtops.OverrideColor = Color.Lerp(Color.BlueViolet, Color.White, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
                }
                else {
                    legendtops.Text = CWRLocText.GetTextValue("Item_LegendOnMouseLang");
                    legendtops.OverrideColor = Color.Lerp(Color.BlueViolet, Color.Gold, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
                }
            }
        }

        public static void SafeLoadItem(int id) {
            if (!Main.dedServ && id > 0 && id < TextureAssets.Item.Length && Main.Assets != null && TextureAssets.Item[id] != null) {
                Main.instance.LoadItem(id);
            }
        }

        public static void SafeLoadProj(int id) {
            if (!Main.dedServ && id > 0 && id < TextureAssets.Projectile.Length && Main.Assets != null && TextureAssets.Projectile[id] != null) {
                Main.instance.LoadProjectile(id);
            }
        }

        public static int GetProjectileHasNum(int targetProjType, int ownerIndex = -1) {
            int num = 0;
            foreach (var proj in Main.ActiveProjectiles) {
                if (ownerIndex >= 0 && ownerIndex != proj.owner) {
                    continue;
                }
                if (proj.type == targetProjType) {
                    num++;
                }
            }
            return num;
        }

        public static int GetProjectileHasNum(this Player player, int targetProjType) => GetProjectileHasNum(targetProjType, player.whoAmI);

        public static void ModifyLegendWeaponDamageFunc(Player player, Item item, int GetOnDamage, int GetStartDamage, ref StatModifier damage) {
            float oldMultiplicative = damage.Multiplicative;
            damage *= GetOnDamage / (float)GetStartDamage;
            damage /= oldMultiplicative;
            //首先，因为SD的运行优先级并不可靠，有的模组的修改在SD之后运行，比如炼狱模式，这个基础伤害缩放保证一些情况不会发生
            damage *= GetStartDamage / (float)item.damage;
            damage *= item.GetPrefixState().damageMult;
        }

        public static void ModifyLegendWeaponKnockbackFunc(Player player, Item item, float GetOnKnockback, float GetStartKnockback, ref StatModifier Knockback) {
            Knockback *= GetOnKnockback / (float)GetStartKnockback;
            //首先，因为SD的运行优先级并不可靠，有的模组的修改在SD之后运行，比如炼狱模式，这个基础击退缩放保证一些情况不会发生
            Knockback *= GetStartKnockback / item.knockBack;
            Knockback *= item.GetPrefixState().knockbackMult;
        }

        public static NPC FindNPCFromeType(int type) {
            NPC npc = null;
            foreach (var n in Main.npc) {
                if (!n.active) {
                    continue;
                }
                if (n.type == type) {
                    npc = n;
                }
            }
            return npc;
        }

        public static Recipe AddBlockingSynthesisEvent(this Recipe recipe) =>
            recipe.AddConsumeItemCallback((Recipe recipe, int type, ref int amount) => { amount = 0; })
            .AddOnCraftCallback(CWRRecipes.SpawnAction);

        /// <summary>
        /// 用于将一个武器设置为手持刀剑类，这个函数若要正确设置物品的近战属性，需要让其在初始化函数中最后调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public static void SetKnifeHeld<T>(this Item item) where T : ModProjectile {
            if (item.shoot == ProjectileID.None || !item.noUseGraphic
                || item.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>()
                || item.DamageType == ModContent.GetInstance<TrueMeleeNoSpeedDamageClass>()) {
                item.CWR().GetMeleePrefix = true;
            }
            item.noMelee = true;
            item.noUseGraphic = true;
            item.CWR().IsShootCountCorlUse = true;
            item.shoot = ModContent.ProjectileType<T>();
            if (item.shootSpeed <= 0) {
                //不能让速度模场为0，这会让向量失去方向的性质，从而影响一些刀剑的方向判定
                item.shootSpeed = 0.0001f;
            }
        }

        /// <summary>
        /// 快捷的将一个物品实例设置为手持对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public static void SetHeldProj<T>(this Item item) where T : ModProjectile {
            item.noUseGraphic = true;
            item.CWR().hasHeldNoCanUseBool = true;
            item.CWR().heldProjType = ModContent.ProjectileType<T>();
        }

        /// <summary>
        /// 快捷的将一个物品实例设置为手持对象
        /// </summary>
        public static void SetHeldProj(this Item item, int id) {
            item.noUseGraphic = true;
            item.CWR().hasHeldNoCanUseBool = true;
            item.CWR().heldProjType = id;
        }

        /// <summary>
        /// 快捷的将一个物品实例设置为填装枪类实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public static void SetCartridgeGun<T>(this Item item, int ammoCapacity = 1) where T : ModProjectile {
            item.SetHeldProj<T>();
            item.CWR().HasCartridgeHolder = true;
            item.CWR().AmmoCapacity = ammoCapacity;
        }

        /// <summary>
        /// 复制一个物品的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public static void SetItemCopySD<T>(this Item item) where T : ModItem => item.CloneDefaults(ModContent.ItemType<T>());

        /// <summary>
        /// 弹药是否应该被消耗，先行判断武器自带的消耗抵消比率，再在该基础上计算玩家的额外消耗抵消比率，比如弹药箱加成或者药水
        /// </summary>
        /// <param name="player"></param>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public static bool CanUseAmmoInWeaponShoot(this Player player, Item weapon) {
            bool result = true;
            if (weapon.type != ItemID.None) {
                Item[] magazineContents = weapon.CWR().MagazineContents;
                if (magazineContents.Length <= 0) {
                    return true;
                }
                result = ItemLoader.CanConsumeAmmo(weapon, magazineContents[0], player);
                if (player.IsRangedAmmoFreeThisShot(magazineContents[0])) {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 判断该弹药物品是否应该被视为无限弹药
        /// </summary>
        /// <param name="ammoItem">要检查的弹药物品</param>
        /// <returns>如果弹药物品是无限的，返回<see langword="true"/>；否则返回<see langword="false"/></returns>
        public static bool IsAmmunitionUnlimited(Item ammoItem) {
            bool result = !ammoItem.consumable;
            if (CWRMod.Instance.luiafk != null || CWRMod.Instance.improveGame != null) {
                if (ammoItem.stack >= 3996) {
                    result = true;
                }
            }
            return result;
        }

        public static bool IsRangedAmmoFreeThisShot(this Player player, Item ammo) {
            bool flag2 = false;
            if (player.magicQuiver && ammo.ammo == AmmoID.Arrow && Main.rand.NextBool(5)) {
                flag2 = true;
            }

            if (player.ammoBox && Main.rand.NextBool(5)) {
                flag2 = true;
            }

            if (player.ammoPotion && Main.rand.NextBool(5)) {
                flag2 = true;
            }

            if (player.huntressAmmoCost90 && Main.rand.NextBool(10)) {
                flag2 = true;
            }

            if (player.chloroAmmoCost80 && Main.rand.NextBool(5)) {
                flag2 = true;
            }

            if (player.ammoCost80 && Main.rand.NextBool(5)) {
                flag2 = true;
            }

            if (player.ammoCost75 && Main.rand.NextBool(4)) {
                flag2 = true;
            }

            return flag2;
        }

        /// <summary>
        /// 赋予玩家无敌状态，这个函数与<see cref="Player.SetImmuneTimeForAllTypes(int)"/>类似
        /// </summary>
        /// <param name="player">要赋予无敌状态的玩家</param>
        /// <param name="blink">是否允许玩家在无敌状态下闪烁默认为 false</param>
        public static void GivePlayerImmuneState(this Player player, int time, bool blink = false) {
            player.immuneNoBlink = !blink;
            player.immune = true;
            player.immuneTime = time;
            for (int k = 0; k < player.hurtCooldowns.Length; k++) {
                player.hurtCooldowns[k] = player.immuneTime;
            }
        }

        /// <summary>
        /// 将热键绑定的提示信息添加到 TooltipLine 列表中
        /// </summary>
        /// <param name="tooltips">要添加提示信息的 TooltipLine 列表</param>
        /// <param name="mhk">Mod 热键绑定</param>
        /// <param name="keyName">替换的关键字，默认为 "[KEY]"</param>
        /// <param name="modName">Mod 的名称，默认为 "Terraria"</param>
        public static void SetHotkey(this List<TooltipLine> tooltips, ModKeybind mhk, string keyName = "[KEY]", string modName = "Terraria") {
            if (Main.dedServ || mhk is null) {
                return;
            }

            string finalKey = mhk.TooltipHotkeyString();
            tooltips.ReplaceTooltip(keyName, finalKey, modName);
        }

        /// <summary>
        /// 替换 TooltipLine 列表中指定关键字的提示信息
        /// </summary>
        /// <param name="tooltips">要进行替换的 TooltipLine 列表</param>
        /// <param name="targetKeyStr">要替换的关键字</param>
        /// <param name="contentStr">替换后的内容</param>
        /// <param name="modName">Mod 的名称，默认为 "Terraria"</param>
        public static void ReplaceTooltip(this List<TooltipLine> tooltips, string targetKeyStr, string contentStr, string modName = "Terraria") {
            TooltipLine line = tooltips.FirstOrDefault(x => x.Mod == modName && x.Text.Contains(targetKeyStr));
            if (line != null) {
                line.Text = line.Text.Replace(targetKeyStr, contentStr);
            }
        }

        /// <summary>
        /// 快速从模组本地化文件中设置对应物品的名称
        /// </summary>
        /// <param name="item"></param>
        /// <param name="key"></param>
        public static void EasySetLocalTextNameOverride(this Item item, string key) {
            if (Main.GameModeInfo.IsJourneyMode) {
                return;
            }
            item.SetNameOverride(Language.GetText($"Mods.CalamityOverhaul.Items.{key}.DisplayName").Value);
        }

        /// <summary>
        /// 在游戏中发送文本消息
        /// </summary>
        /// <param name="message">要发送的消息文本</param>
        /// <param name="colour">（可选）消息的颜色,默认为 null</param>
        public static void Text(string message, Color? colour = null) {
            Color newColor = (Color)(colour == null ? Color.White : colour);
            if (Main.netMode == NetmodeID.Server) {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), (Color)(colour == null ? Color.White : colour));
                return;
            }
            Main.NewText(message, newColor);
        }

        /// <summary>
        /// 快速修改一个物品的简介文本，从模组本地化文本中拉取资源
        /// </summary>
        public static void OnModifyTooltips(Mod mod, List<TooltipLine> tooltips, string key) {
            List<TooltipLine> newTooltips = new(tooltips);
            List<TooltipLine> overTooltips = [];
            List<TooltipLine> prefixTooltips = [];
            foreach (TooltipLine line in tooltips.ToList()) {//复制 tooltips 集合，以便在遍历时修改
                for (int i = 0; i < 9; i++) {
                    if (line.Name == "Tooltip" + i) {
                        line.Hide();
                    }
                }
                if (line.Name == "CalamityDonor" || line.Name == "CalamityDev") {
                    overTooltips.Add(line.Clone());
                    line.Hide();
                }
                if (line.Name.Contains("Prefix")) {
                    prefixTooltips.Add(line.Clone());
                    line.Hide();
                }
            }

            TooltipLine newLine = new(mod, "CWRText"
                , Language.GetText($"Mods.CalamityOverhaul.Items.{key}.Tooltip").Value);
            newTooltips.Add(newLine);
            newTooltips.AddRange(overTooltips);
            tooltips.Clear(); // 清空原 tooltips 集合
            tooltips.AddRange(newTooltips); // 添加修改后的 newTooltips 集合
            tooltips.AddRange(prefixTooltips);
        }

        /// <summary>
        /// 快速修改一个物品的简介文本，从<see cref="CWRLocText"/>中拉取资源
        /// </summary>
        public static void OnModifyTooltips(Mod mod, List<TooltipLine> tooltips, LocalizedText value) {
            List<TooltipLine> newTooltips = new(tooltips);
            List<TooltipLine> overTooltips = [];
            List<TooltipLine> prefixTooltips = [];
            foreach (TooltipLine line in tooltips.ToList()) {//复制 tooltips 集合，以便在遍历时修改
                for (int i = 0; i < 9; i++) {
                    if (line.Name == "Tooltip" + i) {
                        line.Hide();
                    }
                }
                if (line.Name == "CalamityDonor" || line.Name == "CalamityDev") {
                    overTooltips.Add(line.Clone());
                    line.Hide();
                }
                if (line.Name.Contains("Prefix")) {
                    prefixTooltips.Add(line.Clone());
                    line.Hide();
                }
            }

            TooltipLine newLine = new(mod, "CWRText", value.Value);
            newTooltips.Add(newLine);
            newTooltips.AddRange(overTooltips);
            tooltips.Clear(); // 清空原 tooltips 集合
            tooltips.AddRange(newTooltips); // 添加修改后的 newTooltips 集合
            tooltips.AddRange(prefixTooltips);
        }

        public static TooltipLine Clone(this TooltipLine tooltipLine) {
            Mod mod = CWRMod.Instance;
            foreach (Mod mod1 in ModLoader.Mods) {
                if (mod1.Name == tooltipLine.Mod) {
                    mod = mod1;
                }
            }
            TooltipLine line = new TooltipLine(mod, tooltipLine.Name, tooltipLine.Text) {
                OverrideColor = tooltipLine.OverrideColor,
                IsModifier = tooltipLine.IsModifier,
                IsModifierBad = tooltipLine.IsModifierBad
            };
            return line;
        }

        public static CWRNpc CWR(this NPC npc) {
            return npc.GetGlobalNPC<CWRNpc>();
        }

        public static CWRPlayer CWR(this Player player) {
            return player.GetModPlayer<CWRPlayer>();
        }

        public static CWRItems CWR(this Item item) {
            if (item.type == ItemID.None) {
                Text("ERROR!发生了一次空传递，该物品为None!");
                CWRMod.Instance.Logger.Info("ERROR!发生了一次空传递，该物品为None!");
                return null;
            }
            return item.GetGlobalItem<CWRItems>();
        }

        public static CWRProjectile CWR(this Projectile projectile) {
            return projectile.GetGlobalProjectile<CWRProjectile>();
        }

        public static void initialize(this Item item) {
            if (item.CWR().ai == null) {
                item.CWR().ai = [0, 0, 0];
            }
        }

        #endregion

        #region MathUtils

        public static Vector2 randVr(int min, int max) {
            return Main.rand.NextVector2Unit() * Main.rand.Next(min, max);
        }

        public static Vector2 randVr(int max) {
            return Main.rand.NextVector2Unit() * Main.rand.Next(0, max);
        }

        public static Vector2 randVr(float min, float max) {
            return Main.rand.NextVector2Unit() * Main.rand.NextFloat(min, max);
        }

        public static Vector2 randVr(float max) {
            return Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, max);
        }

        public static float GetCorrectRadian(float minusRadian) {
            return minusRadian < 0 ? (MathHelper.TwoPi + minusRadian) / MathHelper.TwoPi : minusRadian / MathHelper.TwoPi;
        }

        public static T[] FastUnion<T>(this T[] front, T[] back) {
            T[] combined = new T[front.Length + back.Length];

            Array.Copy(front, combined, front.Length);
            Array.Copy(back, 0, combined, front.Length, back.Length);

            return combined;
        }

        public static Matrix GetTransfromMatrix() {
            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.ToVector3());
            Matrix view = Main.GameViewMatrix.TransformationMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            return world * view * projection;
        }

        /// <summary>
        /// 生成一组不重复的随机数集合，数字的数量不能大于取值范围
        /// </summary>
        /// <param name="count">集合元素数量</param>
        /// <param name="minValue">元素最小值</param>
        /// <param name="maxValue">元素最大值</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<int> GenerateUniqueNumbers(int count, int minValue, int maxValue) {
            if (count > maxValue - minValue + 1) {
                throw new ArgumentException("Count of unique numbers cannot be greater than the range of values.");
            }

            List<int> uniqueNumbers = [];
            HashSet<int> usedNumbers = [];

            for (int i = minValue; i <= maxValue; i++) {
                _ = usedNumbers.Add(i);
            }

            for (int i = 0; i < count; i++) {
                int randomIndex = Main.rand.Next(usedNumbers.Count);
                int randomNumber = usedNumbers.ElementAt(randomIndex);
                _ = usedNumbers.Remove(randomNumber);
                uniqueNumbers.Add(randomNumber);
            }

            return uniqueNumbers;
        }

        public static float RotTowards(this float curAngle, float targetAngle, float maxChange) {
            curAngle = MathHelper.WrapAngle(curAngle);
            targetAngle = MathHelper.WrapAngle(targetAngle);
            if (curAngle < targetAngle) {
                if (targetAngle - curAngle > (float)Math.PI) {
                    curAngle += (float)Math.PI * 2f;
                }
            }
            else if (curAngle - targetAngle > (float)Math.PI) {
                curAngle -= (float)Math.PI * 2f;
            }

            curAngle += MathHelper.Clamp(targetAngle - curAngle, 0f - maxChange, maxChange);
            return MathHelper.WrapAngle(curAngle);
        }

        /// <summary>
        /// 比较两个角度之间的差异，将结果限制在 -π 到 π 的范围内
        /// </summary>
        /// <param name="baseAngle">基准角度（参考角度）</param>
        /// <param name="targetAngle">目标角度（待比较角度）</param>
        /// <returns>从基准角度到目标角度的差异，范围在 -π 到 π 之间</returns>
        public static float CompareAngle(float baseAngle, float targetAngle) {
            return ((baseAngle - targetAngle + ((float)Math.PI * 3)) % MathHelper.TwoPi) - (float)Math.PI;// 计算两个角度之间的差异并将结果限制在 -π 到 π 的范围内
        }

        /// <summary>
        /// 色彩混合
        /// </summary>
        public static Color RecombinationColor(params (Color color, float weight)[] colorWeightPairs) {
            Vector4 result = Vector4.Zero;

            for (int i = 0; i < colorWeightPairs.Length; i++) {
                result += colorWeightPairs[i].color.ToVector4() * colorWeightPairs[i].weight;
            }

            return new Color(result);
        }

        /// <summary>
        /// 获取一个随机方向的向量
        /// </summary>
        /// <param name="startAngle">开始角度,输入角度单位的值</param>
        /// <param name="targetAngle">目标角度,输入角度单位的值</param>
        /// <param name="ModeLength">返回的向量的长度</param>
        /// <returns></returns>
        public static Vector2 GetRandomVevtor(float startAngle, float targetAngle, float ModeLength) {
            float angularSeparation = targetAngle - startAngle;
            float randomPosx = ((angularSeparation * Main.rand.NextFloat()) + startAngle) * (MathHelper.Pi / 180);
            float cosValue = MathF.Cos(randomPosx);
            float sinValue = MathF.Sin(randomPosx);

            return new Vector2(cosValue, sinValue) * ModeLength;
        }

        /// <summary>
        /// 计算两个向量的点积
        /// </summary>
        public static float DotProduct(this Vector2 vr1, Vector2 vr2) {
            return (vr1.X * vr2.X) + (vr1.Y * vr2.Y);
        }

        /// <summary>
        /// 检测索引的合法性
        /// </summary>
        /// <returns>合法将返回 <see cref="true"/></returns>
        public static bool ValidateIndex(this int index, Array array) {
            return index >= 0 && index < array.Length;
        }

        /// <summary>
        /// 检测索引的合法性
        /// </summary>
        public static bool ValidateIndex(this int index, int cap) {
            return index >= 0 && index < cap;
        }

        /// <summary>
        /// 会自动替补-1元素
        /// </summary>
        /// <param name="list">目标集合</param>
        /// <param name="valueToAdd">替换为什么值</param>
        /// <param name="valueToReplace">替换的目标对象的值，不填则默认为-1</param>
        public static void AddOrReplace(this List<int> list, int valueToAdd, int valueToReplace = -1) {
            int index = list.IndexOf(valueToReplace);
            if (index >= 0) {
                list[index] = valueToAdd;
            }
            else {
                list.Add(valueToAdd);
            }
        }

        /// <summary>
        /// 返回一个集合的筛选副本，排除数默认为-1，该扩展方法不会影响原集合
        /// </summary>
        public static List<int> GetIntList(this List<int> list, int valueToReplace = -1) {
            List<int> result = new(list);
            _ = result.RemoveAll(item => item == -1);
            return result;
        }

        /// <summary>
        /// 委托，用于定义曲线的缓动函数
        /// </summary>
        /// <param name="progress">进度，范围在0到1之间。</param>
        /// <param name="polynomialDegree">如果缓动模式是多项式，则此为多项式的阶数。</param>
        /// <returns>给定进度下的曲线值。</returns>
        public delegate float CurveEasingFunction(float progress, int polynomialDegree);

        /// <summary>
        /// 表示分段函数的一部分
        /// </summary>
        public struct AnimationCurvePart
        {
            /// <summary>
            /// 用于该段的缓动函数类型
            /// </summary>
            public CurveEasingFunction CurveEasingFunction { get; }

            /// <summary>
            /// 段在动画中的起始位置
            /// </summary>
            public float StartX { get; internal set; }

            /// <summary>
            /// 段的起始高度
            /// </summary>
            public float StartHeight { get; }

            /// <summary>
            /// 段内的高度变化量设为0时段为平直线通常在段末应用，但sinebump缓动类型在曲线顶点应用
            /// </summary>
            public float HeightShift { get; }

            /// <summary>
            /// 如果选择的缓动模式是多项式，则此为多项式的阶数
            /// </summary>
            public int PolynomialDegree { get; }

            /// <summary>
            /// 在考虑高度变化后的段结束高度
            /// </summary>
            public float EndHeight => StartHeight + HeightShift;

            public struct StartData
            {
                public float startX;
                public float startHeight;
                public float heightShift;
                public int degree = 1;

                public StartData() { }
            }

            public AnimationCurvePart(CurveEasingFunction curveEasingFunction
                , float startX, float startHeight, float heightShift, int degree = 1) {
                CurveEasingFunction = curveEasingFunction;
                StartX = startX;
                StartHeight = startHeight;
                HeightShift = heightShift;
                PolynomialDegree = degree;
            }

            public AnimationCurvePart(CurveEasingFunction curveEasingFunction, StartData starData) {
                CurveEasingFunction = curveEasingFunction;
                StartX = starData.startX;
                StartHeight = starData.startHeight;
                HeightShift = starData.heightShift;
                PolynomialDegree = starData.degree;
            }
        }

        /// <summary>
        /// 获取自定义分段函数在任意给定X值的高度，使您可以轻松创建复杂的动画曲线。X值自动限定在0到1之间，但函数高度可以超出0到1的范围。
        /// </summary>
        /// <param name="progress">曲线进度。自动限定在0到1之间。</param>
        /// <param name="segments">构成完整动画曲线的曲线段数组。</param>
        /// <returns>给定X值的函数高度。</returns>
        public static float EvaluateCurve(float progress, params AnimationCurvePart[] segments) {
            if (segments.Length == 0) {
                return 0f;
            }

            if (segments[0].StartX != 0) {
                segments[0].StartX = 0;
            }

            progress = MathHelper.Clamp(progress, 0f, 1f); // 限定进度在0到1之间
            float height = 0f;

            for (int i = 0; i < segments.Length; i++) {
                AnimationCurvePart segment = segments[i];
                float startX = segment.StartX;
                float endX = (i < segments.Length - 1) ? segments[i + 1].StartX : 1f;

                if (progress < startX) {
                    continue;
                }


                if (progress >= endX) {
                    continue;
                }

                float segmentProgress = (progress - startX) / (endX - startX); // 计算段内进度
                height = segment.StartHeight + segment.CurveEasingFunction(segmentProgress, segment.PolynomialDegree) * segment.HeightShift;
                break;
            }
            return height;
        }


        public const float TwoPi = MathF.PI * 2;
        public const float FourPi = MathF.PI * 4;
        public const float ThreePi = MathF.PI * 3;
        public const float PiOver3 = MathF.PI / 3f;
        public const float PiOver5 = MathF.PI / 5f;
        public const float PiOver6 = MathF.PI / 6f;

        #endregion

        #region DrawUtils

        #region 普通绘制工具
        /// <summary>
        /// 设置裁切效果
        /// </summary>
        /// <param name="value"></param>
        /// <param name="deductRec"></param>
        /// <returns></returns>
        public static Effect SetDeductEffect(Texture2D value, Rectangle deductRec) {
            Effect effect = GetEffectValue("DeductDraw");
            effect.CurrentTechnique.Passes[0].Apply();
            effect.Parameters["topLeft"].SetValue(deductRec.TopLeft());
            effect.Parameters["width"].SetValue(deductRec.Width);
            effect.Parameters["height"].SetValue(deductRec.Height);
            effect.Parameters["textureSize"].SetValue(value.Size());
            return effect;
        }

        /// <summary>
        /// 安全的获取对应实例的图像资源
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Texture2D T2DValue(this Projectile p, bool loadCeahk = true) {
            if (Main.dedServ) {
                return null;
            }
            if (p.type < 0 || p.type >= TextureAssets.Projectile.Length) {
                return null;
            }
            if (loadCeahk && p.ModProjectile == null) {
                Main.instance.LoadProjectile(p.type);
            }

            return TextureAssets.Projectile[p.type].Value;
        }

        /// <summary>
        /// 安全的获取对应实例的图像资源
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Texture2D T2DValue(this Item i, bool loadCeahk = true) {
            if (Main.dedServ) {
                return null;
            }
            if (i.type < ItemID.None || i.type >= TextureAssets.Item.Length) {
                return null;
            }
            if (loadCeahk && i.ModItem == null) {
                Main.instance.LoadItem(i.type);
            }

            return TextureAssets.Item[i.type].Value;
        }

        /// <summary>
        /// 安全的获取对应实例的图像资源
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Texture2D T2DValue(this NPC n, bool loadCeahk = true) {
            if (Main.dedServ) {
                return null;
            }
            if (n.type < NPCID.None || n.type >= TextureAssets.Npc.Length) {
                return null;
            }
            if (loadCeahk && n.ModNPC == null) {
                Main.instance.LoadNPC(n.type);
            }

            return TextureAssets.Npc[n.type].Value;
        }

        /// <summary>
        /// 获取指定路径的纹理实例 <see cref="Texture2D"/>
        /// </summary>
        /// <param name="texture">纹理路径（相对于模组内容目录的路径）</param>
        /// <param name="immediateLoad">
        /// 是否立即加载纹理：
        /// <br>- <see langword="true"/>：同步加载纹理（适合需要立即使用的资源）</br>
        /// <br>- <see langword="false"/>：异步加载纹理（提升加载性能，适合非紧急资源）</br>
        /// </param>
        /// <returns>返回加载的 Texture2D 实例</returns>
        public static Texture2D GetT2DValue(string texture, bool immediateLoad = false) {
            return ModContent.Request<Texture2D>(texture
                , immediateLoad ? AssetRequestMode.ImmediateLoad : AssetRequestMode.AsyncLoad).Value;
        }

        /// <summary>
        /// 获取指定路径的纹理资源（类型为 Asset&lt;Texture2D&gt;）
        /// </summary>
        /// <param name="texture">纹理路径（相对于模组内容目录的路径）</param>
        /// <param name="immediateLoad">
        /// 是否立即加载纹理：
        /// <br>- <see langword="true"/>：同步加载纹理（适合需要立即使用的资源）</br>
        /// <br>- <see langword="false"/>：异步加载纹理（提升加载性能，适合非紧急资源）</br>
        /// </param>
        /// <returns>返回加载的 Asset&lt;Texture2D&gt; 对象，包含纹理资源及其加载状态</returns>
        public static Asset<Texture2D> GetT2DAsset(string texture, bool immediateLoad = false) {
            return ModContent.Request<Texture2D>(texture
                , immediateLoad ? AssetRequestMode.ImmediateLoad : AssetRequestMode.AsyncLoad);
        }

        /// <summary>
        /// 获取与纹理大小对应的矩形框
        /// </summary>
        /// <param name="value">纹理对象</param>
        public static Rectangle GetRec(Texture2D value) {
            return new Rectangle(0, 0, value.Width, value.Height);
        }
        /// <summary>
        /// 获取与纹理大小对应的矩形框
        /// </summary>
        /// <param name="value">纹理对象</param>
        /// <param name="Dx">X起点</param>
        /// <param name="Dy">Y起点</param>
        /// <param name="Sx">宽度</param>
        /// <param name="Sy">高度</param>
        /// <returns></returns>
        public static Rectangle GetRec(Texture2D value, int Dx, int Dy, int Sx, int Sy) {
            return new Rectangle(Dx, Dy, Sx, Sy);
        }
        /// <summary>
        /// 获取与纹理大小对应的矩形框
        /// </summary>
        /// <param name="value">纹理对象</param>
        /// <param name="frame">帧索引</param>
        /// <param name="frameCounterMax">总帧数，该值默认为1</param>
        /// <returns></returns>
        public static Rectangle GetRec(Texture2D value, int frame, int frameCounterMax = 1) {
            int singleFrameY = value.Height / frameCounterMax;
            return new Rectangle(0, singleFrameY * frame, value.Width, singleFrameY);
        }
        /// <summary>
        /// 获取与纹理大小对应的缩放中心
        /// </summary>
        /// <param name="value">纹理对象</param>
        /// <returns></returns>
        public static Vector2 GetOrig(Texture2D value) {
            return new Vector2(value.Width, value.Height) * 0.5f;
        }
        /// <summary>
        /// 获取与纹理大小对应的缩放中心
        /// </summary>
        /// <param name="value">纹理对象</param>
        /// <param name="frameCounter">帧索引</param>
        /// <param name="frameCounterMax">总帧数，该值默认为1</param>
        /// <returns></returns>
        public static Vector2 GetOrig(Texture2D value, int frameCounterMax = 1) {
            float singleFrameY = value.Height / frameCounterMax;
            return new Vector2(value.Width * 0.5f, singleFrameY / 2);
        }
        /// <summary>
        /// 对帧数索引进行走表
        /// </summary>
        /// <param name="frameCounter"></param>
        /// <param name="intervalFrame"></param>
        /// <param name="Maxframe"></param>
        public static void ClockFrame(ref int frameCounter, int intervalFrame, int maxFrame) {
            if (Main.GameUpdateCount % intervalFrame == 0) {
                frameCounter++;
            }

            if (frameCounter > maxFrame) {
                frameCounter = 0;
            }
        }
        /// <summary>
        /// 对帧数索引进行走表
        /// </summary>
        /// <param name="frameCounter"></param>
        /// <param name="intervalFrame"></param>
        /// <param name="Maxframe"></param>
        /// <param name="startCounter"></param>
        public static void ClockFrame(ref double frameCounter, int intervalFrame, int maxFrame, int startCounter = 0) {
            if (Main.GameUpdateCount % intervalFrame == 0) {
                frameCounter++;
            }

            if (frameCounter > maxFrame) {
                frameCounter = startCounter;
            }
        }

        /// <summary>
        /// 便捷的获取模组内的Effect实例
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Effect GetEffectValue(string name, bool immediateLoad = false) {
            return CWRMod.Instance.Assets.Request<Effect>(CWRConstant.noEffects + name
                , immediateLoad ? AssetRequestMode.ImmediateLoad : AssetRequestMode.AsyncLoad).Value;
        }

        #endregion

        #region 高级绘制工具

        /// <summary>
        /// 使用反射来设置 _uImage1。它的底层数据是私有的，唯一可以公开更改它的方式是通过一个只接受原始纹理路径的方法
        /// </summary>
        /// <param name="shader">着色器</param>
        /// <param name="texture">要使用的纹理</param>
        public static void SetMiscShaderAsset_1(this MiscShaderData shader, Asset<Texture2D> texture) {
            EffectLoader.Shader_Texture_FieldInfo_1.SetValue(shader, texture);
        }

        /// <summary>
        /// 使用反射来设置 _uImage2。它的底层数据是私有的，唯一可以公开更改它的方式是通过一个只接受原始纹理路径的方法
        /// </summary>
        /// <param name="shader">着色器</param>
        /// <param name="texture">要使用的纹理</param>
        public static void SetMiscShaderAsset_2(this MiscShaderData shader, Asset<Texture2D> texture) {
            EffectLoader.Shader_Texture_FieldInfo_2.SetValue(shader, texture);
        }

        /// <summary>
        /// 使用反射来设置 _uImage3。它的底层数据是私有的，唯一可以公开更改它的方式是通过一个只接受原始纹理路径的方法
        /// </summary>
        /// <param name="shader">着色器</param>
        /// <param name="texture">要使用的纹理</param>
        public static void SetMiscShaderAsset_3(this MiscShaderData shader, Asset<Texture2D> texture) {
            EffectLoader.Shader_Texture_FieldInfo_3.SetValue(shader, texture);
        }

        /// <summary>
        /// 任意设置 <see cref=" SpriteBatch "/> 的 <see cref=" BlendState "/>
        /// </summary>
        /// <param name="spriteBatch">绘制模式</param>
        /// <param name="blendState">要使用的混合状态</param>
        public static void ModifyBlendState(this SpriteBatch spriteBatch, BlendState blendState) {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, blendState, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        /// <summary>
        /// 将 <see cref="SpriteBatch"/> 的 <see cref="BlendState"/> 重置为典型的 <see cref="BlendState.AlphaBlend"/>。
        /// </summary>
        /// <param name="spriteBatch">绘制模式</param>
        public static void ResetBlendState(this SpriteBatch spriteBatch) {
            spriteBatch.ModifyBlendState(BlendState.AlphaBlend);
        }

        /// <summary>
        /// 将 <see cref="SpriteBatch"/> 的 <see cref="BlendState"/> 设置为 <see cref="BlendState.Additive"/>。
        /// </summary>
        /// <param name="spriteBatch">绘制模式</param>
        public static void SetAdditiveState(this SpriteBatch spriteBatch) {
            spriteBatch.ModifyBlendState(BlendState.Additive);
        }

        /// <summary>
        /// 将 <see cref="SpriteBatch"/> 重置为无效果的UI画布状态，在大多数情况下，这个适合结束一段在UI中的绘制
        /// </summary>
        /// <param name="spriteBatch">绘制模式</param>
        public static void ResetUICanvasState(this SpriteBatch spriteBatch) {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(0, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
        }
        #endregion

        #endregion

        #region TileUtils
        public static void SafeSquareTileFrame(int x, int y, bool resetFrame = true)
            => SafeSquareTileFrame(new Point(x, y), resetFrame);

        public static void SafeSquareTileFrame(Vector2 tilePos, bool resetFrame = true)
            => SafeSquareTileFrame(new Point((int)tilePos.X, (int)tilePos.Y), resetFrame);

        public static void SafeSquareTileFrame(Point tilePos, bool resetFrame = true) {
            int i = tilePos.X;
            int j = tilePos.Y;
            TMLPatchedTileUtils.TileFrame(i - 1, j - 1);
            TMLPatchedTileUtils.TileFrame(i - 1, j);
            TMLPatchedTileUtils.TileFrame(i - 1, j + 1);
            TMLPatchedTileUtils.TileFrame(i, j - 1);
            try {
                TMLPatchedTileUtils.TileFrame(i, j, resetFrame);
            } catch {
                TMLPatchedTileUtils.DoErrorTile(tilePos, Main.tile[tilePos.X, tilePos.Y]);
                return;
            }
            TMLPatchedTileUtils.TileFrame(i, j + 1);
            TMLPatchedTileUtils.TileFrame(i + 1, j - 1);
            TMLPatchedTileUtils.TileFrame(i + 1, j);
            TMLPatchedTileUtils.TileFrame(i + 1, j + 1);
        }

        /// <summary>
        /// 将可能越界的方块坐标收值为非越界坐标
        /// </summary>
        public static Vector2 PTransgressionTile(Vector2 TileVr, int L = 0, int R = 0, int D = 0, int S = 0) {
            if (TileVr.X > Main.maxTilesX - R) {
                TileVr.X = Main.maxTilesX - R;
            }
            if (TileVr.X < 0 + L) {
                TileVr.X = 0 + L;
            }
            if (TileVr.Y > Main.maxTilesY - S) {
                TileVr.Y = Main.maxTilesY - S;
            }
            if (TileVr.Y < 0 + D) {
                TileVr.Y = 0 + D;
            }
            return new Vector2(TileVr.X, TileVr.Y);
        }

        /// <summary>
        /// 检测该位置是否存在一个实心的固体方块
        /// </summary>
        public static bool HasSolidTile(this Tile tile) {
            return tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType];
        }

        /// <summary>
        /// 获取一个物块目标，输入世界物块坐标，自动考虑收界情况
        /// </summary>
        public static Tile GetTile(int i, int j) {
            return GetTile(new Vector2(i, j));
        }

        /// <summary>
        /// 获取一个物块目标，输入世界物块坐标，自动考虑收界情况
        /// </summary>
        public static Tile GetTile(Vector2 pos) {
            pos = PTransgressionTile(pos);
            return Main.tile[(int)pos.X, (int)pos.Y];
        }

        #endregion

        #region SoundUtils

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="pos">声音播放的位置</param>
        /// <param name="sound">要播放的声音样式（SoundStyle）</param>
        /// <param name="volume">声音的音量</param>
        /// <param name="pitch">声音的音调</param>
        /// <param name="pitchVariance">音调的变化范围</param>
        /// <param name="maxInstances">最大实例数，允许同时播放的声音实例数量</param>
        /// <param name="soundLimitBehavior">声音限制行为，用于控制当达到最大实例数时的行为</param>
        /// <returns>返回声音实例的索引</returns>
        public static SlotId SoundPlayer(
            Vector2 pos,
            SoundStyle sound,
            float volume = 1,
            float pitch = 1,
            float pitchVariance = 1,
            int maxInstances = 1,
            SoundLimitBehavior soundLimitBehavior = SoundLimitBehavior.ReplaceOldest
            ) {
            sound = sound with {
                Volume = volume,
                Pitch = pitch,
                PitchVariance = pitchVariance,
                MaxInstances = maxInstances,
                SoundLimitBehavior = soundLimitBehavior
            };

            SlotId sid = SoundEngine.PlaySound(sound, pos);
            return sid;
        }

        /// <summary>
        /// 更新声音位置
        /// </summary>
        public static void PanningSound(Vector2 pos, SlotId sid) {
            if (!SoundEngine.TryGetActiveSound(sid, out ActiveSound activeSound)) {
                return;
            }
            else {
                activeSound.Position = pos;
            }
        }

        #endregion
    }
}
