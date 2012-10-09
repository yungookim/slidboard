using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace tictactoe
{
    public class TTTBoard
    {
        public String name { get; set; }
        public int row { get; set; }
        public int col { get; set; }

        public TTTBoard(String name, int row, int col)
        {
            this.name = name;
            this.row = row;
            this.col = col;
        }
    }

    

    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {

        string foo = "asdf";

        string[,] perGrid = new string[3,3];
        int current = 0;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            //Init perGrid
            for (int i = 0; i < 3; i++){
                for (int j = 0; j < 3; j++){
                    perGrid[i, j] = "";
                }
            }

            //board_view.Items.Add();
            //board_view.Items.Add(createGrid());
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            SurfaceButton btn = (SurfaceButton)sender;

            if (btn.Content.ToString().Length > 0)
            {
                return;
            }

            if (current == 0)
            {
                btn.Content = "X";
                current = 1;
                int row = int.Parse(btn.Name.Substring(3, 1));
                int col = int.Parse(btn.Name.Substring(4, 1));
                perGrid[row, col] = "X";
            }
            else
            {
                btn.Content = "O";
                current = 0;
                int row = int.Parse(btn.Name.Substring(3, 1));
                int col = int.Parse(btn.Name.Substring(4, 1));
                perGrid[row, col] = "O";
            }

            // if win, then disable all other button left over.
            if (winner_Checker(btn.Content.ToString()))
            {
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++)
                    {
                        perGrid[i, j] = "";
                    }
                }
                //duplicateGrid();
            }
        }

        

        private bool winner_Checker(string content) {

            for (int i = 0; i < 3; i++)
            {
                if (perGrid[i, 0].Equals(perGrid[i, 1].Equals(perGrid[i, 2].Equals(content))))
                {
                    return true;
                }
                else if (perGrid[0, i].Equals(content) && perGrid[1, i].Equals(content) && perGrid[2, i].Equals(content))
                {
                    return true;
                }
            }

            if (perGrid[0, 0].Equals(content) && perGrid[1, 1].Equals(content) && perGrid[2, 2].Equals(content))
            {
                return true;
            }
            else if (perGrid[0, 2].Equals(content) && perGrid[1, 1].Equals(content) && perGrid[2,0].Equals(content))
            {
                return true;
            }

            return false;
        }

        //Duplicate the finished grid and append to the current main scatter view
        private void createGrid()
        {
            /*ScatterViewItem sci = new ScatterViewItem();
            //sci.Content = ((ContentControl)board_template.Content);
            sci.Height = 300;
            sci.Visibility = Visibility.Visible;
            return sci;*/

        }

        private SurfaceButton sbFactory(int row, int col)
        {
            SurfaceButton sb = new SurfaceButton();
            Thickness t = new Thickness(5);
            sb.Margin = t;
            return sb;
        }
    }
}