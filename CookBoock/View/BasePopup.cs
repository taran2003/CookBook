using Mopups.Pages;
using Mopups.Services;

namespace CookBoock.View
{
    public abstract class BasePopup<TReturnValue> : PopupPage
    {
        public virtual async Task<TReturnValue> ShowAsync(bool animate = true)
        {
            try
            {
                await MopupService.Instance.PushAsync(this, animate);
            }
            catch (Exception exception)
            {
            }

            return default;
        }

        public virtual async Task HideAsync(bool animate = true)
        {
            try
            {
                await MopupService.Instance.RemovePageAsync(this, animate);
            }
            catch (Exception exception)
            {
            }
        }
    }
}
