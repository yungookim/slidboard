using System;
using System.Collections;
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

    public class TTTboard
    {

        ScatterViewItem viewItem;
        ScatterView main_view;

        string current = "O";
        string[,] perGrid = new String[3, 3];
        bool win = false;
        int count = 0;

        public TTTboard(ScatterView mv) 
        {
            this.viewItem = createNewBoard();
            this.main_view = mv;
            //Init grid
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    perGrid[i, j] = "";
                }
            }
        }

        public string[,] getGrid()
        {
            return this.perGrid;
        }

        public ScatterViewItem getView()
        {
            return this.viewItem;
        }


        private void change(object sender, RoutedEventArgs e)
        {
            SurfaceButton cur_ele = (SurfaceButton)sender;
            string content = cur_ele.Content.ToString();

            if (content.Equals("") && (!win)) // if the cell is empty, and havn't found a winner yet.
            {
                int col = Grid.GetColumn(cur_ele);
                int row = Grid.GetRow(cur_ele);
                count++;

                cur_ele.Content = current;

                current = current.Equals("X") ? "O" : "X";

                perGrid[row, col] = current; // set the cell content as marked sign

                win = winner_Checker(current); // check winner

                if (win || count == 9)
                {
                    TTTboard _new = new TTTboard(main_view);
                    main_view.Items.Add(_new.getView());
                }
                
            }
            else if (content.Equals("") && (win)) // if the cell is empty, and found a winner.
            {
                cur_ele.IsEnabled = false;// disable the cell from marking new one.

                return;
            }
        }

        private bool winner_Checker(string content)
        {

            for (int i = 0; i < 3; i++)
            {
                if (perGrid[i, 0].Equals(content) && perGrid[i, 1].Equals(content) && perGrid[i, 2].Equals(content))
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


        //Factory function : Add a new game board to the main scatter view
        private ScatterViewItem createNewBoard()
        {
            ScatterViewItem sci = new ScatterViewItem();
            Grid grid = new Grid();

            //Set definitions
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    grid.Children.Add(createSB(i, j));
                }
            }
            sci.Height = 300;
            sci.Content = grid;
            return sci;
        }



        //Factory Function : Create a new SurfaceButton
        private SurfaceButton createSB(int row, int col)
        {
            SurfaceButton sb = new SurfaceButton();
            sb.Margin = new Thickness(5);
            sb.Height = 100;
            sb.Width = 100;
            sb.HorizontalContentAlignment = HorizontalAlignment.Center;
            sb.VerticalContentAlignment = VerticalAlignment.Center;
            sb.Content = "";

            //Set row and column
            Grid.SetColumn(sb, row);
            Grid.SetRow(sb, col);

            sb.Click += new RoutedEventHandler(change);
            return sb;
        }
    }


    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            TTTboard _new = new TTTboard(board_view);
            board_view.Items.Add(_new.getView());
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


    }
}