using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace OneStreamWebMvvm
{
#nullable disable
	public partial class TextBoxView : ComponentBase
	{
        [Parameter] public string ClassNames { get; set; }
        [Parameter] public string Style { get; set; }
        [Parameter] public string MaxLength { get; set; }
        [Parameter] public int? VisibleCharacters { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public bool PreventDefault { get; set; }
        [Parameter] public bool StopPropagation { get; set; }
        [Parameter] public string Value { get; set; }
        [Parameter] public EventCallback<string> ValueInputChanged { get; set; }
        [Parameter] public EventCallback<string> ValueChanged { get; set; }
        [Parameter] public Expression<Func<string>> ValueExpression { get; set; }
        [Parameter] public EventCallback OnClicked { get; set; }
        [Parameter] public bool ReadOnly { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> Attributes { get; set; }

        protected EditContext EditContext { get; set; }
        protected FieldIdentifier FieldIdentifier { get; set; }

        protected virtual string FormatValueAsString(string value) => value?.ToString();

        protected async Task OnInputAsync(ChangeEventArgs e)
        {
            string _value = e.Value as string;
            await ValueInputChanged.InvokeAsync(_value);
        }

        protected async Task OnClickHandler()
        {
            await OnClicked.InvokeAsync(null);
        }

        protected string CurrentValue
        {
            get => this.Value;
            set
            {
                if (!EqualityComparer<string>.Default.Equals(value, Value))
                {
                    Value = value;
                    _ = ValueChanged.InvokeAsync(value);
                    EditContext?.NotifyFieldChanged(FieldIdentifier);
                }
            }

        }

        protected bool TryParseValueFromString(string value, out string result, out string validationErrorMessage)
        {
            result = value;
            validationErrorMessage = null;
            return true;
        }

        protected string CurrentValueAsString
        {
            get => FormatValueAsString(CurrentValue);
            set
            {
                _ = ValueChanged.InvokeAsync(this.Value);
            }
        }

        protected async Task OnChangeAsync(ChangeEventArgs e)
        {
            string value = e?.Value?.ToString();
            bool empty = false;

            if (string.IsNullOrEmpty(value))
            {
                empty = true;
                CurrentValue = default;
            }

            if (!empty)
            {
                var result = TryParseValueFromString(value, out var parsedValue, out var validationErrorMessage);
                if (result == true)
                {
                    this.CurrentValue = parsedValue;
                    if (!EqualityComparer<string>.Default.Equals(CurrentValue, Value))
                    {
                        this.Value = CurrentValue;
                        _ = ValueChanged.InvokeAsync(this.Value);
                    }

                }
            }
            await Task.CompletedTask;
        }
    }
}
