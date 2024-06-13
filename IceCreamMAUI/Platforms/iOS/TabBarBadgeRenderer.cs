using IceCreamMAUI.ViewModels;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace IceCreamMAUI;

public class TabBarBadgeRenderer : ShellRenderer
{
    protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
    {
        return new BadgeShellTabBarAppearanceTracker();
    }

    class BadgeShellTabBarAppearanceTracker : ShellTabBarAppearanceTracker
    {
        private UITabBarItem? _cartTabbarItem;

        public override void UpdateLayout(UITabBarController controller)
        {
            base.UpdateLayout(controller);

            if(_cartTabbarItem is null)
            {
                const int cartTabBarItemIndex = 1;
                var cartTabbarItem = controller.TabBar.Items?[cartTabBarItemIndex];
                UpdateBadge(CartViewModel.TotalCartCount);
                CartViewModel.TotalCartCountChanged += CartViewModel_TotalCartCountChanged;
            }

        }

        private void CartViewModel_TotalCartCountChanged(object? sender, int newCount)
        {
            UpdateBadge(newCount);
        }

        private void UpdateBadge(int count)
        {
            if (_cartTabbarItem is not null)
            {
                if (count <= 0)
                {
                    _cartTabbarItem.BadgeValue = null;
                }
                else
                {
                    _cartTabbarItem.BadgeValue = count.ToString();
                    _cartTabbarItem.BadgeColor = Colors.DeepPink.ToPlatform();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            CartViewModel.TotalCartCountChanged -= CartViewModel_TotalCartCountChanged;
            base.Dispose(disposing);
        }
    }
}
