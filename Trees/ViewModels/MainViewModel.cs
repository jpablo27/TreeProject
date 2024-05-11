// -----------------------------------------------------------------------
// <author>Pablo Sánchez</author>
// <date>2022-09-14</date>
// <summary></summary>
// -----------------------------------------------------------------------

namespace Trees.ViewModels;

using System.Windows;

using Trees.MvvmBase;

/// <summary>
/// Class MainViewModel.
/// Implements the <see cref="ViewModelBase" />
/// </summary>
/// <seealso cref="ViewModelBase" />
internal class MainViewModel : ViewModelBase
{
    #region Static Fields

    /// <summary>
    /// The root
    /// </summary>
    public static Node root;

    /// <summary>
    /// The instance
    /// </summary>
    private static MainViewModel instance;

    #endregion

    #region Fields

    /// <summary>
    /// The balance tree command
    /// </summary>
    private RelayCommand balanceTreeCommand;

    /// <summary>
    /// The new avl node command
    /// </summary>
    private RelayCommand newAvlNodeCommand;

    /// <summary>
    /// The new node command
    /// </summary>
    private RelayCommand newNodeCommand;

    /// <summary>
    /// The new node identifier
    /// </summary>
    private string newNodeId = 0.ToString();

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel" /> class.
    /// </summary>
    public MainViewModel()
    {
        instance = this;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the balance tree command.
    /// </summary>
    /// <value>The balance tree command.</value>
    public RelayCommand BalanceTreeCommand
    {
        get
        {
            return this.balanceTreeCommand = this.balanceTreeCommand ?? new RelayCommand(this.BalanceTreeCommand_Execute, this.BalanceTreeCommand_CanExecute);
        }
    }

    /// <summary>
    /// Creates new avlnodecommand.
    /// </summary>
    /// <value>The new avl node command.</value>
    public RelayCommand NewAvlNodeCommand
    {
        get
        {
            return this.newAvlNodeCommand = this.newAvlNodeCommand ?? new RelayCommand(this.NewAvlNodeCommand_Execute);
        }
    }

    /// <summary>
    /// Creates new nodecommand.
    /// </summary>
    /// <value>The new node command.</value>
    public RelayCommand NewNodeCommand
    {
        get
        {
            return this.newNodeCommand = this.newNodeCommand ?? new RelayCommand(this.NewNodeCommand_Execute);
        }
    }

    /// <summary>
    /// Creates new nodeid.
    /// </summary>
    /// <value>The new node identifier.</value>
    public string NewNodeId
    {
        get
        {
            return this.newNodeId;
        }

        set
        {
            this.newNodeId = value;
            this.OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the tree.
    /// </summary>
    /// <value>The tree.</value>
    public Node Root
    {
        get
        {
            return root;
        }

        set
        {
            root = value;
            this.OnPropertyChanged();
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Deletes the node.
    /// </summary>
    /// <param name="node">The node.</param>
    public static void DeleteNode(Node node)
    {
        if (node.Parent == null)
        {
            root = null;
        }
        else
        {
            if (node.Parent.LeftChild == node)
            {
                node.Parent.LeftChild = null;
                return;
            }

            if (node.Parent.RightChild == node)
            {
                node.Parent.RightChild = null;
                return;
            }
        }

        instance.Refresh();

        root?.Refresh();
    }

    #endregion

    #region Private Methods

    // Allows or avoids clicking on the balance button
    /// <summary>
    /// Balances the tree command can execute.
    /// </summary>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    private bool BalanceTreeCommand_CanExecute()
    {
        return this.Root != null;
    }

    // This method balances the tree
    /// <summary>
    /// Balances the tree command execute.
    /// </summary>
    private void BalanceTreeCommand_Execute()
    {
        this.Root?.TryBalance();
        this.OnPropertyChanged(nameof(this.Root));
        this.Root?.RefreshAll();
    }

    /// <summary>
    /// Creates new avlnodecommand_execute.
    /// </summary>
    private void NewAvlNodeCommand_Execute()
    {
        try
        {
            if (int.TryParse(this.NewNodeId, out var intVal) == false)
            {
                MessageBox.Show("Invalid. Please enter an integer");
                return;
            }

            if (this.Root == null)
            {
                this.Root = new Node(intVal, null);
                return;
            }

            var newNode = this.Root.InsertAvl(intVal);

            if (newNode == null)
            {
                MessageBox.Show("Node already exists. Please insert a non existent id");
            }

            this.OnPropertyChanged(nameof(this.Root));
            this.Root.RefreshAll();
        }
        finally
        {
            this.NewNodeId = string.Empty;
        }
    }

    /// <summary>
    /// Creates new nodecommand_execute.
    /// </summary>
    private void NewNodeCommand_Execute()
    {
        try
        {
            if (int.TryParse(this.NewNodeId, out var intVal) == false)
            {
                MessageBox.Show("Invalid. Please enter an integer");
                return;
            }

            if (this.Root == null)
            {
                this.Root = new Node(intVal, null);
                return;
            }

            var newNode = this.Root.Insert(intVal);

            if (newNode == null)
            {
                MessageBox.Show("Node already exists. Please insert a non existent id");
            }
        }
        finally
        {
            this.NewNodeId = string.Empty;
        }
    }

    #endregion
}