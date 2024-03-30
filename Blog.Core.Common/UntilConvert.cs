using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS_SendToL2LWinServer_ACG.Helper
{
    public static class UntilConvert
    {
        public static bool GetIsEmptyOrNull(this object thisValue)
        {
            return thisValue.GetCString() == "" || thisValue.GetCString() == "undefined" || thisValue.GetCString() == "null";
        }

        public static bool GetIsNotEmptyOrNull(this object thisValue)
        {
            return thisValue.GetCString() != "" && thisValue.GetCString() != "undefined" && thisValue.GetCString() != "null";
        }
        
        public static int GetCInt(this object thisValue)
        {
            int result = 0;
            if (thisValue == null)
            {
                return 0;
            }
            if (thisValue != null && thisValue != DBNull.Value)
            {
                int.TryParse(thisValue.ToString(), out result);
                return result;
            }
            return result;
        }

        public static int GetCInt(this object thisValue, int errorValue)
        {
            int result;
            if (thisValue != null && thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return errorValue;
        }

        public static double GetCMoney(this object thisValue)
        {
            double result;
            if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return 0.0;
        }

        public static double GetCMoney(this object thisValue, double errorValue)
        {
            double result;
            if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return errorValue;
        }

        public static string GetCString(this object thisValue)
        {
            if (thisValue != null)
            {
                return thisValue.ToString().Trim();
            }
            return "";
        }

        public static string GetCString(this object thisValue, string errorValue)
        {
            if (thisValue != null)
            {
                return thisValue.ToString().Trim();
            }
            return errorValue;
        }

        public static decimal GetCDecimal(this object thisValue)
        {
            decimal result;
            if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return decimal.Zero;
        }

        public static decimal GetCDecimal(this object thisValue, decimal errorValue)
        {
            decimal result;
            if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return errorValue;
        }

        public static double GetCDouble(this object thisValue)
        {
            double result;
            if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return 0.0;
        }

        public static double GetCDouble(this object thisValue, double errorValue)
        {
            double result;
            if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return errorValue;
        }

        public static DateTime GetCDate(this object thisValue)
        {
            DateTime result = DateTime.MinValue;
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out result))
            {
                result = Convert.ToDateTime(thisValue);
            }
            return result;
        }

        public static DateTime GetCDate(this object thisValue, DateTime errorValue)
        {
            DateTime result;
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out result))
            {
                return result;
            }
            return errorValue;
        }

        public static bool GetCBool(this object thisValue)
        {
            bool result = false;
            if (thisValue != null && thisValue != DBNull.Value)
            {
                bool.TryParse(thisValue.ToString(), out result);
                return result;
            }
            return result;
        }
    }
}
