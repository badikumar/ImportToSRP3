using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportToSRP3
{
    public partial class MainForm : Form
    {
        private readonly SRP3Entities _dbContext;
        private readonly Logger _logger;
        private NationalCommunity _nationalCommunity;
        private Region _region;
        private Cluster _cluster;
        public MainForm()
        {
            InitializeComponent();
            var databases = SqlHelper.GetSRPDatabases();
            toolStripStatusLabel1.Text = 
                databases.Count == 1 ? "SRP Database found: " + 
                databases.First() : "SRP Database not found. Please install SRP before running this tool.";
            btnImport.Enabled = databases.Count == 1;
            if (databases.Count == 1)
            {
                var connectionString = SqlHelper.GetEFConnectionString(databases.FirstOrDefault());
                _dbContext = new SRP3Entities(connectionString);
                _logger =  new Logger(txtLog);
            }           
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text) ||
                string.IsNullOrEmpty(txtCountryName.Text) ||
                string.IsNullOrEmpty(txtRegionName.Text) ||
                string.IsNullOrEmpty(txtClusterName.Text))
            {
                MessageBox.Show(@"All fields are required.", @"Required Fields", MessageBoxButtons.OK);
                return;
            }
            _nationalCommunity = GetNationalCommunity();
            _region = GetRegion();
            _cluster = GetCluster();
        }

        private Cluster GetCluster()
        {
            _logger.Log("Getting Cluster ...");
            var c = _dbContext.Clusters.FirstOrDefault(n => n.Name == txtClusterName.Text);
            if (c == null)
            {
                _logger.LogEnd("Not found");
                _logger.Log("Creating Cluster ..." + txtClusterName.Text);
                c = new Cluster()
                {
                    Name = txtClusterName.Text,
                    CreatedTimestamp = DateTime.Now,
                    LastUpdatedTimestamp = DateTime.Now,
                    Region = _region
                };
                _dbContext.Clusters.Add(c);
                _dbContext.SaveChanges();
                _logger.LogEnd("...Done");
                return c;
            }
            _logger.LogEnd("Found " + txtClusterName.Text);
            return c;
        }

        private Region GetRegion()
        {
            _logger.Log("Getting Region ...");
            var r = _dbContext.Regions.FirstOrDefault(n => n.Name == txtRegionName.Text);
            if (r == null)
            {
                _logger.LogEnd("Not found");
                _logger.Log("Creating Region ..." + txtRegionName.Text);
                r = new Region()
                {
                    Name = txtRegionName.Text,
                    CreatedTimestamp = DateTime.Now,
                    LastUpdatedTimestamp = DateTime.Now,
                    NationalCommunity = _nationalCommunity
                };
                _dbContext.Regions.Add(r);
                _dbContext.SaveChanges();
                _logger.LogEnd("...Done");
                return r;
            }
            _logger.LogEnd("Found " + txtRegionName.Text);
            return r;
        }

        private NationalCommunity GetNationalCommunity()
        {
            _logger.Log("Getting National Community...");
            var nc = _dbContext.NationalCommunities.FirstOrDefault(n => n.Name == txtCountryName.Text);
            if (nc == null)
            {
                _logger.LogEnd("Not found");
                _logger.Log("Creating National Community..." + txtCountryName.Text);
                nc = new NationalCommunity()
                {
                    Name = txtCountryName.Text,
                    CreatedTimestamp = DateTime.Now,
                    LastUpdatedTimestamp = DateTime.Now
                };
                _dbContext.NationalCommunities.Add(nc);
                _dbContext.SaveChanges();
                _logger.LogEnd("...Done");
                return nc;
            }
            _logger.LogEnd("Found " + txtCountryName.Text);
            return nc;
        }

        private void testImport(string conn)
        {
            using (var context = new SRP3Entities(conn))
            {
                var nationalCommunity = context.NationalCommunities.FirstOrDefault(n => n.Name == "South Africa");
                if (nationalCommunity == null)
                {
                    nationalCommunity = new NationalCommunity()
                    {
                        Name = "South Africa",
                        CreatedTimestamp = DateTime.Now,
                        LastUpdatedTimestamp = DateTime.Now
                    };
                    context.NationalCommunities.Add(nationalCommunity);
                    context.SaveChanges();
                }


                var i = new Individual();
                i.FirstName = "Auto3";
                i.BirthDate = new DateTime(1900, 1, 1);
                i.CreatedTimestamp = DateTime.Now;
                i.LastUpdatedTimestamp = DateTime.Now;
                i.Locality = new Locality()
                {
                    Name = "Locality1",
                    CreatedTimestamp = DateTime.Now,
                    LastUpdatedTimestamp = DateTime.Now,
                    Cluster =
                        new Cluster()
                        {
                            CreatedTimestamp = DateTime.Now,
                            LastUpdatedTimestamp = DateTime.Now,
                            Name = "Cluster1",
                            Region = new Region()
                            {
                                CreatedTimestamp = DateTime.Now,
                                LastUpdatedTimestamp = DateTime.Now,
                                Name = "Region1",
                                NationalCommunity = nationalCommunity
                            }
                        }
                };


                context.Individuals.Add(i);
                context.SaveChanges();
                context.Dispose();
            }
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
                //using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open), new UTF8Encoding())) // do anything you want, e.g. read it
                //{
                //    // ...
                //}
            }
        }
    }
}
