using System;
using Api.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public static class ConfigureServiceOpenIddict
    {
        public static IServiceCollection ConfigureOpenIddict(this IServiceCollection services)
        {
            services.AddOpenIddict()

                // Register the OpenIddict core components.
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<LoginDbContext>()
                        .ReplaceDefaultEntities<Guid>();

                    options.UseQuartz(config =>
                    {
                        config.SetMinimumTokenLifespan(TimeSpan.FromDays(7));
                        config.SetMinimumAuthorizationLifespan(TimeSpan.FromDays(7));
                    });
                })

                // Register the OpenIddict server components.
                .AddServer(options =>
                {
                    options.SetAuthorizationEndpointUris("/connect/authorize")
                        .SetLogoutEndpointUris("/connect/logout")
                        .SetTokenEndpointUris("/connect/token")
                        .SetUserinfoEndpointUris("/connect/user-info")
                        .SetVerificationEndpointUris("/connect/verify");

                    options
                        .AllowPasswordFlow()
                        .AllowRefreshTokenFlow();

                    options
                        .AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();

                    options.RequireProofKeyForCodeExchange();

                    options.UseAspNetCore()
                        .EnableStatusCodePagesIntegration()
                        .EnableAuthorizationEndpointPassthrough()
                        .EnableLogoutEndpointPassthrough()
                        .EnableTokenEndpointPassthrough()
                        .EnableUserinfoEndpointPassthrough()
                        .EnableVerificationEndpointPassthrough()
                        .DisableTransportSecurityRequirement();
                    

                    // Note: if you don't want to specify a client_id when sending
                    // a token or revocation request, uncomment the following line:
                    //
                    options.AcceptAnonymousClients();

                    // Note: if you want to process authorization and token requests
                    // that specify non-registered scopes, uncomment the following line:
                    //
                    options.DisableScopeValidation();

                    // Note: if you don't want to use permissions, you can disable
                    // permission enforcement by uncommenting the following lines:
                    //
                    // options.IgnoreEndpointPermissions()
                    //        .IgnoreGrantTypePermissions()
                    //        .IgnoreResponseTypePermissions()
                    //        .IgnoreScopePermissions();

                    // Note: when issuing access tokens used by third-party APIs
                    // you don't own, you can disable access token encryption:
                    //
                    options.DisableAccessTokenEncryption();
                    options.SetAccessTokenLifetime(TimeSpan.FromMinutes(3));
                    options.SetRefreshTokenLifetime(TimeSpan.FromDays(7));
                })

                // Register the OpenIddict validation components.
                .AddValidation(options =>
                {
                    // Configure the audience accepted by this resource server.
                    // The value MUST match the audience associated with the
                    // "demo_api" scope, which is used by ResourceController.

                    // Import the configuration from the local OpenIddict server instance.
                    options.UseLocalServer();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();

                    // For applications that need immediate access token or authorization
                    // revocation, the database entry of the received tokens and their
                    // associated authorizations can be validated for each API call.
                    // Enabling these options may have a negative impact on performance.
                    //
                    // options.EnableAuthorizationEntryValidation();
                    // options.EnableTokenEntryValidation();
                });
            return services;
        }
    }
}