﻿using FormgenAssistant.Controls;
using FormgenAssistant.Dialogues;
using FormgenAssistant.SavedItems.Templates;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using TemplateList = FormgenAssistant.SavedItems.Templates.Templates;

namespace FormgenAssistant.Pages;
/// <summary>
/// Interaction logic for Templates.xaml
/// </summary>
public partial class Templates : UserControl
{
    public Templates()
    {
        InitializeComponent();
        RefreshTemplates();
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        var dialogue = new NewTemplateDialogue();
        var success = dialogue.ShowDialog();

        if (success == true && dialogue.NewTemplate is not null)
        {
            TemplateList.Instance.TemplateList.Add(dialogue.NewTemplate);
            TemplateList.Save();
            RefreshTemplates();
            if(SavedItems.Settings.Instance.SelectNewTemplate)
                lBoxTemplateList.SelectedItem = dialogue.NewTemplate.Name;
            UpdateDisplay();
        }
    }

    private void btnRemove_Click(object sender, RoutedEventArgs e)
    {
        var selected = lBoxTemplateList.SelectedIndex;
        TemplateList.Instance.TemplateList.RemoveAt(selected);
        TemplateList.Save();
        RefreshTemplates();
        txtDisplay.Text = string.Empty;
        stkVariables.Children.Clear();

    }

    private void lBoxTemplateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lBoxTemplateList.SelectedIndex < 0) return;

        var item = TemplateList.Instance.TemplateList[lBoxTemplateList.SelectedIndex];

        stkVariables.Children.Clear();

        for (int i = 0; i < item.Variables; i++)
        {
            stkVariables.Children.Add(CreateVariableTextBox());
        }
        txtDisplay.Text = item.Text; 
        UpdateDisplay();
    }

    private UIElement CreateVariableTextBox()
    {
        var box = new NewTextBox
        {
            Margin = new Thickness(5, 0, 5, 0),
            MinWidth = 120,
            MaxWidth = 400
            
        };
        GenerateContextMenu(box);
        box.OnTextChanged += Box_OnTextChanged;
        return box;
    }

    private void GenerateContextMenu(NewTextBox box)
    {
        var item = new MenuItem
        {
            Header = "Reserved Variable Names"
        };

        item.Items.Add(AddContextMenuItem(box, "Server ID#", "Notes:ServerID"));
        item.Items.Add(AddContextMenuItem(box, "Company#(s)", "Notes:Companies"));
        item.Items.Add(AddContextMenuItem(box, "Dealership Name", "Notes:Dealership"));
        item.Items.Add(AddContextMenuItem(box, "Contact Name", "Notes:ContactName"));
        item.Items.Add(AddContextMenuItem(box, "E-Mail Address", "Notes:EmailAddress"));
        item.Items.Add(AddContextMenuItem(box, "Phone#", "Notes:Phone"));
        item.Items.Add(AddContextMenuItem(box, "Notes", "Notes:Notes"));
        item.Items.Add(AddContextMenuItem(box, "Case #", "Notes:CaseNumber"));
        item.Items.Add(AddContextMenuItem(box, "Forms", "Notes:Forms"));

        box.ContextMenu ??= new();
        box.ContextMenu.Items.Add(item);
    }

    private static MenuItem AddContextMenuItem(NewTextBox box, string header, string text)
    {
        var item = new MenuItem
        {
            Header = header
        };
        item.Click += (s, e) =>
        {
           box.Text = text;
        };
        return item;
    }

    private void Box_OnTextChanged(object? sender, EventArgs e)
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var selectedIndex = lBoxTemplateList.SelectedIndex;
        if (selectedIndex < 0) return;

        var variables = new List<string>();

        for (int i = 0; i < TemplateList.Instance.TemplateList[selectedIndex].VariableDefaults.Count; i++)
        {
            var varDefault = TemplateList.Instance.TemplateList[selectedIndex].VariableDefaults[i];
            var text = (NewTextBox)stkVariables.Children[i];
            if (text.Text.Equals(string.Empty) )
            ((NewTextBox)stkVariables.Children[i]).Text = varDefault;
        }

        foreach (NewTextBox item in stkVariables.Children) variables.Add(item.Text);

        for (int i = 0; i < variables.Count; i++)
        {
            var variable = variables[i] ?? TemplateList.Instance.TemplateList[selectedIndex].VariableDefaults[i];
            CheckForReserved(variables, i, variable);
        }

        txtDisplay.Text = string.Format(TemplateList.Instance.TemplateList[selectedIndex].Text, variables.ToArray());
    }

    private static void CheckForReserved(List<string> variables, int i, string variable)
    {
        if (variable.StartsWith("Notes:", StringComparison.OrdinalIgnoreCase))
        {
            if (variable.Length <= 7) return;
            var reservedName = variable[6..];
            ReplaceReserved(variables, i, reservedName);

        }
    }

    private static void ReplaceReserved(List<string> variables, int i, string reservedName)
    {
        switch (reservedName.ToLowerInvariant())
        {
            case "serverid":
            case "server":
            case "serv":
                variables[i] = Notes.ServerId ?? string.Empty;
                break;
            case "companies":
            case "company":
            case "comp":
            case "co":
                variables[i] = Notes.Companies ?? string.Empty;
                break;
            case "dealership":
            case "dealer":
            case "dlr":
                variables[i] = Notes.Dealership ?? string.Empty;
                break;
            case "contactname":
            case "name":
                variables[i] = Notes.ContactName ?? string.Empty;
                break;
            case "emailaddress":
            case "email":
                variables[i] = Notes.Email ?? string.Empty;
                break;
            case "phone":
                variables[i] = Notes.Phone ?? string.Empty;
                break;
            case "notes":
                variables[i] = Notes.NotesText ?? string.Empty;
                break;
            case "casenumber":
            case "caseno":
            case "case":
                variables[i] = Notes.CaseText ?? string.Empty;
                break; 
            case "forms":
            case "form":
                variables[i] = Notes.FormsText ?? string.Empty;
                break;

        }
    }

    private void RefreshTemplates()
    {
        lBoxTemplateList.Items.Clear();

        foreach(var template in TemplateList.Instance.TemplateList)
        {
            lBoxTemplateList.Items.Add(template.Name);
        }    
    }

    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
        var selectedIndex = lBoxTemplateList.SelectedIndex;
        if (selectedIndex < 0) return;

        var item = TemplateList.Instance.TemplateList[selectedIndex];
        var dialogue = new NewTemplateDialogue(item);
        var success = dialogue.ShowDialog();

        if (success == true && dialogue.NewTemplate is not null)
        {
            TemplateList.Instance.TemplateList[selectedIndex] = dialogue.NewTemplate;
            TemplateList.Save();
            RefreshTemplates();
            lBoxTemplateList.SelectedItem = dialogue.NewTemplate.Name;
        }
        UpdateDisplay();
    }

    private void btnCopyTemplate_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(txtDisplay.Text);
    }

    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
        UpdateDisplay();
    }
}
