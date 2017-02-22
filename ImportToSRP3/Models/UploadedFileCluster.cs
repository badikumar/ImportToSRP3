using System;
using System.Collections.Generic;
using System.Data;

namespace ImportToSRP3.Models
{
    public class UploadedFileCluster : UploadedFileBase
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

        

        public UploadedFileCluster(long clusterId, SRP3Entities dbContext, string filePath)
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
