using LeagueChampsDb.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Trucks.Data;

namespace LeagueChampsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            HideOnStart();
        }

        private void HideOnStart()
        {
            countByPriceLbl.Visibility = Visibility.Hidden;
            loggedLbl.Visibility = Visibility.Hidden;

            totalBeLbl.Visibility = Visibility.Hidden;
            totalRpLbl.Visibility = Visibility.Hidden;
            resourcesNeeded.Visibility = Visibility.Hidden;
            countLbl.Visibility = Visibility.Hidden;
        }

        private void LoginInfoVisibility(bool visible)
        {
            usernameBox.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
            usernameBoxLbl.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
            passBox.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
            passBoxLbl.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
            loginBtn.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
            statusLbl.Visibility = visible ? Visibility.Visible : Visibility.Hidden;

            countByPriceLbl.Visibility = visible ? Visibility.Hidden : Visibility.Visible;
            loggedLbl.Visibility = visible ? Visibility.Hidden : Visibility.Visible;

            totalBeLbl.Visibility = visible ? Visibility.Hidden : Visibility.Visible;
            totalRpLbl.Visibility = visible ? Visibility.Hidden : Visibility.Visible;
            resourcesNeeded.Visibility = visible ? Visibility.Hidden : Visibility.Visible;
            countLbl.Visibility = visible ? Visibility.Hidden : Visibility.Visible;
        }

        private User currentUser;

        private ObservableCollection<ChampionDisplayableInfo> champs = new ObservableCollection<ChampionDisplayableInfo>();
        private LeagueChampsContext context = new LeagueChampsContext();
        private List<ChampionDisplayableInfo> removedChamps = new List<ChampionDisplayableInfo>();

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginInfoVisibility(false);

            statusLbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            usernameBox.Text = usernameBox.Text.Trim();


            if (usernameBox.Text.Length < 3 || usernameBox.Text.Length > 16 || usernameBox.Text.Contains(" "))
            {
                statusLbl.Content = "Length must be 3-16 no spaces";
                LoginInfoVisibility(true);
                return;
            }

            var user = context.Users
                .Include(x => x.UsersChampions)
                .ThenInclude(x => x.Champion)
                .FirstOrDefault(u => u.Username == usernameBox.Text);

            if (user == null)
            {
                statusLbl.Content = "Invalid Username";
                LoginInfoVisibility(true);
                return;
            }

            if (user.Password != passBox.Password)
            {
                statusLbl.Content = "Invalid Password";
                LoginInfoVisibility(true);
                return;
            }

            loggedLbl.Content = "Logged in as " + user.Username;


            foreach (var champ in user.UsersChampions)
            {
                champs.Add(new ChampionDisplayableInfo()
                {
                    Name = champ.Champion.Name,
                    PriceBE = champ.Champion.PriceBE,
                    PriceRP = champ.Champion.PriceRP
                });
            }
            champsListView.ItemsSource = champs;
            currentUser = user;
            UpdateTotalPriceAndCountLabels();
        }

        private void buyBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = champsListView.SelectedItems;
            int j = selectedItems.Count;
            for (int i = 0; i < j; i++)
            {
                var item = (ChampionDisplayableInfo)selectedItems[0];
                var champ = champs.FirstOrDefault(x => x.Name == item.Name);
                removedChamps.Add(champ);
                champs.Remove(champ);
            }
            champsListView.ClearValue(ItemsControl.ItemsSourceProperty);
            champsListView.ItemsSource = champs;

            UpdateTotalPriceAndCountLabels();
            UpdateDatabase(context);
        }

        private void UpdateTotalPriceAndCountLabels()
        {
            totalBeLbl.Content = "BE " + champs.Sum(c => c.PriceBE);
            totalRpLbl.Content = "RP " + champs.Sum(c => c.PriceRP);
            countLbl.Content = "Count: " + champs.Count();

            var count6300 = champs.Count(x => x.PriceBE == 6300);
            var count4800 = champs.Count(x => x.PriceBE == 4800);
            var count3150 = champs.Count(x => x.PriceBE == 3150);
            var count1350 = champs.Count(x => x.PriceBE == 1350);
            var count450 = champs.Count(x => x.PriceBE == 450);

            var sb = new StringBuilder();

            sb.AppendLine(count6300 > 0 ? $"6300 champs count: {count6300}": "");
            sb.AppendLine(count4800 > 0 ? $"4800 champs count: {count4800}": "");
            sb.AppendLine(count3150 > 0 ? $"3150 champs count: {count3150}": "");
            sb.AppendLine(count1350 > 0 ? $"1350 champs count: {count1350}": "");
            sb.AppendLine(count450 > 0 ? $"450 champs count: {count450}": "");

            countByPriceLbl.Content = sb.ToString();
        }

        private void UpdateDatabase(LeagueChampsContext context)
        {
            var user = context.Users.FirstOrDefault(x => x == currentUser);
            foreach (var champ in removedChamps)
            {
                var current = user.UsersChampions.FirstOrDefault(x => x.Champion.Name == champ.Name);
                currentUser.UsersChampions.Remove(current);
            }
            context.SaveChangesAsync();
        }

        private void champList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void champsListView_Initialized(object sender, EventArgs e)
        {
        }
        public class ChampionDisplayableInfo
        {
            public string Name { get; set; }
            public int PriceBE { get; set; }
            public int PriceRP { get; set; }
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null && champsListView.Items.Count != 0)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }
        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(champsListView.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();

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

        private void loginBtn_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = (e.Key == Key.Enter || e.Key == Key.Return);
        }

        bool enterPressed = false;
        private void passBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!enterPressed)
            {
                if (e.Key == Key.Enter)
                {
                    enterPressed = true;
                    loginBtn_Click(null, null);
                }
            }
        }

        private void usernameBox_KeyDown(object sender, KeyEventArgs e)
        {
            passBox_KeyDown(sender, e);
        }
    }
}