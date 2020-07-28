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

        private string generateFaction(string FunNameClass, string FunName, string Author, int side)
        {
            string config = $@"class CfgPatches
{{
    class {FunNameClass}
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
    class {FunNameClass}_faction
    {{
        displayName = ""{FunName}"";
        priority = 3;
        side = {side};
        icon = """";
    }};
}};
class cfgVehicleClasses
{{
    class {FunNameClass}_Men     
    {{
        displayName=""Men"";
    }};
}};";
            return config;

        }

        private void generateConfig(object sender, RoutedEventArgs e)
        {
            string FunNameClass = fnc.Text;
            string FunName = fn.Text;
            string Author = auth.Text;
            Author = Author + " via A3FG";
            FactionGenVars.author = Author;
            FactionGenVars.factionClass = FunNameClass;
            int side = sideListBox.SelectedIndex;
            string finall = "";
            finall = generateFaction(FunNameClass, FunName, Author, side);
            debug.Text = finall;
        }

    }
}
