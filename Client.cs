using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDR2;
using RDR2.Math;
using RDR2.Native;
using RDR2.UI;

namespace ThunderHawk
{
    public class Client : Script
    {
        private Vector3 lastHitPos = Vector3.Zero;
        public Client() { 
            Tick +=OnTick;
            Interval = 1;
        }
        async void delayAction (int delay)
        {
            await Task.Delay(delay);
        }
        private void OnTick(object sender, EventArgs e)
        {
            //if the player is not currently wielding a Tomahawk, the function is skipped
            if (CurrentPlayerWeapon()!=WeaponHash.ThrownTomahawk)
            {
                return;
            }

            var out1 = new OutputArgument();
            Function.Call(Hash.GET_PED_LAST_WEAPON_IMPACT_COORD, Game.Player.Character.Handle, out1);
            var pos = out1.GetResult<Vector3>();

            var out2 = new InputArgument(WeaponHash.ThrownTomahawk);

            if (pos.DistanceTo(Game.Player.Character.Position) < 200f && lastHitPos!=pos)
            {
                Function.Call((Hash)0x67943537D179597C, pos.X,pos.Y, pos.Z);//calling the hash for the thunder to strike
                Function.Call((Hash)0x14E56BC5B5DB6A19, Game.Player.Character.Handle, out2, 2);//replenishing ammo to the tomahawk
                lastHitPos = pos;
            }
        }
        private WeaponHash CurrentPlayerWeapon()    
        {
            var out1 = new OutputArgument();
            Function.Call(Hash.GET_CURRENT_PED_WEAPON,Game.Player.Character.Handle, out1,0,0,1);
            var hash = out1.GetResult<WeaponHash>();
            delayAction(3000);
            return hash;
        }
    }
}
