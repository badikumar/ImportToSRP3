﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImportToSRP3.Models;

namespace ImportToSRP3
{
    public partial class MainForm : Form
    {
        private readonly string _connectionString;
        private readonly Logger _logger;
        
        private ISrpImporter _importer;
        public MainForm()
        {
            
            InitializeComponent();

            txtCountryName.Text = Properties.Settings.Default.NationalCommunity;
            txtRegionName.Text = Properties.Settings.Default.Region;
            txtClusterName.Text = Properties.Settings.Default.Cluster;
            txtFilePath.Text = Properties.Settings.Default.FilePath;

            var databases = SqlHelper.GetSrpDatabases();
            toolStripStatusLabel1.Text =
                databases.Count == 1 ? "SRP Database found: " +
                databases.First() : "SRP Database not found. Please install SRP before running this tool.";
            btnImport.Enabled = databases.Count == 1;
            if (databases.Count == 1)
            {
                _connectionString = SqlHelper.GetEfConnectionString(databases.FirstOrDefault());
                _logger = new Logger(txtLog);
            }
        }

        

        private void btnImport_Click(object sender, EventArgs e)
        {
            btnImport.Enabled = false;
            if (string.IsNullOrEmpty(txtFilePath.Text) ||
                string.IsNullOrEmpty(txtCountryName.Text) ||
                string.IsNullOrEmpty(txtRegionName.Text) ||
                string.IsNullOrEmpty(txtClusterName.Text))
            {
                MessageBox.Show(@"All fields are required.", @"Required Fields", MessageBoxButtons.OK);
                return;
            }
            _importer = new SrpImporter(_connectionString,_logger, new SrpImporterRequest()
            {
                Cluster = txtClusterName.Text,
                FilePath = txtFilePath.Text,
                NationalCommunity = txtCountryName.Text,
                Region = txtRegionName.Text
            });
            _importer.Import();
            btnImport.Enabled = true;
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel files | *.xlsx"; // file types, that will be allowed to upload
            dialog.Multiselect = false; // allow/deny user to upload more than one file at a time
            if (dialog.ShowDialog() == DialogResult.OK) // if user clicked OK
            {
                String path = dialog.FileName; // get name of file
                txtFilePath.Text = path;
            }
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.NationalCommunity = txtCountryName.Text;
            Properties.Settings.Default.Region = txtRegionName.Text;
            Properties.Settings.Default.Cluster= txtClusterName.Text;
            Properties.Settings.Default.FilePath= txtFilePath.Text;
            Properties.Settings.Default.Save();
        }


    }
}
