using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Arma3FactionGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static class FactionGenVars
        {
            public static string author;
            public static string factionClass;
        }
        public MainWindow()
        {
            InitializeComponent();
            string[] sideArray = { "OPFOR", "BLUFOR", "INDFOR" };
            foreach (string side in sideArray)
            {
                sideListBox.Items.Add(side);
            }
            sideListBox.SelectedIndex = 0;
        }

        private string generateFaction(string FacNameClass, string FacName, string Author, int side)
        {
            string config = $@"class CfgPatches
{{
    class {FacNameClass}
    {{
        units[]= {{}};
        weapons[]={{}};
        requiredVersion = 0.1;
        requiredAddons[] = {{}};
        author = ""{Author}"";
    }};
}};
class cfgFactionClasses
{{
    class {FacNameClass}_faction
    {{
        displayName = ""{FacName}"";
        priority = 3;
        side = {side};
        icon = """";
    }};
}};
class cfgVehicleClasses
{{
    class {FacNameClass}_Men     
    {{
        displayName=""Men"";
    }};
}};";
            return config;

        }
        private string generateSoldiers(string uniform, string SVest, string SBackpack, string Weapon, string WeaponAmmo, int WeaponAmmoNum, string MGWeapon, string MGWeaponAmmo, int MGWeaponAmmoNum, string NightVis, string SMask, string SHelmet,string FacNameClass)
        {
            string mags = "";
            for (int i = 0; i < WeaponAmmoNum-1; i++)
            {
                mags = mags + '"'+WeaponAmmo+'"'+",";
            }
            mags = mags + '"' + WeaponAmmo + '"';
            string MGmags = "";
            for (int i = 0; i < WeaponAmmoNum; i++)
            {
                MGmags = MGmags + '"' + MGWeaponAmmo + '"' + ",";
            }
            MGmags = MGmags + '"' + MGWeaponAmmo + '"';
            string SquadLead = $@"
class CfgVehicles
{{
    class B_Soldier_base_F;
    class {FacNameClass}_f_Squadleader : B_Soldier_base_F
    {{
        _generalMacro = ""{FacNameClass}_f_Squadleader""; 
	    scope = 2;
	    displayName = ""Squad Leader"";
	    faction = {FacNameClass}_faction; 
	    vehicleClass = ""{FacNameClass}_Men"";
	    icon = ""iconManLeader"";
	    nakedUniform = ""U_BasicBody"";  
	    uniformClass = ""{uniform}"";
	    backpack = ""{backpack}"";
	    linkedItems[] = {{""{vest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}}; 
	    respawnLinkedItems[] = {{""{vest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}};
	    weapons[] = {{""{Weapon}"",""Binocular""}};
	    respawnweapons[] = {{""{Weapon}"",""Binocular""}};
	    magazines[] = {{{mags},""HandGrenade"",""HandGrenade"",}};
	    Respawnmagazines[] = {{{mags},""HandGrenade"",""HandGrenade"",}};
    }};";
            string Soldier = $@"
    class {FacNameClass}_f_Soldier : B_Soldier_base_F
    {{
        _generalMacro = ""{FacNameClass}_f_Soldier""; 
	    scope = 2;
	    displayName = ""Soldier"";
	    faction = {FacNameClass}_faction; 
	    vehicleClass = ""{FacNameClass}_Men"";
	    icon = ""iconMan"";
	    nakedUniform = ""U_BasicBody"";  
	    uniformClass = ""{uniform}"";
	    backpack = ""{backpack}"";
	    linkedItems[] = {{""{vest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}}; 
	    respawnLinkedItems[] = {{""{vest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}};
	    weapons[] = {{""{Weapon}"",""Binocular""}};
	    respawnweapons[] = {{""{Weapon}"",""Binocular""}};
	    magazines[] = {{{mags},""HandGrenade"",""HandGrenade"",}};
	    Respawnmagazines[] = {{{mags},""HandGrenade"",""HandGrenade"",}};
    }};";
            string Mgunner = $@"
    class {FacNameClass}_f_mg : B_Soldier_base_F
    {{
        _generalMacro = ""{FacNameClass}_f_mg""; 
	    scope = 2;
	    displayName = ""Machine Gunner"";
	    faction = {FacNameClass}_faction; 
	    vehicleClass = ""{FacNameClass}_Men"";
	    icon = ""iconManMG"";
	    nakedUniform = ""U_BasicBody"";  
	    uniformClass = ""{uniform}"";
	    backpack = ""{backpack}"";
	    linkedItems[] = {{""{vest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}}; 
	    respawnLinkedItems[] = {{""{vest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}};
	    weapons[] = {{""{MGWeapon}"",""Binocular""}};
	    respawnweapons[] = {{""{MGWeapon}"",""Binocular""}};
	    magazines[] = {{{MGmags},""HandGrenade"",""HandGrenade"",}};
	    Respawnmagazines[] = {{{MGmags},""HandGrenade"",""HandGrenade"",}};
    }};
}};";
            return SquadLead+Soldier+Mgunner;
        }
        private string generateGroups(int side,string FacNameClass, string FacName)
        {
            string sideName = "";
            string restOfUnits = "";
            switch (side)
            {
                case 0:
                    sideName = "EAST";
                    break;
                case 1:
                    sideName = "WEST";
                    break;
                case 2:
                    sideName = "GUER";
                    break;
            }
            restOfUnits = generateUnits(FacNameClass);
            string config = $@"
class CfgGroups
{{
    class {sideName}
    {{
        name = ""Custom Group"";
        side = {side};
        class Infantry
        {{
            name=""Infantry"";
            class {FacNameClass}_g_inf
            {{
                name = ""{FacName}"";
                side= {side};
                faction= ""{FacNameClass}"";
                class Unit0
                {{
                    side = {side};
                    vehicle = ""{FacNameClass}_f_Squadleader""
					rank = ""CORPORAL"";
                    position[] = {{ 0, 0, 0 }};
                }};
                {restOfUnits}
            }};
        }};
    }};
                    
}};";
            return config;
        }
        private string generateUnits(string FacNameClass)
        {
            string units = "";
            return units;
        }
        private void generateConfig(object sender, RoutedEventArgs e)
        {
            string uniform = uni.Text;
            string SVest = vest.Text;
            string SBackpack = backpack.Text;
            string Weapon = weap.Text;
            string MGWeapon = mg_weap.Text;
            string WeaponAmmo = weap_mag.Text;
            string MGWeaponAmmo = mg_weap_mag.Text;
            int WeaponAmmoNum = (int)weap_mag_num.Value;
            int MGWeaponAmmoNum = (int)mg_weap_mag_num.Value;
            string NightVis = nvg.Text;
            string SMask = mask.Text;
            string SHelmet = helmet.Text;
            string FacNameClass = fnc.Text;
            string FacName = fn.Text;
            string Author = auth.Text;
            Author = Author + " via A3FG";
            FactionGenVars.author = Author;
            FactionGenVars.factionClass = FacNameClass;
            int side = sideListBox.SelectedIndex;
            string finall = "";
            finall = generateFaction(FacNameClass, FacName, Author, side);
            finall = finall + generateSoldiers(uniform, SVest, SBackpack, Weapon, WeaponAmmo, WeaponAmmoNum, MGWeapon, MGWeaponAmmo, MGWeaponAmmoNum, NightVis, SMask, SHelmet, FacNameClass);
            finall = finall + generateGroups(side, FacNameClass, FacName);
            debug.Text = finall;
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string path = dialog.FileName;
                File.WriteAllText(path+"/config.cpp",finall);
                MessageBox.Show("Wygenerowano plik config.cpp w folderze " + path, "Generowanie zakończone powodzeniem");
            }
        }

    }
}
