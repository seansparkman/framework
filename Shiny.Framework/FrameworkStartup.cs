﻿using DryIoc;
using DryIoc.Microsoft.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Shiny.Impl;

using System;

namespace Shiny
{
    public class FrameworkStartup : ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection services, IPlatform platform)
        {
            services.AddSingleton<IDialogs, XfMaterialDialogs>();
            services.AddSingleton<GlobalExceptionHandler>();
        }


        internal static IContainer? Container { get; private set; }
        public override IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            var container = new Container(Rules
                .Default
                .WithConcreteTypeDynamicRegistrations(reuse: Reuse.Transient)
                .With(Made.Of(FactoryMethod.ConstructorWithResolvableArguments))
                .WithFuncAndLazyWithoutRegistration()
                .WithTrackingDisposableTransients()
                .WithoutFastExpressionCompiler()
                .WithFactorySelector(Rules.SelectLastRegisteredFactory())
            );
            DryIocAdapter.Populate(container, services);
            Container = container;
            return container.GetServiceProvider();
        }
    }
}
