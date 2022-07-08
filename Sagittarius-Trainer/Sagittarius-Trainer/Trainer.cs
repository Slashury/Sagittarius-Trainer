using System;
using System.Windows.Forms; 
using GTA; 
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;
using Screen = GTA.UI.Screen;


public class Main : Script
{
    ObjectPool pool;
    NativeMenu menu;
    NativeMenu playerOptions;
    NativeItem resetwantedlevel;
    NativeCheckboxItem playergodmode;
    NativeMenu vehicleOptions;
    NativeMenu miscOptions;
    NativeItem vehiclerepair;
    NativeMenu weaponOptions;
    NativeCheckboxItem infiniteammo;
    NativeCheckboxItem vehiclegodmode;
    NativeCheckboxItem neverwanted;
    NativeItem playeraddcash;
    NativeItem playerremovecash;
    NativeItem givemaxammo;
    NativeItem giveallweapons;
    NativeMenu teleportOptions;
    NativeItem teleportforward;
    NativeItem cleanped;

    public Main()
    {
       
        Setup(); 
        Tick += OnTick;
        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp; 
    }
    void Setup()
    {

    pool = new ObjectPool(); 
        menu = new NativeMenu("Sagittarius", "Choose an Option");
        pool.Add(menu);
        
        playerOptions = new NativeMenu("Player Options", "Player Options");
        pool.Add(playerOptions);
        menu.AddSubMenu(playerOptions);

        vehicleOptions = new NativeMenu("Vehicle Options", "Vehicle Options");
        pool.Add(vehicleOptions);
        menu.AddSubMenu(vehicleOptions);

        weaponOptions = new NativeMenu("Weapon Options", "Weapon Options");
        pool.Add(weaponOptions);
        menu.AddSubMenu(weaponOptions);

        teleportOptions = new NativeMenu("Teleport Options", "Teleport Options");
        pool.Add(teleportOptions);
        menu.AddSubMenu(teleportOptions);

        miscOptions = new NativeMenu("Misc Options", "Misc Options");
        pool.Add(miscOptions);
        menu.AddSubMenu(miscOptions);
        

        PlayerFunctions(); 

    }
   
    void PlayerFunctions()
    {
        ResetWantedLevel();
        PlayerGodMode();
        VehicleRepair();
        InfiniteAmmo();
        VehicleGodmode();
        NeverWanted();
        PlayerMoney();
        SpecificWeapon();
        GiveMaxAmmo();
        GiveAllWeapons();
        TeleportForwar();
        CleanPed();

    }
    void CleanPed(){
        cleanped = new NativeItem("Clean Ped");
        playerOptions.Add(cleanped);
        playerOptions.ItemActivated += (sender, e) =>
        {
            if(e.Item == cleanped){
                Ped Player;
                Player = Game.Player.Character;
                Function.Call<bool>(Hash.CLEAR_PED_BLOOD_DAMAGE, Player);
            }
        }
    }

    void TeleportForwar()
    {
        teleportforward = new NativeItem("Teleport Forward");
        teleportOptions.Add(teleportforward);
        teleportOptions.ItemActivated += (sender, e) =>
        {
            if(e.Item == teleportforward)
            {
                Ped Player;
                Player = Game.Player.Character;
                Vector3 coords = Game.Player.Character.GetOffsetPosition(new Vector3(0, -5, 0));

            }
        };
    }


    void GiveAllWeapons()
    {
        giveallweapons = new NativeItem("Give All Weapons");
        weaponOptions.Add(giveallweapons);
        weaponOptions.ItemActivated += (senderr, e) =>
        {
            if(e.Item == giveallweapons) {
                Screen.ShowSubtitle("~b~All Weapons~w~ has been added!");
            WeaponHash[] weapons = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
            Ped Player = Game.Player.Character;
            foreach (WeaponHash w in weapons)
            {
                Player.Weapons.Give(w, 0, true, true);
            }
            }
        };
}
    void GiveMaxAmmo()
    {

        givemaxammo = new NativeItem("Add Max Ammo");
        weaponOptions.Add(givemaxammo);
        weaponOptions.ItemActivated += (senderr, e) =>
        {
            if(e.Item == givemaxammo) {

                Ped Player;
                Player = Game.Player.Character;
                Weapon currentWeapon = Player.Weapons.Current;
                currentWeapon.Ammo = currentWeapon.MaxAmmo;
                Screen.ShowSubtitle("~b~Max Ammo~w~ has been added to your " + currentWeapon);

            }
        };
          
    }
    void SpecificWeapon()
    {
        NativeListItem<dynamic> list = new NativeListItem<dynamic>("Weapon: "); ;
        weaponOptions.Add(list);
        WeaponHash[] allWeaponHashes = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
        for(int i = 0; i < allWeaponHashes.Length; i++)
        {
            list.Add(allWeaponHashes[i]);
        }
        weaponOptions.ItemActivated += (sender, e) =>
        {
            if(e.Item == list)
            {
                int listIndex = list.SelectedIndex;
                WeaponHash currentHash = allWeaponHashes[listIndex];
                Game.Player.Character.Weapons.Give(currentHash, 9999, true, true);
                Screen.ShowSubtitle(currentHash + "weapon has been added!");
            }
        };
    }

