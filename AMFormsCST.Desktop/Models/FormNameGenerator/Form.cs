using AMFormsCST.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleType = AMFormsCST.Core.Types.BestPractices.Models.AutoMateFormModel.SoldTrade;
using Format = AMFormsCST.Core.Types.BestPractices.Models.AutoMateFormModel.FormFormat;

namespace AMFormsCST.Desktop.Models.FormNameGenerator
{
    public partial class Form : ObservableObject
    {
        [ObservableProperty]
        private string? _title = string.Empty;
        partial void OnTitleChanged(string? value) => OnPropertyChanged(nameof(FileName));
        [ObservableProperty]
        private string? _code = string.Empty;
        partial void OnCodeChanged(string? value) => OnPropertyChanged(nameof(FileName));
        [ObservableProperty]
        private string? _revisionDate = string.Empty;
        partial void OnRevisionDateChanged(string? value) => OnPropertyChanged(nameof(FileName));
        [ObservableProperty]
        private string? _state = string.Empty;
        partial void OnStateChanged(string? value) => OnPropertyChanged(nameof(FileName));
        [ObservableProperty]
        private string? _Dealer = string.Empty;
        partial void OnDealerChanged(string? value) => OnPropertyChanged(nameof(FileName));
        [ObservableProperty]
        private string? _oem = string.Empty;
        partial void OnOemChanged(string? value) => OnPropertyChanged(nameof(FileName));
        [ObservableProperty]
        private string? _bank = string.Empty;
        partial void OnBankChanged(string? value) => OnPropertyChanged(nameof(FileName));
        [ObservableProperty]
        private string? _provider = string.Empty; 
        partial void OnProviderChanged(string? value) => OnPropertyChanged(nameof(FileName));

        private string? _fileName => GetFileName();

        public string? FileName
        {
            get => _fileName;
        }

        public ObservableCollection<Tag> Tags { get; } = [];

        public enum Tag
        {
            Pdf,
            Impact,
            Sold,
            Trade,
            Law,
            Custom,
            VehicleMerchandising
        }

        public void AddTag(Tag tag)
        {
            if (!Tags.Contains(tag))
            {
                Tags.Add(tag);
            }

            if(tag is Tag.Pdf && Tags.Contains(Tag.Impact)) Tags.Remove(Tag.Impact);
            if(tag is Tag.Impact && Tags.Contains(Tag.Pdf)) Tags.Remove(Tag.Pdf);

            if (tag is Tag.Sold && Tags.Contains(Tag.Trade)) Tags.Remove(Tag.Trade);
            if (tag is Tag.Trade && Tags.Contains(Tag.Sold)) Tags.Remove(Tag.Sold);
        }
        public void RemoveTag(Tag tag) => Tags.Remove(tag);

        public void Clear()
        {
            Title = string.Empty;
            Code = string.Empty;
            RevisionDate = string.Empty;
            State = string.Empty;
            Dealer = string.Empty;
            Oem = string.Empty;
            Bank = string.Empty;
            Provider = string.Empty;
            Tags.Clear();
            Tags.Add(Tag.Pdf);
        }

        public string GetFileName()
        {
            _supportTool.Enforcer.FormNameBestPractice.Model = new Core.Types.BestPractices.Models.AutoMateFormModel
            {
                Name = Title ?? string.Empty,
                Code = Code ?? string.Empty,
                RevisionDate = RevisionDate ?? string.Empty,
                State = State ?? string.Empty,
                Dealership = Dealer ?? string.Empty,
                Manufacturer = Oem ?? string.Empty,
                Bank = Bank ?? string.Empty,
                IsLAW = Tags.Contains(Tag.Law),
                IsCustom = Tags.Contains(Tag.Custom),
                VehicleType = Tags.Contains(Tag.Sold) ? VehicleType.Sold :
                              Tags.Contains(Tag.Trade) ? VehicleType.Trade : VehicleType.None,
                IsVehicleMerchandising = Tags.Contains(Tag.VehicleMerchandising),
                Format = Tags.Contains(Tag.Pdf) ? Format.Pdf : Format.LegacyImpact
            };
            return _supportTool.Enforcer.GetFormName();
        }
        private readonly ISupportTool _supportTool;
        public Form(ISupportTool supportTool)
        {
            _supportTool = supportTool ?? throw new ArgumentNullException(nameof(supportTool));
            Tags.CollectionChanged += (s, e) => OnPropertyChanged(nameof(FileName));
            AddTag(Tag.Pdf);
        }
    }
}
