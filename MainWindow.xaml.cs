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
        private string generateSoldiers(
            string uniform, 
            string SVest, 
            string SBackpack, 
            string Weapon, 
            string WeaponAmmo, 
            int WeaponAmmoNum, 
            string MGWeapon, 
            string MGWeaponAmmo, 
            int MGWeaponAmmoNum, 
            string NightVis, 
            string SMask, 
            string SHelmet,
            string FacNameClass,
            int marksmanWeaponAmmoNum,
            string marksmanWeaponAmmo,
            string marksmanWeapon,
            int atWeaponAmmoNum,
            string atWeaponAmmo,
            string atWeapon)
        {
            string mags = "";
            for (int i = 0; i < WeaponAmmoNum-1; i++)
            {
                mags = mags + '"'+WeaponAmmo+'"'+",";
            }
            mags = mags + '"' + WeaponAmmo + '"';
            string MGmags = "";
            for (int i = 0; i < MGWeaponAmmoNum; i++)
            {
                MGmags = MGmags + '"' + MGWeaponAmmo + '"' + ",";
            }
            MGmags = MGmags + '"' + MGWeaponAmmo + '"';
            string ATmags = "";
            for (int i = 0; i < atWeaponAmmoNum; i++)
            {
                ATmags = ATmags + '"' + atWeaponAmmo + '"' + ",";
            }
            ATmags = ATmags + '"' + atWeaponAmmo + '"';
            string Marksmanmags = "";
            for (int i = 0; i < marksmanWeaponAmmoNum; i++)
            {
                Marksmanmags = Marksmanmags + '"' + marksmanWeaponAmmo + '"' + ",";
            }
            Marksmanmags = Marksmanmags + '"' + marksmanWeaponAmmo + '"';
            string finallConfig = "";
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
	    backpack = ""{SBackpack}"";
	    linkedItems[] = {{""{SVest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}}; 
	    respawnLinkedItems[] = {{""{SVest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}};
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
	    backpack = ""{SBackpack}"";
	    linkedItems[] = {{""{SVest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}}; 
	    respawnLinkedItems[] = {{""{SVest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}};
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
	    backpack = ""{SBackpack}"";
	    linkedItems[] = {{""{SVest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}}; 
	    respawnLinkedItems[] = {{""{SVest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}};
	    weapons[] = {{""{MGWeapon}"",""Binocular""}};
	    respawnweapons[] = {{""{MGWeapon}"",""Binocular""}};
	    magazines[] = {{{MGmags},""HandGrenade"",""HandGrenade"",}};
	    Respawnmagazines[] = {{{MGmags},""HandGrenade"",""HandGrenade"",}};
    }};";
            string ATSoldier = $@"
    class {FacNameClass}_f_at : B_Soldier_base_F
    {{
        _generalMacro = ""{FacNameClass}_f_at""; 
	    scope = 2;
	    displayName = ""AT Operator"";
	    faction = {FacNameClass}_faction; 
	    vehicleClass = ""{FacNameClass}_Men"";
	    icon = ""iconManAT"";
	    nakedUniform = ""U_BasicBody"";  
	    uniformClass = ""{uniform}"";
	    backpack = ""{SBackpack}"";
	    linkedItems[] = {{""{SVest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}}; 
	    respawnLinkedItems[] = {{""{SVest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}};
	    weapons[] = {{""{Weapon}"",""{atWeapon}"",""Binocular""}};
	    respawnweapons[] = {{""{Weapon}"",""{atWeapon}"",""Binocular""}};
	    magazines[] = {{{mags},{ATmags},""HandGrenade"",""HandGrenade"",}};
	    Respawnmagazines[] = {{{mags},{ATmags},""HandGrenade"",""HandGrenade"",}};
    }};";
            string Marksman = $@"
    class {FacNameClass}_f_mark : B_Soldier_base_F
    {{
        _generalMacro = ""{FacNameClass}_f_mark""; 
	    scope = 2;
	    displayName = ""Marksman"";
	    faction = {FacNameClass}_faction; 
	    vehicleClass = ""{FacNameClass}_Men"";
	    icon = ""iconManRecon"";
	    nakedUniform = ""U_BasicBody"";  
	    uniformClass = ""{uniform}"";
	    backpack = ""{SBackpack}"";
	    linkedItems[] = {{""{SVest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}}; 
	    respawnLinkedItems[] = {{""{SVest}"", ""{SHelmet}"", ""{NightVis}"", ""ItemMap"", ""ItemCompass"", ""ItemWatch"", ""ItemRadio""}};
	    weapons[] = {{""{marksmanWeapon}"",""Binocular""}};
	    respawnweapons[] = {{""{marksmanWeapon}"",""Binocular""}};
	    magazines[] = {{{Marksmanmags},""HandGrenade"",""HandGrenade"",}};
	    Respawnmagazines[] = {{{Marksmanmags},""HandGrenade"",""HandGrenade"",}};
    }};";
            //return SquadLead + Soldier + Mgunner + ATSoldier + Marksman;
            finallConfig = finallConfig + SquadLead;
            bool riflemanOn = (bool)rifleman_on.IsChecked;
            bool atOn = (bool)at_on.IsChecked;
            bool mgOn = (bool)rifleman_on.IsChecked;
            bool marksmanOn = (bool)marksman_on.IsChecked;
            if (riflemanOn) { finallConfig = finallConfig + Soldier; }
            if (atOn) { finallConfig = finallConfig + ATSoldier; }
            if (mgOn) { finallConfig = finallConfig + Mgunner; }
            if (marksmanOn) { finallConfig = finallConfig + Marksman; }
            finallConfig = finallConfig + "}};";
            return finallConfig;
        }
        private string generateGroups(int side,string FacNameClass, string FacName,int riflemanAmmountVal, int MGAmmountVal, int ATAmmountVal, int MarksmanAmmountVal)
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
            restOfUnits = generateUnits(FacNameClass, side, riflemanAmmountVal, MGAmmountVal, ATAmmountVal, MarksmanAmmountVal);
            string config = $@"
class CfgGroups
{{
    class {sideName}
    {{
        name = ""Custom Group"";
        side = {side};
        class {FacNameClass} {{
            name=""{FacName}"";
            class Infantry
            {{
                name=""Custom Infantry Groups"";
                class {FacNameClass}_g_inf
                {{
                    name = ""Infantry Squad"";
                    side= {side};
                    faction= ""{FacNameClass}"";
                    class Unit0
                    {{
                        side = {side};
                        vehicle = ""{FacNameClass}_f_Squadleader"";
					    rank = ""CORPORAL"";
                        position[] = {{ 0, 0, 0 }};
                    }};
                    {restOfUnits}
                }};
            }};
        }};
    }};
                    
}};";
            return config;
        }
        private string generateUnits(string FacNameClass, int side, int riflemanAmmountVal, int MGAmmountVal, int ATAmmountVal, int MarksmanAmmountVal)
        {
            int counter = 1;
            int pos1 = 5;
            int pos2 = -5;
            string units = "";
            //riflemanAmmountVal, MGAmmountVal, ATAmmountVal, MarksmanAmmountVal
            if (riflemanAmmountVal != 0)
            {
                for (int i = 0; i < riflemanAmmountVal; i++)
                {
                    string patternRifleman = $@"
                class Unit{counter}
                {{
                    side = {side};
                    vehicle = ""{FacNameClass}_f_Soldier"";
				    rank = ""PRIVATE"";
                    position[] = {{ {pos1}, {pos2}, 0 }};
                }};
                ";
                    if (counter % 2 == 0)
                    {
                        pos2 = pos2 - 5;
                        pos1 = pos1 - 5;
                    }
                    counter++;
                    units = units + patternRifleman;
                    pos1 = pos1 * -1;
                }
            }
            if (riflemanAmmountVal != 0)
            {
                for (int i = 0; i < MGAmmountVal; i++)
                {
                    string patternMG = $@"
                class Unit{counter}
                {{
                    side = {side};
                    vehicle = ""{FacNameClass}_f_mg"";
				    rank = ""PRIVATE"";
                    position[] = {{ {pos1}, {pos2}, 0 }};
                }};
                ";
                    if (counter % 2 == 0)
                    {
                        pos2 = pos2 - 5;
                        pos1 = pos1 - 5;
                    }
                    counter++;
                    units = units + patternMG;
                    pos1 = pos1 * -1;
                }
            }
            if (riflemanAmmountVal != 0)
            {
                for (int i = 0; i < ATAmmountVal; i++)
                {
                    string patternAT = $@"
                class Unit{counter}
                {{
                    side = {side};
                    vehicle = ""{FacNameClass}_f_at"";
				    rank = ""PRIVATE"";
                    position[] = {{ {pos1}, {pos2}, 0 }};
                }};
                ";
                    if (counter % 2 == 0)
                    {
                        pos2 = pos2 - 5;
                        pos1 = pos1 - 5;
                    }
                    counter++;
                    units = units + patternAT;
                    pos1 = pos1 * -1;
                }
            }
            if (riflemanAmmountVal != 0)
            {
                for (int i = 0; i < MarksmanAmmountVal; i++)
                {
                    string patternMarksman = $@"
                class Unit{counter}
                {{
                    side = {side};
                    vehicle = ""{FacNameClass}_f_mark"";
				    rank = ""PRIVATE"";
                    position[] = {{ {pos1}, {pos2}, 0 }};
                }};
                ";
                    if (counter % 2 == 0)
                    {
                        pos2 = pos2 - 5;
                        pos1 = pos1 - 5;
                    }
                    counter++;
                    units = units + patternMarksman;
                    pos1 = pos1 * -1;
                }
            }
            return units;
        }
        private void generateConfig(object sender, RoutedEventArgs e)
        {
            string atWeapon = at_weap.Text;
            string atWeaponAmmo = at_weap_mag.Text;
            int atWeaponAmmoNum = (int)at_weap_mag_num.Value;
            string marksmanWeapon = marksman_weap.Text;
            string marksmanWeaponAmmo = marksman_weap_mag.Text;
            int marksmanWeaponAmmoNum = (int)marksman_weap_mag_num.Value;
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
            int riflemanAmmountVal = (int)riflemanAmmount.Value;
            int MGAmmountVal = (int)MGAmmount.Value;
            int ATAmmountVal = (int)ATAmmount.Value;
            int MarksmanAmmountVal = (int)MarksmanAmmount.Value;
            Author = Author + " via A3FG";
            FactionGenVars.author = Author;
            FactionGenVars.factionClass = FacNameClass;
            int side = sideListBox.SelectedIndex;
            string finall = "";
            finall = generateFaction(FacNameClass, FacName, Author, side);
            finall = finall + generateSoldiers(uniform, SVest, SBackpack, Weapon, WeaponAmmo, WeaponAmmoNum, MGWeapon, MGWeaponAmmo, MGWeaponAmmoNum, NightVis, SMask, SHelmet, FacNameClass, marksmanWeaponAmmoNum, marksmanWeaponAmmo, marksmanWeapon, atWeaponAmmoNum, atWeaponAmmo, atWeapon);
            finall = finall + generateGroups(side, FacNameClass, FacName, riflemanAmmountVal, MGAmmountVal, ATAmmountVal, MarksmanAmmountVal);
            //debug----------------------------------------------
            debug.Text = finall;
            //debug----------------------------------------------
            string finallSettings = "";
            string[] Settings = new string[] { at_weap.Text,
            at_weap_mag.Text,
            at_weap_mag_num.Value.ToString(),
            marksman_weap.Text,
            marksman_weap_mag.Text,
            marksman_weap_mag_num.Value.ToString(),
            uni.Text,
            vest.Text,
            backpack.Text,
            weap.Text,
            mg_weap.Text,
            weap_mag.Text,
            mg_weap_mag.Text,
            weap_mag_num.Value.ToString(),
            mg_weap_mag_num.Value.ToString(),
            nvg.Text,
            mask.Text,
            helmet.Text,
            fnc.Text,
            fn.Text,
            auth.Text,
            sideListBox.SelectedIndex.ToString(),
            riflemanAmmount.Value.ToString(),
            MGAmmount.Value.ToString(),
            ATAmmount.Value.ToString(),
            MarksmanAmmount.Value.ToString()
            };
            foreach (string s in Settings)
            {
                if (s == "")
                {
                    finallSettings = finallSettings + "ValueWasNotDefined" + "\n";
                }
                else
                {
                    finallSettings = finallSettings + s + "\n";
                }
            }
            exportAll(finallSettings, finall);
        }
        private void importSettings(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "TEXT Files (*.txt)|*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filePath = dlg.FileName;
                //debug.Text = dlg.FileName+"\n";
                string[] preprocess = File.ReadAllLines(filePath);
                int counter = 0;
                foreach (string s in preprocess)
                {
                    if (s == "ValueWasNotDefined") { counter++; continue;}
                    //debug.Text = debug.Text + s+"\n";
                    switch (counter)
                    {
                        case 0:
                            at_weap.Text = s;
                            break;
                        case 1:
                            at_weap_mag.Text = s;
                            break;
                        case 2:
                            at_weap_mag_num.Value = Convert.ToInt32(s);
                            break;
                        case 3:
                            marksman_weap.Text = s;
                            break;
                        case 4:
                            marksman_weap_mag.Text = s;
                            break;
                        case 5:
                            marksman_weap_mag_num.Value = Convert.ToInt32(s);
                            break;
                        case 6:
                            uni.Text = s;
                            break;
                        case 7:
                            vest.Text = s;
                            break;
                        case 8:
                            backpack.Text = s;
                            break;
                        case 9:
                            weap.Text = s;
                            break;
                        case 10:
                            mg_weap.Text = s;
                            break;
                        case 11:
                            weap_mag.Text = s;
                            break;
                        case 12:
                            mg_weap_mag.Text = s;
                            break;
                        case 13:
                            weap_mag_num.Value = Convert.ToInt32(s);
                            break;
                        case 14:
                            mg_weap_mag_num.Value = Convert.ToInt32(s);
                            break;
                        case 15:
                            nvg.Text = s;
                            break;
                        case 16:
                            mask.Text = s;
                            break;
                        case 17:
                            helmet.Text = s;
                            break;
                        case 18:
                            fnc.Text = s;
                            break;
                        case 19:
                            fn.Text = s;
                            break;
                        case 20:
                            auth.Text = s;
                            break;
                        case 21:
                            sideListBox.SelectedIndex = Convert.ToInt32(s);
                            break;
                        case 22:
                            riflemanAmmount.Value = Convert.ToInt32(s);
                            break;
                        case 23:
                            MGAmmount.Value = Convert.ToInt32(s);
                            break;
                        case 24:
                            ATAmmount.Value = Convert.ToInt32(s);
                            break;
                        case 25:
                            MarksmanAmmount.Value = Convert.ToInt32(s);
                            break;
                    }
                    counter++;
                }
            }
        }
        private void exportSettings(object sender, RoutedEventArgs e) {
            string finallSettings = "";
            string[] Settings = new string[] { at_weap.Text,
            at_weap_mag.Text,
            at_weap_mag_num.Value.ToString(),
            marksman_weap.Text,
            marksman_weap_mag.Text,
            marksman_weap_mag_num.Value.ToString(),
            uni.Text,
            vest.Text,
            backpack.Text,
            weap.Text,
            mg_weap.Text,
            weap_mag.Text,
            mg_weap_mag.Text,
            weap_mag_num.Value.ToString(),
            mg_weap_mag_num.Value.ToString(),
            nvg.Text,
            mask.Text,
            helmet.Text,
            fnc.Text,
            fn.Text,
            auth.Text,
            sideListBox.SelectedIndex.ToString(),
            riflemanAmmount.Value.ToString(),
            MGAmmount.Value.ToString(),
            ATAmmount.Value.ToString(),
            MarksmanAmmount.Value.ToString()
            };
            foreach (string s in Settings) {
                if (s == "") {
                    finallSettings = finallSettings + "ValueWasNotDefined" + "\n";
                }
                else
                {
                    finallSettings = finallSettings + s + "\n";
                }
            }
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DateTime now = DateTime.Now;
                string timestamp = "";
                timestamp = now.Hour.ToString() + "_"+ now.Minute.ToString() + "_" + now.Second.ToString() + "_" + now.Millisecond.ToString();
                string path = dialog.FileName;
                File.WriteAllText(path + "/settings_" + timestamp + ".txt", finallSettings);
                MessageBox.Show("Wygenerowano plik settings.txt w folderze " + path, "Generowanie zakończone powodzeniem");
            }
        }
        private void exportAll(string settings, string config) {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DateTime now = DateTime.Now;
                string timestamp = "";
                timestamp = now.Hour.ToString() + "_" + now.Minute.ToString() + "_" + now.Second.ToString() + "_" + now.Millisecond.ToString();
                string path = dialog.FileName;
                File.WriteAllText(path + "/settings_" + timestamp + ".txt", settings);
                File.WriteAllText(path + "/config.cpp", config);
                MessageBox.Show("Wygenerowano 2 pliki (settings.txt, config.cpp) w folderze " + path, "Generowanie zakończone powodzeniem");
            }
        }
    }
}
