using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel;

namespace ImportToSRP3.Models
{
    public class FileHelpers
    {
        public static bool FileErrorCheck(DataSet result, Dictionary<string, bool> columns)
        {
            if (columns.Count != result.Tables[0].Columns.Count)
                throw new InvalidOperationException("The file uploaded is invalid. Does not have all the prescribed columns from the template.");
            for (var rindex = 0; rindex < result.Tables[0].Rows.Count - 1; rindex++)
            {
                var row = result.Tables[0].Rows[rindex];
                for (var i = 0; i < columns.Count; i++)
                {
                    var value = Convert.ToString(row[result.Tables[0].Columns[i]]);
                    if (string.IsNullOrEmpty(value) && columns.ElementAt(i).Value)
                    {
                        throw new InvalidOperationException($@"Row {rindex + 2} has missing value for ""{columns.ElementAt(i).Key}""");
                    }
                }
            }
            return true;
        }

        public static bool ConvertYesNoToBoolean(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str == "Yes" || str=="Y")
                    return true;
                if (str == "No" || str=="N")
                    return false;
            }
            return false;
        }

        public static byte ConvertMaleFemaleToInt(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str == "M" || str=="Male")
                    return 1;
                if (str == "F" || str =="Female")
                    return 0;
            }
            return 1;
        }


        public static DataSet ReadFile(string filePath)
        {
            if (Path.GetFileName(filePath)?.EndsWith(".xlsx") ?? false )
            {
                try
                {
                    var reader = ExcelReaderFactory.CreateOpenXmlReader(new FileStream(filePath, FileMode.Open));
                    reader.IsFirstRowAsColumnNames = true;
                    var dset = reader.AsDataSet();
                    //hack to remove empty row in the end
                    dset.Tables[0].Rows.RemoveAt(reader.Depth - 2);
                    reader.Close();
                    return dset;
                }
                catch (IOException ex)
                {
                    throw new InvalidOperationException("The file is open. Please close the file before importing.");
                }
            }
            throw new InvalidOperationException("This file format is not supported");
        }
    }
}
