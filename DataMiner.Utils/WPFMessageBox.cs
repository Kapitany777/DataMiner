using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataMiner.Utils
{
    public class WPFMessageBox
    {
        /// <summary>
        /// Error message
        /// </summary>
        /// <param name="text">The error message</param>
        public static void MsgError(string text)
        {
            MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Two rows error message
        /// </summary>
        /// <param name="text1">Text of first row</param>
        /// <param name="text2">Text of second row</param>
        public static void MsgError(string text1, string text2)
        {
            MsgError(string.Format("{0}{1}{2}", text1, Environment.NewLine, text2));
        }

        /// <summary>
        /// Two rows error message
        /// </summary>
        /// <param name="text">The error message</param>
        /// <param name="ex">Exception</param>
        public static void MsgError(string text, Exception ex)
        {
            MsgError(text, ex.Message);
        }

        /// <summary>
        /// Warning message
        /// </summary>
        /// <param name="text">The warning message</param>
        public static void MsgWarning(string text)
        {
            MessageBox.Show(text, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Two rows warning message
        /// </summary>
        /// <param name="text1">Text of the first row</param>
        /// <param name="text2">Text of the second row</param>
        public static void MsgWarning(string text1, string text2)
        {
            MsgWarning(string.Format("{0}{1}{2}", text1, Environment.NewLine, text2));
        }

        /// <summary>
        /// Information message
        /// </summary>
        /// <param name="text">The information message</param>
        public static void MsgInfo(string text)
        {
            MessageBox.Show(text, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Two rows information message
        /// </summary>
        /// <param name="text1">Text of the first row</param>
        /// <param name="text2">Text of the second row</param>
        public static void MsgInfo(string text1, string text2)
        {
            MsgInfo(string.Format("{0}{1}{2}", text1, Environment.NewLine, text2));
        }

        /// <summary>
        /// Confirmation message
        /// </summary>
        /// <param name="text">The question</param>
        /// <returns></returns>
        public static MessageBoxResult MsgYesNo(string text)
        {
            return MessageBox.Show(text, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}
