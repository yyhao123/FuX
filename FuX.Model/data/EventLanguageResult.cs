using FuX.Model.@enum;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    public class EventLanguageResult : EventArgsAsync
    {
        public bool Status { get; set; }

        public string? Message { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LanguageType? Language { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;


        public EventLanguageResult()
        {
        }

        public EventLanguageResult(EventLanguageResult result)
        {
            Status = result.Status;
            Message = result.Message;
            Language = result.Language;
            Time = result.Time;
        }

        public EventLanguageResult(bool status, string message, LanguageType? language = null)
        {
            Status = status;
            Language = language;
            Message = message;
        }

        public bool GetDetails(out string? message, out LanguageType? language)
        {
            language = Language;
            message = Message;
            return Status;
        }

        public bool GetDetails(out LanguageType? language)
        {
            language = Language;
            return Status;
        }

        public bool GetDetails(out string? message)
        {
            message = Message;
            return Status;
        }

        public bool GetDetails(out EventLanguageResult result)
        {
            result = this;
            return Status;
        }

        public static EventLanguageResult CreateSuccessResult(string successMessage)
        {
            return new EventLanguageResult(status: true, successMessage);
        }

        public static EventLanguageResult CreateFailureResult(string failureMessage)
        {
            return new EventLanguageResult(status: false, failureMessage);
        }

        public static EventLanguageResult CreateSuccessResult(string successMessage, LanguageType? language)
        {
            return new EventLanguageResult(status: true, successMessage, language);
        }

        public static EventLanguageResult CreateFailureResult(string failureMessage, LanguageType? language)
        {
            return new EventLanguageResult(status: false, failureMessage, language);
        }

        public override string ToString()
        {
            return this.ToJson(formatting: true) ?? string.Empty;
        }
    }
}
