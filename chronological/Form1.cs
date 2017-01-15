using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;


namespace chronological
{
    public partial class Form1 : Form
    {
        List<item> items = new List<item>();
        Queue<item> final = new Queue<item>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //entry point
            loadItems();
            richTextBox1.Clear();

            extractInfo();
            sort();
            pro();
        }

        private void loadItems()
        {
            int counter = 0;
            string line;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose a text file with a list of things";
            // Read the file and display it line by line. 
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    StreamReader file = new StreamReader(ofd.OpenFile());
                    while ((line = file.ReadLine()) != null)
                    {

                        items.Add(new item(line));
                        counter++;
                    }

                    file.Close();
                    richTextBox1.AppendText("\n" + counter + " words read from file");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        void extractInfo()
        {
            foreach (item s in items)
            {
                MatchCollection dates = Regex.Matches(s.inString, @"([0-9])\w+");
                MatchCollection text = Regex.Matches(s.inString, @"([a-zA-Z - ])*.?([a-zA-Z])");

                if (dates.Count > 1)
                {
                    s.startDate = Convert.ToInt32(dates[0].Value);
                    s.endDate = Convert.ToInt32(dates[1].Value);
                }
                else
                {
                    s.startDate = Convert.ToInt32(dates[0].Value);
                    s.endDate = 0;
                }
                if (text.Count > 0)
                    s.text = text[0].Value;

            }



        }
            item j;
        void sort()
        {
            int min = 100000;

            while (items.Count > 1)
            {
                foreach (item i in items)
                {
                    Console.WriteLine(items.Count);
                    if (i.startDate < min)
                    {
                        min = i.startDate;
                        j = i;
                    }
                }

                final.Enqueue(j);
                items.Remove(j);
                min = 100000;
            }


        }

        void pro()
        {
            item j;
            int count = final.Count();

            for (int i = 0; i < count; i++)
            {
                j = final.Dequeue();
                if (j.endDate == 0)
                    richTextBox1.AppendText("\n" + j.startDate + " - " + j.text);
                else
                    richTextBox1.AppendText("\n" + j.startDate + " - " + j.endDate + "  - " + j.text);
            }
        }

    }


    public class item
    {
        public item(string s) { inString = s; }


        public string inString;
        public int startDate;
        public int endDate = 0;
        public string text;

    }

}
