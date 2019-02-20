using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cacu
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 計算機螢幕的數字(結果)
        /// </summary>
        public string ResultNumber
        {
            get { return this.resultNumber; }

            set
            {
                if (ResultNumber != value)
                {
                    resultNumber = value;
                    OnPropertyChanged(nameof(this.ResultNumber));
                }
            }
        }

        /// <summary>
        /// 計算機按鈕
        /// </summary>
        public ICommand OnButton { get; set; }

        public MainPageViewModel()
        {
            OnButton = new Command<string>(value =>
            {
                try
                {
                    switch (value)
                    {
                        case "0":
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                        case "9":
                            // 按過運算符號 再按數字鍵 將數值取代
                            if (this.useMathSymbol)
                            {
                                this.ResultNumber = value;
                                this.useMathSymbol = false;
                            }
                            else
                            {
                                // 如果是0 取代數值 不是0把數字加到後面
                                this.ResultNumber = this.ResultNumber == "0" ? value : $"{this.ResultNumber}{value}";
                            }

                            break;
                        case "+":
                        case "-":
                        case "*":
                        case "/":
                            this.useMathSymbol = true;
                            this.CheckDecimalPoint();
                            // 第一次按運算符號將 數值 和運算符號 儲存
                            // 第二次按運算符號   計算出結果  並將結果和運算符號儲存
                            if (mathList.Count >= 2)
                            {
                                this.ResultNumber = Calculate();
                                mathList.Clear();
                                mathList.Add(this.ResultNumber);
                                mathList.Add(value);
                            }
                            else
                            {
                                mathList.Add(this.ResultNumber);
                                mathList.Add(value);
                            }
                            break;
                        case "Equal":
                            this.useMathSymbol = true;
                            this.CheckDecimalPoint();
                            this.ResultNumber = Calculate();
                            mathList.Clear();
                            break;
                        case "Clear":
                            this.ResultNumber = "0";
                            mathList.Clear();
                            break;
                        case "Reverse":
                            string rever = $"{this.ResultNumber}* -1";
                            this.ResultNumber = new DataTable().Compute(rever, null).ToString();
                            break;
                        case "Percent":
                            string percent = $"{this.ResultNumber}* 0.01";
                            this.ResultNumber = new DataTable().Compute(percent, null).ToString();
                            break;
                        case "DecimalPoint":
                            if (!this.ResultNumber.Contains("."))
                            {
                                this.ResultNumber = $"{this.ResultNumber }.";
                            }
                            break;

                        default: break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            });
        }


        /// <summary>
        /// 計算
        /// </summary>
        private string Calculate()
        {
            string _math = string.Empty;
            foreach (var _m in mathList)
            {
                _math += _m;
            }
            _math = $"{_math}{this.ResultNumber}";
            string result = new DataTable().Compute(_math, null).ToString();

            // 如果有小數點
            if (result.Contains("."))
            {
                string[] spiltrResult = result.Split('.');
                // 保留小數點8位
                result = $"{spiltrResult[0]}.{spiltrResult[1].CutString(8)}";
            }

            return result;
        }

        /// <summary>
        /// 檢查小數點
        /// </summary>
        private void CheckDecimalPoint()
        {
            if (this.ResultNumber[(this.ResultNumber.Length - 1)] == '.')
            {
                this.ResultNumber = this.ResultNumber.Replace('.', ' ');
            }
        }




        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string resultNumber = "0";
        List<string> mathList = new List<string>();
        bool useMathSymbol = false;
    }



    /// <summary>
    /// 擴充方法 (應該cs檔統一放擴充方法)
    /// </summary>
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

    }
}
