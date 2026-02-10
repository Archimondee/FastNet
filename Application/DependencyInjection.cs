using Application.Auth.LoginUser;
using Application.Auth.RegisterUser;
using Application.Behavior;
using Application.Interface;
using Application.Users.CreateUser;
using Application.Users.UpdateUser;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped(typeof(BehaviorExecutor<,>));
        services.AddScoped(
            typeof(IBehavior<,>),
            typeof(TransactionBehavior<,>));

        services.AddScoped<CreateUserHandler>();
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();
        services.AddScoped<UpdateUserHandler>();
        return services;
    }
}