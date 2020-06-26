using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hangman
{
    public partial class Hangman : Form
    {
        public Hangman()
        {
            InitializeComponent();
        }

        private string word;
        public char[] guess;
        public int lives = 11;
        int maxLives = 11;
        
        public Thread timer;
        List<string> sprites = new List<string>();
        
        

        private void Hangman_Load(object sender, EventArgs e)
        {
            List<string> path = Directory.GetFiles("sprites").OrderBy(p => int.Parse(p.Substring(8).Split('.')[0])).ToList();
            
            foreach (var item in path)
            {
                sprites.Add(item);
            }
            
           Restart();
            

        }
        private void Hangman_Exit(object sender,EventArgs e)
        {
            timer.Abort();
        }
        
        public void ButtonPress(object sender, EventArgs e)
        {
            char text = char.Parse((sender as Button).Text);
            bool found = false;
            for (int i = 0; i < word.Length; i++)
            {
                if(word[i] == text && guess[i] != text)
                {
                    found = true;
                    guess[i] = text;
                    
                }

                
            }
            string current = "";
            foreach (var item in guess)
            {
                current += item;
            }
            if(word == current)
            {
                timer.Abort();
                MessageBox.Show("You won");
                Restart();
            }
            if (found)
            {
                this.label3.Text = "";
                string temp = "";
                for (int i = 0; i < guess.Length; i++)
                {
                    if(guess[i] == '_')
                    {
                        temp += '_' + " ";
                    }
                    else
                    {
                        temp += guess[i];
                    }
                }
                this.label3.Text = temp;
            }
            else
            {
                
                int temp = maxLives - lives;
                lives--;
                if (lives == 0)
                {
                    this.pictureBox1.ImageLocation = sprites[maxLives-1];
                    timer.Abort();
                    MessageBox.Show("You lose");
                    Restart();

                }
                else
                {
                    this.pictureBox1.ImageLocation = sprites[temp];
                    
                    this.label1.Text = "Lives: " + lives;
                }
            }
        }
        public void Restart()
        {

            ChooseWord();
            this.pictureBox1.ImageLocation = "";
            lives = maxLives;
            this.label1.Text = "Lives: " + lives;
            timer = new Thread(()=> {

                DateTime timerEnd = DateTime.Now.AddMinutes(3);
                while (true)
                {
                    TimeSpan ts = timerEnd - DateTime.Now;
                    this.Invoke((MethodInvoker)delegate {
                        this.label2.Text = "Time: " + Math.Round(ts.TotalSeconds);
                    });
                    if (ts.TotalSeconds <0)
                    {
                        //End
                        break;
                    }
                    Thread.Sleep(1000);
                }
            });
            timer.Start();
            
        }
        public void ChooseWord()
        {
            using (StreamReader sr = new StreamReader("words.txt"))
            {
                string line;
                List<string> words = new List<string>();
                while ((line = sr.ReadLine()) != null)
                {
                    words.Add(line);
                }
                Random r = new Random();
                word = words[r.Next(0, words.Count)].ToUpper();
                
                guess = new char[word.Length];
                this.label3.Text = "";
                for (int i = 0; i < word.Length; i++)
                {
                    guess[i] = '_';
                    this.label3.Text += "_ ";
                }
                if(1 == 1)
                {
                    this.Text = word;
                }

            }
        }
    }
}
