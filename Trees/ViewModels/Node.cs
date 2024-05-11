// -----------------------------------------------------------------------
// <author>Pablo Sánchez</author>
// <date>2022-09-07</date>
// <summary></summary>
// -----------------------------------------------------------------------

namespace Trees.ViewModels;

using System;
using System.Windows;
using System.Windows.Threading;

using Trees.Enums;
using Trees.MvvmBase;

/// <summary>
/// Class Node.
/// </summary>
public class Node : ViewModelBase
{
    #region Fields

    /// <summary>
    /// The balance command
    /// </summary>
    private RelayCommand balanceCommand;

    /// <summary>
    /// The delete command
    /// </summary>
    private RelayCommand<object> deleteCommand;

    /// <summary>
    /// The identifier
    /// </summary>
    private int id;

    /// <summary>
    /// The left child
    /// </summary>
    private Node leftChild;

    /// <summary>
    /// The parent
    /// </summary>
    private Node parent;

    /// <summary>
    /// The right child
    /// </summary>
    private Node rightChild;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Node" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="parent">The parent.</param>
    public Node(int id, Node parent)
    {
        this.Parent = parent;
        this.Id = id;
    }

    #endregion

    #region Public Properties

    public RelayCommand BalanceCommand
    {
        get
        {
            return this.balanceCommand ??= new RelayCommand(this.BalanceCommand_Execute);
        }
    }

