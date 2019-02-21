using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Cacu
{

    public static class MyExtensions
    {

        /// <summary>
        /// 小數點擷取
        /// </summary>
        public static string CutString(this string text, int decimalLength)
        {

            if (text.Length >= decimalLength)
            {

                return text.Substring(0, decimalLength);
            }
            else
            {
                return text;
            }
        }
       
        /// <summary>
        /// 文字轉語音
        /// </summary>

        public static async Task SpeakNowDefaultSettings(this string text)
        {
            await TextToSpeech.SpeakAsync(text);
        }
    }
}
