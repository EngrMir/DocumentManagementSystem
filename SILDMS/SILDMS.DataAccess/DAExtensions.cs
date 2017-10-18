using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SILDMS.DataAccess
{
    public static class DAExtensions
    {
        /// <summary>
        /// Checks for null or zero 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool IsNullOrZero(this object param)
        {
            if (param == null)
                return true;
            if (param == DBNull.Value)
                return true;
            if (param.ToString() == "0")
                return true;

            return false;
        }

        /// <summary>
        /// To Prefix "E" with the Error Code provided from Database.
        /// </summary>
        /// <param name="param">ErrorCode</param>
        /// <returns></returns>
        public static string PrefixErrorCode(this object param)
        {
            if (param != null)
                return param.ToString();

            return "";
        }

        #region Get Database Values

        // returns a boolean value, checking for nulls
        public static bool GetBoolean(this IDataReader dr, int i)
        {
            if (!dr.IsDBNull(i))
                return dr.GetBoolean(i);
            else
                return false;
        }

        // returns a boolean value, checking for nulls
        public static bool GetBoolean(this IDataReader dr, string fieldName)
        {
            if (!(dr[fieldName] is System.DBNull))
                return Convert.ToBoolean(dr[fieldName]);
            else
                return false;
        }

        // returns a boolean value, checking for nulls
        public static bool GetBoolean(this DataRow row, int i)
        {
            if (!row.IsNull(i))
                return (bool)row[i];
            else
                return false;
        }

        // returns a boolean value, checking for nulls
        public static bool GetBoolean(this DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
                return Convert.ToBoolean(row[fieldName]);
            else
                return false;
        }

        // returns a double value, checking for nulls
        public static double GetDouble(this DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
                return Convert.ToDouble(row[fieldName]);
            else
                return 0;
        }

        // returns a double value, checking for nulls
        public static double GetDouble(this IDataReader dr, int i)
        {
            if (!dr.IsDBNull(i))
                return dr.GetDouble(i);
            else
                return 0;
        }

        // returns a decimal value, checking for nulls
        public static decimal GetDecimal(this IDataReader dr, int i)
        {
            if (!dr.IsDBNull(i))
                return dr.GetDecimal(i);
            else
                return 0;
        }
        // returns a DateTime value, checking for nulls
        public static DateTime GetDateTime(this IDataReader dr, int i)
        {
            if (!dr.IsDBNull(i))
                return dr.GetDateTime(i);
            else
                return DateTime.MinValue;
        }

        // returns a DateTime value, checking for nulls
        public static DateTime GetDateTime(this IDataReader dr, string fieldName)
        {
            if (!(dr[fieldName] is System.DBNull))
                return (DateTime)dr[fieldName];
            else
                return DateTime.MinValue;
        }

        // returns a double value, checking for nulls
        public static double GetDouble(this IDataReader dr, string fieldName)
        {
            if (!(dr[fieldName] is System.DBNull))
                return Convert.ToDouble(dr[fieldName]);
            else
                return 0;
        }

        // returns a decimal value, checking for nulls
        public static decimal GetToDecimal(this DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
                return Convert.ToDecimal(row[fieldName]);
            else
                return 0;
        }

        // always returns a DateTime value, checking for nulls
        public static DateTime GetDateTime(this DataRow row, int i)
        {
            if (!row.IsNull(i))
                return Convert.ToDateTime(row[i]);
            else
                return DateTime.MinValue;
        }

        // always returns a DateTime value, checking for nulls
        public static DateTime GetDateTime(this DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
                return (DateTime)row[fieldName];
            else
                return DateTime.MinValue;
        }

        // always returns a DateTime value, checking for nulls
        public static string GetDateTime_UTC_TimeZone(this DataRow row, string fieldName, int offSetMins)
        {
            if (!row.IsNull(fieldName))
            {
                // Getting datetime in UTC.
                DateTime utc = (DateTime)row[fieldName];

                // Converting UTC datetime to given timezone datetime
                DateTime dt = utc.AddMinutes(offSetMins * -1);

                return dt.ToString("dddd, MMMM dd, yyyy hh:mm tt", new System.Globalization.CultureInfo("en-US"));
            }
            else
                return DateTime.MinValue.ToString("dddd, MMMM dd, yyyy hh:mm tt", new System.Globalization.CultureInfo("en-US"));
        }

        /// <summary>
        /// Get DateTime in Universal Sorta­ble DateTime Pat­tern.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetDateTime_UTCFormat(this DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
            {
                // Getting datetime in UTC.
                DateTime dt = (DateTime)row[fieldName];

                return dt.ToString("yyyy-MM-dd HH:mm:ss.fff", new System.Globalization.CultureInfo("en-US"));
            }
            else
                return string.Empty;
        }

        // returns a integer value, checking for nulls
        public static Int16 GetInt16(this IDataReader dr, int i)
        {
            if (!dr.IsDBNull(i))
                return dr.GetInt16(i);
            else
                return 0;
        }

        // returns a integer value, checking for nulls
        public static Int16 GetInt16(this IDataReader dr, string fieldName)
        {
            if (!(dr[fieldName] is System.DBNull))
                return Convert.ToInt16(dr[fieldName].ToString());
            else
                return 0;
        }

        // returns a short value, checking for nulls
        public static short GetInt16(this DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
                return Convert.ToInt16(row[fieldName]);
            else
                return 0;
        }

        // returns a integer value, checking for nulls
        public static int GetInt32(this IDataReader dr, int i)
        {
            if (!dr.IsDBNull(i))
                return dr.GetInt32(i);
            else
                return 0;
        }

        // returns a integer value, checking for nulls
        public static int GetInt32(this IDataReader dr, string fieldName)
        {
            if (!(dr[fieldName] is System.DBNull))
                return Convert.ToInt32(dr[fieldName].ToString());
            else
                return 0;
        }

        // returns a integer value, checking for nulls
        public static int GetInt32(this DataRow row, int i)
        {
            if (!row.IsNull(i))
                return (int)row[i];
            else
                return 0;
        }

        // returns a integer value, checking for nulls
        public static int GetInt32(this DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
                return Convert.ToInt32(row[fieldName]);
            else
                return 0;
        }

        // returns a integer value, checking for nulls
        public static Int64 GetInt64(this IDataReader dr, int i)
        {
            if (!dr.IsDBNull(i))
                return dr.GetInt64(i);
            else
                return 0;
        }

        // returns a integer value, checking for nulls
        public static Int64 GetInt64(this IDataReader dr, string fieldName)
        {
            if (!(dr[fieldName] is System.DBNull))
                return (Int64)dr[fieldName];
            else
                return 0;
        }

        // returns a long value, checking for nulls
        public static long GetInt64(this DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
                return Convert.ToInt64(row[fieldName]);
            else
                return 0;
        }
        // returns a string value, checking for nulls
        public static string GetString(this IDataReader dr, int i)
        {
            if (!dr.IsDBNull(i))
                return dr.GetString(i);
            else
                return string.Empty;
        }

        // returns a string value, checking for nulls
        public static string GetString(this DataRow row, int i)
        {
            if (!row.IsNull(i))
                return row[i].ToString();
            else
                return string.Empty;
        }

        // returns a string value, checking for nulls
        public static string GetString(this DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
                return row[fieldName].ToString();
            else
                return string.Empty;
        }

        // returns a string value, checking for nulls
        public static string GetString(this IDataReader dr, string fieldName)
        {
            if (!(dr[fieldName] is System.DBNull))
                return dr[fieldName].ToString();
            else
                return string.Empty;
        }

        // returns a string value, checking for nulls
        public static decimal GetDecimal(this IDataReader dr, string fieldName)
        {
            if (!(dr[fieldName] is System.DBNull))
                return Convert.ToDecimal(dr[fieldName].ToString());
            else
                return 0;
        }

        /// <summary>
        /// Return 0 if the object is null.
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public static int NullToZero(this object dataObject)
        {
            if (dataObject is System.DBNull)
                return 0;
            else if (dataObject == null)
                return 0;
            else
                return Convert.ToInt32(dataObject);
        }

        /// <summary>
        /// Return emty string if the object is null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String NullToEmptyString(this Object value)
        {
            return (value != null && !(value is System.DBNull)) ? (String)value : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public static String NullToString(this object dataObject)
        {
            if (dataObject is System.DBNull)
                return string.Empty;
            else if (dataObject == null)
                return string.Empty;
            else
                return Convert.ToString(dataObject);
        }

        /// <summary>
        /// Convert to long
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public static long NullToLong(this object dataObject)
        {
            if (dataObject is System.DBNull)
                return 0;
            else if (dataObject == null)
                return 0;
            else
                return Convert.ToInt64(dataObject);
        }

        /// <summary>
        /// Convert to int
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public static int NullToInt(this object dataObject)
        {
            if (dataObject is System.DBNull)
                return 0;
            else if (dataObject == null)
                return 0;
            else
                return Convert.ToInt32(dataObject);
        }

        /// <summary>
        /// Convert to double
        /// </summary>
        /// <param name="inObj"></param>
        /// <returns></returns>
        public static double NullToDouble(this Object inObj)
        {
            return (inObj != null) ? Convert.ToDouble(inObj) : 0.00;
        }

        /// <summary>
        /// Check the value is integer
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInteger(this string value)
        {
            bool retValue = false;
            if (value != null)
            {
                string strNumber = value.ToString();
                System.Text.RegularExpressions.Regex notIntPattern = new System.Text.RegularExpressions.Regex("[^0-9-]");
                System.Text.RegularExpressions.Regex intPattern = new System.Text.RegularExpressions.Regex("^-[0-9]+$|^[0-9]+$");
                retValue = !notIntPattern.IsMatch(strNumber) && intPattern.IsMatch(strNumber);

            }
            return retValue;
        }

        /// <summary>
        /// Check the value is a valid date
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ValidDate(this DateTime value)
        {
            DateTime minDate = Convert.ToDateTime("1/1/1753");
            DateTime maxDate = Convert.ToDateTime("12/31/9999");
            if (value < minDate || value > maxDate)
                return null;
            else
                return value;
        }

        /// <summary>
        /// StringToDouble
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double StringToDouble(this string value)
        {
            if (IsInteger(value))
                return Convert.ToDouble(value);
            else
                return 0;
        }

        /// <summary>
        /// StringToLong
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long StringToLong(this string value)
        {
            if (IsInteger(value))
                return Convert.ToInt64(value);
            else
                return 0;
        }

        /// <summary>
        /// StringToInt
        /// </summary>
        /// <param name="intString"></param>
        /// <returns></returns>
        public static int StringToInt(this string value)
        {
            if (IsInteger(value))
                return Convert.ToInt32(value);
            else
                return 0;
        }

        /// <summary>
        /// StringToDecimal
        /// </summary>
        /// <param name="decString"></param>
        /// <returns></returns>
        public static decimal StringToDecimal(this string value)
        {
            decimal result;
            if (decimal.TryParse(value, out result))
                return result;
            else
                return (decimal)(0.00);
        }

        /// <summary>
        /// String To Bool
        /// </summary>
        /// <param name="isTrue"></param>
        /// <returns></returns>
        public static bool StringToBool(this string isTrue)
        {
            bool result = false;
            if (isTrue == "1" || isTrue.ToLower() == "true")
                result = true;
            else if (isTrue == "0" || isTrue.ToLower() == "false")
                result = false;
            return result;
        }
        /// <summary>
        /// Checks if A is in B
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool AInB(this object a, object b)
        {
            if ((a == null) || (b == null))
            {
                throw (new ArgumentNullException());
            }
            if (a.GetType() != b.GetType())
            {
                return false; //different types
            }
            return (((int)a & (int)b) == (int)a);
        }

        /// <summary>
        /// Convert to Minutes
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static double GetMinute(this double hour)
        {
            double intPart = Math.Truncate(hour);
            double decimalPart = hour - intPart;
            double minuteVal = (intPart + (decimalPart * 100 / 60)) * 60;
            return minuteVal;
        }

        /// <summary>
        /// Convert to Hours
        /// </summary>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static double GetHour(this double minute)
        {
            double hour = minute / 60;
            double intPart = Math.Truncate(hour);
            double decimalPart = hour - intPart;
            hour = intPart + (decimalPart * 60 / 100);
            return hour;
        }

        /// <summary>
        /// Serializes the passed object to Xml string.
        /// </summary>
        /// <param name="objectToSerialize"></param>
        /// <returns>XML String</returns>
        public static string Serialize(this object objectToSerialize)
        {
            try
            {
                XmlWriterSettings writerSettings = new XmlWriterSettings();

                writerSettings.OmitXmlDeclaration = true;

                StringWriter stringWriter = new StringWriter();
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, writerSettings))
                {
                    XmlSerializer serializer = new XmlSerializer(objectToSerialize.GetType());
                    serializer.Serialize(xmlWriter, objectToSerialize, ns);
                }
                return stringWriter.ToString();

            }
            catch (Exception ex)
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// TO retrieve the description of an enum value
        /// </summary>
        /// <param name="enumValue">enum object</param>
        /// <returns>description value</returns>
        public static string GetDescriptionValueOf(object enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return enumValue.ToString();
        }

        /// <summary>
        /// To modify the given xml and deserialize to assign data to the given serializable entity class.
        /// </summary>
        /// <param name="dataToDeserialize">Serializable class to populate values.</param>
        /// <param name="xmlToDeserialize">XML to modify as string.</param>
        /// <param name="datasetName">DataSetName to remove from the xml.</param>
        /// <param name="tableName">TableName to replace with entityname in the xml.</param>
        /// <param name="entityName">Entity name to replace tablename in the xml.</param>
        /// <returns>Populated given class as object.</returns>
        public static object Deserialize(this object dataToDeserialize, string xmlToDeserialize, string datasetName, string tableName, string entityName)
        {
            string xmlForSingleEntity = string.Empty;

            //Validate the argument and raise an argument exception if invalid.
            if (dataToDeserialize == null || xmlToDeserialize == null || datasetName == null || tableName == null || entityName == null)
                throw new ArgumentException("Argument can not be null.");

            //As the given xml is generated from dataset.getxml() method it contains the  dataset/table 
            //default name or given name. For deserializing the xml to a single entity we need to remove 
            //the dataset name and modify the table name to match entity name.
            //XML.substring(
            //PARAM 1 : datasetName.Length + 2 = given datasetname + 2(for '<' & '>') as startindex (So it will cut 
            //the dataset name start tag).
            //PARAM 2 :(xmlToDeSerialize.Length - (datasetName.Length + 2)) - (datasetName.Length + 3)
            //As we have removed the start tag current string length = total length - removed start tag length.
            //After calculating the current string length remove the endtag (Dataset name) by 
            //subtracting dataset name + 3 (For '<' & '/' & '>').
            xmlForSingleEntity = xmlToDeserialize.Substring(datasetName.Length + 2, (xmlToDeserialize.Length - (datasetName.Length + 2)) - (datasetName.Length + 3));

            //Replace the tablename with entity name.
            xmlForSingleEntity = xmlForSingleEntity.Replace(tableName, entityName);

            //Call the deserialize function to deserialize the modified xml to the given class object.
            return Deserialize(dataToDeserialize, xmlForSingleEntity);
        }

        /// <summary>
        /// To deserialize the given xml and assign data to the given serializable class.
        /// </summary>
        /// <param name="dataToDeserialize">Serializable class to populate values.</param>
        /// <param name="xmlToDeserialize">Corresponding XML string to deserialize the given class.</param>
        /// <returns>Populated given class as object.</returns>
        public static object Deserialize(this object dataToDeserialize, string xmlToDeserialize)
        {

            XmlSerializer xmlSerializer = null;
            TextReader txtReader = null;

            //Validate the arguments and raise an argument exception if invalid.
            if (dataToDeserialize == null || xmlToDeserialize == null)
                throw new ArgumentException("Argument can not be null.");

            try
            {
                //Initialize textreader and Read the given xml string.
                txtReader = new StringReader(xmlToDeserialize);

                //Initialize the XMLSerializer with the type of the object used for deserializing.
                xmlSerializer = new XmlSerializer(dataToDeserialize.GetType());

                //Deserialize the xml and assign to the given object.
                dataToDeserialize = xmlSerializer.Deserialize(txtReader);

            }
            finally
            {
                //Dispose the objects.
                if (xmlSerializer != null)
                    xmlSerializer = null;

                if (txtReader != null)
                {
                    txtReader.Dispose();
                }
            }

            return dataToDeserialize;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdfBytes"></param>
        /// <returns></returns>
        public static string CreateFileFromByte(byte[] pdfBytes, string fileName)
        {
            System.IO.File.WriteAllBytes(fileName, pdfBytes);
            return fileName;
        }
        #endregion
    }
}
