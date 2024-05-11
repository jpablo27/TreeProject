// -----------------------------------------------------------------------
// <author>Pablo Sánchez</author>
// <date>2022-09-07</date>
// <summary></summary>
// -----------------------------------------------------------------------

namespace Trees.Views
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for DiagramContainerView.xaml
    /// </summary>
    public partial class DiagramContainerView : UserControl
    {
        #region Static Fields

        /// <summary>
        /// The node property
        /// </summary>
        public static readonly DependencyProperty NodeProperty = DependencyProperty.Register(
            nameof(Node),
            typeof(Node),
            typeof(DiagramContainerView),
            new PropertyMetadata(default(Node), PropertyChangedCallback));

        #endregion

        #region Constructors and Destructors

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>The node.</value>
        public Node Node
        {
            get
            {
                return (Node)this.GetValue(NodeProperty);
            }
            set
            {
                this.SetValue(NodeProperty, value);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Properties the changed callback.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DiagramContainerView;
            if (control is { Node: null })
            {
                return;
            }

            if (control == null)
            {
                return;
            }

            control.InitializeComponent();
        }

        #endregion
    }
}