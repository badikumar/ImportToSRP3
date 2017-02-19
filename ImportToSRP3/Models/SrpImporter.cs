using System;
using System.Linq;
using System.Threading.Tasks;

namespace ImportToSRP3.Models
{
    public class SrpImporter : ISrpImporter
    {
        private readonly SRP3Entities _dbContext;
        private readonly ILogger _logger;
        private readonly SrpImporterRequest _request;
        private readonly NationalCommunity _nationalCommunity;
        private readonly Region _region;
        private readonly Cluster _cluster;
        public SrpImporter(string connectionString, ILogger logger, SrpImporterRequest request)
        {
            _dbContext = new SRP3Entities(connectionString);
            _logger = logger;
            _request = request;
            _nationalCommunity = GetNationalCommunity();
            _region = GetRegion();
            _cluster = GetCluster();
        }

        public void Import()
        {
            try
            {
                var uploadedFile = new UploadedFile(_cluster.Id,_dbContext,_request.FilePath);
                _logger.LogEnd("Please wait while loading individuals. This might take a while...");
                var individuals = uploadedFile.SerializeIndividuals();
                _logger.LogEnd("Ok." + individuals.Count + " individuals found.");
                _logger.Log("Importing " + individuals.Count + " into SRP database...");
                _dbContext.Individuals.AddRange(individuals);
                _dbContext.SaveChanges();
                _logger.LogEnd("Done");
            }
            catch (InvalidOperationException e)
            {
                _logger.LogEnd("Error: **** " + e.Message + " ****");
            }
            
        }

        private Cluster GetCluster()
        {
            _logger.Log("Getting Cluster ...");
            var c = _dbContext.Clusters.FirstOrDefault(n => n.Name == _request.Cluster);
            if (c == null)
            {
                _logger.LogEnd("Not found");
                _logger.Log("Creating Cluster ..." + _request.Cluster);
                c = new Cluster()
                {
                    Name = _request.Cluster,
                    CreatedTimestamp = DateTime.Now,
                    LastUpdatedTimestamp = DateTime.Now,
                    Region = _region
                };
                _dbContext.Clusters.Add(c);
                _dbContext.SaveChanges();
                _logger.LogEnd("...Done");
                return c;
            }
            _logger.LogEnd("Found " + _request.Cluster);
            return c;
        }

        private Region GetRegion()
        {
            _logger.Log("Getting Region ...");
            var r = _dbContext.Regions.FirstOrDefault(n => n.Name == _request.Region);
            if (r == null)
            {
                _logger.LogEnd("Not found");
                _logger.Log("Creating Region ..." + _request.Region);
                r = new Region()
                {
                    Name = _request.Region,
                    CreatedTimestamp = DateTime.Now,
                    LastUpdatedTimestamp = DateTime.Now,
                    NationalCommunity = _nationalCommunity
                };
                _dbContext.Regions.Add(r);
                _dbContext.SaveChanges();
                _logger.LogEnd("...Done");
                return r;
            }
            _logger.LogEnd("Found " + _request.Region);
            return r;
        }

        private NationalCommunity GetNationalCommunity()
        {
            _logger.Log("Getting National Community...");
            var nc = _dbContext.NationalCommunities.FirstOrDefault(n => n.Name == _request.NationalCommunity);
            if (nc == null)
            {
                _logger.LogEnd("Not found");
                _logger.Log("Creating National Community..." + _request.NationalCommunity);
                nc = new NationalCommunity()
                {
                    Name = _request.NationalCommunity,
                    CreatedTimestamp = DateTime.Now,
                    LastUpdatedTimestamp = DateTime.Now
                };
                _dbContext.NationalCommunities.Add(nc);
                _dbContext.SaveChanges();
                _logger.LogEnd("...Done");
                return nc;
            }
            _logger.LogEnd("Found " + _request.NationalCommunity);
            return nc;
        }
    }
}