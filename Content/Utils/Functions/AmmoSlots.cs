using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ReloadableGunsLibrary.Content.Utils.Functions
{
    public class AmmoFinderSystem : ModSystem
    {
        public static int[] GetAllBulletSlots(Player player) {
            int[] foundSlots = [];
            int[] scanOrder = new int[58];
            for (int i = 0; i < 4; i++) scanOrder[i] = 54 + i;
            for (int i = 0; i < 54; i++) scanOrder[i + 4] = i;

            foreach (int slot in scanOrder) {
                Item item = player.inventory[slot];
                if (!item.IsAir && item.ammo == AmmoID.Bullet && item.stack > 0) {
                    foundSlots.Append(slot);
                }
            }
            return foundSlots;
        }
        public static int GetFirstBulletSlot(Player player)
        {
            int[] scanOrder = new int[58];
            for (int i = 0; i < 4; i++) scanOrder[i] = 54 + i;
            for (int i = 0; i < 54; i++) scanOrder[i + 4] = i;
            foreach (int slot in scanOrder) {
                Item item = player.inventory[slot];
                if (!item.IsAir && item.ammo == AmmoID.Bullet && item.stack > 0) {
                    return slot;
                }
            }
            return -1;
        }
    }
}