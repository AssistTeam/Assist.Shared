using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Assist.ViewModels;
using AssistUser.Lib.Account;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Assist.Shared.Settings.Accounts;

public sealed partial class AccountSettings : ViewModelBase
{
    
    public static AccountSettings Default { get; set; }
    public static readonly string BaseFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AssistData", "Accounts");
    public static readonly string FilePath = Path.Combine(BaseFolderPath, "AccountSettings.json"); // Note: This file does not have a debug version, this is to share Riot accounts across different testing cycles.

    
    static AccountSettings()
    {
        Default = new AccountSettings();
    }

    public static void Save()
    {
        if (Default == null) return;
        File.WriteAllText(FilePath, JsonSerializer.Serialize(Default, new JsonSerializerOptions() { WriteIndented = true }), Encoding.UTF8);
    }

    public static void Delete()
    {
        if (File.Exists(FilePath)) File.Delete(FilePath);
    }

    [ObservableProperty] private List<AccountProfile> _accounts = new List<AccountProfile>();
    [ObservableProperty] private string _defaultAccount;

    public async Task UpdateAccount(AccountProfile accountProfile)
    {
        var itemIndex = Accounts.FindIndex(x => x.Id == accountProfile.Id);

        if (itemIndex == -1)
        {
            Accounts.Add(accountProfile);

            
            Save();
            return;
        }
        
        Accounts.RemoveAt(itemIndex);
        Accounts.Add(accountProfile);
        Save();
    }
}