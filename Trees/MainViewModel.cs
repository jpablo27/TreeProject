// -----------------------------------------------------------------------
// <author>Pablo Sánchez</author>
// <date>2022-09-14</date>
// <summary></summary>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.Windows;

namespace Trees;

/// <summary>
///     Class MainViewModel.
///     Implements the <see cref="Trees.ViewModelBase" />
/// </summary>
/// <seealso cref="Trees.ViewModelBase" />
internal class MainViewModel : ViewModelBase
{
    /// <summary>
    /// Deletes the node
    /// </summary>
    /// <param name="node"></param>
    public static void DeleteNode(Node node)
    {
        if (node.Parent == null)
        {
            RootNode = null;
        }
        else
        {
            if (node.Parent.LeftChild == node)
                node.Parent.LeftChild = null;
            // Todo: Dive deeper and balance
            else if (node.Parent.RightChild == node) node.Parent.RightChild = null;
            // Todo: Dive deeper and balance
        }

        _instance.Refresh();

        RootNode?.Refresh();
    }

    #region Fields

    /// <summary>
    ///     The new node command
    /// </summary>
    private RelayCommand newNodeCommand;

    /// <summary>
    ///     The new node identifier
    /// </summary>
    private string newNodeId = 0.ToString();

    /// <summary>
    ///     The new avl node command
    /// </summary>
    private RelayCommand newAvlNodeCommand;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    ///     The instance
    /// </summary>
    private static MainViewModel _instance;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MainViewModel" /> class.
    /// </summary>
    public MainViewModel()
    {
        _instance = this;
    }

    #endregion

    #region Public Properties

    /// <summary>
    ///     Gets or sets the tree.
    /// </summary>
    /// <value>The tree.</value>
    public Node Root
    {
        get => RootNode;

        set
        {
            RootNode = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Creates new nodeid.
    /// </summary>
    /// <value>The new node identifier.</value>
    public string NewNodeId
    {
        get => newNodeId;

        set
        {
            newNodeId = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Creates new nodecommand.
    /// </summary>
    /// <value>The new node command.</value>
    public RelayCommand NewNodeCommand
    {
        get { return newNodeCommand = newNodeCommand ?? new RelayCommand(NewNodeCommand_Execute); }
    }

    /// <summary>
    ///     Creates new avlnodecommand.
    /// </summary>
    /// <value>The new avl node command.</value>
    public RelayCommand NewAvlNodeCommand
    {
        get { return newAvlNodeCommand ??= new RelayCommand(NewAvlNodeCommand_Execute); }
    }

    /// <summary>
    /// The balance tree command
    /// </summary>
    public RelayCommand BalanceTreeCommand
    {
        get { return balanceTreeCommand ??= new RelayCommand(BalanceTreeCommand_Execute); }
    }
    
    /// <summary>
    /// Is new node focused
    /// </summary>
    public bool NewNodeIdIsFocused { get; set; } = true;

    /// <summary>
    /// Refresh Whole UI
    /// </summary>
    public static void RefreshWholeUi()
    {
        if (Application.Current.Dispatcher.CheckAccess())
        {
            _instance.OnPropertyChanged(nameof(Root));
            _instance.Root?.RefreshAll();
        }
        else
        {
            Application.Current.Dispatcher.Invoke(RefreshWholeUi);
        }
    }

    /// <summary>
    /// Allows or avoids clicking on the balance button
    /// </summary>
    /// <returns></returns>
    private bool BalanceTreeCommand_CanExecute()
    {
        return Root != null;
    }

    /// <summary>
    /// This method balances the tree
    /// </summary>
    private void BalanceTreeCommand_Execute()
    {
        Root?.BalanceNode();
        OnPropertyChanged(nameof(Root));
        Root?.RefreshAll();
    }


    /// <summary>
    ///     Creates new avlnodecommand_execute.
    /// </summary>
    private void NewAvlNodeCommand_Execute()
    {
        if (int.TryParse(NewNodeId, out var intVal) == false)
        {
            MessageBox.Show("Invalid. Please enter an integer");
            return;
        }

        if (Root == null)
        {
            Root = new Node(intVal, null);
            return;
        }

        var newNode = Root.InsertAvl(intVal);

        if (newNode == null) MessageBox.Show("Node already exists. Please insert a non existent id");

        OnPropertyChanged(nameof(Root));
        Root.RefreshAll();
    }

    /// <summary>
    ///     Creates new nodecommand_execute.
    /// </summary>
    private void NewNodeCommand_Execute()
    {
        try
        {
            if (int.TryParse(NewNodeId, out var intVal) == false)
            {
                MessageBox.Show("Invalid. Please enter an integer");
                return;
            }

            if (Root == null)
            {
                Root = new Node(intVal, null);
                return;
            }

            var newNode = Root.Insert(intVal);

            if (newNode == null) MessageBox.Show("Node already exists. Please insert a non existent id");
        }
        finally
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                NewNodeId = string.Empty;
                NewNodeIdIsFocused = false;
                OnPropertyChanged(nameof(NewNodeIdIsFocused));
                NewNodeIdIsFocused = true;
                OnPropertyChanged(nameof(NewNodeIdIsFocused));
            });
        }
    }

    private static Node rootNode;

    /// <summary>
    ///     The root
    /// </summary>
    public static Node RootNode
    {
        get
        {
            return rootNode;
        }

        set
        {
            rootNode = value;

            if (rootNode != null)
            {
                rootNode.Parent = null;
            }

            RefreshWholeUi();
        }
    }

    /// <summary>
    ///     The balance Tree Command
    /// </summary>
    private RelayCommand balanceTreeCommand;


    #endregion
}