using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ReloadableGunsLibrary.Content.Utils.Functions;
using ReloadableGunsLibrary.Content.Config;
namespace ReloadableGunsLibrary.Content.Items.Weapons
{
    public class BurstGunExample : ModItem{
        SoundStyle shootSound = new SoundStyle("ReloadableGunsLibrary/Content/Sound/Weapons/BurstGunExampleburst") {
            Volume = 0.8f,
            Pitch = 0.1f,
            MaxInstances = 3 
        };
        SoundStyle reloadSound = new SoundStyle("ReloadableGunsLibrary/Content/Sound/Weapons/BurstGunExamplereload") {
            Volume = 0.8f,
            Pitch = 0.1f,
            MaxInstances = 3
        };
        private ReloadableGun Gun => Item.GetGlobalItem<ReloadableGun>();
        public override void SetDefaults(){

			Item.rare = ItemRarityID.Green;
            Item.useTime = 3;           
            Item.useAnimation = 9;
            Item.reuseDelay = 10;
			Item.useStyle = ItemUseStyleID.Shoot; 
			Item.DamageType = DamageClass.Ranged; 
			Item.damage = 20;
			Item.knockBack = 500f; 
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 20f; 
			Item.useAmmo = AmmoID.None; //We want this to be None since the ammo consumption is donne in the shoot function
            if (Item.TryGetGlobalItem(out ReloadableGun gun)) {
                gun.IsReloadable=true;
                gun.maxAmmo = 15;
                gun.reloadTime = (int)(60 * 1.5);
                gun.reloadSound = reloadSound;
            }
        }
        public override void SetStaticDefaults() {
            Terraria.Localization.Language.GetOrRegister("Mods.ReloadableGunsLibrary.Items.BurstGunExample.DisplayName", () => "BurstGun Example");
        }
        public override bool CanUseItem(Player player) {
            if (Gun.isReloading) return false;
            if (Gun.ammo <= 0) {
                SoundEngine.PlaySound(SoundID.MenuTick, player.position);
                return true;
            }

            return true;
        }
        public override void HoldItem(Player player)
        {
            if (KeybindSystem.Reload.JustPressed) {
                if (!Gun.isReloading && Gun.ammo < Gun.maxAmmo) {
                    Gun.reload(player); 
                }
            }
        }
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
           if (Gun.ammo > 0 && Gun.loadedBullets.Count > 0) {
                Projectile.NewProjectile(source, position, velocity, Gun.loadedBullets[0], damage, knockback, player.whoAmI);
                SoundEngine.PlaySound(shootSound, player.position);
                Gun.loadedBullets.RemoveAt(0);
                Gun.ammo--;
            }
            else {
                SoundEngine.PlaySound(SoundID.MenuTick, player.position);
            }
            return false;
		}
       
        public override void AddRecipes(){
            CreateRecipe()
            .AddIngredient(ItemID.DirtBlock,1)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
        
	}
}