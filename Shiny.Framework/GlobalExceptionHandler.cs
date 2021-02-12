﻿using System;

using Microsoft.Extensions.Logging;

using ReactiveUI;


namespace Shiny
{
    public class GlobalExceptionHandler : IObserver<Exception>, IShinyStartupTask
    {
        readonly IDialogs dialogs;
        readonly ILogger logger;


        public GlobalExceptionHandler(IDialogs dialogs, ILogger<GlobalExceptionHandler> logger)
        {
            this.dialogs = dialogs;
            this.logger = logger;
        }


        public void Start() => RxApp.DefaultExceptionHandler = this;
        public void OnCompleted() { }
        public void OnError(Exception error) { }


        public async void OnNext(Exception value)
        {
            this.logger.LogError(value, "Error in view");
            await this.dialogs.Alert(value.ToString(), "ERROR");
        }
    }
}