using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DateneinlesenXML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            getXML();
        }

        private void getXML() {
            listBox1.Items.Clear();

            try
            {
                string eintrag;
                string wechselkurs = @"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml"; //"wechselkurs_neu.xml";

                XmlTextReader reader = new XmlTextReader(wechselkurs);

                while (reader.Read())
                {
                    if (reader.Name != "")
                    {
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            if (reader.Name == "Cube") //prüft den Knoten nach dem Namen Cube
                            { 
                                if (reader.AttributeCount == 1)
                                {

                                    reader.MoveToAttribute("time");

                                    listBox1.Items.Add("ECB Stand: " + reader.Value);
                                    listBox1.Items.Add("_________________________");
                                }
                                if (reader.AttributeCount == 2)
                                {
                                    reader.MoveToAttribute("currency");
                                    eintrag = (reader.Value);

                                    reader.MoveToAttribute("rate");
                                    Decimal kurs = decimal.Parse(reader.Value.Replace(".", ","));

                                    listBox1.Items.Add(eintrag + "\tKurs: " + kurs);
                                }
                                reader.MoveToNextAttribute();
                            }
                        } 
                    } 
                }

                reader.Close();
            }
            catch (Exception ex) {
                MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void listCurrency()
        {
            comboBox1.Items.Add("EUR");
            comboBox2.Items.Add("EUR");
            try
            {
                string eintrag;
                string wechselkurs = @"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml"; //"wechselkurs_neu.xml";

                XmlTextReader reader = new XmlTextReader(wechselkurs);

                while (reader.Read())
                {
                    if (reader.Name != "")
                    {
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            if (reader.Name == "Cube")
                            {
                               
                                if (reader.AttributeCount == 2)
                                {
                                    reader.MoveToAttribute("currency");
                                    eintrag = (reader.Value);
                                    comboBox1.Items.Add(eintrag);
                                    comboBox2.Items.Add(eintrag);

                                }
                                reader.MoveToNextAttribute();
                            }
                        }
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
            }
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            listCurrency();
        }


        private double getCurrencyRate(string currency) {
            double rate = 0.00;

            try
            {
                if (currency == "EUR") 
                {
                    rate = 1.00;
                }
                else { 
                string wechselkurs = @"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";

                XmlTextReader reader = new XmlTextReader(wechselkurs);

                while (reader.Read())
                {
                    if (reader.Name != "")
                    {
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            if (reader.Name == "Cube")
                            {
                                 if (reader.AttributeCount == 2)
                                {
                                        reader.MoveToAttribute("currency");

                                        if (reader.Value == currency) {
                                        reader.MoveToAttribute("rate");
                                        Decimal kurs = decimal.Parse(reader.Value.Replace(".", ","));
                                        rate = Convert.ToDouble(kurs);
                                        break;
                                    }
                                }
                                reader.MoveToNextAttribute();
                            }
                        }
                    }
                }
                reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
            }
            return rate;
        }

        private double inEuro(double rate, string wert) {

            double ergebnis = double.Parse(wert) / rate;
            return ergebnis;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            double rate = getCurrencyRate(comboBox1.Text);

            double euro = inEuro(rate, textBox1.Text);

            double fremdw = euro * getCurrencyRate(comboBox2.Text);

            textBox2.Text = getCurrencyRate(comboBox2.Text).ToString();


            textBox3.Text = fremdw.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

            textBox2.Text = "";
            textBox3.Text = "";
        }
    }
}
