using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Interop;

namespace MPC.Views
{
    class DropBehavior : Behavior<Window>
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DropBehavior));


        protected override void OnAttached()
        {
            AssociatedObject.PreviewDragOver += PreviewDragOver;
            AssociatedObject.Drop += PreviewDrop;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewDragOver -= PreviewDragOver;
            AssociatedObject.PreviewDrop -= PreviewDrop;
        }

        private void PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = IsDropAllow(e) ? DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }

        private void PreviewDrop(object sender, DragEventArgs e)
        {
            if (IsDropAllow(e))
            {
                var path = (e.Data.GetData(DataFormats.FileDrop) as string[])?[0];
                if (path != null)
                {
                    Command.Execute(path);
                }
                e.Handled = true;
            }
        }

        private bool IsDropAllow(DragEventArgs e)
        {
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            return paths?.Length == 1 && Path.GetExtension(paths[0]) == ".pw" && !ComponentDispatcher.IsThreadModal;
        }
    }
}
