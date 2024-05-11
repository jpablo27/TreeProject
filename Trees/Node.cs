// -----------------------------------------------------------------------
// <author>Pablo Sánchez</author>
// <date>2022-09-07</date>
// <summary></summary>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.Windows;

namespace Trees
{
    using System;
    using System.Windows.Threading;
    using Enums;

    /// <summary>
    /// Class Node.
    /// </summary>
    public class Node : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// The left child
        /// </summary>
        private Node leftChild;

        /// <summary>
        /// The right child
        /// </summary>
        private Node rightChild;

        /// <summary>
        /// The identifier
        /// </summary>
        private int id;

        /// <summary>
        /// The delete command
        /// </summary>
        private RelayCommand<object> deleteCommand;

        /// <summary>
        /// The balance command
        /// </summary>
        private RelayCommand<object> balanceCommand;

        /// <summary>
        /// The parent
        /// </summary>
        private Node parent;

        #endregion

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

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether this instance has children.
        /// </summary>
        /// <value><c>true</c> if this instance has children; otherwise, <c>false</c>.</value>
        public bool HasChildren
        {
            get { return this.LeftChild != null || this.RightChild != null; }
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id
        {
            get { return this.id; }

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
            get { return this.leftChild; }

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
        /// Gets or sets the right child.
        /// </summary>
        /// <value>The right child.</value>
        public Node RightChild
        {
            get { return this.rightChild; }
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
        /// Gets the delete command.
        /// </summary>
        /// <value>The delete command.</value>
        public RelayCommand<object> DeleteCommand
        {
            get { return this.deleteCommand ??= new RelayCommand<object>(this.DeleteCommand_Execute); }
        }

        /// <summary>
        /// Balance Command
        /// </summary>
        public RelayCommand<object> BalanceCommand
        {
            get { return this.balanceCommand ??= new RelayCommand<object>(this.BalanceCommand_Execute); }
        }

        /// <summary>
        /// Balance Command Execute 
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void BalanceCommand_Execute(object obj)
        {
            Debug.WriteLine($"Requested balance for {this} ------ ");

            this.BalanceNode();

            MainViewModel.RefreshWholeUi();
        }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public Node Parent
        {
            get { return this.parent; }
            set
            {
                this.parent = value;
                this.OnPropertyChanged();
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

        /// <summary>
        /// Deletes the command execute.
        /// </summary>
        private void DeleteCommand_Execute(object obj)
        {
            Application.Current.Dispatcher.Invoke(
                () => { MainViewModel.DeleteNode(this); },
                DispatcherPriority.ApplicationIdle);
        }

        #endregion

        #region Public Methods

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

            return newId > this.Id
                ? this.InsertNode(ref this.rightChild, newId)
                : this.InsertNode(ref this.leftChild, newId);
        }

        /// <summary>
        /// Gets the height of the tree
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetMaxDepth()
        {
            if (this.rightChild != null && this.leftChild == null)
            {
                return 1 + this.rightChild.GetMaxDepth();
            }

            if (this.leftChild != null && this.rightChild == null)
            {
                return 1 + this.leftChild.GetMaxDepth();
            }

            if (this.rightChild != null && this.leftChild != null)
            {
                return 1 + Math.Max(this.leftChild.GetMaxDepth(), this.rightChild.GetMaxDepth());
            }

            return 1;
        }

        /// <summary>
        /// Gets the balance.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetBalanceWeight()
        {
            if (this.rightChild != null && this.leftChild == null)
            {
                return -this.rightChild.GetMaxDepth();
            }

            if (this.leftChild != null && this.rightChild == null)
            {
                return this.leftChild.GetMaxDepth();
            }

            if (this.rightChild != null && this.leftChild != null)
            {
                return this.leftChild.GetMaxDepth() - this.rightChild.GetMaxDepth();
            }

            return 0;
        }

        #endregion

        #region Private Methods

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

        #endregion

        /// <summary>
        /// Return id as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Id.ToString();
        }

        /// <summary>
        /// Insert Avl node
        /// </summary>
        /// <param name="newId"></param>
        /// <returns></returns>
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
                var bal = this.GetBalanceWeight();

                if (bal > 1 && this.leftChild.GetBalanceWeight() >= 0)
                {
                    this.RotateToRightRight();
                }
                else if (bal > 1 && this.leftChild.GetBalanceWeight() <= 0)
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
                var bal = this.GetBalanceWeight();

                if (bal < -1 && this.rightChild.GetBalanceWeight() <= 0)
                {
                    this.RotateToLeftLeft();
                }
                else if (bal < -1 && this.rightChild.GetBalanceWeight() >= 0)
                {
                    this.RotateToRightLeft();
                }

                return newNode;
            }
        }

        /// <summary>
        /// Rotates to right left.
        /// </summary>
        private void RotateToLeftRight()
        {
            var nodeToLeftDown = this.LeftChild;
            this.LeftChild = nodeToLeftDown.RightChild;
            nodeToLeftDown.RightChild = this.LeftChild.LeftChild;
            this.LeftChild.LeftChild = nodeToLeftDown;

            this.RotateToRightRight();
        }

        /// <summary>
        /// Rotates to right left
        /// </summary>
        private void RotateToRightLeft()
        {
            var nodeToRightDown = this.RightChild;
            this.RightChild = nodeToRightDown.LeftChild;
            nodeToRightDown.LeftChild = this.RightChild.RightChild;
            this.RightChild.RightChild = nodeToRightDown;

            this.RotateToLeftLeft();
        }

        /// <summary>
        /// Rotate to right right
        /// </summary>
        private void RotateToRightRight()
        {
            var currentParent = this.Parent ?? MainViewModel.RootNode;
            switch (this.Side)
            {
                case Side.Root:
                    MainViewModel.RootNode = this.LeftChild;
                    this.LeftChild = this.LeftChild?.RightChild;
                    MainViewModel.RootNode.RightChild = this;
                    break;
                case Side.Left:
                    currentParent.LeftChild = this.LeftChild;
                    this.LeftChild = this.LeftChild?.RightChild;
                    currentParent.LeftChild.RightChild = this;
                    break;
            }
        }

        /// <summary>
        /// Rotates to left -> CCW
        /// </summary>
        private void RotateToLeftLeft()
        {
            var currentParent = this.Parent ?? MainViewModel.RootNode;
            switch (this.Side)
            {
                case Side.Root:
                    MainViewModel.RootNode = this.RightChild;
                    this.RightChild = this.RightChild?.LeftChild;
                    MainViewModel.RootNode.LeftChild = this;
                    break;
                case Side.Right:
                    currentParent.RightChild = this.RightChild;
                    this.RightChild = this.RightChild?.LeftChild;
                    currentParent.RightChild.LeftChild = this;
                    break;
            }
        }

        /// <summary>
        /// Refresh all nodes
        /// </summary>
        public void RefreshAll()
        {
            this.Refresh();
            this.leftChild?.RefreshAll();
            this.rightChild?.RefreshAll();
        }

        /// <summary>
        /// Balance Node
        /// </summary>
        internal void BalanceNode()
        {
            // Here we are getting the height of the children + the current level. That is why the  +1 
            var heightLeft = this.LeftChild?.GetMaxDepth() + 1 ?? 0;
            var heightRight = this.RightChild?.GetMaxDepth() + 1 ?? 0;

            var maxDepth = this.GetMaxDepth();


            Debug.WriteLine(
                $"Balance ----- Current Node {this} : Height {maxDepth} : Balance {this.GetBalanceWeight()}");
            Debug.WriteLine($"\t Left height = {heightLeft}  VS   Right height = {heightRight}");

            if ((heightRight >= 3 || heightLeft >= 3) && maxDepth != 3)
            {
                var diveSide = heightLeft >= heightRight ? Side.Left : Side.Right;

                switch (diveSide)
                {
                    case Side.Left:
                        this.LeftChild?.BalanceNode();
                        break;
                    case Side.Right:
                        this.RightChild?.BalanceNode();
                        break;
                }
            }

            // RE-calculate in case it changed while balancing children
            var thisHeight = this.GetMaxDepth();
            var balanceWeight = this.GetBalanceWeight();
            if (thisHeight >= 3 && Math.Abs(balanceWeight) > 1)
            {
                this.PerformRotations(balanceWeight);
            }
        }

        /// <summary>
        /// Perform Rotations
        /// </summary>
        /// <param name="balanceWeight"></param>
        private void PerformRotations(int balanceWeight)
        {
            switch (balanceWeight)
            {
                case > 1 when this.leftChild.GetBalanceWeight() >= 0:
                    this.RotateToRightRight();
                    break;
                case > 1 when this.leftChild.GetBalanceWeight() <= 0:
                    this.RotateToLeftRight();
                    break;
                case < -1 when this.rightChild.GetBalanceWeight() <= 0:
                    this.RotateToLeftLeft();
                    break;
                case < -1 when this.rightChild.GetBalanceWeight() >= 0:
                    this.RotateToRightLeft();
                    break;
            }
        }
    }
}