using game.Properties;
using Microsoft.VisualBasic.ApplicationServices;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Formats.Asn1.AsnWriter;

namespace game
{
    public partial class Form1 : Form
    {
        private bool start = false;
        private int pictureSpeed = 5;
        private bool movingRight = true;
        private bool movingUp = true;
        private List<PictureBox> pictures = new List<PictureBox>();
        private List<PictureBox> picture1 = new List<PictureBox>();
        private List<PictureBox> coins = new List<PictureBox>();//lista monet
        private Random random = new Random();
        private int score = 0;
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            GenerateCoins(5);
            pictures.Add(pictureBox3);
            pictures.Add(pictureBox4);
            pictures.Add(pictureBox5);
            pictures.Add(pictureBox6);
            picture1.Add(pictureBox7);
            picture1.Add(pictureBox8);
            coins.Add(pictureBox10);
            coins.Add(pictureBox11);
            coins.Add(pictureBox12);
            coins.Add(pictureBox13);
            coins.Add(pictureBox14);

            for (int i = 0; i < coins.Count; i++)
            {
                coins[i].Visible = false;
            }
            for (int i = 0; i < pictures.Count; i++)
            {
                pictures[i].Visible = false;
            }
            for (int i = 0; i < picture1.Count; i++)
            {
                picture1[i].Visible = false;
            }
            pictureBox9.Visible = false;
        }


        private void GenerateCoins(int numberOfCoins)
        {
            List<Point> usedLocations = new List<Point>();
            foreach (var coin in coins)
            {
                Point newLocation;
                do
                {
                    int x = random.Next(50, pictureBox1.Width - coin.Width - 50);
                    int y = random.Next(50, pictureBox1.Height - coin.Height - 50);
                    newLocation = new Point(x, y);
                }
                while (usedLocations.Contains(newLocation));
                usedLocations.Add(newLocation);
                coin.Location = newLocation;
            }
        }


        private void PositionCoinRandomly(PictureBox coin)
        {
            int maxX = pictureBox1.Width - coin.Width;
            int maxY = pictureBox1.Height - coin.Height;
            coin.Location = new Point(random.Next(1, maxX), random.Next(1, maxY));
        }
        private void UpdatePictureBoxesPosition(object sender, EventArgs e)//poruszanie przeciwnikow
        {

            for (int i = 0; i < pictures.Count; i++)
            {
                if (movingRight)
                {
                    if (pictures[i].Right < pictureBox1.Right - 50)
                    {
                        pictures[i].Left += pictureSpeed;
                    }
                    else
                    {
                        movingRight = false;
                    }
                }
                else
                {
                    if (pictures[i].Left > pictureBox1.Left + 50)
                    {
                        pictures[i].Left -= pictureSpeed;
                    }
                    else
                    {
                        movingRight = true;
                    }
                }

            }
            for (int i = 0; i < picture1.Count; i++)
            {
                if (movingUp)
                {
                    if (picture1[i].Top > pictureBox1.Top + 50)
                    {
                        picture1[i].Top -= pictureSpeed;
                    }
                    else
                    {
                        movingUp = false;
                    }
                }
                else
                {

                    if (picture1[i].Bottom < pictureBox1.Bottom - 50)
                    {
                        picture1[i].Top += pictureSpeed;
                    }
                    else
                    {
                        movingUp = true;
                    }
                }
            }
        }
        private void startGame_Click(object sender, EventArgs e)//klikniecie start
        {
            timer2.Start();
            start = true;
            startGame.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            label2.Visible = true;
            label1.Visible = false;
            for (int i = 0; i < pictures.Count; i++)
            {
                pictures[i].Visible = true;
            }
            for (int i = 0; i < picture1.Count; i++)
            {
                picture1[i].Visible = true;
            }
            pictureBox9.Visible = true;
            for (int i = 0; i < coins.Count; i++)
            {
                coins[i].Visible = true;
            }

        }

