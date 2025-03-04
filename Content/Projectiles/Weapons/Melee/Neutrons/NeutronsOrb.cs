﻿using CalamityOverhaul.Content.PRTTypes;
using InnoVault.PRT;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Melee.Neutrons
{
    internal class NeutronsOrb : ModProjectile
    {
        public override string Texture => CWRConstant.Placeholder;
        public override void SetDefaults() {
            Projectile.width = Projectile.height = 22;
            Projectile.timeLeft = 120;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI() {
            BasePRT spark = new PRT_HeavenfallStar(Projectile.Center, Projectile.velocity, false, 17, Main.rand.NextFloat(0.2f, 0.3f), Color.BlueViolet);
            PRTLoader.AddParticle(spark);
        }
    }
}
