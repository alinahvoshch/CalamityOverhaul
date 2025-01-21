﻿using CalamityMod.Items.Weapons.Magic;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Magic;
using CalamityOverhaul.Content.RemakeItems.Core;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Magic
{
    internal abstract class RMagicStaff<TItem> : BaseRItem where TItem : ModItem
    {
        public override bool DrawingInfo => false;
        public override bool FormulaSubstitution => true;
        public override int TargetID => ModContent.ItemType<TItem>();
        public override void SetDefaults(Item item) => item.SetHeldProj(CWRMod.Instance.Find<ModProjectile>(typeof(TItem).Name + "Held").Type);
    }

    internal abstract class BaseMagicStaff<TItem> : BaseMagicGun where TItem : ModItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Magic + typeof(TItem).Name;
        public override int targetCayItem => ModContent.ItemType<TItem>();
        public override int targetCWRItem => CWRServerConfig.Instance.WeaponOverhaul
            ? ItemID.None : CWRMod.Instance.Find<ModItem>(typeof(TItem).Name + "EcType").Type;
        private int useAnimation;
        public sealed override void SetMagicProperty() {
            ShootPosToMouLengValue = 0;
            ShootPosNorlLengValue = 0;
            HandFireDistanceX = 0;
            HandFireDistanceY = 0;
            InOwner_HandState_AlwaysSetInFireRoding = true;
            Onehanded = true;
            GunPressure = 0;
            ControlForce = 0;
            Recoil = 0;
            SetStaffProperty();
        }

        public override void Initialize() => useAnimation = Item.useAnimation;

        public virtual void SetStaffProperty() {

        }

        public override void HanderPlaySound() {
            if (Item.ModItem != null && !Item.ModItem.CanUseItem(Owner)) {
                return;
            }
            useAnimation -= Item.useTime;
            if (useAnimation <= 0) {
                SoundEngine.PlaySound(Item.UseSound, Projectile.Center);
                useAnimation = Item.useAnimation;
            }
        }

        public override void FiringShoot() {
            if (Item.ModItem != null && !Item.ModItem.CanUseItem(Owner)) {
                return;
            }
            OrigItemShoot();
        }

        public override void FiringShootR() {
            if (Item.ModItem != null && !Item.ModItem.CanUseItem(Owner)) {
                return;
            }
            Owner.altFunctionUse = 2;
            OrigItemShoot();
        }

        public override void GunDraw(Vector2 drawPos, ref Color lightColor) {
            float rot = DirSign > 0 ? MathHelper.PiOver4 : -MathHelper.PiOver4;
            float offsetRot = DrawGunBodyRotOffset * (DirSign > 0 ? 1 : -1);
            Vector2 orig = DirSign > 0 ? new Vector2(0, TextureValue.Height) : new Vector2(0, 0);
            Main.EntitySpriteDraw(TextureValue, drawPos, null, lightColor
                , Projectile.rotation + offsetRot + rot, orig, Projectile.scale
                , DirSign > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        }
    }

    //internal class ArtAttackHeld : BaseMagicStaff<ArtAttack> { }
    //internal class ArtAttackRItem : RMagicStaff<ArtAttack> { }

    internal class AsteroidStaffHeld : BaseMagicStaff<AsteroidStaff> { }
    internal class AsteroidStaffRItem : RMagicStaff<AsteroidStaff> { }

    internal class AstralachneaStaffHeld : BaseMagicStaff<AstralachneaStaff> { }
    internal class AstralachneaStaffRItem : RMagicStaff<AstralachneaStaff> { }

    internal class AstralStaffHeld : BaseMagicStaff<AstralStaff> { }
    internal class AstralStaffRItem : RMagicStaff<AstralStaff> { }

    internal class AtlantisHeld : BaseMagicStaff<Atlantis> { }
    internal class AtlantisRItem : RMagicStaff<Atlantis> { }

    internal class BloodBathHeld : BaseMagicStaff<BloodBath> { }
    internal class BloodBathRItem : RMagicStaff<BloodBath> { }

    internal class BrimroseStaffHeld : BaseMagicStaff<BrimroseStaff> { }
    internal class BrimroseStaffRItem : RMagicStaff<BrimroseStaff> { }
    //这个东西暂时没有被制作进游戏
    //internal class CarnageRayHeld : BaseMagicStaff<CarnageRay> { }
    //internal class CarnageRayRItem : RMagicStaff<CarnageRay> { }

    internal class ClamorNoctusHeld : BaseMagicStaff<ClamorNoctus> { }
    internal class ClamorNoctusRItem : RMagicStaff<ClamorNoctus> { }

    internal class DeathhailStaffHeld : BaseMagicStaff<DeathhailStaff> { }
    internal class DeathhailStaffRItem : RMagicStaff<DeathhailStaff> { }

    internal class DivineRetributionHeld : BaseMagicStaff<DivineRetribution> { }
    internal class DivineRetributionRItem : RMagicStaff<DivineRetribution> { }

    internal class DownpourHeld : BaseMagicStaff<Downpour> { }
    internal class DownpourRItem : RMagicStaff<Downpour> { }

    internal class EidolonStaffHeld : BaseMagicStaff<EidolonStaff> { }
    internal class EidolonStaffRItem : RMagicStaff<EidolonStaff> { }

    //internal class ElementalRayHeld : BaseMagicStaff<ElementalRay> { }
    //internal class ElementalRayRItem : RMagicStaff<ElementalRay> { }

    internal class FabstaffHeld : BaseMagicStaff<Fabstaff> { }
    internal class FabstaffRItem : RMagicStaff<Fabstaff> { }

    internal class GleamingMagnoliaHeld : BaseMagicStaff<GleamingMagnolia> { }
    internal class GleamingMagnoliaRItem : RMagicStaff<GleamingMagnolia> { }

    internal class HeliumFlashHeld : BaseMagicStaff<HeliumFlash> { }
    internal class HeliumFlashRItem : RMagicStaff<HeliumFlash> { }

    internal class HellwingStaffHeld : BaseMagicStaff<HellwingStaff> { }
    internal class HellwingStaffRItem : RMagicStaff<HellwingStaff> { }

    internal class HematemesisHeld : BaseMagicStaff<Hematemesis> { }
    internal class HematemesisRItem : RMagicStaff<Hematemesis> { }

    internal class HyphaeRodHeld : BaseMagicStaff<HyphaeRod> { }
    internal class HyphaeRodRItem : RMagicStaff<HyphaeRod> { }

    //internal class IceBarrageHeld : BaseMagicStaff<IceBarrage> { }
    //internal class IceBarrageRItem : RMagicStaff<IceBarrage> { }

    internal class IcicleStaffHeld : BaseMagicStaff<IcicleStaff> { }
    internal class IcicleStaffRItem : RMagicStaff<IcicleStaff> { }

    internal class IcicleTridentHeld : BaseMagicStaff<IcicleTrident> { }
    internal class IcicleTridentRItem : RMagicStaff<IcicleTrident> { }

    //internal class InfernalRiftHeld : BaseMagicStaff<InfernalRift> { }
    //internal class InfernalRiftRItem : RMagicStaff<InfernalRift> { }

    internal class KeelhaulHeld : BaseMagicStaff<Keelhaul> { }
    internal class KeelhaulRItem : RMagicStaff<Keelhaul> { }

    internal class ManaRoseHeld : BaseMagicStaff<ManaRose> { }
    internal class ManaRoseRItem : RMagicStaff<ManaRose> { }

    internal class MiasmaHeld : BaseMagicStaff<Miasma> { }
    internal class MiasmaRItem : RMagicStaff<Miasma> { }

    internal class MistlestormHeld : BaseMagicStaff<Mistlestorm> { }
    internal class MistlestormRItem : RMagicStaff<Mistlestorm> { }

    internal class NightsRayHeld : BaseMagicStaff<NightsRay> { }
    internal class NightsRayRItem : RMagicStaff<NightsRay> { }

    internal class ParasiticSceptorHeld : BaseMagicStaff<ParasiticSceptor> { }
    internal class ParasiticSceptorRItem : RMagicStaff<ParasiticSceptor> { }

    internal class PhantasmalFuryHeld : BaseMagicStaff<PhantasmalFury> { }
    internal class PhantasmalFuryRItem : RMagicStaff<PhantasmalFury> { }

    internal class PhotosynthesisHeld : BaseMagicStaff<Photosynthesis> { }
    internal class PhotosynthesisRItem : RMagicStaff<Photosynthesis> { }

    internal class PlagueStaffHeld : BaseMagicStaff<PlagueStaff> { }
    internal class PlagueStaffRItem : RMagicStaff<PlagueStaff> { }

    internal class PlasmaRodHeld : BaseMagicStaff<PlasmaRod> { }
    internal class PlasmaRodRItem : RMagicStaff<PlasmaRod> { }

    internal class SandstreamScepterHeld : BaseMagicStaff<SandstreamScepter> { }
    internal class SandstreamScepterRItem : RMagicStaff<SandstreamScepter> { }

    internal class SanguineFlareHeld : BaseMagicStaff<SanguineFlare> { }
    internal class SanguineFlareRItem : RMagicStaff<SanguineFlare> { }

    internal class ShaderainStaffHeld : BaseMagicStaff<ShaderainStaff> { }
    internal class ShaderainStaffRItem : RMagicStaff<ShaderainStaff> { }

    internal class ShadowboltStaffHeld : BaseMagicStaff<ShadowboltStaff> { }
    internal class ShadowboltStaffRItem : RMagicStaff<ShadowboltStaff> { }

    //internal class ShiftingSandsHeld : BaseMagicStaff<ShiftingSands> { }
    //internal class ShiftingSandsRItem : RMagicStaff<ShiftingSands> { }

    internal class SkyGlazeHeld : BaseMagicStaff<SkyGlaze> { }
    internal class SkyGlazeRItem : RMagicStaff<SkyGlaze> { }

    internal class SnowstormStaffHeld : BaseMagicStaff<SnowstormStaff> { }
    internal class SnowstormStaffRItem : RMagicStaff<SnowstormStaff> { }

    internal class SoulPiercerHeld : BaseMagicStaff<SoulPiercer> { }
    internal class SoulPiercerRItem : RMagicStaff<SoulPiercer> { }

    //internal class StaffofBlushieHeld : BaseMagicStaff<StaffofBlushie> { }
    //internal class StaffofBlushieRItem : RMagicStaff<StaffofBlushie> { }

    internal class TacticiansTrumpCardHeld : BaseMagicStaff<TacticiansTrumpCard> { }
    internal class TacticiansTrumpCardRItem : RMagicStaff<TacticiansTrumpCard> { }

    internal class TeslastaffHeld : BaseMagicStaff<Teslastaff> { }
    internal class TeslastaffRItem : RMagicStaff<Teslastaff> { }

    internal class TheWandHeld : BaseMagicStaff<TheWand> { }
    internal class TheWandRItem : RMagicStaff<TheWand> { }

    internal class ThornBlossomHeld : BaseMagicStaff<ThornBlossom> { }
    internal class ThornBlossomRItem : RMagicStaff<ThornBlossom> { }

    internal class UltraLiquidatorHeld : BaseMagicStaff<UltraLiquidator> { }
    internal class UltraLiquidatorRItem : RMagicStaff<UltraLiquidator> { }

    internal class UndinesRetributionHeld : BaseMagicStaff<UndinesRetribution> { }
    internal class UndinesRetributionRItem : RMagicStaff<UndinesRetribution> { }

    internal class ValkyrieRayHeld : BaseMagicStaff<ValkyrieRay> { }
    internal class ValkyrieRayRItem : RMagicStaff<ValkyrieRay> { }

    //internal class VehemenceHeld : BaseMagicStaff<Vehemence> { }
    //internal class VehemenceRItem : RMagicStaff<Vehemence> { }

    internal class VenusianTridentHeld : BaseMagicStaff<VenusianTrident> { }
    internal class VenusianTridentRItem : RMagicStaff<VenusianTrident> { }

    internal class VisceraHeld : BaseMagicStaff<Viscera> { }
    internal class VisceraRItem : RMagicStaff<Viscera> { }

    internal class VitriolicViperHeld : BaseMagicStaff<VitriolicViper> { }
    internal class VitriolicViperRItem : RMagicStaff<VitriolicViper> { }

    internal class WyvernsCallHeld : BaseMagicStaff<WyvernsCall> { }
    internal class WyvernsCallRItem : RMagicStaff<WyvernsCall> { }
}
