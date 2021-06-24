using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.DataStructure.Tree
{
    public class TreeNode
    {
        public virtual int Id { get; set; }

        public virtual int ParentId { get; set; }

        public IEnumerable<TreeNode> Children { get; set; }
    }

    public class SortableTreeNode : TreeNode
    {
        public virtual int Sort { get; set; }
    }
}
