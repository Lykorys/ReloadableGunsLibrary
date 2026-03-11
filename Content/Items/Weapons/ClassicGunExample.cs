using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ReloadableGunsLibrary.Content.Utils.Functions;
using ReloadableGunsLibrary.Content.Config;

namespace ReloadableGunsLibrary.Content.Items.Weapons
{
    public class ClassicGunExample : ModItem{
        SoundStyle shootSound = new SoundStyle("ReloadableGunsLibrary/Content/Sound/Weapons/ClassicGunExampleshoot") {
            Volume = 0.8f,
            Pitch = 0.1f,
            MaxInstances = 3 
        };
        SoundStyle reloadSound = new SoundStyle("ReloadableGunsLibrary/Content/Sound/Weapons/ClassicGunExamplereload") {
            Volume = 0.8f,
            Pitch = 0.1f,
            MaxInstances = 3
        };

        private ReloadableGun Gun => Item.GetGlobalItem<ReloadableGun>();
        public override void SetDefaults(){
			Item.rare = ItemRarityID.Green;
			Item.useTime = 8;
			Item.useAnimation = 8;
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
                gun.maxAmmo = 8;
                gun.reloadTime = (int)(60 * 1.9);
                gun.reloadSound = reloadSound;
            }
        }
        public override void SetStaticDefaults() {
            Terraria.Localization.Language.GetOrRegister("Mods.ReloadableGunsLibrary.Items.ClassicGunExample.DisplayName", () => "ClassicGun Example");
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
            if (Gun.loadedBullets.Count > 0) {
                SoundEngine.PlaySound(shootSound, player.position);
                Projectile.NewProjectile(source, position, velocity, Gun.loadedBullets[0], damage, knockback, player.whoAmI);
                Gun.loadedBullets.RemoveAt(0);
                Gun.ammo--;
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