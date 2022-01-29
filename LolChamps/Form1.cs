using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LolChamps // This project was originally created for League of Legends, that's why the names looks like this :D
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            fileDirectory();
        }
        public int ChampCount
        {
            get => int.Parse(champCountLabel.Text);
            set => champCountLabel.Text = value.ToString();
        }
        private string filePath = @"champList.txt";
        private StreamReader sr;
        private void fileDirectory()
        {
            if (!File.Exists(@"..\champList.txt"))
            {
                File.Create(@"..\champList.txt").Close();
            }
            else
            {
                champListBox.Items.Clear();
                try
                {
                    sr = new StreamReader(filePath);
                    while (!sr.EndOfStream)
                    {
                        champListBox.Items.Add(sr.ReadLine());
                    }
                    sr.Dispose();
                }
                catch (Exception) { }
                finally { ChampCount = champListBox.Items.Count; }
            }
        }
        private void AddChamp()
        {
            string currentChamp = champNameBox.Text.ToLower().Trim();
            if (currentChamp == "")
            {
                MessageBox.Show("Name cannot be empty", "Oops..", MessageBoxButtons.OK);
                return;
            }
            currentChamp = FixName(currentChamp);
            if (!champListBox.Items.Contains(currentChamp))
            {
                champListBox.Items.Add(currentChamp);
                StreamWriter sw = File.AppendText(filePath);
                ChampCount++;
                sw.WriteLine(currentChamp);
                sw.Dispose();
            }
            else
            {
                MessageBox.Show($"{currentChamp} is already on the list!", "Already on the list!", MessageBoxButtons.OK);
            }
            champNameBox.Clear();
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            AddChamp();
        }

        private void ClearListBtn_Click(object sender, EventArgs e)
        {
            champListBox.Items.Clear();
            ChampCount = 0;
        }
        static string FixName(string str)
        {
            str = str.Trim();
            char[] letters = str.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);
            string result = string.Empty;
            bool upTheNextOne = false;
            foreach (var letter in letters)
            {
                if (upTheNextOne)
                {
                    char current = char.ToUpper(letter);
                    result += current;
                    upTheNextOne = false;
                    continue;
                }
                if (letter == ' ' || letter == '\'')
                {
                    upTheNextOne = true;
                }

                result += letter;
            }
            return result;
        }

        private void champNameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddChamp();
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            bool hasInput = false;
            string currentChamp = champNameBox.Text.Trim();
            if (currentChamp != "")
            {
                currentChamp = FixName(currentChamp);
                if (champListBox.Items.Count > 0)
                {
                    if (champListBox.Items.Contains(currentChamp))
                    {
                        champListBox.Items.Remove(currentChamp);
                        ChampCount--;
                    }
                    else
                    {
                        MessageBox.Show($"{currentChamp} is not on the list!", "Not on the list!", MessageBoxButtons.OK);
                    }
                }
                hasInput = true;
                champNameBox.Text = "";
            }
            if (!hasInput)
            {
                if (champListBox.SelectedIndex == -1)
                    MessageBox.Show("Please select a champ or type it's name!", "No item selected!", MessageBoxButtons.OK);
                else
                {
                    champListBox.Items.Remove(champListBox.SelectedItem);
                    ChampCount--;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StreamWriter sw = new StreamWriter(filePath);
            foreach (var item in champListBox.Items)
            {
                sw.WriteLine(item);
            }
            sw.Dispose();
        }
    }
}
