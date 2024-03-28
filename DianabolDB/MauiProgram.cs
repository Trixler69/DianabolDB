using Microsoft.Extensions.Logging;
using Radzen;
using DataAccess.Service;
using DataAccess.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace DianabolDB;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddScoped<TooltipService>();
        builder.Services.AddScoped<ContextMenuService>();
        
        builder.Services.AddSingleton<IOpenFoodFactsService,OpenFoodFactsService>();

        builder.Services.AddSingleton<IDianabolService>(new DianabolService(FileSystem.AppDataDirectory));

        return builder.Build();
	}
}