    /// <summary>
    /// Gets the delete command.
    /// </summary>
    /// <value>The delete command.</value>
    public RelayCommand<object> DeleteCommand
    {
        get
        {
            return this.deleteCommand ??= new RelayCommand<object>(this.DeleteCommand_Execute);
        }
    }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id
    {
        get
        {
            return this.id;
        }

        set
        {
            this.id = value;
            this.OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the left child.
    /// </summary>
    /// <value>The left child.</value>
    public Node LeftChild
    {
        get
        {
            return this.leftChild;
        }

        set
        {
            this.leftChild = value;
            this.OnPropertyChanged();

            if (this.leftChild != null)
            {
                this.leftChild.Parent = this;
            }
        }
    }

    /// <summary>
    /// Gets or sets the parent.
    /// </summary>
    /// <value>The parent.</value>
    public Node Parent
    {
        get
        {
            return this.parent;
        }
        set
        {
            this.parent = value;
            this.OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the right child.
    /// </summary>
    /// <value>The right child.</value>
    public Node RightChild
    {
        get
        {
            return this.rightChild;
        }
        set
        {
            this.rightChild = value;
            this.OnPropertyChanged();

            if (this.rightChild != null)
            {
                this.rightChild.Parent = this;
            }
        }
    }

    /// <summary>
    /// Gets or sets the side.
    /// </summary>
    /// <value>The side.</value>
    public Side Side
    {
        get
        {
            if (this.Parent?.LeftChild == this)
            {
                return Side.Left;
            }

            if (this.Parent?.RightChild == this)
            {
                return Side.Right;
            }

            return Side.Root;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets the balance.
    /// </summary>
    /// <returns>System.Int32.</returns>
    public int GetBalance()
    {
        if (this.rightChild != null && this.leftChild == null)
        {
            return -this.rightChild.GetHeight();
        }

        if (this.leftChild != null && this.rightChild == null)
        {
            return this.leftChild.GetHeight();
        }

        if (this.rightChild != null && this.leftChild != null)
        {
            return this.leftChild.GetHeight() - this.rightChild.GetHeight();
        }

        return 0;
    }

    /// <summary>
    /// Gets the height of the tree
    /// </summary>
    /// <returns>System.Int32.</returns>
    public int GetHeight()
    {
        if (this.rightChild != null && this.leftChild == null)
        {
            return 1 + this.rightChild.GetHeight();
        }

        if (this.leftChild != null && this.rightChild == null)
        {
            return 1 + this.leftChild.GetHeight();
        }

        if (this.rightChild != null && this.leftChild != null)
        {
            return 1 + Math.Max(this.leftChild.GetHeight(), this.rightChild.GetHeight());
        }

        return 1;
    }

    /// <summary>
    /// Inserts the specified identifier.
    /// </summary>
    /// <param name="newId">The identifier.</param>
    /// <returns>Node.</returns>
    public Node Insert(int newId)
    {
        if (newId == this.Id)
        {
            return null;
        }

        return newId > this.Id ? this.InsertNode(ref this.rightChild, newId) : this.InsertNode(ref this.leftChild, newId);
    }

    public Node InsertAvl(int newId)
    {
        if (newId == this.Id)
        {
            return null;
        }

        if (newId < this.Id)
        {
            if (this.leftChild == null)
            {
                this.leftChild = new Node(newId, this);
                this.Refresh();
                return this.leftChild;
            }

            var newNode = this.leftChild.InsertAvl(newId);
            var bal = this.GetBalance();

            if (bal > 1 && this.leftChild.GetBalance() >= 0)
            {
                this.RotateToRightRight();
            }
            else if (bal > 1 && this.leftChild.GetBalance() <= 0)
            {
                this.RotateToLeftRight();
            }

            return newNode;
        }
        else
        {
            if (this.rightChild == null)
            {
                this.rightChild = new Node(newId, this);
                this.Refresh();
                return this.rightChild;
            }

            var newNode = this.rightChild.InsertAvl(newId);
            var bal = this.GetBalance();

            if (bal < -1 && this.rightChild.GetBalance() <= 0)
            {
                this.RotateToLeftLeft();
            }
            else if (bal < -1 && this.rightChild.GetBalance() >= 0)
            {
                this.RotateToRightLeft();
            }

            return newNode;
        }
    }

    public void RefreshAll()
    {
        this.Refresh();

        this.leftChild?.RefreshAll();
        this.rightChild?.RefreshAll();
    }

    #endregion

    #region Internal Methods

    internal void TryBalance()
    {
        var balance = this.GetBalance();
        switch (balance)
        {
            case > 1 when this.LeftChild != null:
                this.LeftChild.TryBalance();
                this.ExecuteBalance();
                this.Parent?.TryBalance();
                break;
            case < -1 when this.RightChild != null:
                this.RightChild.TryBalance();
                this.ExecuteBalance();
                this.Parent?.TryBalance();
                break;
            default:
                this.LeftChild?.TryBalance();
                this.RightChild?.TryBalance();
                break;
        }
    }

    #endregion

    #region Private Methods

    private void BalanceCommand_Execute()
    {
        this.TryBalance();
        this.RefreshAll();
        this.Parent?.Refresh();
    }

    /// <summary>
    /// Deletes the command execute.
    /// </summary>
    private void DeleteCommand_Execute(object obj)
    {
        Application.Current.Dispatcher.BeginInvoke(
            () => { MainViewModel.DeleteNode(this); },
            DispatcherPriority.ApplicationIdle);
    }

    /// <summary>
    /// as the balance.
    /// </summary>
    private void ExecuteBalance()
    {
        var bal = this.GetBalance();

        if (bal > 1 && this.leftChild.GetBalance() >= 0)
        {
            this.RotateToRightRight();
        }
        else if (bal > 1 && this.leftChild.GetBalance() <= 0)
        {
            this.RotateToLeftRight();
        }
        else if (bal < -1 && this.rightChild.GetBalance() <= 0)
        {
            this.RotateToLeftLeft();
        }
        else if (bal < -1 && this.rightChild.GetBalance() >= 0)
        {
            this.RotateToRightLeft();
        }
    }

    /// <summary>
    /// Inserts the node.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="newId">The identifier.</param>
    /// <returns>Node.</returns>
    private Node InsertNode(ref Node node, int newId)
    {
        if (node != null)
        {
            return node.Insert(newId);
        }

        node = new Node(newId, this);
        this.Refresh();
        return node;
    }

    private void RotateToLeftLeft()
    {
        switch (this.Side)
        {
            case Side.Root:
                // rotate
                var rightTemp = this.RightChild;
                this.RightChild = rightTemp.LeftChild;
                rightTemp.LeftChild = this;

                // update parent
                rightTemp.Parent = null;

                // return reference
                MainViewModel.root = rightTemp;
                break;
            case Side.Right:
                this.Parent.RightChild = this.RightChild;
                this.RightChild = this.RightChild?.LeftChild;
                this.Parent.RightChild.LeftChild = this;
                break;
            case Side.Left:
                this.Parent.LeftChild = this.RightChild;
                this.RightChild = this.RightChild?.LeftChild;
                this.Parent.LeftChild.LeftChild = this;
                break;
        }
    }

    /// <summary>
    /// Rotates to right left.
    /// </summary>
    private void RotateToLeftRight()
    {
        var leaf = this.LeftChild.RightChild.LeftChild;
        this.LeftChild.RightChild.LeftChild = this.LeftChild;
        this.LeftChild = this.LeftChild.RightChild;
        this.LeftChild.LeftChild.RightChild = leaf;

        this.RotateToRightRight();
    }

    // Rotates to right left
    private void RotateToRightLeft()
    {
        var leaf = this.RightChild.LeftChild.RightChild;
        this.RightChild.LeftChild.RightChild = this.RightChild;
        this.RightChild = this.RightChild.LeftChild;
        this.RightChild.RightChild.LeftChild = leaf;

        this.RotateToLeftLeft();
    }

    private void RotateToRightRight()
    {
        switch (this.Side)
        {
            case Side.Root:
                // rotate
                var leftTemp = this.LeftChild;
                this.LeftChild = leftTemp.RightChild;
                leftTemp.RightChild = this;

                // update parent
                leftTemp.Parent = null;

                // return reference
                MainViewModel.root = leftTemp;
                break;
            case Side.Left:
                this.Parent.LeftChild = this.LeftChild;
                this.LeftChild = this.LeftChild?.RightChild;
                this.Parent.LeftChild.RightChild = this;
                break;
            case Side.Right:
                this.Parent.RightChild = this.LeftChild;
                this.LeftChild = this.LeftChild?.RightChild;
                this.Parent.RightChild.RightChild = this;
                break;
        }
    }

    #endregion
}