        private void KeyIsDown(object sender, KeyEventArgs e)//poruszanie postaci
        {
            if (!start) return;

            int moveAmount = 15;
            Point playerPosition = pictureBox2.Location;

            switch (e.KeyCode)
            {
                case Keys.A:
                    if (playerPosition.X - moveAmount >= 12)
                        playerPosition.X -= moveAmount;
                    break;
                case Keys.D:
                    if (playerPosition.X + moveAmount + pictureBox2.Width <= pictureBox1.Width + 12)
                        playerPosition.X += moveAmount;
                    break;
                case Keys.W:
                    if (playerPosition.Y - moveAmount >= 12)
                        playerPosition.Y -= moveAmount;
                    break;
                case Keys.S:
                    if (playerPosition.Y + moveAmount + pictureBox2.Height <= pictureBox1.Height + 12)
                        playerPosition.Y += moveAmount;
                    break;
            }
            pictureBox2.Location = playerPosition;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)//timer
        {
            CheckForCollision();
            UpdatePictureBoxesPosition(sender, e);
            UpdatePictureBox9Position();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }
        private void EndGame()
        {
            label3.Visible = true;
            for (int i = 0; i < pictures.Count; i++)
            {
                pictures[i].Visible = false;
            }
            for (int i = 0; i < picture1.Count; i++)
            {
                picture1[i].Visible = false;
            }
            label2.Visible = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            for (int i = 0; i < coins.Count; i++)
            {
                coins[i].Visible = false;
            }
            pictureBox9.Visible = false;
            for (int i = 0; i < coins.Count; i++)
            {
                coins[i].Visible = false;
            }
            timer2.Stop();
            textBox1.Visible = true;
            button3.Visible = true;
            label4.Visible = true;
        }

        private void CheckForCollision()//funkcja sprawdza pozycje gracza i przeciwników
        {
            int j = 0;
            for (int i = 0; i < coins.Count; i++)
            {
                if (coins[i].Visible == false)
                {
                    j++;
                }
            }
            if (j == coins.Count)
            {
                GenerateCoins(5);
                for (int i = 0; i < coins.Count; i++)
                {
                    coins[i].Visible = true;
                }
            }
            for (int i = 0; i < pictures.Count; i++)
            {
                if (pictures[i].Bounds.IntersectsWith(pictureBox2.Bounds))
                {
                    EndGame();
                    break;
                }
            }
            for (int i = 0; i < picture1.Count; i++)
            {
                if (picture1[i].Bounds.IntersectsWith(pictureBox2.Bounds))
                {
                    EndGame();
                    break;
                }
            }
            if (pictureBox9.Bounds.IntersectsWith(pictureBox2.Bounds))
            {
                EndGame();
            }
            for (int i = 0; i < coins.Count; i++)
            {
                if (pictureBox2.Bounds.IntersectsWith(coins[i].Bounds) && coins[i].Visible == true)
                {
                    coins[i].Visible = false;
                    score++;
                    label2.Text = "Score:" + score;
                }
            }

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }
        private void UpdatePictureBox9Position()
        {
            int followSpeed = 2;
            int xDiff = pictureBox2.Left - pictureBox9.Left;
            int yDiff = pictureBox2.Top - pictureBox9.Top;
            if (Math.Abs(xDiff) > followSpeed)
            {
                pictureBox9.Left += Math.Sign(xDiff) * followSpeed;
            }
            if (Math.Abs(yDiff) > followSpeed)
            {
                pictureBox9.Top += Math.Sign(yDiff) * followSpeed;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ranking.Items.Clear();
            label5.Visible = true;
            Ranking.Visible = true;
            startGame.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            label3.Visible = false;
            label1.Visible = false;
            button4.Visible = true;
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead("plik.txt")) // Zmienione na OpenRead
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    Ranking.Items.Add(line); // Dodawanie linii jako elementów do ListBox
                }
            }
        }

        private void backToMenu()
        {
            label5.Visible = false;
            label2.Visible = false;
            label4.Visible = false;
            label3.Visible = false;
            button3.Visible = false;
            textBox1.Visible = false;
            startGame.Visible = true;
            label1.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            Ranking.Visible = false;
            button4.Visible = false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            textBox1.Visible = false;
            using (StreamWriter w = File.AppendText("plik.txt"))
            {
                w.WriteLine(textBox1.Text + " " + score);
            }
            SortScores();
            backToMenu();
        }
        private void SortScores()
        {

            var lines = File.ReadAllLines("plik.txt");


            var sortedLines = lines.Select(line =>
            {
                var parts = line.Split(' ');
                int parsedScore;
                int.TryParse(parts.Last(), out parsedScore);
                return new { Text = string.Join(" ", parts.Take(parts.Length - 1)), Score = parsedScore };
            })
            .OrderByDescending(item => item.Score)
            .Select(item => item.Text + " " + item.Score);


            File.WriteAllLines("plik.txt", sortedLines);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            backToMenu();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }
    }
}
