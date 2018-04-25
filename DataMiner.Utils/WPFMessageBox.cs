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
        /// <param name="text">A hibaüzenet szövege</param>
        public static void MsgError(string text)
        {
            MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Két soros hibaüzenet küldése
        /// </summary>
        /// <param name="text1">A hibaüzenet első sorának szövege</param>
        /// <param name="text2">A hibaüzenet második sorának szövege</param>
        public static void MsgError(string text1, string text2)
        {
            MsgError(string.Format("{0}{1}{2}", text1, Environment.NewLine, text2));
        }

        /// <summary>
        /// Két soros hibaüzenet küldése egy kivétel üzenetével együtt
        /// </summary>
        /// <param name="text">A hibaüzenet szövege</param>
        /// <param name="ex">A kivétel</param>
        public static void MsgError(string text, Exception ex)
        {
            MsgError(text, ex.Message);
        }

        /// <summary>
        /// Figyelmeztető üzenet küldése
        /// </summary>
        /// <param name="text">A figyelmeztető üzenet szövege</param>
        public static void MsgWarning(string text)
        {
            MessageBox.Show(text, "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Két soros figyelmeztető üzenet küldése
        /// </summary>
        /// <param name="text1">A figyelmeztető üzenet első sorának szövege</param>
        /// <param name="text2">A figyelmeztető üzenet második sorának szövege</param>
        public static void MsgWarning(string text1, string text2)
        {
            MsgWarning(string.Format("{0}{1}{2}", text1, Environment.NewLine, text2));
        }

        /// <summary>
        /// Információs üzenet küldése
        /// </summary>
        /// <param name="text">Az információs üzenet szövege</param>
        public static void MsgInfo(string text)
        {
            MessageBox.Show(text, "Információ", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Két soros információs üzenet küldése
        /// </summary>
        /// <param name="text1">Az információs üzenet első sorának szövege</param>
        /// <param name="text2">Az információs üzenet második sorának szövege</param>
        public static void MsgInfo(string text1, string text2)
        {
            MsgInfo(string.Format("{0}{1}{2}", text1, Environment.NewLine, text2));
        }

        /// <summary>
        /// Megerősítés (Igen / Nem) kérése
        /// </summary>
        /// <param name="text">A megerősítést kérő üzenet szövege</param>
        /// <returns></returns>
        public static MessageBoxResult MsgYesNo(string text)
        {
            return MessageBox.Show(text, "Megerősítés", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}
