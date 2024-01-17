namespace SD85_WebBookOnline.Client.Models
{
    public class feeShipGhnViewModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public FeeShipData data { get; set; }
        public class FeeShipData
        {
            public decimal total { get; set; }
        }
    }
}
