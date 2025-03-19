using FormgenAssistant.Controls;
using FormgenAssistant.SavedItems;
using FormgenAssistant.SavedItems.Templates;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FormgenAssistant.Pages
{
    /// <summary>
    /// Interaction logic for Notes.xaml
    /// </summary>
    public partial class Notes : UserControl
    {
        public static NotesInfo? SelectedNote { get; set; }

        private List<NotesInfo> NotesList = [];
        public class NotesInfo(TabItem tabItem)
        {
            public string? ServerId { get; set; }
            public string? Companies { get; set; }
            public string? Dealership { get; set; }
            public string? ContactName { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public string? NotesText { get; set; }
            public string? CaseText { get; set; }
            public string? FormsText { get; set; }
            public string? DealText { get; set; }
            public TabItem TabItem { get; set; } = tabItem;
            public bool IsSelected => TabItem.IsSelected;

            public void Clear()
            {
                ServerId = string.Empty;
                Companies = string.Empty;
                Dealership = string.Empty;
                ContactName = string.Empty;
                Email = string.Empty;
                Phone = string.Empty;
                NotesText = string.Empty;
                CaseText = string.Empty;
                FormsText = string.Empty;
                DealText = string.Empty;
            }
        }
        public Notes()
        {
            InitializeComponent();
            var defaultNote = new NotesInfo(tiTabItemTemplate);
            NotesList.Add(defaultNote);
            Notes.SelectedNote = defaultNote;
        }

        private void btnCopyServer_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtServerId.Text);
        private void btnCopyCompanies_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtCompanies.Text);
        private void btnCopyDealer_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtDealer.Text);
        private void btnCopyName_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtName.Text);
        private void btnCopyEmail_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtEmail.Text);
        private void btnCopyPhone_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtPhone.Text != "" ? txtPhone.Text + (txtPhoneExt.Text != "" ? " x" + txtPhoneExt.Text : "") : "");
        private void btnCopyNotes_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtNotes.Text);
        private void btnCopyCase_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtCaseNo.Text);
        private void btnCopyForms_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtForms.Text);
        private void btnCopyDeal_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtDeal.Text);

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Instance.NotesCopyAll) CopyAll();
            txtServerId.Text = string.Empty;
            txtCompanies.Text = string.Empty;
            txtDealer.Text = string.Empty;
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtPhoneExt.Text = string.Empty;
            txtNotes.Text = string.Empty;
            txtCaseNo.Text = string.Empty;
            txtForms.Text = string.Empty;
            txtDeal.Text = string.Empty;
            
        }

        private void btnCopyAll_Click(object sender, RoutedEventArgs e)
        {
            CopyAll();
        }

        private void CopyAll()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Server: " + txtServerId.Text + "\tCompany(s): " + txtCompanies.Text);
            sb.AppendLine("Dealership: " + txtDealer.Text);
            sb.AppendLine("Contact: " + txtName.Text + "\tE-Mail: " + txtEmail.Text + "\tPhone: " + (txtPhone.Text != "" ? txtPhone.Text + (txtPhoneExt.Text != "" ? " x" + txtPhoneExt.Text : "") : ""));
            sb.AppendLine("Case: " + txtCaseNo.Text + "\nTest Deal: " + txtDeal.Text + "\nForms: " + txtForms.Text);
            sb.AppendLine("Notes: " + txtNotes.Text);
            Clipboard.SetText(sb.ToString());
        }

        private void txtServerId_OnTextChanged(object sender, System.EventArgs e)
        {
            var selected = GetSelected(NotesList);
            if (selected is null || txtServerId.Text is null) return;

            selected.ServerId = txtServerId.Text.ToUpper();
            Notes.SelectedNote = selected;
        }

        private void txtCompanies_OnTextChanged(object sender, System.EventArgs e)
        {
            var selected = GetSelected(NotesList);
            if (selected is null || txtCompanies.Text is null) return;

            selected.Companies = txtCompanies.Text;
            Notes.SelectedNote = selected;
        }

        private void txtDealer_OnTextChanged(object sender, System.EventArgs e)
        {
            var selected = GetSelected(NotesList);
            if (selected is null || txtDealer.Text is null) return;

            selected.Dealership = txtDealer.Text;
            Notes.SelectedNote = selected;
        }

        private void txtName_OnTextChanged(object sender, System.EventArgs e)
        {
            var selected = GetSelected(NotesList);
            if (selected is null || txtName.Text is null) return;

            selected.ContactName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(txtName.Text.ToLowerInvariant());
            Notes.SelectedNote = selected;
        }

        private void txtEmail_OnTextChanged(object sender, System.EventArgs e)
        {
            var selected = GetSelected(NotesList);
            if (selected is null || txtEmail.Text is null) return;

            selected.Email = txtEmail.Text;
            Notes.SelectedNote = selected;
        }

        private void txtPhone_OnTextChanged(object sender, System.EventArgs e)
        {
            var selected = GetSelected(NotesList);
            if (selected is null || txtPhone.Text is null) return;

            selected.Phone = txtPhone.Text != "" ? txtPhone.Text + (txtPhoneExt.Text != "" ? " x" + txtPhoneExt.Text : "") : ""; ;
            Notes.SelectedNote = selected;
        }

        private void txtPhoneExt_OnTextChanged(object sender, System.EventArgs e)
        {
            var selected = GetSelected(NotesList);
            if (selected is null || txtPhoneExt.Text is null) return;

            selected.Phone = txtPhone.Text != "" ? txtPhone.Text + (txtPhoneExt.Text != "" ? " x" + txtPhoneExt.Text : "") : "";
            Notes.SelectedNote = selected;
        }

        private void txtNotes_OnTextChanged(object sender, System.EventArgs e)
        {
            var selected = GetSelected(NotesList);
            if (selected is null || txtNotes.Text is null) return;

            selected.NotesText = txtNotes.Text;
            Notes.SelectedNote = selected;
        }

        private void txtCaseNo_OnTextChanged(object sender, System.EventArgs e)
        {
            var source = (TextBox)sender;
            var selected = GetSelected(NotesList);
            if (selected is null) return;

            var note = selected;
            note.TabItem.Header = string.IsNullOrEmpty(source.Text) ? "No Case#" : txtCaseNo.Text;

            if (string.IsNullOrEmpty(source.Text)) return;

            selected.CaseText = source.Text;
            Notes.SelectedNote = selected;
        }

        private void txtForms_OnTextChanged(object sender, System.EventArgs e)
        {
            var selected = GetSelected(NotesList);
            if (selected is null || txtForms.Text is null) return;

            selected.FormsText = txtForms.Text;
            Notes.SelectedNote = selected;
        }
        private void txtDeal_OnTextChanged(object sender, System.EventArgs e)
        {
            var selected = GetSelected(NotesList);
            if (selected is null || txtDeal.Text is null) return;

            selected.DealText = txtDeal.Text;
            Notes.SelectedNote = selected;
        }

        private void txtServerId_LostFocus(object sender, RoutedEventArgs e)
        {
            txtServerId.Text = txtServerId.Text.ToUpperInvariant();
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtName.Text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(txtName.Text.ToLowerInvariant());
        }

        private void btnNameGen_Click(object sender, RoutedEventArgs e)
        {
            txtForms.Text += FileNameGenerator.FileName;
        }

        private void TabControlContextMenuNew(object sender, RoutedEventArgs e)
        {
            var template = NotesList[0].TabItem;

            NotesInfo newNote = new(template.Clone());

            NotesList.Add(newNote);
            tcTabs.Items.Add(newNote.TabItem);
            tcTabs.SelectedItem = newNote.TabItem;
            ToggleClose();
        }

        private void TabControlContextMenuClose(object sender, RoutedEventArgs e)
        {
            var selected = GetSelected(NotesList);
            if (selected is null) return;

            tcTabs.Items.Remove(selected.TabItem);
            NotesList.RemoveAll(x => x.TabItem == selected.TabItem);
            NotesList[0].TabItem.IsSelected = true;
            ToggleClose();
        }

        private void SwitchNotes(NotesInfo info)
        {
            txtServerId.Text = info.ServerId ?? string.Empty;
            txtCompanies.Text = info.Companies ?? string.Empty;
            txtDealer.Text = info.Dealership ?? string.Empty;
            txtName.Text = info.ContactName ?? string.Empty;
            txtEmail.Text = info.Email ?? string.Empty;
            txtNotes.Text = info.NotesText ?? string.Empty;
            txtCaseNo.Text = info.CaseText ?? string.Empty;
            txtForms.Text = info.FormsText ?? string.Empty;
            txtDeal.Text = info.DealText ?? string.Empty;
            var phone = info.Phone?.Split('x');
            txtPhone.Text = phone is not null ? phone[0] : string.Empty;
            txtPhoneExt.Text = phone is not null && phone.Length > 1 ? phone[1] : string.Empty;

            Notes.SelectedNote = info;
        }

        private void tcTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = GetSelected(NotesList);
            if (selected is null) return;

            SwitchNotes(selected);
        }

        private static NotesInfo? GetSelected(List<NotesInfo> notes)
        {
            foreach (var note in notes)
                if (note.IsSelected)
                    return note;

            return null;
        }

        private void ToggleClose()
        {
            TabItem item = (TabItem)tcTabs.Items[0];
            MenuItem menu = (MenuItem)item.ContextMenu.Items[1];
            if (tcTabs.Items.Count > 1)
            {
                menu.IsEnabled = true;
            }
            else
            {
                menu.IsEnabled = false;
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            var text = Clipboard.GetText();

            if (string.IsNullOrEmpty(text)) return;

            var lines = text.Split('\n');

            if (!lines[0].Equals("Case Number\r")) return;

            var newNote = LoadSFNotes(lines);

            NotesList.Add(newNote);
            tcTabs.Items.Add(newNote.TabItem);
            tcTabs.SelectedItem = newNote.TabItem;
            ToggleClose();

            Clipboard.Clear();
        }

        private NotesInfo LoadSFNotes(string[] notes)
        {
            var submittedIdx = Array.IndexOf(notes, "Submitted Values:\r");
            var serverIdx = Array.IndexOf(notes, "Server-Provided Values:\r");
            string[]? submittedValues = null;
            string[]? serverValues = null;

            if (submittedIdx > 0)
                submittedValues = notes[submittedIdx..];

            if (submittedIdx > 0 && serverIdx > 0)
                submittedValues = notes[submittedIdx..serverIdx];

            if(serverIdx > 0)
                serverValues = notes[serverIdx..];

            NotesInfo newNote = new(NotesList[0].TabItem.Clone())
            {
                CaseText = notes.Length >= 1 ? notes[1].Trim() : string.Empty,
                ContactName = notes.Length >= 10 ? notes[10].Trim() : string.Empty,
                Email = submittedValues is not null && submittedValues.Length >= 3 ? submittedValues[3][7..].Trim() : string.Empty,
                Phone = submittedValues is not null && submittedValues.Length >= 4 ? submittedValues[4][6..].Trim() : string.Empty,
                Companies = submittedValues is not null && submittedValues.Length >= 5 ? submittedValues[5][15..].Trim() : string.Empty,
                Dealership = submittedValues is not null && submittedValues.Length >= 6 ? submittedValues[6][13..].Trim() : string.Empty,
                ServerId = serverValues is not null && serverValues.Length >= 2 ? serverValues[2][10..].Trim() : string.Empty
            };

            return newNote;
        }
    }
}
