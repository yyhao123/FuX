using FuX.Model.@enum;
using FuX.Unility;
using System.Text.Json.Serialization;


namespace FuX.Model.data
{
    public class AddressValue : AddressDetails
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QualityType Quality { get; set; } = QualityType.None;


        public string? Message { get; set; }

        public object? ResultValue { get; set; }

        public object? OriginalValue { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;


        public AddressValue()
        {
        }

        public AddressValue(QualityType quality, object resultValue, object originalValue, string message, DateTime time)
        {
            Quality = quality;
            ResultValue = resultValue;
            OriginalValue = originalValue;
            Message = message;
            Time = time;
        }

        public AddressValue(QualityType quality, object resultValue, object originalValue, string message, DateTime time, AddressDetails addressDetails)
        {
            Quality = quality;
            ResultValue = resultValue;
            OriginalValue = originalValue;
            Message = message;
            Time = time;
            SET(addressDetails);
        }

        public AddressValue(QualityType quality, object resultValue, object originalValue, string message)
        {
            Quality = quality;
            ResultValue = resultValue;
            OriginalValue = originalValue;
            Message = message;
        }

        public AddressValue(QualityType quality, object resultValue, object originalValue, string message, AddressDetails addressDetails)
        {
            Quality = quality;
            ResultValue = resultValue;
            OriginalValue = originalValue;
            Message = message;
            SET(addressDetails);
        }

        public QualityType GetDetails(out object? resultValue, out object? originalValue, out string? message, out DateTime? time)
        {
            resultValue = ResultValue;
            originalValue = OriginalValue;
            message = Message;
            time = Time;
            return Quality;
        }

        public AddressValueSimplify GetSimplify()
        {
            return new AddressValueSimplify(base.SN, base.AddressName, base.Length, base.EncodingType, Quality, ResultValue.ToJson(), Message, Time.Ticks, base.AddressDataType);
        }

        public AddressValue SET(AddressDetails addressDetails)
        {
            base.SN = addressDetails.SN;
            base.AddressName = addressDetails.AddressName;
            base.AddressAnotherName = addressDetails.AddressAnotherName;
            base.AddressPropertyName = addressDetails.AddressPropertyName;
            base.AddressDescribe = addressDetails.AddressDescribe;
            base.AddressParseParam = addressDetails.AddressParseParam;
            base.AddressMqParam = addressDetails.AddressMqParam;
            base.AddressExtendParam = addressDetails.AddressExtendParam;
            base.IsEnable = addressDetails.IsEnable;
            base.AddressDataType = addressDetails.AddressDataType;
            base.AddressType = addressDetails.AddressType;
            base.Length = addressDetails.Length;
            base.EncodingType = addressDetails.EncodingType;
            return this;
        }

        public override bool Equals(object? o)
        {
            if (o == null)
            {
                return false;
            }
            return this.Comparer(o as AddressValue, new string[1] { "Time" }).result;
        }
    }
}
