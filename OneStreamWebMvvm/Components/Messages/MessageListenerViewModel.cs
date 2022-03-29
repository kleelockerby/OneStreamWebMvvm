
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class MessageListenerViewModel : ViewModelBase, IHandleViewModelMessage<MessageTitle>
    {
        private readonly IMessageAggregator? messageAggregator;
        private string? messageText;

        public string? MessageText
        {
            get => messageText;
            set => SetProperty(ref messageText, value, nameof(MessageText));
        }

        public MessageListenerViewModel(IMessageAggregator? MessageAggregator)
        {
            this.messageAggregator = MessageAggregator;
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            messageAggregator?.Subscribe(this);
        }

        public Task HandleAsync(MessageTitle message)
        {
            this.MessageText = message.Title;
            return Task.CompletedTask;
        }
    }
}