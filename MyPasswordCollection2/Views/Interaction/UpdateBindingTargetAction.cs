﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace MPC.Views
{
    class UpdateBindingTargetAction : TriggerAction<Button>
    {
        public Control View
        {
            get { return (Control)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for View.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register(nameof(View), typeof(Control), typeof(UpdateBindingTargetAction));



        public DependencyProperty Property
        {
            get { return (DependencyProperty)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Property.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyProperty =
            DependencyProperty.Register("Property", typeof(DependencyProperty), typeof(UpdateBindingTargetAction));




        protected override void Invoke(object parameter)
        {
            View.GetBindingExpression(Property).UpdateTarget();
        }
    }
}
