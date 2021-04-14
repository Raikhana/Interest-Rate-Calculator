using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing.Printing;
using System.Text.RegularExpressions;


namespace DateTimeStructure
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, System.EventArgs e)
        {
            txtSendInformation.Text = "";
            try
            {
                string number = txtInterestRate.Text.Trim();
                string digitsOnly1 = "";
                foreach (char c in number)
                {
                    if (!(c == '%'))
                    {
                        digitsOnly1 += c;
                    }
                }
                txtInterestRate.Text = digitsOnly1;

                if (Validatingform())
                {
                    decimal Investment = decimal.Parse(txtMonthlyInvestment.Text, NumberStyles.Currency);
                    decimal InterestRate = decimal.Parse(txtInterestRate.Text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowCurrencySymbol);
                    int years = int.Parse(txtYears.Text, NumberStyles.None);

                    int months = years * 12;
                    decimal monthlyInterestRate = InterestRate / 12 / 100;
                    decimal futureValue = CalculateFutureValue(
                        Investment, monthlyInterestRate, months);

                    decimal interestRatePercent = InterestRate / 100;
                    txtMonthlyInvestment.Text = Investment.ToString("c");
                    txtInterestRate.Text = interestRatePercent.ToString("p");
                    txtYears.Text = years.ToString();
                    txtFutureValue.Text = futureValue.ToString("c");
                    txtMonthlyInvestment.Focus();

                    string phoneNumber = txtPhoneNumber.Text.Trim(); //txtPhoneNumber.Text
                    string digitsOnly = "";
                    foreach (char c in phoneNumber)
                    {
                        if (!(
                            c == '(' || c == ')' ||
                            c == ' ' || c == '-' || c == '.'
                            ))
                        {
                            digitsOnly += c;
                        }
                    }
                    string standardFormat = digitsOnly.Insert(3, "-");
                    standardFormat = standardFormat.Insert(7, "-");
                    if (!Regex.Match(standardFormat, @"^[1-9]\d{2}-[1-9]\d{2}-\d{4}$").Success)
                    {
                        MessageBox.Show("Invalid phone number");
                    }
                    txtPhoneNumber.Text = standardFormat;

                    string fullName = txtFullName.Text; // txtFullName.Text
                    string[] names = fullName.Trim().Split(' ');

                    string first = "";
                    string middle = "";
                    string last = "";

                    if (names.Length == 1)
                        first = names[0];
                    else if (names.Length == 2)
                    {
                        first = names[0];
                        last = names[1];
                    }
                    else if (names.Length > 2)
                    {
                        first = names[0];
                        middle = names[1];
                        last = names[2];
                    }
                    txtFullName.Text =  this.ToInitialCap(first) + " " +
                                        this.ToInitialCap(middle) + " " +
                                        this.ToInitialCap(last);
                    string fillName =   this.ToInitialCap(first) + " " +
                                        this.ToInitialCap(middle) + " " +
                                        this.ToInitialCap(last);

                    DateTime currentDate = DateTime.Today; // txtCurrentDate.Text
                    txtCurrentDate.Text = currentDate.ToShortDateString();

                    string dateString = txtBirthDate.Text;//txtBirthDate.Text;
                    DateTime dateOut;
                    if ((DateTime.TryParseExact(dateString, "yyyy-MM-dd",
                                      new CultureInfo("en-US"),
                                      DateTimeStyles.None,
                                      out dateOut)))
                    {
                        txtBirthDate.Text = dateString;
                        DateTime birthDate = DateTime.Parse(txtBirthDate.Text);
                        int age = currentDate.Year - birthDate.Year; //txtAge
                        if (currentDate.DayOfYear < birthDate.DayOfYear)
                            age--;
                        txtBirthDate.Text = birthDate.ToShortDateString();
                        txtAge.Text = age.ToString();

                        txtSendInformation.Text += "Full name:\t  " + fillName + "\r\n" +
                                         "Phone number:\t  " + standardFormat + "\r\n" + "\r\n" +
                                         "Current date:\t  " + currentDate.ToShortDateString() + "\r\n" +
                                         "Birth date:\t  " + birthDate.ToShortDateString() +"\r\n" +
                                         "Age:\t\t  " + age + "\r\n" + "\r\n" +
                                         "Monthly Investment:  " + Investment.ToString("c") + "\r\n" +
                                         "Yearly Interest Rate:  " + interestRatePercent.ToString("p") + "\r\n" +
                                         "Number of Years:\t  " + years.ToString() + "\r\n" +
                                         "Future Value:\t  " + futureValue.ToString("c") + "\r\n";
                    }
                    else
                    {
                        MessageBox.Show("A birth date string {0} is invalid.", dateString);
                        txtBirthDate.Text = " ";
                        txtInterestRate.Text = " ";
                    }
                }
            }
            catch (OverflowException)
            {
                MessageBox.Show(
                    "An overflow exception. Please enter smaller values.",
                    "Entry Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" +
                    ex.GetType().ToString() + "\n" +
                    ex.StackTrace, "Exception");
            }
        }

        private string ToInitialCap(string s)
        {
            if (s.Length > 0)
            {
                string initialCap = s.Substring(0, 1).ToUpper();
                string lowerCaseLetters = s.Substring(1).ToLower();
                s = initialCap + lowerCaseLetters;
            }
            return s;
        }

        public bool Validatingform()
        {
            return
                Present(txtMonthlyInvestment, "Monthly Investment") &&
                Currency(txtMonthlyInvestment, "Monthly Investment") &&
                WithinRange(txtMonthlyInvestment, "Monthly Investment", 1, 1000) &&

                Present(txtInterestRate, "Yearly Interest Rate") &&
                Decimal(txtInterestRate, "Yearly Interest Rate") &&
                WithinRange(txtInterestRate, "Yearly Interest Rate", 1, 20) &&

                Present(txtYears, "Number of Years") &&
                Int32(txtYears, "Number of Years") &&
                WithinRange(txtYears, "Number of Years", 1, 40) &&

                Present(txtBirthDate, "Enter a birth date:");
        }

        public bool Present(TextBox textBox, string name)
        {
            if (textBox.Text == "")
            {
                MessageBox.Show(name + " is a required field.", "Entry Error");
                textBox.Focus();
                return false;
            }
            return true;
        }
        public bool Decimal(TextBox textBox, string name)
        {
            decimal number = 0m;
            if (decimal.TryParse(textBox.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out number))//
            {
                return true;
            }
            else
            {
                MessageBox.Show(name + " must be a decimal value.", "Entry Error");
                textBox.Focus();
                return false;
            }
        }
        public bool Currency(TextBox textBox, string name)
        {
            decimal number = 0m;
            if (decimal.TryParse(textBox.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out number))
            {
                return true;
            }
            else
            {
                MessageBox.Show(name + " must be in currency format.", "Entry Error");
                textBox.Focus();
                return false;
            }
        }
        public bool Int32(TextBox textBox, string name)
        {
            int number = 0;
            if (int.TryParse(textBox.Text, NumberStyles.None, CultureInfo.CurrentCulture, out number))
            {
                return true;
            }
            else
            {
                MessageBox.Show(name + " must be an integer.", "Entry Error");
                textBox.Focus();
                return false;
            }
        }
        public bool WithinRange(TextBox textBox, string name,
            decimal min, decimal max)
        {
            decimal number = decimal.Parse(textBox.Text, NumberStyles.Currency);
            if (number < min || number > max)
            {
                MessageBox.Show(name + " must be between " + min +
                    " and " + max + ".", "Entry Error");
                textBox.Focus();
                return false;
            }
            return true;
        }

        private decimal CalculateFutureValue(decimal monthlyInvestment,
            decimal interestRateMonthly, int months)
        {
            decimal futureValue = 0m;
            for (int i = 0; i < months; i++)
            {
                futureValue = (futureValue + monthlyInvestment)
                    * (1 + interestRateMonthly);
            }
            return futureValue;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            PrintDocument p = new PrintDocument();
            p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
            {
                e1.Graphics.DrawString(txtSendInformation.Text, new Font("Times New Roman", 12),
                                       new SolidBrush(Color.Black),
                                       new RectangleF(0, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
            };
            try
            {
                p.Print();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            txtMonthlyInvestment.Text = "";
            txtInterestRate.Text = "";
            txtYears.Text = "";
            txtFutureValue.Text = "";

            txtPhoneNumber.Text = "";
            txtFullName.Text = "";
            txtCurrentDate.Text = "";
            txtBirthDate.Text = "";
            txtAge.Text = "";

            txtSendInformation.Text = "";
        }
    }
}
