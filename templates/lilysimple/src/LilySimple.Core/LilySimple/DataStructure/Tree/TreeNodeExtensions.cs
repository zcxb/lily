using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LilySimple.DataStructure.Tree
{
    public static class TreeNodeExtensions
    {
        public static IEnumerable<T> BuildTree<T>(this IEnumerable<T> items)
            where T : TreeNode
        {
            var roots = items.Where(i => !items.Select(x => x.Id).Contains(i.ParentId));

            foreach (T root in roots)
            {
                root.Children = FindChildren(root, items);
            }

            IEnumerable<T> FindChildren(T node, IEnumerable<T> rest)
            {
                var children = rest.Where(i => i.ParentId == node.Id);
                foreach (T child in children)
                {
                    child.Children = FindChildren(child, items);
                }
                return children;
            }

            return roots;
        }

        public static IEnumerable<T> BuildSortableTree<T>(this IEnumerable<T> items)
            where T : SortableTreeNode
        {
            var roots = items.Where(i => !items.Select(x => x.Id).Contains(i.ParentId))
                .OrderBy(i => i.Sort);

            foreach (T root in roots)
            {
                root.Children = FindChildren(root, items);
            }

            IEnumerable<T> FindChildren(T node, IEnumerable<T> rest)
            {
                var children = rest.Where(i => i.ParentId == node.Id).OrderBy(i => i.Sort);
                foreach (T child in children)
                {
                    child.Children = FindChildren(child, items);
                }
                return children;
            }

            return roots;
        }
    }
}