    void PlayerMoney()
    {
        playeraddcash = new NativeItem("Add Cash");
        playerOptions.Add(playeraddcash);
        playerOptions.ItemActivated += (sender, e) =>
        {
            if(e.Item == playeraddcash)
            {
                Game.Player.Money += 100000;
                Screen.ShowSubtitle("100000$ has been added!");
            }
        };

        playerremovecash = new NativeItem("Remove Cash");
        playerOptions.Add(playerremovecash);

        playerOptions.ItemActivated += (sender, e) =>
        {
            if(e.Item == playerremovecash)
            {
                Game.Player.Money -= 100000;
                Screen.ShowSubtitle("100000$ has been removed!");
            }
        };
    }

    void NeverWanted()
    {
        neverwanted = new NativeCheckboxItem("Never Wanted", "Do you wish to enable Never Wanted?");
        playerOptions.Add(neverwanted);
        neverwanted.CheckboxChanged += (sender, e) => 
        {
            bool checked_ = neverwanted.Checked;
            if (checked_) 
            {
                Game.Player.WantedLevel = 0;
                Screen.ShowSubtitle("~b~Never wanted~w~ has been enabled!");
                Game.MaxWantedLevel = 0;
            }
            else
            {
                Game.MaxWantedLevel = 5;
                Screen.ShowSubtitle("~b~Never wanted~w~ has been disabled!");
            }
        };
    }   

    void InfiniteAmmo()
    {
        infiniteammo = new NativeCheckboxItem("Infinite Ammo", "Do you wish to enable infinite ammo?");
        weaponOptions.Add(infiniteammo);
        infiniteammo.CheckboxChanged += (sender, e) =>
        {
            bool checked_ = infiniteammo.Checked;
            Weapon w = Game.Player.Character.Weapons.Current;
            w.InfiniteAmmoClip = checked_;
            Screen.ShowSubtitle(checked_ ? "~b~Infinite ammo~w~ has been enabled!" : "~b~Infinite ammo~w~ has been disabled!");   
        };
    }

    void VehicleGodmode()
    {
        vehiclegodmode = new NativeCheckboxItem("Vehicle Godmode", "Do you wish to enable Vehicle Godmode?");
        vehicleOptions.Add(vehiclegodmode);
        vehiclegodmode.CheckboxChanged += (sender, e) =>
        {
            bool checked_ = vehiclegodmode.Checked;

            if (!Game.Player.Character.IsInVehicle())
            {
                Screen.ShowSubtitle("Player is not in a ~b~vehicle!~w~");
            }
            if (checked_ && Game.Player.Character.IsInVehicle())
            {
                Vehicle car = Game.Player.Character.CurrentVehicle;
                car.IsInvincible = checked_;
                car.Repair();
                
                Function.Call<bool>(Hash.SET_ENTITY_PROOFS, car, checked_, checked_, checked_, checked_, checked_, checked_);
                Function.Call<bool>(Hash.SET_ENTITY_CAN_BE_DAMAGED, car, checked_);
                Screen.ShowSubtitle(checked_ ? "~b~Vehicle Godmode~w~ has been enabled!" : "~b~Vehicle Godmode~w~ has been disabled!");

            }



        };
    }

    void VehicleRepair()
    {
        vehiclerepair = new NativeItem("Vehicle Repair", "Do you wish to repair your vehicle?");
        vehicleOptions.Add(vehiclerepair);
        vehicleOptions.ItemActivated += (sender, e) =>
        {
            if (e.Item == vehiclerepair)
            {
                Vehicle car;
                car = Game.Player.Character.CurrentVehicle;

                if (Function.Call<bool>(Hash.IS_PED_IN_ANY_VEHICLE, Game.Player.Character, false))
                {
                    car.Repair();
                    car.Wash();
                    car.IsInvincible = true;
                    Screen.ShowSubtitle("Your car has been ~b~repaired~w~!");
                }
               
            }
        };
        
    }

    void PlayerGodMode()
    {

        playergodmode = new NativeCheckboxItem("Player Godmode", "Do you wish to enable Godmode?");
        playerOptions.Add(playergodmode);
            playergodmode.CheckboxChanged += (sender, e) =>
            {
                bool checked_ = playergodmode.Checked;
                Game.Player.Character.IsInvincible = checked_;
                Game.Player.IsInvincible = checked_;
                Screen.ShowSubtitle(checked_ ? "~b~Godmode~w~ has been enabled!" : "~b~Godmode~w~ has been disabled!");
                
            };
    }

    void ResetWantedLevel()
    {

        resetwantedlevel = new NativeItem("Reset Wanted Level");
        playerOptions.Add(resetwantedlevel);

        playerOptions.ItemActivated += (sender, e) =>
        {
            if (e.Item == resetwantedlevel)
            {
                if (Game.Player.WantedLevel == 0)
                {
                    Screen.ShowSubtitle("You have no ~b~wanted levels!~w~");
                }
                else
                {
                    Game.Player.WantedLevel = 0;
                    Screen.ShowSubtitle("~b~Your wanted~w~ level has been cleared!");
                }
            };
        };
    }

    void OnTick(object sender, EventArgs e)
    {
        pool.Process(); 
    }

     void OnKeyDown(object sender, KeyEventArgs e)
    {

    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F3)
        {
            menu.Visible = true;
        }

    }

}