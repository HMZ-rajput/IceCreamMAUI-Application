using Google.Android.Material.Badge;
using Google.Android.Material.BottomNavigation;
using IceCreamMAUI.ViewModels;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;

namespace IceCreamMAUI;

public class TabBarBadgeRenderer : ShellRenderer
{
    protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
    {
        return new BadgeShellBottomNavViewAppearanceTracker(this, shellItem);
    }

    class BadgeShellBottomNavViewAppearanceTracker : ShellBottomNavViewAppearanceTracker
    {
        private BadgeDrawable _badgeDrawable;

        public BadgeShellBottomNavViewAppearanceTracker(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem)
        {
        }

        public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {

            base.SetAppearance(bottomView, appearance);

            if (_badgeDrawable is null)
            {
                const int cartTabBarItemIndex = 1;
                _badgeDrawable = bottomView.GetOrCreateBadge(cartTabBarItemIndex);
                UpdateBadge(CartViewModel.TotalCartCount);
                CartViewModel.TotalCartCountChanged += CartViewModel_TotalCountChanged;
            }
        }

        private void CartViewModel_TotalCountChanged(object? sender, int newCount) => UpdateBadge(newCount);

        private void UpdateBadge(int count)
        {
            if (count <= 0)
            {
                _badgeDrawable.SetVisible(false);
            }
            else
            {
                _badgeDrawable.Number = count;
                _badgeDrawable.BackgroundColor = Colors.DeepPink.ToPlatform();
                _badgeDrawable.BadgeTextColor = Colors.White.ToPlatform();
                _badgeDrawable.SetVisible(true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            CartViewModel.TotalCartCountChanged -= CartViewModel_TotalCountChanged;
            base.Dispose(disposing);
        }


    }
}

