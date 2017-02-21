using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ImportToSRP3.Models
{
    public abstract class UploadedFileBase
    {
        protected List<Individual> Individuals;
        protected SRP3Entities DbContext;
        protected DataSet Result;
        protected long ClusterId;
        protected long NationalCommunityId;
        protected abstract Dictionary<string, bool> Columns { get; set; }
        protected void AddBooks(Individual individual, bool b1, bool b2, bool b31, bool b32, bool b33, bool b4, bool b5, bool b6, bool b7, bool b8, bool b9, bool b10)
        {
            individual.ActivityStudyItemIndividuals = new List<ActivityStudyItemIndividual>();
            if (b1)
            {
                individual.ActivityStudyItemIndividuals.Add(AddBook(1, 1));
            }
            if (b2)
            {
                individual.ActivityStudyItemIndividuals.Add(AddBook(2, 2));
            }
            if (b31)
            {
                individual.ActivityStudyItemIndividuals.Add(AddBook(3, 3));
            }
            if (b32)
            {
                individual.ActivityStudyItemIndividuals.Add(AddBook(3, 4));
            }
            if (b33)
            {
                individual.ActivityStudyItemIndividuals.Add(AddBook(3, 5));
            }
            if (b4)
            {
                individual.ActivityStudyItemIndividuals.Add(AddBook(4, 6));
            }
            if (b5)
            {
                individual.ActivityStudyItemIndividuals.Add(AddBook(5, 7));
            }
            if (b6)
            {
                individual.ActivityStudyItemIndividuals.Add(AddBook(6, 8));
            }
            if (b7)
            {
                individual.ActivityStudyItemIndividuals.Add(AddBook(7, 9));
            }
            if (b8)
            {
                individual.ActivityStudyItemIndividuals.Add(AddBook(8, 10));
            }
            if (b9)
            {
                individual.ActivityStudyItemIndividuals.Add(AddBook(9, 11));
            }
            if (b10)
            {
                individual.ActivityStudyItemIndividuals.Add(AddBook(10, 12));
            }
        }

        private ActivityStudyItemIndividual AddBook(int number, int sequence)
        {
            var asii = new ActivityStudyItemIndividual()
            {
                IndividualType = 1,
                IndividualRole = 7,
                IsCurrent = true,
                IsCompleted = true,
                StudyItemId = DbContext.StudyItems.First(si => si.ActivityStudyItemType == "Book" && si.Number == number && si.Sequence == sequence).Id,
                CreatedTimestamp = DateTime.Now,
                LastUpdatedTimestamp = DateTime.Now,
            };

            return asii;
        }

        protected void AddIndividualEmails(Individual individual, string email)
        {
            individual.IndividualEmails = new List<IndividualEmail>();
            short order = 0;
            if (!string.IsNullOrEmpty(email))
            {
                ++order;
                individual.IndividualEmails.Add(new IndividualEmail()
                {
                    Order = order,
                    LastUpdatedTimestamp = DateTime.Now,
                    CreatedTimestamp = DateTime.Now,
                    Email = email,
                });
            }
        }

        protected void AddIndividualPhones(Individual individual, string homeTelephone, string workTelephone, string cellPhone)
        {
            individual.IndividualPhones = new List<IndividualPhone>();
            short order = 0;
            if (!string.IsNullOrEmpty(homeTelephone))
            {
                ++order;
                individual.IndividualPhones.Add(new IndividualPhone()
                {
                    Order = order,
                    LastUpdatedTimestamp = DateTime.Now,
                    CreatedTimestamp = DateTime.Now,
                    Phone = homeTelephone,
                });
            }
            if (!string.IsNullOrEmpty(workTelephone))
            {
                ++order;
                individual.IndividualPhones.Add(new IndividualPhone()
                {
                    Order = order,
                    LastUpdatedTimestamp = DateTime.Now,
                    CreatedTimestamp = DateTime.Now,
                    Phone = workTelephone,
                });
            }
            if (!string.IsNullOrEmpty(cellPhone))
            {
                ++order;
                individual.IndividualPhones.Add(new IndividualPhone()
                {
                    Order = order,
                    LastUpdatedTimestamp = DateTime.Now,
                    CreatedTimestamp = DateTime.Now,
                    Phone = cellPhone,
                });
            }
        }

        protected string GetAddress(string address, string suburb, string code)
        {
            var result = string.Empty;
            if (!string.IsNullOrEmpty(address))
                result += address + Environment.NewLine;
            if (!string.IsNullOrEmpty(suburb))
                result += suburb + Environment.NewLine;
            if (!string.IsNullOrEmpty(code))
                result += code + Environment.NewLine;
            return result;
        }

        protected long? GetFocusNeighborhood(long localityId, string focusNeighborhoodName)
        {
            if (string.IsNullOrEmpty(focusNeighborhoodName))
                return null;
            var foc = DbContext.Subdivisions.FirstOrDefault(f => f.LocalityId == localityId && f.Name == focusNeighborhoodName);
            if (foc == null)
            {
                foc = new Subdivision()
                {
                    Name = focusNeighborhoodName,
                    CreatedTimestamp = DateTime.Now,
                    LastUpdatedTimestamp = DateTime.Now,
                    LocalityId = localityId
                };
                DbContext.Subdivisions.Add(foc);
                DbContext.SaveChanges();
            }

            return foc.Id;
        }

        protected long GetLocality(string localityName)
        {
            var loc = DbContext.Localities.FirstOrDefault(l => l.ClusterId == ClusterId && l.Name == localityName);
            if (loc == null)
            {
                loc = new Locality()
                {
                    Name = localityName,
                    CreatedTimestamp = DateTime.Now,
                    LastUpdatedTimestamp = DateTime.Now,
                    ClusterId = ClusterId,
                    GUID = Guid.NewGuid()
                };
                DbContext.Localities.Add(loc);
                DbContext.SaveChanges();
            }

            return loc.Id;
        }

        protected void GetRegistrationDate(Individual i, string registrationDate)
        {
            if (!string.IsNullOrEmpty(registrationDate))
            {
                DateTime regDate;
                if (DateTime.TryParse(registrationDate, out regDate))
                {
                    i.RegistrationDate = regDate;
                    i.DisplayRegistrationDate = regDate.ToString("yyyy-MM-dd");
                    return;
                }
            }
            i.RegistrationDate = new DateTime(DateTime.Now.Year, 1, 1);
            i.DisplayRegistrationDate = i.RegistrationDate.Value.ToString("yyyy-MM-dd");
        }

        protected void GetEstimatedYearOfBirthDate(Individual i, string ageCategory, string estimatedAge, string birthDate)
        {
            i.IsSelectedEstimatedYearOfBirthDate = true;
            if (!string.IsNullOrEmpty(birthDate))
            {
                DateTime dob;
                if (DateTime.TryParse(birthDate, out dob))
                {
                    i.EstimatedYearOfBirthDate = (short)dob.Year;
                    i.BirthDate = dob;
                    i.DisplayBirthDate = dob.ToString("yyyy-MM-dd");
                    i.IsSelectedEstimatedYearOfBirthDate = false;
                    return;
                }
            }
            if (!string.IsNullOrEmpty(estimatedAge))
            {
                short age;
                if (Int16.TryParse(estimatedAge, out age))
                {
                    i.EstimatedYearOfBirthDate = (short)(DateTime.Now.Year - age);
                    i.BirthDate = new DateTime(i.EstimatedYearOfBirthDate.Value, 1, 1);
                    i.DisplayBirthDate = i.EstimatedYearOfBirthDate.Value.ToString();

                    return;
                }
            }
            if (!string.IsNullOrEmpty(ageCategory))
            {
                Random r = new Random();
                ageCategory = ageCategory.ToLower();
                if (ageCategory == "adult")
                {
                    i.EstimatedYearOfBirthDate = (short)(DateTime.Now.Year - r.Next(21, 60));
                }
                else if (ageCategory == "junior Youth")
                {
                    i.EstimatedYearOfBirthDate = (short)(DateTime.Now.Year - r.Next(11, 16));
                }
                else if (ageCategory == "youth")
                {
                    i.EstimatedYearOfBirthDate = (short)(DateTime.Now.Year - r.Next(16, 21));
                }
                else if (ageCategory == "child")
                {
                    i.EstimatedYearOfBirthDate = (short)(DateTime.Now.Year - r.Next(0, 11));
                }
                else
                {
                    i.EstimatedYearOfBirthDate = 1900;
                }
                i.BirthDate = new DateTime(i.EstimatedYearOfBirthDate.Value, 1, 1);
                i.DisplayBirthDate = i.EstimatedYearOfBirthDate.Value.ToString();
            }
        }

        protected long GetCluster(string clusterName,long regionId)
        {
            var c = DbContext.Clusters.FirstOrDefault(n => n.Name == clusterName && n.RegionId == regionId);
            if (c == null)
            {
                c = new Cluster()
                {
                    Name = clusterName,
                    CreatedTimestamp = DateTime.Now,
                    LastUpdatedTimestamp = DateTime.Now,
                    RegionId = regionId,
                    GUID = Guid.NewGuid()
                };
                DbContext.Clusters.Add(c);
                DbContext.SaveChanges();
                   
                return c.Id;
            }
            return c.Id;
        }

        protected long GetRegion(string regionName,long nationalCommunityId)
        {
            
            var r = DbContext.Regions.FirstOrDefault(n => n.Name == regionName && n.NationalCommunityId == nationalCommunityId);
            if (r == null)
            {
                
                r = new Region()
                {
                    Name = regionName,
                    CreatedTimestamp = DateTime.Now,
                    LastUpdatedTimestamp = DateTime.Now,
                    NationalCommunityId = nationalCommunityId,
                    GUID = Guid.NewGuid()
                };
                DbContext.Regions.Add(r);
                DbContext.SaveChanges();

                return r.Id;
            }
            
            return r.Id;
        }
        public abstract List<Individual> SerializeIndividuals();

    }

    public class UploadedFileNational : UploadedFileBase
    {
        protected sealed override Dictionary<string, bool> Columns { get; set; }
        = new Dictionary<string, bool>()
        {
            { "First Name", true},
            { "Family Name", false },
            { "Sex", true },
            { "Age Category", true },
            { "Estimated Age", false },
            { "Birth Date", false },
            { "Registered Bahai", true },
            { "Date Registered", false },
            { "Region", true },
            { "Cluster", true },
            { "Locality", true },
            { "Focus Neighborhood", false },
            { "Address", false },
            { "Suburb", false },
            { "Code", false },
            { "Home Telephone", false },
            { "Work Telephone", false },
            { "Cell Phone", false },
            { "Email", false },
            { "B1", false },
            { "B2", false },
            { "B3(1)", false },
            { "B3(2)", false },
            { "B3(3)", false },
            { "B4", false },
            { "B5", false },
            { "B6", false },
            { "B7", false },
            { "B8", false },
            { "B9", false },
            { "B10", false }
        };

        public UploadedFileNational(long nationalCommunityId, SRP3Entities dbContext, string filePath)
        {
            NationalCommunityId = nationalCommunityId;
            DbContext = dbContext;
            Result = FileHelpers.ReadFile(filePath);
            FileHelpers.FileErrorCheck(Result, Columns);
        }

        public override List<Individual> SerializeIndividuals()
        {
            try
            {
                Individuals = new List<Individual>();

                foreach (DataRow row in Result.Tables[0].Rows)
                {
                    var i = new Individual();
                    //Basics
                    i.FirstName = Convert.ToString(row[Result.Tables[0].Columns[0]]);
                    i.FamilyName = Convert.ToString(row[Result.Tables[0].Columns[1]]);
                    if (string.IsNullOrEmpty(i.FirstName) && string.IsNullOrEmpty(i.FamilyName))
                        continue;

                    //Gender
                    i.Gender = FileHelpers.ConvertMaleFemaleToInt(Convert.ToString(row[Result.Tables[0].Columns[2]]));

                    //Age/BirthDate
                    var ageCategory = Convert.ToString(row[Result.Tables[0].Columns[3]]);
                    var estimatedAge = Convert.ToString(row[Result.Tables[0].Columns[4]]);
                    var birthDate = Convert.ToString(row[Result.Tables[0].Columns[5]]);
                    GetEstimatedYearOfBirthDate(i, ageCategory, estimatedAge, birthDate);

                    //Bahai/Registration
                    i.IsRegisteredBahai = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[6]]));
                    if (i.IsRegisteredBahai)
                    {
                        var registrationDate = Convert.ToString(row[Result.Tables[0].Columns[7]]);
                        GetRegistrationDate(i, registrationDate);
                    }

                    //Region/Cluster/Locality/Focus
                    var region = Convert.ToString(row[Result.Tables[0].Columns[8]]);
                    var cluster = Convert.ToString(row[Result.Tables[0].Columns[9]]);
                    i.LocalityId = GetLocality(Convert.ToString(row[Result.Tables[0].Columns[10]]),region,cluster);
                    i.SubdivisionId = GetFocusNeighborhood(i.LocalityId, Convert.ToString(row[Result.Tables[0].Columns[11]]));

                    //Address
                    var address = Convert.ToString(row[Result.Tables[0].Columns[12]]);
                    var suburb = Convert.ToString(row[Result.Tables[0].Columns[13]]);
                    var code = Convert.ToString(row[Result.Tables[0].Columns[14]]);
                    i.Address = GetAddress(address, suburb, code);

                    //Phones
                    var homeTelephone = Convert.ToString(row[Result.Tables[0].Columns[15]]);
                    var workTelephone = Convert.ToString(row[Result.Tables[0].Columns[16]]);
                    var cellPhone = Convert.ToString(row[Result.Tables[0].Columns[17]]);
                    AddIndividualPhones(i, homeTelephone, workTelephone, cellPhone);

                    //Email
                    var email = Convert.ToString(row[Result.Tables[0].Columns[18]]);
                    AddIndividualEmails(i, email);

                    //Books                                                                                          
                    var b1 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[19]]));
                    var b2 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[20]]));
                    var b31 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[21]]));
                    var b32 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[22]]));
                    var b33 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[23]]));
                    var b4 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[24]]));
                    var b5 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[25]]));
                    var b6 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[26]]));
                    var b7 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[27]]));
                    var b8 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[28]]));
                    var b9 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[29]]));
                    var b10 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[30]]));
                    AddBooks(i, b1, b2, b31, b32, b33, b4, b5, b6, b7, b8, b9, b10);

                    i.CreatedTimestamp = DateTime.Now;
                    i.LastUpdatedTimestamp = DateTime.Now;
                    i.GUID = Guid.NewGuid();
                    Individuals.Add(i);
                }
                return Individuals;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("File uploaded is invalid");
            }
        }

        private long GetLocality(string locality, string region, string cluster)
        {
            var rId = GetRegion(region, NationalCommunityId);
            var cId = GetCluster(cluster, rId);
            var loc = DbContext.Localities.FirstOrDefault(l => l.ClusterId == cId && l.Name == locality);
            if (loc == null)
            {
                loc = new Locality()
                {
                    Name = locality,
                    CreatedTimestamp = DateTime.Now,
                    LastUpdatedTimestamp = DateTime.Now,
                    ClusterId = cId,
                    GUID = Guid.NewGuid()
                };
                DbContext.Localities.Add(loc);
                DbContext.SaveChanges();
            }

            return loc.Id;
        }
    }

    public class UploadedFile : UploadedFileBase
    {
        protected override sealed Dictionary<string, bool> Columns { get; set; }
            = new Dictionary<string, bool>()
        {
            { "First Name", true},
            { "Family Name", false },
            { "Sex", true },
            { "Age Category", true },
            { "Estimated Age", false },
            { "Birth Date", false },
            { "Registered Bahai", true },
            { "Date Registered", false },
            { "Locality", true },
            { "Focus Neighborhood", false },
            { "Address", false },
            { "Suburb", false },
            { "Code", false },
            { "Home Telephone", false },
            { "Work Telephone", false },
            { "Cell Phone", false },
            { "Email", false },
            { "B1", false },
            { "B2", false },
            { "B3(1)", false },
            { "B3(2)", false },
            { "B3(3)", false },
            { "B4", false },
            { "B5", false },
            { "B6", false },
            { "B7", false },
            { "B8", false },
            { "B9", false },
            { "B10", false }
        };

        

        public UploadedFile(long clusterId, SRP3Entities dbContext, string filePath)
        {
            ClusterId = clusterId;
            DbContext = dbContext;
            Result = FileHelpers.ReadFile(filePath);
            FileHelpers.FileErrorCheck(Result, Columns);
        }

        public override List<Individual> SerializeIndividuals()
        {
            try
            {
                Individuals = new List<Individual>();

                foreach (DataRow row in Result.Tables[0].Rows)
                {
                    var i = new Individual();
                    //Basics
                    i.FirstName = Convert.ToString(row[Result.Tables[0].Columns[0]]);
                    i.FamilyName = Convert.ToString(row[Result.Tables[0].Columns[1]]);
                    if (string.IsNullOrEmpty(i.FirstName) && string.IsNullOrEmpty(i.FamilyName))
                        continue;

                    //Gender
                    i.Gender = FileHelpers.ConvertMaleFemaleToInt(Convert.ToString(row[Result.Tables[0].Columns[2]]));

                    //Age/BirthDate
                    var ageCategory = Convert.ToString(row[Result.Tables[0].Columns[3]]);
                    var estimatedAge = Convert.ToString(row[Result.Tables[0].Columns[4]]);
                    var birthDate = Convert.ToString(row[Result.Tables[0].Columns[5]]);
                    GetEstimatedYearOfBirthDate(i, ageCategory, estimatedAge, birthDate);

                    //Bahai/Registration
                    i.IsRegisteredBahai = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[6]]));
                    if (i.IsRegisteredBahai)
                    {
                        var registrationDate = Convert.ToString(row[Result.Tables[0].Columns[7]]);
                        GetRegistrationDate(i, registrationDate);
                    }

                    //Locality/Focus
                    i.LocalityId = GetLocality(Convert.ToString(row[Result.Tables[0].Columns[8]]));
                    i.SubdivisionId = GetFocusNeighborhood(i.LocalityId, Convert.ToString(row[Result.Tables[0].Columns[9]]));

                    //Address
                    var address = Convert.ToString(row[Result.Tables[0].Columns[10]]);
                    var suburb = Convert.ToString(row[Result.Tables[0].Columns[11]]);
                    var code = Convert.ToString(row[Result.Tables[0].Columns[12]]);
                    i.Address = GetAddress(address, suburb, code);

                    //Phones
                    var homeTelephone = Convert.ToString(row[Result.Tables[0].Columns[13]]);
                    var workTelephone = Convert.ToString(row[Result.Tables[0].Columns[14]]);
                    var cellPhone = Convert.ToString(row[Result.Tables[0].Columns[15]]);
                    AddIndividualPhones(i, homeTelephone, workTelephone, cellPhone);

                    //Email
                    var email = Convert.ToString(row[Result.Tables[0].Columns[16]]);
                    AddIndividualEmails(i, email);
                           
                    //Books                                                                                          
                    var b1   = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[17]]));
                    var b2   = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[18]]));
                    var b31 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[19]]));
                    var b32 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[20]]));
                    var b33 = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[21]]));
                    var b4   = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[22]]));
                    var b5   = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[23]]));
                    var b6   = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[24]]));
                    var b7   = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[25]]));
                    var b8   = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[26]]));
                    var b9   = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[27]]));
                    var b10  = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[Result.Tables[0].Columns[28]]));
                    AddBooks(i, b1, b2, b31, b32, b33, b4, b5, b6, b7, b8, b9, b10);

                    i.CreatedTimestamp = DateTime.Now;
                    i.LastUpdatedTimestamp = DateTime.Now;
                    i.GUID = Guid.NewGuid();
                    Individuals.Add(i);
                }
                return Individuals;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("File uploaded is invalid");
            }
        }
        
    }
}
