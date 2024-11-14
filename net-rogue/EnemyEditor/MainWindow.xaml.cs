using Rogue;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Data;

namespace EnemyEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void ButtonSaveToJSON_Click(object sender, RoutedEventArgs e)
        {
            bool canBeSaved = CheckValues();

            if (!canBeSaved)
            {
                return;
            }

            int enemyCount = EnemyList.Items.Count;
            Enemy[] enemies = new Enemy[enemyCount];

            for (int i = 0; i < enemyCount; i++)
            {
                enemies[i] = (Enemy)EnemyList.Items[i];
            }

            string enemiesArrayJson = JsonConvert.SerializeObject(enemies);

            using (StreamWriter enemyWriter = new StreamWriter("data/enemies.json"))
            {
                enemyWriter.Write(enemiesArrayJson);
            }

            // Tyhjentää käyttäjän syötteet
            EnemyName.Clear();
            SpriteId.Clear();
            PositionX.Clear();
            PositionY.Clear();

            // Näyttää käyttäjälle, että kaikki onnistui
            ErrorLabel.Content = "Enemy saved!";
        }
        private bool CheckValues()
        {
            ErrorLabel.Foreground = Brushes.Red;
            // Tarkista ettei ole tyhjät
            if (string.IsNullOrEmpty(SpriteId.Text))
            {
                ErrorLabel.Content = "Sprite ID can't be empty";
                return false;
            }

            if (string.IsNullOrEmpty(EnemyName.Text))
            {
                ErrorLabel.Content = "Enemy name can't be empty";
                return false;
            }

            if (string.IsNullOrEmpty(PositionX.Text) || string.IsNullOrEmpty(PositionY.Text))
            {
                ErrorLabel.Content = "Position can't be empty";
                return false;
            }

            // tarkista että arvot ovat numeroita
            if (!int.TryParse(SpriteId.Text, out _))
            {
                ErrorLabel.Content = $"Sprite ID has to be a number";
                return false;
            }

            if (!int.TryParse(PositionX.Text, out _) || !int.TryParse(PositionY.Text, out _))
            {
                ErrorLabel.Content = $"Position has to be a number";
                return false;
            }

            ErrorLabel.Foreground = Brushes.White;

            string name = EnemyName.Text;
            Vector2 position = new Vector2(int.Parse(PositionX.Text), int.Parse(PositionY.Text));
            int spriteIndex = int.Parse(SpriteId.Text);

            Enemy newEnemy = new Enemy(name, position, spriteIndex);
            EnemyList.Items.Add(newEnemy);

            return true;
        }
    }
}