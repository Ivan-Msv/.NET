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

        private void ButtonLoadFromJson_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "enemies"; // Default file name
            dialog.DefaultExt = ".json"; // Default file extension
            dialog.InitialDirectory = Path.GetFullPath("data");
            dialog.Filter = "Json documents (.json)|*.json"; // Filter files by extension

            bool? result = dialog.ShowDialog();

            if (result == false)
            {
                return;
            }

            string fileName = dialog.FileName;

            string fileContents;
            using (StreamReader reader = new StreamReader(fileName))
            {
                fileContents = reader.ReadToEnd();
            }

            try
            {
                List<Enemy> enemies = JsonConvert.DeserializeObject<List<Enemy>>(fileContents);

                foreach (var enemy in enemies)
                {
                    EnemyList.Items.Add(enemy);
                }
                ErrorLabel.Content = "Loaded from Json.";
            }
            catch
            {
                ErrorLabel.Content = "Cannot deserialize given Json object.";
            }
        }
        private void ButtonSaveToJSON_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "enemies"; // Default file name
            dialog.DefaultExt = ".json"; // Default file extension
            dialog.InitialDirectory = Path.GetFullPath("data");
            dialog.Filter = "Json documents (.json)|*.json"; // Filter files by extension

            bool? result = dialog.ShowDialog();

            if (result == false)
            {
                return;
            }

            string fileName = dialog.FileName;

            int enemyCount = EnemyList.Items.Count;
            Enemy[] enemies = new Enemy[enemyCount];

            for (int i = 0; i < enemyCount; i++)
            {
                enemies[i] = (Enemy)EnemyList.Items[i];
            }

            string enemiesArrayJson = JsonConvert.SerializeObject(enemies);

            using (StreamWriter enemyWriter = new StreamWriter(fileName))
            {
                enemyWriter.Write(enemiesArrayJson);
            }

            ErrorLabel.Content = "Saved to Json.";
        }
        private void AddToEnemyList_Click(object sender, RoutedEventArgs e)
        {
            bool canBeSaved = CheckValues();

            if (!canBeSaved)
            {
                return;
            }

            // Tyhjentää käyttäjän syötteet
            EnemyName.Clear();
            SpriteId.Clear();

            // Näyttää käyttäjälle, että kaikki onnistui
            ErrorLabel.Content = "Enemy saved!";
        }
        private void RemoveEnemy_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelection())
            {
                return;
            }

            EnemyList.Items.Remove(EnemyList.SelectedItem);
            ErrorLabel.Content = "Successfully removed enemy";
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

            // tarkista että arvot ovat numeroita
            if (!int.TryParse(SpriteId.Text, out _))
            {
                ErrorLabel.Content = $"Sprite ID has to be a number";
                return false;
            }

            ErrorLabel.Foreground = Brushes.White;

            string name = EnemyName.Text;
            Vector2 position = new Vector2(0, 0);
            int spriteIndex = int.Parse(SpriteId.Text);

            Enemy newEnemy = new Enemy(name, position, spriteIndex);
            EnemyList.Items.Add(newEnemy);

            return true;
        }

        private bool CheckSelection()
        {
            ErrorLabel.Foreground = Brushes.Red;
            if (EnemyList.SelectedItem == null)
            {
                ErrorLabel.Content = "Select an enemy first.";
                return false;
            }

            ErrorLabel.Foreground = Brushes.White;
            return true;
        }
    }
}