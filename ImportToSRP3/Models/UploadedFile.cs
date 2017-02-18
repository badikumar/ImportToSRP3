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
                    i.EstimatedYearOfBirthDate = GetEstimatedYearOfBirthDate(ageCategory, estimatedAge, birthDate);
                    //i.DisplayBirthDate = GetDisplayBirthDate(ageCategory, estimatedAge, birthDate);
                    //i.BirthDate = GetBirthDate(ageCategory, estimatedAge, birthDate);
                }
                return _individuals;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("File uploaded is invalid");
            }
        }

        private short GetEstimatedYearOfBirthDate(string ageCategory, string estimatedAge, string birthDate)
        {
            if (!string.IsNullOrEmpty(birthDate))
            {
                DateTime dob;
                if (DateTime.TryParse(birthDate,out dob))
                {
                    return (short)dob.Year;
                }
            }
            if (!string.IsNullOrEmpty(estimatedAge))
            {
                short age;
                if (Int16.TryParse(estimatedAge, out age))
                {
                    return (short) (DateTime.Now.Year - age);
                }
            }
            if (!string.IsNullOrEmpty(ageCategory))
            {
                Random r = new Random();
                if (ageCategory == "Adult")
                {
                    return (short)(DateTime.Now.Year  - r.Next(21, 90));
                }
                if (ageCategory == "Junior Youth")
                {
                    return (short)(DateTime.Now.Year - r.Next(11, 16));
                }
                if (ageCategory == "Youth")
                {
                    return (short)(DateTime.Now.Year - r.Next(16, 21));
                }
                if (ageCategory == "Child")
                {
                    return (short)(DateTime.Now.Year - r.Next(0, 11));
                }
            }
            return 1900;
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
