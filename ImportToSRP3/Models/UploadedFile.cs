﻿using System;
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
                }
                return _individuals;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("File uploaded is invalid");
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
