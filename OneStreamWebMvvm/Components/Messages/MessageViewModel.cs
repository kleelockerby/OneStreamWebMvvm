using Microsoft.AspNetCore.Components;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class MessageViewModel : ViewModelBase
    {
        private readonly IMessageAggregator messageAggregator;

        public AsyncRelayCommand? Command;
        public string ClassName => "btn btn-primary";
        public string? MessageText { get; set; } = string.Empty;

        public MessageViewModel(IMessageAggregator MessageAggregator)
        {
            this.messageAggregator = MessageAggregator;
            this.Command = new AsyncRelayCommand(GetMessageAsync, CanGetMessage);
        }

        public void OnTextChanged(ChangeEventArgs e)
        {
            string? txt = e.Value as string;
            this.MessageText = txt;
        }

        public async Task GetMessageAsync()
        {
            MessageTitle message = new MessageTitle() { Title = this.MessageText };
            await messageAggregator.PublishAsync(message);
        }

        public bool CanGetMessage()
        {
            return this.MessageText?.Length > 0;
        }
    }
}