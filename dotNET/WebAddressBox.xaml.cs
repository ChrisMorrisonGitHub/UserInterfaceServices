using System;
using System.Collections.Generic;
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

namespace UserInterfaceServices
{
    /// <summary>
    /// Contains values to indicate what action a consumer of the WebAddressBox.AddressBarButtonClicked event should perform.
    /// </summary>
    public enum AddressBarAction
    {
        /// <summary>
        /// The client should attempt to navigate to the web page whose address is in the address portion of the control.
        /// </summary>
        Navigate,
        /// <summary>
        /// The client should cancel the current navigation.
        /// </summary>
        CancelNavigation,
        /// <summary>
        /// The client should refresh the current page.
        /// </summary>
        Refresh
    }

    /// <summary>
    /// Contains information about a WebAddressBox.AddressBarButtonClicked event.
    /// </summary>
    public class AddressBarButtonClickedEventArgs : EventArgs
    {
        AddressBarAction addressBarAction = AddressBarAction.Navigate;

        /// <summary>
        /// Create a new instance of the AddressBarButtonClickedEventArgs object representing the given action.
        /// </summary>
        /// <param name="action">The action required.</param>
        public AddressBarButtonClickedEventArgs(AddressBarAction action)
        {
            addressBarAction = action;
        }

        /// <summary>
        /// Gets the action that the consumer of this event should perform.
        /// </summary>
        public AddressBarAction ActionRequired
        {
            get
            {
                return addressBarAction;
            }
        }
    }

    /// <summary>
    /// Represents a method that will handle the WebAddressBox.AddressBarButtonClicked event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">An object containing information about the event.</param>
    public delegate void AddressBarButtonClickedHandler(object sender, AddressBarButtonClickedEventArgs e);

    /// <summary>
    /// Interaction logic for WebAddressBox.xaml
    /// </summary>
    public partial class WebAddressBox : UserControl
    {
        public event AddressBarButtonClickedHandler AddressBarButtonClicked;
        AddressBarAction currentAction = AddressBarAction.Navigate;

        public WebAddressBox()
        {
            InitializeComponent();
        }

        protected void OnAddressBarButtonClicked()
        {
            if (AddressBarButtonClicked != null) AddressBarButtonClicked(this, new AddressBarButtonClickedEventArgs(currentAction));
        }

        private void BarButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.OnAddressBarButtonClicked();
        }

        private void CertLabel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Thickness margin = AddressTextBox.Margin;
            margin.Left = (CertLabel.ActualWidth + CertLabel.Margin.Left + CertLabel.Margin.Right + 2);
            AddressTextBox.Margin = margin;
        }

        private void AddressTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //this.BorderBrush = new (SystemColors.ControlBrushKey);
        }

        private void AddressTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Gets to sets the text in the address portion of the control.
        /// </summary>
        public string Text
        {
            get
            {
                return AddressTextBox.Text;
            }
            set
            {
                AddressTextBox.Text = value;
            }
        }

        public new bool IsEnabled
        {
            get
            {
                return AddressTextBox.IsReadOnly;
            }
            set
            {
                AddressTextBox.IsReadOnly = value;
                BarButton.IsEnabled = value;
            }
        }

        /// <summary>
        /// Attempts to set focus to this element.
        /// </summary>
        /// <returns></returns>
        public new bool Focus()
        {
            return AddressTextBox.Focus();
        }

        private void AddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BarButton.IsEnabled = (AddressTextBox.Text != "");
        }

        private void AddressTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) this.OnAddressBarButtonClicked();
        }
    }
}
