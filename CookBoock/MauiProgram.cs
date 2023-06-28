#if ANDROID
using Android.App;
using Android.Content;
using Android.OS;
#endif
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using CommunityToolkit.Maui;
using Mopups.Hosting;

namespace CookBoock;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureMopups()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.ConfigureLifecycleEvents(events =>
            {
#if ANDROID
				events.AddAndroid(android => android
                .OnActivityResult((activity, requestCode, resultCode, data) => OnActivityResult(requestCode, resultCode, data))
                .OnPostCreate((activity, bundle) => OnCreate(activity, bundle)));
                static void OnActivityResult(int requestCode, Result resultCode, Intent intent)
                {
                    if (NativeMedia.Platform.CheckCanProcessResult(requestCode, resultCode, intent))
                        NativeMedia.Platform.OnActivityResult(requestCode, resultCode, intent);
                }
                static void OnCreate(Activity activity, Bundle savedInstanceState)
                {
                    NativeMedia.Platform.Init(activity, savedInstanceState);
                }
#endif

            })
            .ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.AddHandler(typeof(Shell), typeof(CookBoock.Platforms.CustomShellRenderer));
#endif
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
