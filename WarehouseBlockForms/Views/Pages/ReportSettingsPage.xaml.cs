using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WarehouseBlockForms.Classes;
using WarehouseBlockForms.Classess;

namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportSettingsPage.xaml
    /// </summary>
    public partial class ReportSettingsPage : Page
    {
        IniFile iniFile = new IniFile("settings.ini");
        public ReportSettingsPage()
        {
            InitializeComponent();

            if(iniFile.KeyExists("report_directory_path"))
            {
                tbxCurrentDirectoryPath.Text = iniFile.Read("report_directory_path");
            }

            btnOpenDirectoryPath.Click += delegate
            {
                var dlg = new FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = dlg.ShowDialog(this.GetIWin32Window());
                if(result == DialogResult.OK)
                {
                    saveReportsPath(dlg.SelectedPath);
                }
            };
        }

        private void saveReportsPath (string path)
        {
            tbxCurrentDirectoryPath.Text = path;
            iniFile.Write("report_directory_path", path);
        }
    }
}
