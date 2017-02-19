using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportToSRP3.Models
{
    public class UploadedFile
    {
        private readonly Dictionary<string, bool> _columns = new Dictionary<string, bool>()
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

        private List<Individual> _individuals;
        private SRP3Entities _dbContext;
        private string _filePath;
        private DataSet _result;

        public UploadedFile(SRP3Entities dbContext, string filePath)
        {
            _dbContext = dbContext;
            _filePath = filePath;
            _result = FileHelpers.ReadFile(filePath);
            FileHelpers.FileErrorCheck(_result, _columns);
        }

        public List<Individual> SerializeIndividuals()
        {
            try
            {
                _individuals = new List<Individual>();

                foreach (DataRow row in _result.Tables[0].Rows)
                {
                    var i = new Individual();

                    i.FirstName = Convert.ToString(row[_result.Tables[0].Columns[0]]);
                    i.FamilyName = Convert.ToString(row[_result.Tables[0].Columns[1]]);
                    i.Gender = FileHelpers.ConvertMaleFemaleToInt(Convert.ToString(row[_result.Tables[0].Columns[2]]));
                    i.IsSelectedEstimatedYearOfBirthDate = true;
                    var ageCategory = Convert.ToString(row[_result.Tables[0].Columns[3]]);
                    var estimatedAge = Convert.ToString(row[_result.Tables[0].Columns[4]]);
                    var birthDate = Convert.ToString(row[_result.Tables[0].Columns[5]]);
                    GetEstimatedYearOfBirthDate(i,ageCategory, estimatedAge, birthDate);
                    i.IsRegisteredBahai = FileHelpers.ConvertYesNoToBoolean(Convert.ToString(row[_result.Tables[0].Columns[6]]));
                    if (i.IsRegisteredBahai)
                    {
                        var registrationDate = Convert.ToString(row[_result.Tables[0].Columns[7]]);
                        GetRegistrationDate(i, registrationDate);
                    }
                    _individuals.Add(i);
                }
                return _individuals;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("File uploaded is invalid");
            }
        }

        private void GetRegistrationDate(Individual i, string registrationDate)
        {
            if (!string.IsNullOrEmpty(registrationDate))
            {
                DateTime regDate;
                if (DateTime.TryParse(registrationDate, out regDate))
                {
                    i.RegistrationDate = regDate;
                    i.DisplayRegistrationDate = regDate.ToString("YYYY-MM-dd");
                    return;
                }
            }
            i.RegistrationDate = new DateTime(DateTime.Now.Year,1,1);
            i.DisplayRegistrationDate = i.RegistrationDate.Value.ToString("YYYY-MM-dd");
        }

        private void GetEstimatedYearOfBirthDate(Individual i, string ageCategory, string estimatedAge, string birthDate)
        {
            if (!string.IsNullOrEmpty(birthDate))
            {
                DateTime dob;
                if (DateTime.TryParse(birthDate,out dob))
                {
                    i.EstimatedYearOfBirthDate = (short)dob.Year;
                    i.BirthDate = dob;
                    i.DisplayBirthDate = dob.ToString("YYYY-MM-dd");
                    return;
                }
            }
            if (!string.IsNullOrEmpty(estimatedAge))
            {
                short age;
                if (Int16.TryParse(estimatedAge, out age))
                {
                    i.EstimatedYearOfBirthDate = (short) (DateTime.Now.Year - age);
                    i.BirthDate = new DateTime(i.EstimatedYearOfBirthDate.Value,1,1);
                    i.DisplayBirthDate = i.EstimatedYearOfBirthDate.Value.ToString();
                    return;
                }
            }
            if (!string.IsNullOrEmpty(ageCategory))
            {
                Random r = new Random();
                
                if (ageCategory == "Adult")
                {
                    i.EstimatedYearOfBirthDate = (short)(DateTime.Now.Year  - r.Next(21, 90));
                }
                else if (ageCategory == "Junior Youth")
                {
                    i.EstimatedYearOfBirthDate = (short)(DateTime.Now.Year - r.Next(11, 16));
                }
                else if (ageCategory == "Youth")
                {
                    i.EstimatedYearOfBirthDate = (short)(DateTime.Now.Year - r.Next(16, 21));
                }
                else if (ageCategory == "Child")
                {
                    i.EstimatedYearOfBirthDate = (short) (DateTime.Now.Year - r.Next(0, 11));
                }
                else
                {
                    i.EstimatedYearOfBirthDate = 1900;
                }
                i.BirthDate = new DateTime(i.EstimatedYearOfBirthDate.Value, 1, 1);
                i.DisplayBirthDate = i.EstimatedYearOfBirthDate.Value.ToString();
            }
        }

        public List<ActivityStudyItemIndividual> SerializeActivityStudyItemIndividuals()
        {
            return new List<ActivityStudyItemIndividual>();

            var asii = new ActivityStudyItemIndividual()
            {
                IndividualType = 1,
                IndividualRole = 7,
                IsCurrent = true,
                IsCompleted = true,
                StudyItemId = _dbContext.StudyItems.First(si => si.ActivityStudyItemType == "Book" && si.Number == 1).Id,
                IndividualId = 1
            };
        }
    }
}
