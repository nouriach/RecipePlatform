namespace RecipePlatform.NotificationService.Config;

public static class ConfigExtensions
{
    public static IConfigurationBuilder AddApiConfiguration(
        this IConfigurationBuilder builder, Action<ParameterStoreConfigurationSource> configure)
    {
        var source = new ParameterStoreConfigurationSource();
        
        configure(source);
  
        builder.Add(source);
        return builder;
    } 
}