namespace SD85_WebBookOnline.Client.Models
{
    public class ThongKeViewModel
    {
        public Guid Id { get; set; }
        public string tensach { get; set; }
        public int TongSoSachBanDuoc { get; set; }
        public int SoSachConLai { get; set; }

        public decimal TongDoanhThusach { get; set; }
        public decimal LoiNhuansach { get; set; }
        public decimal ChiPhiGocsach { get; set; }
        public DateTime? ThoiGian { get; set; }
    }
     public class ThongKeViewModel2
    {
        
        public string tensach { get; set; }
        public int TongSoSachBanDuoc { get; set; }
     

        public decimal TongDoanhThusach { get; set; }
        public decimal LoiNhuansach { get; set; }
        public decimal ChiPhiGocsach { get; set; }
      
    }

}
